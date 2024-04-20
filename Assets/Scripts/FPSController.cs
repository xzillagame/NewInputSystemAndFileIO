using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class FPSController : MonoBehaviour
{
    // references
    CharacterController controller;
    [SerializeField] GameObject cam;
    [SerializeField] Transform gunHold;
    [SerializeField] Gun initialGun;

    // stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;

    // private variables
    Vector3 origin;
    Vector3 velocity;
    bool grounded;
    float xRotation;
    List<Gun> equippedGuns = new List<Gun>();
    int gunIndex = 0;
    Gun currentGun = null;

    // properties
    public GameObject Cam { get { return cam; } }


    //Save Data Variables
    private float health = 50;


    private void OnEnable()
    { 

        PlayerInputManager.playerControls.Enable();
        PlayerInputManager.playerControls.FPSControles.Jump.performed += OnJump;
        PlayerInputManager.playerControls.FPSControles.PrimaryShoot.performed += OnFire;
        PlayerInputManager.playerControls.FPSControles.SwapWeapons.performed += OnWeaponSwap;
        PlayerInputManager.playerControls.FPSControles.AlternateShoot.performed += OnAltWeaponFire;
    }

    private void OnDisable()
    {
        PlayerInputManager.playerControls.FPSControles.Jump.performed -= OnJump;
        PlayerInputManager.playerControls.FPSControles.PrimaryShoot.performed -= OnFire;
        PlayerInputManager.playerControls.FPSControles.SwapWeapons.performed -= OnWeaponSwap;
        PlayerInputManager.playerControls.FPSControles.AlternateShoot.performed -= OnAltWeaponFire;
        PlayerInputManager.playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // start with a gun
        if(initialGun != null)
            AddGun(initialGun);

        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Look();

        if(PlayerInputManager.playerControls.FPSControles.PrimaryAutomaticShoot.IsPressed() == true && currentGun?.AttemptAutomaticFire() == true)
        {
            currentGun?.AttemptFire();
        }



        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);
    }

    void Movement()
    {
        grounded = controller.isGrounded;

        if(grounded && velocity.y < 0)
        {
            velocity.y = -1;// -0.5f;
        }

        //Now uses the New Input System
        Vector2 movement = PlayerInputManager.playerControls.FPSControles.Movement.ReadValue<Vector2>();

        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * (PlayerInputManager.playerControls.FPSControles.Sprint.IsPressed() ? 2 : 1) * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        Vector2 looking = PlayerInputManager.playerControls.FPSControles.Look.ReadValue<Vector2>();
        float lookX = looking.x * lookSensitivityX * Time.deltaTime;
        float lookY = looking.y * lookSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    void HandleSwitchGun()
    {
        if (equippedGuns.Count == 0)
            return;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            gunIndex++;
            if (gunIndex > equippedGuns.Count - 1)
                gunIndex = 0;

            EquipGun(equippedGuns[gunIndex]);
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gunIndex--;
            if (gunIndex < 0)
                gunIndex = equippedGuns.Count - 1;

            EquipGun(equippedGuns[gunIndex]);
        }
    }


    void EquipGun(Gun g)
    {
        // disable current gun, if there is one
        currentGun?.Unequip();
        currentGun?.gameObject.SetActive(false);

        // enable the new gun
        g.gameObject.SetActive(true);
        g.transform.parent = gunHold;
        g.transform.localPosition = Vector3.zero;
        currentGun = g;

        g.Equip(this);
    }

    // public methods

    public void AddGun(Gun g)
    {
        // add new gun to the list
        equippedGuns.Add(g);

        // our index is the last one/new one
        gunIndex = equippedGuns.Count - 1;

        // put gun in the right place
        EquipGun(g);
    }

    public void IncreaseAmmo(int amount)
    {
        currentGun.AddAmmo(amount);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    // Input methods
    void FireGun()
    {
        // don't fire if we don't have a gun
        if (currentGun == null)
            return;

        // pressed the fire button
        if(GetPressFire())
        {
            currentGun?.AttemptFire();
        }

        // holding the fire button (for automatic)
        else if(GetHoldFire())
        {
            if (currentGun.AttemptAutomaticFire())
                currentGun?.AttemptFire();
        }

        // pressed the alt fire button
        if (GetPressAltFire())
        {
            currentGun?.AttemptAltFire();
        }
    }

    bool GetPressFire()
    {
        return Input.GetButtonDown("Fire1");
    }

    bool GetHoldFire()
    {
        return Input.GetButton("Fire1");
    }

    bool GetPressAltFire()
    {
        return Input.GetButtonDown("Fire2");
    }

    Vector2 GetPlayerMovementVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    Vector2 GetPlayerLook()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    bool GetSprint()
    {
        return Input.GetButton("Sprint");
    }


    //Added Functions by Xavian Escamilla 4/17/24
    private void OnJump(InputAction.CallbackContext ctx)
    {

        if (grounded == true)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -1 * gravity);
        }

    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (currentGun?.AttemptAutomaticFire() == true)
        {
            return;
        }
        currentGun?.AttemptFire();
    }

    private void OnAltWeaponFire(InputAction.CallbackContext ctx)
    {
        currentGun?.AttemptAltFire();
    }

    private void OnWeaponSwap(InputAction.CallbackContext ctx)
    {
        if (equippedGuns.Count == 0)
            return;

        float axis = ctx.ReadValue<float>();

        if (axis > 0)
        {
            gunIndex++;
            if (gunIndex > equippedGuns.Count - 1)
                gunIndex = 0;

            EquipGun(equippedGuns[gunIndex]);
        }

        else if (axis < 0)
        {
            gunIndex--;
            if (gunIndex < 0)
                gunIndex = equippedGuns.Count - 1;

            EquipGun(equippedGuns[gunIndex]);
        }
    }


    public void OnSave()
    {
        SaveData newSaveData = new SaveData();
        newSaveData.position = transform.position;
        newSaveData.playerHealth = health;

        if(currentGun != null)
        {
            newSaveData.currentAmmo = currentGun.GetCurrentAmmo();
        }

        string saveData = JsonUtility.ToJson(newSaveData);
        File.WriteAllText(Application.dataPath + "/FPSSaveData.json", saveData);

    }


    public void OnLoad()
    {
        if(!File.Exists(Application.dataPath + "/FPSSaveData.json"))
        {
            Debug.Log("Save Data Does Not Exist");
            return;
        }

        string saveText = File.ReadAllText(Application.dataPath + "/FPSSaveData.json");

        //Disable controller to allow position to be adjusted
        controller.enabled = false;

        SaveData loadData = JsonUtility.FromJson<SaveData>(saveText);
        transform.position = loadData.position;
        health = loadData.playerHealth;

        if (currentGun != null)
        {
            currentGun.AddAmmo(loadData.currentAmmo);
        }

        controller.enabled = true;

    }

    // Collision methods

    // Character Controller can't use OnCollisionEnter :D thanks Unity
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Damager>())
        {
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);
        }

        if (hit.gameObject.GetComponent <KillZone>())
        {
            Respawn();
        }
    }


}

public class SaveData
{
    public Vector3 position;
    public float playerHealth;
    public int currentAmmo = 0;

    public SaveData() { }

}
