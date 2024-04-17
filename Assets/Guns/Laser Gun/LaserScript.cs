using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    [SerializeField] LineRenderer Line;

    [SerializeField] float ShrinkSpeed;

    // Update is called once per frame
    void Update()
    {

        if (Line.GetPosition(1) != Line.GetPosition(0))
        {
            Line.SetPosition(1, Vector3.MoveTowards(Line.GetPosition(1), Line.GetPosition(0), ShrinkSpeed * Time.deltaTime) );
        }
        else
        {
            Line.SetPosition(1, Vector3.zero);
            Line.enabled = false;
        }

    }

    public void SetLaserPosition(Vector3 orginPoint, Vector3 endPoint)
    {

        //Orgin Point
        Line.SetPosition(0, orginPoint);


        //End Point
        Line.SetPosition(1, endPoint);
    }

    public void ActivateLineRender(float laserLength)
    {
        Line.enabled = true;
    }


}
