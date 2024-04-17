using System;
using UnityEngine;

public class BlackHoleLauncher : Gun
{

    [SerializeField] Blackhole BlackHoleProjectile;

    [Header("Blackhole Effects")]
    [SerializeField] ParticleSystem IdleParticleSystem;
    [SerializeField] ParticleSystem CharingParticleSystem;
    [SerializeField] AudioSource BlackholeLauncherAudioSource;


    [Serializable] private struct BlackHoleProperties
    {
        public float life;
        public float gravityForce;
        public float speed;

    }
    [SerializeField] private BlackHoleProperties BlackHoleProjectileProperties;


    public override bool AttemptFire()
    {
        if(!base.AttemptFire())
        {
            return false;
        }



        Blackhole blackhole = Instantiate(BlackHoleProjectile, gunBarrelEnd.position, gunBarrelEnd.rotation);
        blackhole.InitializeBlackHole(BlackHoleProjectileProperties.life, BlackHoleProjectileProperties.gravityForce,
                                                BlackHoleProjectileProperties.speed);

        //Play Sound
        BlackholeLauncherAudioSource.Play();

        anim.SetTrigger("shoot");
        elapsed = 0f;
        ammo -= 1;
        return true;
    }


    //Enable/Disable Particle Effects
    public void ParticlePlayCharingBlackHole()
    {
        CharingParticleSystem.Play();
        IdleParticleSystem.Stop();
    }

    public void ParticleStopCharingBlackHole()
    {
        CharingParticleSystem.Stop();
        IdleParticleSystem.Play();
    }


}
