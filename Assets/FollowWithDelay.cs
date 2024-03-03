using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithDelay : MonoBehaviour
{
    [SerializeField]
    private float rotationLerpSpeed = 1f;

    [SerializeField]
    private float positionLerpSpeed = 1f;

    [SerializeField]
    private bool lerpPosition = false;

    [SerializeField]
    private bool lerpRotation = false;

    private Quaternion storedRotation;

    private Vector3 storedPosition;

    private Quaternion baseRotation;

    private Vector3 baseOffset;


    private void Awake()
    {
        storedRotation = transform.rotation;
        storedPosition = transform.position;

        baseOffset = transform.localPosition;
        baseRotation = transform.localRotation;
    }

    private void Update()
    {
        if (lerpPosition)
        {
            Vector3 targetLocal = baseOffset;
            transform.position = storedPosition;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetLocal,
                Mathf.Clamp01(positionLerpSpeed * Time.deltaTime)
            );

            transform.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);

            storedPosition = transform.position;
        }

        if (lerpRotation)
        {
            Quaternion target = baseRotation;
            transform.rotation = storedRotation;

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                target,
                rotationLerpSpeed * Time.deltaTime
            );

            storedRotation = transform.rotation;
        }
    }

}
