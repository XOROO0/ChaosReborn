using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    [SerializeField]
    private int Health = 50;

    [SerializeField] GameObject explosion;
    [SerializeField] AnimationCurve speedCurve;

    Animator anim;

    //private int currentHealth;

    private Rigidbody rb;

    bool first = true;

    bool isFlying = false;

    [SerializeField]
    private float _timeScale = 1;
    public float timeScale
    {
        get { return _timeScale; }
        set
        {
            if (!first)
            {
                rb.mass *= timeScale;
                rb.velocity /= timeScale;
                rb.angularVelocity /= timeScale;
            }
            first = false;

            _timeScale = Mathf.Abs(value);

            rb.mass /= timeScale;
            rb.velocity *= timeScale;
            rb.angularVelocity *= timeScale;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timeScale = _timeScale;

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(isFlying)
        {
            if(WallRun.isWallRunning)
            {
                if (Vector3.Distance(transform.position, FPSController.currentPos) < 12)
                {
                    SlowMo();
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, FPSController.currentPos) < 6)
                {
                    SlowMo();
                }
            }

        }

        anim.SetBool("Flying", isFlying);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime * timeScale;
        rb.velocity += Physics.gravity / rb.mass * dt;
    }

    public void TakeDamage(int amt)
    {
        Health -= amt;

        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Pull()
    {
        FPSController.canMove = false;

        isFlying = true;
        Vector3 pullVector = (FPSController.currentPos - transform.position).normalized;

        GetComponent<Rigidbody>().AddForce(pullVector * 1200); 
        GetComponent<Rigidbody>().AddForce(transform.up * 300); 

        //Invoke(nameof(SlowMo), 0.2f);
    }

    private void SlowMo()
    {
        isFlying = true;
        FPSController.canMove = true;
        if (WallRun.isWallRunning)
        {
            TimeManager.Instance.SlowMo(0.35f, 0.5f);
        }

        rb.useGravity = false;
        timeScale = 0.15f;

        Invoke(nameof(ResetTime), 2f);
    }

    private void ResetTime()
    {
        isFlying = false;
        timeScale = 1f;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (FPSController.isSliding)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 500);
                Invoke(nameof(SlowMo), 0.2f);
            }
        }
    }
}