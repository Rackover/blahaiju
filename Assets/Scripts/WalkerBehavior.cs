using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBehavior : EnemyBehavior
{
    public override EnemyType Type => EnemyType.Walker;

    [SerializeField]
    private GameObject[] possibleSigns;

    [SerializeField]
    private Transform signParent;

    [SerializeField]
    private Animator animator;

    public void GiveSign()
    {
        animator.SetBool("HasSign", true);
        GameObject sign = possibleSigns[Random.Range(0, possibleSigns.Length)];
       
        var inst =Instantiate(sign, signParent);
        inst.transform.localPosition = Vector3.zero;
        inst.transform.localRotation= Quaternion.identity;
        inst.transform.localScale = Vector3.one;
    }
}
