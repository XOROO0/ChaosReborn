using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnemy : MonoBehaviour
{
    [SerializeField]
    private int health = 200;

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
                Invoke(nameof(AllowPlayerToMove), 2f);
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

        Vector3 pullVector = -MoveCamera.CamHolder.forward;
        rd.ActivateRagdoll();


        rd.AddForce(pullVector *
           Vector3.Distance(FPSController.playerTransform.position, transform.position) * 10);

        rd.AddForce(transform.up * 20);
        flag = false;

        FPSController.playerTransform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        FPSController.canMove = false;
        //Invoke(nameof(CheckForQ), 1f);
        update = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Normal_Enemy>().enabled = false;
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
        CameraShake.Shake(0.2f, 2f);
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

}
