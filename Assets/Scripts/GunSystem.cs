using UnityEngine;
using TMPro;
using DitzeGames.Effects;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GunSystem : MonoBehaviour
{
    public Animator anim;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public int bulletsAdd;
    public float coolDownForSmash = 10;
    public Image smashImage;
    public float rocketLauncherRadius = 2f;
    public GameObject explosionEffect;
    public GameObject missile;

    //bools
    public bool shooting;
    bool  readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public LayerMask playerMask;

    //Graphics
    public ParticleSystem muzzleFlash;
    public GameObject bulletHoleGraphic;
    public TextMeshProUGUI text;

    // Camera Shake
    public Vector3 Amount = new Vector3(1f, 1f, 0);
    public float Duration = 1;
    public float Speed = 10;
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public bool DeltaMovement = true;
    public bool allowToShoot = true;

    private float timer;

    public Recoil recoil;

    Vector3 hitPoint;
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();

        anim.SetBool("Moving", FPSController.IsMoving);
        anim.SetBool("Sliding", FPSController.isSliding);


        timer += Time.deltaTime;
        smashImage.fillAmount = Mathf.Clamp(timer / coolDownForSmash, 0, 1);

        if(timer >= coolDownForSmash)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (transform.name == "AR")
                    anim.Play("Gun_Hit");
                else if (transform.name == "Shotgun")
                    anim.Play("Gun_Hit_S");
                else if (transform.name == "RocketLauncher")
                    anim.Play("Gun_Hit_S");
                else if (transform.name == "Reaper" || transform.name == "Reaper (1)")
                    anim.Play("Gun_Hit_S");
            }
        }

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    public void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && allowToShoot)
        {
                bulletsShot = bulletsPerTap;
                Shoot();

        }
        else
        {
            anim.SetBool("Shooting", false);
        }
    }
    private void Shoot()
    {

        recoil.RecoilFire();
        /*CameraShake.Shake(0.1f, camShakeSterngth);*/
        CameraEffects.ShakeOnce(Duration, Speed, Amount, Camera.main, DeltaMovement, Curve);

        if (transform.name == "Shotgun")
            Camera.main.transform.GetComponent<Animator>().Play("ShotGunShoot_Cam");
        else if (transform.name == "RocketLauncher")
            Camera.main.transform.GetComponent<Animator>().Play("RocketLauncherShoot_Cam");
        else if (transform.name == "Reaper" || transform.name == "Reaper (1)")
            Camera.main.transform.GetComponent<Animator>().Play("RevolverShoot_Cam", -1, 0f);

        if (transform.name == "AR")
            AudioManager.instance.Play("AR_Shot");
        else if (transform.name == "Shotgun")
            AudioManager.instance.Play("Shotgun_Shot");
        else if (transform.name == "Reaper" || transform.name == "Reaper (1)")
            AudioManager.instance.Play("Revolver_Shot");
        else if (transform.name == "RocketLauncher")
            AudioManager.instance.Play("Missile_Launch");

            readyToShoot = false;

        //Spread4f,
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, ~playerMask))
        {

            if(rayHit.collider != null)
            {
                hitPoint = rayHit.point;
                if(transform.name == "RocketLauncher")
                {
                    FireRocket();
                }
                else
                {

                    if ((whatIsEnemy & (1 << rayHit.collider.gameObject.layer)) != 0)
                    {
                        rayHit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(damage, rayHit.point, rayHit.normal, false);
                    }
                }

                //Graphics
                Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal)).GetComponent<ParticleSystem>().Emit(1);
            }
        }



        muzzleFlash.Emit(1);

        anim.SetBool("Shooting", true);



        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void GunSmash()
    {
        Vector3 direction = fpsCam.transform.forward;

        RaycastHit hit;
        //RayCast
        if (Physics.SphereCast(fpsCam.transform.position, 0.3f, direction, out hit, 2))
        {
            if (hit.collider != null)
            {
                if ((whatIsEnemy & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    hit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(100, hit.point, hit.normal, false);

                    if(GetComponentInParent<DualRevolver>() != null)
                    {
                        GetComponentInParent<DualRevolver>().ReloadRevolvers();
                    }
                    else
                    {
                        bulletsLeft = Mathf.Clamp(bulletsLeft + bulletsAdd, 0, magazineSize);
                    }

                    timer = 0f;
                }
            }
        }
    }

    private void FireRocket()
    {
        Instantiate(missile, attackPoint.position, Quaternion.identity).
            GetComponent<Missile>().MissileSetUp(hitPoint);
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(hitPoint, rocketLauncherRadius, whatIsEnemy);
        List<RagdollEnemy> enemies = new List<RagdollEnemy>();

        foreach (Collider collider in colliders)
        {
            enemies.Add(collider.transform.root.GetComponent<RagdollEnemy>());
            enemies = enemies.Distinct().ToList();
        }

        foreach (RagdollEnemy enemy in enemies)
        {
            enemy.TakeDamage(100, hitPoint, Vector3.zero, true);
        }

        Instantiate(explosionEffect, hitPoint, Quaternion.identity);
        CameraEffects.ShakeOnce(0.5f, 10, new Vector3(5, 5, 5));
    }

    public void RevolverReload()
    {
        bulletsLeft = Mathf.Clamp(bulletsLeft + bulletsAdd, 0, magazineSize);
    }
}