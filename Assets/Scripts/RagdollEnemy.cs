using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    }

    public void Pull()
    {
        Vector3 pullVector = -FPSController.playerTransform.forward;

        rd.ActivateRagdoll();


        rd.AddForce(pullVector *
            Vector3.Distance(FPSController.playerTransform.position, transform.position) * 4);  
        rd.AddForce(transform.up * 40);

        update = true;
    }

    public void StopMoving()
    {
        update = false;
    }

    private void FixedUpdate()
    {
        if(update)
        {
            var dir = FPSController.playerTransform.up *
                Input.GetAxis("Mouse Y") * 20 + FPSController.playerTransform.right
                * Input.GetAxis("Mouse X") * 10;

            rd.AddForce(dir);
        }
    }

    public void TakeDamage(int amt)
    {
        //Debug.Log("Take Damage");

        health -= amt;

        if (health <= 50)
        {
            Stunned();
        }

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(explosion, rd.hipPosition, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Stunned()
    {
        blinkTimer = blinkDuration;
        IsStunned = true;
        transform.GetComponent<Normal_Enemy>().runAway = true;
    }

}
