using UnityEngine;

public class LaserGun : Gun
{
    //[SerializeField] LaserScript LaserLine;


    [SerializeField] LaserScript Laser;
    [SerializeField] float LaserLength;
    [SerializeField] float damage;

    [SerializeField] LayerMask LayersToHitDamage;
    [SerializeField] LayerMask LayersToCutRenderDist;

    [SerializeField] private bool HasSpecialEffect;



    private float laserElapsed = 0f;

    bool laserOn = false;


    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
        {
            return false;
        }


        Ray laserRay = new Ray(gunBarrelEnd.position, gunBarrelEnd.forward);
        Vector3 laserEndPosition = gunBarrelEnd.position + (gunBarrelEnd.forward * LaserLength);


        if(Physics.Raycast(laserRay,out RaycastHit hit, LaserLength, LayersToCutRenderDist))
        {
            laserEndPosition = hit.point;
        }

        //Laser.gameObject.SetActive(true);
        Laser.ActivateLineRender(LaserLength);
        Laser.SetLaserPosition(gunBarrelEnd.position, laserEndPosition);

        laserOn = true;


        return true;
    }



    private void FixedUpdate()
    {
        //If not able to shoot return
        if (laserOn == false)
            return;


        //Increase laser elapsed time
        laserElapsed += Time.deltaTime;

        //If laser elapsed time is less than timeBetweenDamage, return
        if (laserElapsed < timeBetweenShots)
            return;

        // Attempt Deal Damage 
        Ray laserRay = new Ray(gunBarrelEnd.position, gunBarrelEnd.forward);
        if (Physics.Raycast(laserRay, out RaycastHit hit, LaserLength, LayersToHitDamage))
        {
            hit.collider.GetComponent<Damageable>().TakeDamage(damage);
        }



        //Reset laser state
        laserOn = false;
        laserElapsed = 0f;
        ammo -= 1;

    }



}
