#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Ranged_Enemy : Enemy
{
    private Animator anim;

    public bool canShoot => IsInDonut() && hasLOS();
    public float minRange;
    public float maxRange;

    public int damage = 10;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsInDonut() && inRange() && hasLOS() )
        {
            EnemyAgent.enabled = true;
            EnemyAgent.destination = GetDestination(Player.Position);
        }

        if(hasLOS())
        {
            transform.LookAt(new Vector3(Player.Position.x, transform.position.y, Player.Position.z));
        }

        if(!inRange())
        {
            EnemyAgent.enabled = false;
        }

        anim.SetBool("Moving", !EnemyAgent.isStopped);
        anim.SetBool("Attack", canShoot);
    }

    Vector3 GetDestination(Vector3 playerPos)
    {
        Vector3 direction = transform.position - playerPos;
        direction = direction.normalized;
        Vector3 target = direction * (((maxRange - minRange) / 2) + minRange) + playerPos;

        return target;
    }

    bool IsInDonut()
    {
        float distance = Vector3.Distance(Player.Position, transform.position);

        if (distance > minRange && distance < maxRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = Color.red;
        Handles.CircleHandleCap(-1, Player.Position, Quaternion.LookRotation(Vector3.up, -Vector3.forward), minRange, EventType.Repaint);
        Handles.color = Color.green;
        Handles.CircleHandleCap(-1, Player.Position, Quaternion.LookRotation(Vector3.up, -Vector3.forward), maxRange, EventType.Repaint);
    } 

#endif
}
