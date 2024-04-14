using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Prefab : MonoBehaviour
{
    public LayerMask notToHit;
    public LayerMask playerMask;
    public float damageRadius = 2f;
    public float maxDamageAmount = 10f;
    public GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        if ((notToHit & (1 << other.gameObject.layer)) != 0)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius, playerMask);

        if (colliders.Length != 0)
        {
            float distance = 
                Vector3.Distance(transform.position, colliders[0].transform.root.position);

            float damageScaler = Mathf.Abs(distance - damageRadius) / damageRadius;

            Player.Instance.transform.parent.GetComponent<PlayerHealth>()
                .DamagePlayer(maxDamageAmount * damageScaler);
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
