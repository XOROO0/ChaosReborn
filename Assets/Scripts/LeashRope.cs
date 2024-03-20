using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeashRope : MonoBehaviour
{
    [SerializeField] int quality;
    [SerializeField] Leash leash;
    [SerializeField] float damper;
    [SerializeField] float strength;
    [SerializeField] float velocity;
    [SerializeField] float waveCount;
    [SerializeField] float waveHeight;
    [SerializeField] AnimationCurve effectCurve;


    private LineRenderer lineRenderer;
    private Vector3 currentLeashPos;
    private Spring spring;

    Vector3 up;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    private void LateUpdate()
    {

        DrawLeash();
    }

    void DrawLeash()
    {
        if(!leash.isLeashing)
        {
            currentLeashPos = leash.leashPoint.position;
            spring.Reset();

            if (lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;

            return;
        }

        if(lineRenderer.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);

        spring.UpdateSpring(Time.deltaTime);

        var leashPoint = leash.GetHitPoint;
        var startPos = leash.leashPoint.position;
        up = Quaternion.LookRotation((leashPoint - startPos).normalized) * Vector3.up;

        currentLeashPos = Vector3.Lerp(currentLeashPos, leashPoint, Time.deltaTime * 12f);

        for(int i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * 
                spring.Value * effectCurve.Evaluate(delta));

            lineRenderer.SetPosition(i, Vector3.Lerp(startPos, currentLeashPos, delta) + offset);
        }
    }

}
