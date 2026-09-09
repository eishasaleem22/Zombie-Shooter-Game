using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera mainCamera;       //Main Camera 
    public ParticleSystem bloodFX;  //particle effect

    [Header("Audio Setup")]
    public AudioSource audioSource; // Player's AudioSource 
    public AudioClip gunShotSound;  //  gunfire.wav file 

    // Raycast Weapon Logic from Screen Center
    public void ShootWeapon()
    {
        // 1. Jab bhi shoot button click ho, audio play hogi
        if (audioSource != null && gunShotSound != null)
        {
            audioSource.PlayOneShot(gunShotSound);
        }

        // Console par print hoga
        Debug.Log("Gun Fired!");

        if (mainCamera == null) mainCamera = Camera.main;

        RaycastHit hit;

        // Ray hamesha camera ke lens ke beech se samne ki taraf niklegi
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Yeh bataye ga ke goli kis cheez se takrayi (chahe wall ho ya ground)
            Debug.Log("Raycast Hit Object: " + hit.collider.name);

            // Check karein ke kya samne aam Zombie hai
            if (hit.collider.CompareTag("Zombie"))
            {
                // 2. Jab Zombie successfully hit ho kar delete hone lagega
                Debug.Log("Zombie Killed!");

                // Requirement 9: Particle System play hoga hit point par
                if (bloodFX != null)
                {
                    ParticleSystem fx = Instantiate(bloodFX, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(fx.gameObject, 2f);
                }

                // Zombie ko delete kar do
                Destroy(hit.collider.gameObject);
            }

            // --- FOR BOSS MONSTER ---
            // Check karein ke kya samne bada Boss Monster hai
            else if (hit.collider.CompareTag("Boss"))
            {
                // Boss Monster par lagi hui script ko dhoondo
                BossMonster boss = hit.collider.GetComponent<BossMonster>();

                if (boss != null)
                {
                    // Boss ko 1 shot ka damage do (Health kam karo)
                    boss.TakeDamage();

                    // Boss ko goli lagne par bhi wahi blood particle play hoga hit point par
                    if (bloodFX != null)
                    {
                        ParticleSystem fx = Instantiate(bloodFX, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(fx.gameObject, 1f);
                    }
                }
            }
        }
    }
}