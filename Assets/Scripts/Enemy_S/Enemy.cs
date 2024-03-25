using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator anim;

    public float detectionRange = 10;

    private NavMeshAgent _enemyAgent;
    public NavMeshAgent EnemyAgent
    {
        get
        {
            if (_enemyAgent == null)
            {
                _enemyAgent = GetComponent<NavMeshAgent>();
            }
            return _enemyAgent;
        }
    }

    protected bool inRange()
    {
        float distance = Vector3.Distance(FPSController.playerTransform.position, transform.position);

        if(distance < detectionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
