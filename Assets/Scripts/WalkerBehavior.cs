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

    [SerializeField]
    private float lookAtSpeed = 2f;

    private Transform signTransform;

    public void GiveSign()
    {
        if (signParent)
        {
            animator.SetBool("HasSign", true);
            GameObject sign = possibleSigns[Random.Range(0, possibleSigns.Length)];

            var inst = Instantiate(sign, signParent);
            inst.transform.localPosition = Vector3.zero;
            inst.transform.localRotation = Quaternion.identity;
            inst.transform.localScale = Vector3.one;

            signTransform = inst.transform;
            //inst.transform.LookAt(Camera.main.transform);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (signTransform)
        {
            Quaternion look = Quaternion.LookRotation(Camera.main.transform.position - signTransform.position);
            signTransform.rotation = look;
            //signTransform.rotation = Quaternion.Lerp(signTransform.rotation, look, Time.deltaTime * lookAtSpeed);
        }
    }
}
