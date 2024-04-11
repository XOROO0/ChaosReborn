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
                GetComponent<ClearLeashPath>().doClearPath = false;
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

        GetComponent<ClearLeashPath>().doClearPath = true;

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


    public void TakeDamage(int amt, Vector3 hitPoint, Vector3 hitNormal)
    {
        //Debug.Log("Take Damage");

        health -= amt;
        GetComponent<Animator>().Play("Zombie Reaction Hit");

        CameraEffects.ShakeOnce(0.2f);

        if (health <= 0)
            Die();

        Instantiate(shatteredModel.GetChild(Random.Range(0, 23)), hitPoint, Quaternion.identity).GetComponent<Rigidbody>().
            AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);

        Instantiate(hitBlood, hitPoint, Quaternion.LookRotation(hitNormal, transform.up));

        if (health <= 50 && !IsStunned)
        {
            Stunned();
        }

    }

    private void Die()
    {
        Instantiate(shatteredModel, transform.position, transform.rotation);
        Instantiate(explosion, rd.hipPosition, Quaternion.LookRotation(transform.forward, transform.up));
        AudioManager.instance.Play("Splash");

        if(update)
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

    private void CheckForQ()
    {
        update = true;
    }

    private void OnDestroy()
    {
        FPSController.canMove = true;
    }


}
