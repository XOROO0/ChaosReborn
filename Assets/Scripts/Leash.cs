using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leash : MonoBehaviour
{

    public Vector3 hitPoint;

    public float pullForce = 100;

    [HideInInspector]
    public bool isLeashing;

    [SerializeField] LayerMask whatIsLeashable;
    [SerializeField] float minLeashRange, maxLeashRange;
    public Transform leashPoint, cam;

    [HideInInspector] public bool resetLeash;

    private Transform caughtEnemy;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StartLeash();
        }

        if(caughtEnemy != null)
        {
            hitPoint = caughtEnemy.position;
        }

        if (caughtEnemy == null)
            isLeashing = false;

        if (isLeashing)
        {
            Vector3 dir = Camera.main.transform.forward;
            GetComponent<Rigidbody>().AddForce(dir * pullForce, ForceMode.Force);

            if (Vector3.Distance(transform.position, caughtEnemy.position) < 2)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                StopLeash();
            }
        }
    }



    void StartLeash()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, maxLeashRange, whatIsLeashable))
        {
            if(hit.transform.root.GetComponent<RagdollEnemy>().IsStunned)
            {
                if(hit.distance >= minLeashRange)
                {
                    isLeashing = true;
                    caughtEnemy = hit.transform;
                    hitPoint = hit.point;


                    Invoke(nameof(PullLeash), 0.3f);
                }
                else
                {
                    GetComponent<Kick>().DoKick(hit.transform);
                }
            }

        }
    }

    void PullLeash()
    {
        if (caughtEnemy == null)
            return;


        //caughtEnemy.transform.root.GetComponent<RagdollEnemy>().Pull();

        FPSController.canMove = false;
    }

    void StopLeash()
    {
        isLeashing = false;
        FPSController.canMove = true;
        if(caughtEnemy == null) return;
        //caughtEnemy.transform.root.GetComponent<RagdollEnemy>().StopMoving();
    }

    public Vector3 GetHitPoint => hitPoint;
}
