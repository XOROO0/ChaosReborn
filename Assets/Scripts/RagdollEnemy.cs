using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnemy : MonoBehaviour
{
    [SerializeField]
    private int health = 200;
    public int dropHealth = 20;

    [SerializeField] GameObject explosion;

    Ragdoll rd;

    bool update = false;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    public float Health { get { return health; } }

    public bool IsStunned = false;

    private bool flag = false;

    private void Start()
    {
        rd = GetComponent<Ragdoll>();

    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;

        foreach (var mat in transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials)
        {
            mat.color = Color.red * intensity;
        }

        if (blinkTimer <= 0 && IsStunned)
            blinkTimer = blinkDuration;

        if(update)
        {
            //Debug.Log(Vector3.Distance(FPSController.playerTransform.position, transform.GetChild(1).position));


            if(Vector3.Distance(FPSController.playerTransform.position,
                transform.GetChild(1).position) < 2)
            {

                Debug.Log("Punch Enemy");

                rd.StopAllForces();
                rd.DisableGravity();
                AllowPlayerToMove();
                //rd.DeactivateRagdoll();
            }
        }

    }

    private void AllowPlayerToMove()
    {
        FPSController.canMove = true;
    }

    public void Pull()
    {

        FPSController.canMove = false;


        Vector3 pullVector = WallRun.isWallRunning ? -Camera.main.transform.forward :
            -FPSController.playerTransform.forward;
        rd.ActivateRagdoll();


        var force = WallRun.isWallRunning ? 4000 : 2000;
        rd.AddForce(pullVector * force);


        rd.AddForce(transform.up * 300);
        //rd.DisableGravity();
        flag = false;

        //Invoke(nameof(CheckForQ), 1f);
        update = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Normal_Enemy>().enabled = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }

/*    public void StopMoving()
    {
        update = false;
    }*/

    private void FixedUpdate()
    {
/*        if(update)
        {
            var dir = FPSController.playerTransform.up *
                Input.GetAxis("Mouse Y") * 20 + FPSController.playerTransform.right
                * Input.GetAxis("Mouse X") * 10;

            rd.AddForce(dir);
        }*/
    }

    

    public void TakeDamage(int amt)
    {
        //Debug.Log("Take Damage");

        health -= amt;

        if (health <= 50 && !IsStunned)
        {
            Stunned();
        }

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(explosion, rd.hipPosition, Quaternion.identity);

        if(update)
        {
            FPSController.playerTransform.
                GetComponentInParent<PlayerHealth>().AddHealth(dropHealth);
        }

        //CameraShake.Shake(0.2f, 2f);
        Destroy(gameObject);
    }

    private void Stunned()
    {
        blinkTimer = blinkDuration;
        IsStunned = true;
        //transform.GetComponent<Normal_Enemy>().runAway = true;
    }

    private void CheckForQ()
    {
        update = true;
    }

    private void OnDestroy()
    {
        FPSController.canMove = true;
    }

    public void PushBack()
    {
        //transform.Translate();
    }

}
