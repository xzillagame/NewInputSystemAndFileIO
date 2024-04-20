using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInputManager.playerControls.FPSControles.PausePressed.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        PlayerInputManager.playerControls.FPSControles.PausePressed.performed -= OnPausePressed;
    }



    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if(SceneManager.GetSceneByName("Pause").isLoaded == false)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);

            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            SceneManager.UnloadSceneAsync("Pause");

            Time.timeScale = 1;
        }
    }




}
