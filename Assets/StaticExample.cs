using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StaticExample", menuName = "ScriptableObjects/StaticExampleSO")]
public class StaticExample : ScriptableObject
{

    UnityAction testAction;




    private void OnDisable()
    {
        testAction = null;
    }


    public void CallAction()
    {
        testAction?.Invoke();
    }


    public void AddAction(UnityAction newAction)
    {
        testAction += newAction;
    }

    public void RemoveAction(UnityAction removeAction)
    {
        testAction -= removeAction;
    }

}
