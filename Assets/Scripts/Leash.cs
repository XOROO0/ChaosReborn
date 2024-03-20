using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leash : MonoBehaviour
{

    public Vector3 hitPoint;

    [HideInInspector]
    public bool isLeashing;

    [SerializeField] LayerMask whatIsLeashable;
    public Transform leashPoint, cam;

    [HideInInspector] public bool resetLeash;

    private Transform caughtEnemy;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StartLeash();
        }

/*        if(caughtEnemy != null)
        {
            hitPoint = caughtEnemy.position;
        }*/
    }



    void StartLeash()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 50, whatIsLeashable))
        {
            isLeashing = true;
            caughtEnemy = hit.transform;
            hitPoint = hit.point;


            Invoke(nameof(PullLeash), 0.3f);
        }
    }

    void PullLeash()
    {
        if (caughtEnemy == null)
            return;


        caughtEnemy.transform.GetComponent<Blast>().Pull();

        Invoke(nameof(StopLeash), 0.2f);
    }

    void StopLeash()
    {
        isLeashing = false;
    }

    public Vector3 GetHitPoint => hitPoint;
}
