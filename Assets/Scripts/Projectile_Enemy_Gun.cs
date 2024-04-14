using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Projectile_Enemy_Gun : MonoBehaviour
{
    public Ranged_Enemy ranged_Enemy;
    public GameObject projectilePrefab;
    public Transform spawnProjectilePos;
    [Range(20.0f, 75.0f)] public float LaunchAngle;



    void Shoot(Vector3 target)
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnProjectilePos.position, transform.rotation);
        Rigidbody rigid = projectile.GetComponent<Rigidbody>();

        Vector3 projectileXZPos = new Vector3(projectile.transform.position.x, projectile.transform.position.y, projectile.transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, projectile.transform.position.y, target.z);
        projectile.transform.LookAt(targetXZPos);

        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.y - projectile.transform.position.y;

        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = projectile.transform.TransformDirection(localVelocity);

        rigid.velocity = globalVelocity;
    }

    public void Throw()
    {
        if(ranged_Enemy.canShoot)
        {
            Shoot(Player.Position);
        }
    }
}
