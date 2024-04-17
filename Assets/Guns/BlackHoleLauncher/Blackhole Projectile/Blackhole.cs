using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    [SerializeField] float LifeTime = 2f;
    private float currentLifeTime = 0f;


    [SerializeField] float ProjectileSpeed = 10f;


    [SerializeField] float GravityForce = 75f;
    List<Rigidbody> entites = new List<Rigidbody>();



    [SerializeField][Range(0f,100f)] private float RBVelocityClampMax;


    [SerializeField] ParticleSystem OrbitalPS;
    [SerializeField] ParticleSystem ZPlanSwirlEffect;


    public void InitializeBlackHole(float life = 2f, float gravForce = 75f, float projSpeed = 10f)
    {
        LifeTime = life;
        GravityForce = gravForce;
        ProjectileSpeed = projSpeed;

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            
            if(!entites.Contains(rb))
            {
                entites.Add(rb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            if(entites.Contains(rb))
            {
                rb.velocity = rb.velocity * 0.5f;
                entites.Remove(rb);
            }
        }
    }

    private void FixedUpdate()
    {
       for(int i = 0; i < entites.Count; i++)
        {
            Vector3 rbLocation = entites[i].position;
            Vector3 centerLocation = transform.position;
            Vector3 forceDirection = (centerLocation - rbLocation).normalized;

            float NewtonsGravityForce = 1 / Mathf.Pow(forceDirection.magnitude, 2);
            float scaledForce = NewtonsGravityForce * GravityForce;

            forceDirection *= scaledForce;


            entites[i].AddForce(forceDirection, ForceMode.Force);
            entites[i].velocity = Vector3.ClampMagnitude(entites[i].velocity, RBVelocityClampMax);

        }
    }



    private void OnDisable()
    {
        
        for(int i = 0; i < entites.Count; i++)
        {
            if (entites[i] != null)
                entites[i].velocity = entites[i].velocity * 0.5f;
        }


        ParticleSystem.MainModule oribtalMain = OrbitalPS.main;
        oribtalMain.loop = false;

        OrbitalPS.transform.parent = null;

        ParticleSystem.MainModule zPlaneSwirlMain = ZPlanSwirlEffect.main;
        zPlaneSwirlMain.loop = false;

        ZPlanSwirlEffect.transform.parent = null;
    }




    private void Update()
    {
        if(currentLifeTime >= LifeTime)
        {
            Destroy(this.gameObject);
        }

        currentLifeTime += Time.deltaTime;

        transform.Translate(new Vector3(0, 0, ProjectileSpeed * Time.deltaTime));

    }


}
