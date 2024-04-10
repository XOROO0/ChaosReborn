using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEnemy : MonoBehaviour
{
    private Rigidbody[] rbs;

    public float force = 10;

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs) 
        {
            var dir = Random.insideUnitCircle * force;
            rb.AddForce(FPSController.playerTransform.forward * dir, ForceMode.Impulse);
        }
    }

}
