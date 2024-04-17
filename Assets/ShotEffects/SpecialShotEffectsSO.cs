using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShotEffectsSO", menuName = "SciptableObjects/Shot Effects")]
public class SpecialShotEffectsSO : ScriptableObject
{
    [SerializeField] List<SpecialEffect> ListOfEffectPrefabs;

    public void ActivateEffect(HitData hitData)
    {

        for (int i = 0; i < ListOfEffectPrefabs.Count; i++)
        {
            SpecialEffect se = Instantiate(ListOfEffectPrefabs[i], hitData.location, Quaternion.identity);
            se.ActivateEffect(hitData);
        }

    }




}
