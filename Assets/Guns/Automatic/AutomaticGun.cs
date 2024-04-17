using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGun : Gun
{



    [SerializeField] private float BulletDamage = 2f;
    [SerializeField] private float BulletSpeed = 50f;
    [SerializeField] private float BulletLifeTime = 6f;
    [SerializeField] private float BulletKnockBackForce = 0.01f;


    [Header("Special Effect Spawn")]
    [SerializeField] private float bulletHitTimeLeway = 1f;
    [SerializeField] private int bulletEffectThreshold = 0;
    [SerializeField] private bool HasSpecialEffect;
    [SerializeField] SpecialShotEffectsSO ShotEffect;

    private int bulletHitFromMag = 0;



    public override bool AttemptFire()
    {
        if(!base.AttemptFire())
            return false;



        GameObject projectile = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        Projectile p = projectile.GetComponent<Projectile>();



        if (elapsed >= bulletHitTimeLeway)
        {
            bulletHitFromMag = 0;
        }


        if (HasSpecialEffect == true)
        {
            p.Initialize(BulletDamage, BulletSpeed, BulletLifeTime, BulletKnockBackForce, CountBullets);
        }
        else
        {
            p.Initialize(BulletDamage, BulletSpeed, BulletLifeTime, BulletKnockBackForce, null);
        }


        anim.SetTrigger("shoot");
        elapsed= 0;
        ammo -= 1;

        return true;
    }


    private void CountBullets(HitData data)
    {

        bulletHitFromMag += 1;

        if(bulletHitFromMag >= bulletEffectThreshold)
        {
            ShotEffect.ActivateEffect(data);
            bulletHitFromMag = 0;
        }



    }





}
