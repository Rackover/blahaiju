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
            if (other.transform.parent != null && other.transform.parent.GetComponent<EnemyBehavior>() != null)
            {
                Destroy(other.transform.parent.gameObject);
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
        eggsService.LoseEgg(this);
    }
}
