using System;
using System.Data;
using UnityEngine;


public class ShotGun : Gun
{
    [Header("Shotgun Stats")]
    [SerializeField] int PelletsPerShot;
    [SerializeField] float DamagePerPellet;
    [SerializeField] float PelletSpeed = 50f;
    [SerializeField] float PelletLifeTime = 0.75f;
    [SerializeField] float PelletKnockBackForce = 10f;



    [Header("Special Effect Spawn")]
    [SerializeField] private bool HasSpecialEffect;
    [SerializeField] SpecialShotEffectsSO ShotEffect;



    [System.Serializable]
    private struct PelletShotAngle
    {
        [Range(-50f,0f)] public float lowerXBound;
        [Range(0f, 50f)] public float upperXBound;

        [Space(20)]

        [Range(-50f, 0f)] public float lowerYBound;
        [Range(0f, 50f)] public float upperYBound;
    }
    [SerializeField] PelletShotAngle ShotAngles;


    private int PelletsHit = 0;




    public override bool AttemptFire()
    {

        if (!base.AttemptFire())
        {
            return false;
        }


        if(elapsed >= timeBetweenShots)
        {
            PelletsHit = 0;
        }

        for(int i = 0; i < PelletsPerShot; i++)
        {

            float randomX = UnityEngine.Random.Range(ShotAngles.lowerXBound, ShotAngles.upperXBound);
            float randomY = UnityEngine.Random.Range(ShotAngles.lowerYBound, ShotAngles.upperYBound);

            Quaternion randomRotationOffSet = Quaternion.Euler(randomX, randomY, 0);


            GameObject g = Instantiate(bulletPrefab, gunBarrelEnd.position, gunBarrelEnd.rotation * randomRotationOffSet);
            Projectile p = g.GetComponent<Projectile>();

            if (HasSpecialEffect == true)
            {
                p.Initialize(DamagePerPellet, PelletSpeed, PelletLifeTime, PelletKnockBackForce, CountBullets);
            }
            else
            {
                p.Initialize(DamagePerPellet, PelletSpeed, PelletLifeTime, PelletKnockBackForce, null);
            }

        }

        anim.SetTrigger("shoot");
        elapsed = 0f;
        ammo -= 1;
        return true;

    }


    private void CountBullets(HitData data)
    {


        PelletsHit += 1;


        if (PelletsHit >= PelletsPerShot)
        {
            Debug.Log("All Pellets hit");
            PelletsHit = 0;
            ShotEffect.ActivateEffect(data);
        }



    }



}
