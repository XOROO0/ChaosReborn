using UnityEngine;
using TMPro;
using DitzeGames.Effects;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    public Animator anim;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public int bulletsAdd;
    public float coolDownForSmash = 10;
    public Image smashImage;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public LayerMask notPlayerMask;

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

    private float timer;

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
            }
        }

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
    }
    private void Shoot()
    {
        recoil.RecoilFire();
        /*CameraShake.Shake(0.1f, camShakeSterngth);*/
        CameraEffects.ShakeOnce(Duration, Speed, Amount, Camera.main, DeltaMovement, Curve);

        if (transform.name == "AR")
            AudioManager.instance.Play("AR_Shot");
        else if (transform.name == "Shotgun")
            AudioManager.instance.Play("Shotgun_Shot");

        readyToShoot = false;

        //Spread4f,
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, notPlayerMask))
        {


            if(rayHit.collider != null)
            {
                if((whatIsEnemy & (1 << rayHit.collider.gameObject.layer)) != 0) 
                {
                    rayHit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(damage);
                    rayHit.transform.root.GetComponent<RagdollEnemy>().PushBack();
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
                    hit.transform.root.GetComponent<RagdollEnemy>().TakeDamage(100);
                    bulletsLeft = Mathf.Clamp(bulletsLeft + bulletsAdd, 0, magazineSize);
                    timer = 0f;
                }
            }
        }
    }
}