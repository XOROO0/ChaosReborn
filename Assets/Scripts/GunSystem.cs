using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public Animator anim;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public ParticleSystem muzzleFlash;
    public GameObject bulletHoleGraphic;
    //public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;
    public float camShakeSterngth = 0.05f;

    public Recoil recoil;
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

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        else
        {
            anim.SetBool("Shooting", false);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (transform.name == "AR")
                anim.Play("Gun_Hit");
            else if (transform.name == "Shotgun")
                anim.Play("Gun_Hit_S");
        }
    }
    private void Shoot()
    {
        recoil.RecoilFire();
        CameraShake.Shake(0.1f, camShakeSterngth);
        readyToShoot = false;

        //Spread4f,
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {


            if(rayHit.collider != null)
            {
                if((whatIsEnemy & (1 << rayHit.collider.gameObject.layer)) != 0) 
                {
                    rayHit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(damage);
                }
            }
        }

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal)).GetComponent<ParticleSystem>().Emit(1);

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
                    if(hit.transform.root.GetComponent<RagdollEnemy>().IsStunned)
                    {
                        hit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(100);
                    }
                }
            }
        }
    }
}