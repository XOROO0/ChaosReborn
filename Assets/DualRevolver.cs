using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualRevolver : MonoBehaviour
{
    [SerializeField] private GunSystem gun1;
    [SerializeField] private GunSystem gun2;


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(gun1.allowToShoot)
            {
                gun1.allowToShoot = false;
                gun2.allowToShoot = true;
            }
            else if(gun2.allowToShoot)
            {
                gun1.allowToShoot = true;
                gun2.allowToShoot = false;
            }
        }
    }
}
