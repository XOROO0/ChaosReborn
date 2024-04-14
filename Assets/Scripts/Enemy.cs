using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float rayCastPlayerYOffset = 0f;
    public float rayCastAiYOffset = 0f;
    public LayerMask playerMask;
    public float detectionRange = 25;

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
        float distance = Vector3.Distance(Player.Position, transform.position);

        if(distance < detectionRange)
        {
            return true;
        }

        return false;
    }

    protected bool hasLOS()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * rayCastAiYOffset), ((Player.Position + (Vector3.up * rayCastPlayerYOffset)) - transform.position).normalized, out hit, Mathf.Infinity, playerMask))
        {
            if(hit.collider.gameObject == Player.Instance.gameObject)
            {
                Debug.DrawRay(transform.position + (Vector3.up * rayCastAiYOffset), ((Player.Position + (Vector3.up * rayCastPlayerYOffset)) - transform.position).normalized * hit.distance, Color.yellow);
                return true;
            }    
        }
        return false;
    }


#if UNITY_EDITOR

    protected virtual void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.CircleHandleCap(-1, Player.Position, Quaternion.LookRotation(Vector3.up, -Vector3.forward), detectionRange, EventType.Repaint);
    }

#endif
}
