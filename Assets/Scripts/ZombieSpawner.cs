using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnDelay = 7f;

    [Header("Spawn Distance Settings")]
    public float minSpawnDistance = 40f; // Player se kam az kam itni door zombie paida ho (Safe Zone)
    public float maxSpawnDistance = 60f; // Player se zyada se zyada itni door nikal sake

    [Header("Spawner Limits")]
    public int maxTotalZombies = 7;      // Pooray game mein total sirf itne hi zombies spawn honge

    private Transform playerTransform;
    private int totalSpawnedSoFar = 0;   // Yeh track rakhega ke ab tak total kitne spawn ho chuke hain

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        StartCoroutine(SpawnOnNavMeshRoutine());
    }

    IEnumerator SpawnOnNavMeshRoutine()
    {
        while (true)
        {
            // 1. Agar total spawn hone wale zombies limit tak pohanch gaye hain, toh spawner ko stop kar do
            if (totalSpawnedSoFar >= maxTotalZombies)
            {
                Debug.Log("Total limit of " + maxTotalZombies + " zombies reached! Spawner stopping permanently.");
                yield break; // Isse coroutine hamesha ke liye khatam ho jayegi
            }

            yield return new WaitForSeconds(spawnDelay);

            if (zombiePrefab != null && playerTransform != null)
            {
                // Ek random angle aur distance calculate karo (Donut shape area banega)
                float randomAngle = Random.Range(0f, 360f);
                float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

                // Trigonometry use karke random X aur Z coordinates nikalein
                float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
                float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

                Vector3 randomPosition = new Vector3(spawnX, playerTransform.position.y, spawnZ);

                NavMeshHit navHit;

                // Radius badha kar 5f kiya taake baked NavMesh perfectly dhoond sake
                if (NavMesh.SamplePosition(randomPosition, out navHit, 5f, NavMesh.AllAreas))
                {
                    Instantiate(zombiePrefab, navHit.position, Quaternion.identity);

                    // 2. Ek zombie paida hone par counter ko barha do
                    totalSpawnedSoFar++;

                    Debug.Log("Zombie Spawned! (" + totalSpawnedSoFar + "/" + maxTotalZombies + ")");
                }
            }
        }
    }
}