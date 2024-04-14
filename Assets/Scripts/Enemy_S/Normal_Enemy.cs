
using UnityEngine;

public class Normal_Enemy : Enemy
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool runAway = false;

    private void Update()
    {
        if (runAway)
        {

            anim.SetBool("RunAway", true);
            RunAway();
        }
        else
        {
            if(Vector3.Distance(FPSController.playerTransform.position, transform.position)
                <= EnemyAgent.stoppingDistance)
            {
                anim.Play("Zombie Attack");
            }

            EnemyAgent.destination = FPSController.playerTransform.position;

            anim.SetBool("Walking", !EnemyAgent.isStopped);
        }
    }

    private void RunAway()
    {

        var dir = -transform.forward;
        dir.y = 0;

        //transform.rotation = Quaternion.LookRotation(dir, transform.up);

        EnemyAgent.Move(dir * 8f * Time.deltaTime);
    }

    public void ZombieSlash()
    {
        if (Vector3.Distance(FPSController.playerTransform.position, transform.position)
    <= EnemyAgent.stoppingDistance)
        {
            FPSController.playerTransform.GetComponentInParent<PlayerHealth>().DamagePlayer(10);
        }
    }
}
