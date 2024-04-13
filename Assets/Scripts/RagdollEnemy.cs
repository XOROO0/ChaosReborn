using DitzeGames.Effects;
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
    [SerializeField] private Transform shatteredModel;
    public int dropHealth = 20;

    [SerializeField] GameObject explosion;
    [SerializeField] GameObject hitBlood;

    Ragdoll rd;

    string update;

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

        if(update == "PULL")
        {
            if(Vector3.Distance(FPSController.playerTransform.position,
                transform.GetChild(1).position) < 2)
            {

                rd.StopAllForces();
                rd.DisableGravity();
                AllowPlayerToMove();
                GetComponent<ClearLeashPath>().doClearPath = false;
            }
        }
        else if(update == "PUSH")
        {
            if (Vector3.Distance(FPSController.playerTransform.position,
                transform.GetChild(1).position) > 5)
            {

                rd.StopAllForces();
                rd.DisableGravity();
                AllowPlayerToMove();
                GetComponent<ClearLeashPath>().doClearPath = false;
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

        GetComponent<ClearLeashPath>().doClearPath = true;

        var force = WallRun.isWallRunning ? 4000 : 2000;
        rd.AddForce(pullVector * force);


        rd.AddForce(transform.up * 300);
        //rd.DisableGravity();

        //Invoke(nameof(CheckForQ), 1f);
        update = "PULL";
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Normal_Enemy>().enabled = false;
    }

    public void Push()
    {
        Vector3 pushDir = FPSController.playerTransform.forward;

        rd.ActivateRagdoll();

        GetComponent<ClearLeashPath>().doClearPath = true;

        rd.AddForce(pushDir * 1000);
        rd.AddForce(transform.up * 300);

        update = "PUSH";
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Normal_Enemy>().enabled = false;

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }


    public void TakeDamage(int amt, Vector3 hitPoint, Vector3 hitNormal, bool rocket)
    {
        //Debug.Log("Take Damage");

        health -= amt;
        GetComponent<Animator>().Play("Zombie Reaction Hit");

        CameraEffects.ShakeOnce(0.2f);

        if (health <= 0)
            Die(rocket);

        Instantiate(shatteredModel.GetChild(Random.Range(0, 23)), hitPoint, Quaternion.identity).GetComponent<Rigidbody>().
            AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);

        Instantiate(hitBlood, hitPoint, Quaternion.LookRotation(hitNormal, transform.up));

        if (health <= 50 && !IsStunned)
        {
            Stunned();
        }

    }

    private void Die(bool rocket)
    {
        if(rocket)
        {
            Instantiate(shatteredModel, transform.GetChild(1).position, transform.rotation).
                GetComponent<ShatteredEnemy>().RocketBlast();
        }
        else
        {
            Instantiate(shatteredModel, transform.GetChild(1).position, transform.rotation).
                GetComponent<ShatteredEnemy>().RegularBlast();
        }


        Instantiate(explosion, rd.hipPosition, Quaternion.LookRotation(transform.forward, transform.up));
        AudioManager.instance.Play("Splash");

        if(update == "PULL" || update == "PUSH")
        {
            FPSController.playerTransform.
                GetComponentInParent<PlayerHealth>().AddHealth(dropHealth);
        }

        CameraEffects.ShakeOnce(0.3f, 15);
        Destroy(gameObject);
    }

    private void Stunned()
    {
        blinkTimer = blinkDuration;
        IsStunned = true;
        //transform.GetComponent<Normal_Enemy>().runAway = true;
    }


    private void OnDestroy()
    {
        FPSController.canMove = true;
    }


}
