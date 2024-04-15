using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator anim;

    Rigidbody hipRB;

    public float slowDownGravityScale = 0.2f;

    private float gravityScale = 1f;

    public Vector3 hipPosition { get { return hipRB.position; } }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();

        hipRB = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            rb.useGravity = false;
        }

        DeactivateRagdoll();
    }

    private void FixedUpdate()
    {
        Vector3 gravity = -9.81f * gravityScale * Vector3.up;

        hipRB.AddForce(gravity, ForceMode.Acceleration);
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
        hipRB.AddForce(force, ForceMode.Impulse);
    }

    public void StopAllForces()
    {
        foreach (var r in rbs)
        {
            r.velocity = r.velocity.normalized * 2f;
        }
    }

    public void DisableGravity()
    {
        foreach(var r in rbs)
        {
            gravityScale = slowDownGravityScale;
        }
    }
}
