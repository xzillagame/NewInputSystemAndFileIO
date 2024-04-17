using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    TMP_Text text;

    //Allow numbers to go a random direction besides directly up
    Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    { 
    }

    private void Awake()
    {
        //Assing random value for number to float in direction of
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.up + randomDirection).normalized * moveSpeed * Time.deltaTime;
    }

    public void SetNumber(float num)
    {
        text.text = num.ToString();
    }
}
