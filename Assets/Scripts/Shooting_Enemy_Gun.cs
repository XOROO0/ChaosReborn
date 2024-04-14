using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Enemy : MonoBehaviour
{
    public Ranged_Enemy ranged_Enemy;
    public LayerMask playerMask;
    public ParticleSystem muzzleFlash;
    private RaycastHit hit;
    public int fireRate;
    private float currentTime;
    public int magSize;
    public float idleErrorRadius;
    public float movingErrorRadius;
    public float reloadTime;
    private int currentMag;
    private Coroutine reloadRoutine;
    private float errorRadius;

    private void Start()
    {
        currentMag = magSize;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (FPSController.IsMoving)
            errorRadius = movingErrorRadius;
        else
        errorRadius = idleErrorRadius;

        if (ranged_Enemy.canShoot && (currentTime > 1f/fireRate))
        {
            if(currentMag > 0 )
            {
                currentMag--;
                Shoot(Player.Position);
                currentTime = 0;
            }
            else
            {
                if(reloadRoutine == null)
                {
                    reloadRoutine = StartCoroutine(Reload());
                }
            }
        }
    }
    
    IEnumerator Reload()
    {
        Debug.Log("Reload!");
        yield return new WaitForSeconds(reloadTime);
        currentMag = magSize;
        reloadRoutine = null;
    }

    void Shoot(Vector3 target)
    {
        Vector3 randomPoint = (Random.insideUnitSphere * errorRadius) + target;
        muzzleFlash.Emit(1);

        if (Physics.Raycast(transform.position, (randomPoint - transform.position).normalized, out hit, Mathf.Infinity, playerMask))
        {
            if (hit.collider.gameObject == Player.Instance.gameObject)
            {
                Debug.Log("Hit");
                //Deal Damage to player

                Player.Instance.transform.parent.GetComponent<PlayerHealth>().DamagePlayer(ranged_Enemy.damage);
            }
            else
            {
                Debug.Log("Missed");
            }
        }
        else
        {
            Debug.Log("Where did you shoot bruh?");
        }
    }

    private void OnDrawGizmos()
    {
        if (hit.collider != null)
        {
            if (hit.collider.gameObject == Player.Instance.gameObject)
            {
                Debug.DrawRay(transform.position, (hit.point - transform.position).normalized * Vector3.Distance(transform.position, hit.point), Color.green);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(hit.point, 0.25f);
            }
            else
            {
                Debug.DrawRay(transform.position, (hit.point - transform.position).normalized * Vector3.Distance(transform.position, hit.point), Color.yellow);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hit.point, 0.25f);
            }

        }
        else
        {
            Debug.DrawRay(transform.position, (hit.point - transform.position).normalized * Vector3.Distance(transform.position, hit.point), Color.red);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point, 0.25f);
        }
    }
}
