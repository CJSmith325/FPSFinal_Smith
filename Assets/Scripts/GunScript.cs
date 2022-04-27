using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 28f;

    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public GameObject otherHitEffect;
    public Camera fpsCam;
    private AudioClip gunShot;
    private AudioSource audioSource;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gunShot = GetComponent<AudioClip>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
            PlayShootingSound();
            Shoot();
            
        }
    }

    void PlayShootingSound()
    {

        //If you need to change the volume
        //audioSource.volume = volume;
        //Play the sound once
        audioSource.PlayOneShot(gunShot);



    }

    void Shoot()
    {
        RaycastHit hit;
        muzzleFlash.Play();
        audioSource.Play();
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

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2.5f);
            }

            
            
        }
    }
}
