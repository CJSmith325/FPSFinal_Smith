using UnityEngine;
using TMPro;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 28f;

    int maxAmmo = 8;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public GameObject otherHitEffect;
    public Camera fpsCam;
    public AudioClip gunShot;
    public AudioSource animSource;
    [Header("Reload")]
    public AudioClip reloadClip;
    public AudioSource reloadSource;
    public TextMeshProUGUI ammoVal;

    private Animation anim;

    private void Start()
    {
        //animSource = GetComponent<AudioSource>();
        //gunShot = GetComponent<AudioClip>();
        anim = GetComponent<Animation>();
        currentAmmo = maxAmmo;
        ammoVal.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(GunReload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.Play();
            PlayShootingSound();
            Shoot();
            
        }

        if (Input.GetButtonDown("R"))
        {
            if (currentAmmo != maxAmmo)
            {
                StartCoroutine(GunReload());
            }
            
        }
    }

    public IEnumerator GunReload()
    {
        isReloading = true;
        reloadSource.PlayOneShot(reloadClip, 2f);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        ammoVal.text = currentAmmo.ToString();
        isReloading = false;
    }

    void PlayShootingSound()
    {

        //If you need to change the volume
        //audioSource.volume = volume;
        //Play the sound once
        animSource.PlayOneShot(gunShot);



    }

    void Shoot()
    {
        RaycastHit hit;
        currentAmmo--;
        ammoVal.text = currentAmmo.ToString();
        muzzleFlash.Play();
        animSource.Play();
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            //Other other = hit.transform.GetComponent<Other>();

            if (hit.transform.gameObject.CompareTag("Other"))
            {
                // environment particle effect here
                GameObject otherimpactGO = Instantiate(otherHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(otherimpactGO, 2.5f);
            }

            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            EnemyAI2 enemy2 = hit.transform.GetComponent<EnemyAI2>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2.5f);
            }

            if (enemy2 != null)
            {
                enemy2.TakeDamage(damage);
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2.5f);
            }

            
            
        }
    }
}
