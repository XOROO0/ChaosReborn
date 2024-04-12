using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEnemy : MonoBehaviour
{
    private Rigidbody[] rbs;

    public float force = 10;

    void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
    }

    public void RegularBlast()
    {
        foreach (Rigidbody rb in rbs)
        {
            var dir = Random.insideUnitCircle * force;
            rb.AddForce(FPSController.playerTransform.forward * dir, ForceMode.Impulse);
        }
    }

    public void RocketBlast()
    {
        foreach (Rigidbody rb in rbs)
        {
            var dir = Random.insideUnitSphere * force;
            rb.AddForce(dir, ForceMode.Impulse);
        }
    }

}
