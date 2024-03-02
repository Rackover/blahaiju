using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public EggsService eggsService;

    void OnCollisionEnter(Collision _collision)
    {
        GameObject other = _collision.gameObject;
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent);
            }
            else
            {
                Destroy(other);
            }
            GetHurt();
        }
    }

    void GetHurt()
    {
        eggsService.LoseDevelopment();
        gameObject.SetActive(false);
    }
}
