using UnityEngine;

public class DealExtraDamage : SpecialEffect
{

    [SerializeField] float DamageMultipler;
    [SerializeField] float ExtraDamageToDeal;


    public override void ActivateEffect(HitData hitData)
    {
        hitData.target.TakeDamage(ExtraDamageToDeal * DamageMultipler);
        Destroy(this.gameObject);
    }



}
