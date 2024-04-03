using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlast : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] LayerMask ground;

    float velocityMagnitude;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        velocityMagnitude = rb.velocity.sqrMagnitude;
    }

    // Update is called once per fram

/*    private void OnCollisionEnter(Collision collision)
    {

        if((ground & (1 << collision.gameObject.layer)) != 0)
        {

            if (velocityMagnitude - rb.velocity.sqrMagnitude > 80)
            {
                transform.parent.GetComponent<RagdollEnemy>().TakeDamage(100);
            }
        }

    }*/
}