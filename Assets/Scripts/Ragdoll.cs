using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator anim;

    Rigidbody hipRB;

    public Vector3 hipPosition { get { return hipRB.position; } }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();

        hipRB = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();

        DeactivateRagdoll();
    }


    public void DeactivateRagdoll()
    {
        foreach (var r in rbs) 
        {
            r.isKinematic = true;
        }

        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var r in rbs)
        {
            r.isKinematic = false;
        }

        anim.enabled = false;
    }
    public void AddForce(Vector3 force)
    {
        hipRB.AddForce(force, ForceMode.VelocityChange);
    }

    public void StopAllForces()
    {
        foreach (var r in rbs)
        {
            r.velocity = Vector3.zero;
        }
    }
}
