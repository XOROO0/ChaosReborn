using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 startPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void OnShake(float duration, float strength)
    {
        if (!DOTween.IsTweening(transform))
        {
            transform.DOShakePosition(duration, strength, 10, 30, false, true, ShakeRandomnessMode.Harmonic);
            //transform.DOShakeRotation(duration, strength);
        }
    }

    public static void Shake(float duration, float strength)
    {
        instance.OnShake(duration, strength);
    }
}

