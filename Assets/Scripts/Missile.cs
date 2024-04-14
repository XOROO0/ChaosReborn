using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float missileSpeed = 5f;

    private Vector3 hitPoint;
    // Update is called once per frame
    void Update()
    {
        if (transform.position == hitPoint)
        {
            Camera.main.transform.GetChild(2).GetComponent<GunSystem>().Explode();
            AudioManager.instance.Play("Missile_Explosion");
            Destroy(gameObject);
        }


        transform.position = 
            Vector3.MoveTowards(transform.position, hitPoint, missileSpeed * Time.deltaTime);
    }

    public void MissileSetUp(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
        transform.LookAt(hitPoint);
    }
}
