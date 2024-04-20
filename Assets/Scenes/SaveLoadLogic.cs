using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadLogic : MonoBehaviour
{
   

    public void OnSavePressed()
    {
        FPSController player = FindAnyObjectByType<FPSController>();
        player.OnSave();
    }

    public void OnLoadPressed()
    {
        FPSController player = FindAnyObjectByType<FPSController>();
        player.OnLoad();
    }






}
