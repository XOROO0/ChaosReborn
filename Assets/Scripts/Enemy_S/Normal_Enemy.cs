
using UnityEngine;

public class Normal_Enemy : Enemy
{
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool runAway = false;

    private void Update()
    {
        if (runAway)
        {
            EnemyAgent.enabled = true;
            anim.SetBool("RunAway", true);
            RunAway();
        }
        else
        {
            if (inRange())
            {
                EnemyAgent.enabled = true;
                EnemyAgent.destination = FPSController.playerTransform.position;

                anim.SetBool("Walking", !EnemyAgent.isStopped);
            }
            else
            {
                EnemyAgent.enabled = false;
                anim.SetBool("Walking", false);
            }
        }
    }

    private void RunAway()
    {

        var dir = (transform.position - FPSController.playerTransform.position).normalized;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir, transform.up);

        EnemyAgent.Move(dir * 8f * Time.deltaTime);
    }
}
