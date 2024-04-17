using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHp = 100;
    public UnityEvent<Vector3> OnHit;
    [SerializeField] GameObject damageNumberPrefab;

    float currentHp;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();   
    }

    public void Initialize()
    {
        currentHp = maxHp;
    }

    public void Hit(Vector3 knockback, float damageAmount)
    {
        //owner.ApplyKnockback(knockback);
        OnHit.Invoke(knockback);//, damageAmount);
        TakeDamage(damageAmount);
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;

        if (currentHp < 0)
            currentHp = 0;
                                                                                //To Offset to Allow numbers to spawn above head
        var damageNumber = Instantiate(damageNumberPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        damageNumber.GetComponent<DamageNumber>().SetNumber(amount);
    }
}
