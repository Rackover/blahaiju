using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float speed = 180f;
    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
