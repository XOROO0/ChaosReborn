using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquippingScript : MonoBehaviour
{

    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;

    public TextMeshProUGUI lowAmmoText;

    public static Transform equipped;

    // Start is called before the first frame update
    void Start()
    {
        Equip2();
        equipped = Slot2.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Equip1();
            equipped = Slot1.transform;
        }

        if (Input.GetKeyDown("2"))
        {
            Equip2();
            equipped = Slot2.transform;
        }

        if (Input.GetKeyDown("3"))
        {
            Equip3();
            equipped = Slot3.transform;
        }

        if (Input.GetKeyDown("4"))
        {
            Equip4();
            equipped = Slot4.transform;
        }

        CheckAmmo();
    }

    private void CheckAmmo()
    {
        GunSystem gun = equipped.GetComponent<GunSystem>();

        if(gun == null)
        {
            gun = equipped.GetComponentInChildren<GunSystem>();
        }

        if (gun == null)
            return;


        if ((float)gun.bulletsLeft / (float)gun.magazineSize <= 0.3f)
        {
            lowAmmoText.gameObject.SetActive(true);
        }
        else
        {
            lowAmmoText.gameObject.SetActive(false);
        }
    }

    void Equip1()
    {
        Slot1.SetActive(true);
        Slot2.SetActive(false);
        Slot3.SetActive(false);
        Slot4.SetActive(false);
    }

    void Equip2()
    {
        Slot1.SetActive(false);
        Slot2.SetActive(true);
        Slot3.SetActive(false);
        Slot4.SetActive(false);
    }

    void Equip3()
    {
        Slot1.SetActive(false);
        Slot2.SetActive(false);
        Slot3.SetActive(true);
        Slot4.SetActive(false);
    }

    void Equip4()
    {
        Slot1.SetActive(false);
        Slot2.SetActive(false);
        Slot3.SetActive(false);
        Slot4.SetActive(true);
    }
}
