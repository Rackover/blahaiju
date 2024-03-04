using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hook : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.1f;

    [SerializeField]
    private float strength = 0.1f;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        OnDemandRendering.renderFrameInterval = 0;
    }

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
