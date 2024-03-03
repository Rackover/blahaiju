using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.1f;

    [SerializeField]
    private float strength = 0.1f;

    [ContextMenu("Shake!")]
    public void ShakeCamera()
    {
        ShakeCamera(duration, strength);
    }

    public void ShakeCamera(float strength, float duration)
    {
        Vector3 originalPosition = transform.position;
        transform.DOShakePosition(duration, strength)
            .OnComplete(() =>
            {
                if (this)
                {
                    transform.position = originalPosition;
                }
            });
    }
}
