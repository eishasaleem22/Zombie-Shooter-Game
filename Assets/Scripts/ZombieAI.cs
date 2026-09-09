using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    // Humne isko private kar diya hai taake Inspector mein drag hi na karna pade
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Code khud hi scene mein se "Player" tag wale object ko dhoond lega
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene! Make sure the Player has the 'Player' tag.");
        }
    }

    void Update()
    {
        // Safety check: Agar player mil gaya hai, sirf tabhi uski taraf bhago
        if (player != null && agent != null)
        {
            // Zombie ko player ki position ki taraf bhejta hai
            agent.SetDestination(player.position);
        }
    }
}


