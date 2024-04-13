using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEnemy : MonoBehaviour
{
    private Rigidbody[] rbs;

    public float force = 10;
    public float explosiveForce = 10;

    void Awake()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
    }

    public void RegularBlast()
    {
        foreach (Rigidbody rb in rbs)
        {
            //var dir = Random.insideUnitCircle * force;

            var dir = FPSController.playerTransform.forward +
                (FPSController.playerTransform.right * Random.Range(-3, 3));

            rb.AddForce(dir * force, ForceMode.Impulse);
        }
    }

    public void RocketBlast()
    {
        foreach (Rigidbody rb in rbs)
        {
            var dir = Random.insideUnitSphere * explosiveForce;
            rb.AddForce(dir, ForceMode.Impulse);
        }
    }

}
