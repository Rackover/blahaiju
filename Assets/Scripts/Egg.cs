using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public EggsService eggsService;

    [SerializeField]
    private Transform visualTransform;

    private void Awake()
    {
        visualTransform.localScale = Vector3.zero;
        visualTransform.DOScale(1f, 1f)
            .OnComplete(()=>
            {
                // TODO: PLAY FX HERE!
            });
    }

    void OnCollisionEnter(Collision _collision)
    {
        GameObject other = _collision.gameObject;
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponentInParent<EnemyBehavior>();

            if (enemy)
            {
                if (enemy.GetType() != typeof(PoliticianBehavior))
                {
                    enemy.Die(false);
                }
                else
                {
                    eggsService.Lose();
                }
            }
            
            GetHurt();
        }
    }

    void GetHurt()
    {
        eggsService.LoseEgg(this);
    }
}
