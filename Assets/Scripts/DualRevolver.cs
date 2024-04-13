using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualRevolver : MonoBehaviour
{
    [SerializeField] private GunSystem gun1;
    [SerializeField] private GunSystem gun2;

    public GunSystem gunToShoot;

    private void Start()
    {
        gun1.allowToShoot = true;
        gun2.allowToShoot = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwitchGun();
        }
    }

    public void SwitchGun()
    {
        if(gun1.allowToShoot)
        {
            gun1.allowToShoot = false;
            gun2.allowToShoot = true;
        }
        else if (gun2.allowToShoot)
        {
            gun1.allowToShoot = true;
            gun2.allowToShoot = false;
        }
    }

    public void ReloadRevolvers()
    {
        gun1.RevolverReload();
        gun2.RevolverReload();
    }
}
