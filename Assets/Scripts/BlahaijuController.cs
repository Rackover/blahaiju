using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class BlahaijuController : MonoBehaviour
{
    public Rigidbody body;
    public Transform head;
    public float forceAmount;


    void Update()
    {
        //Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - head.position;
        //body.AddForceAtPosition(forceAmount, head, ForceMode.Acceleration);
    }
}
