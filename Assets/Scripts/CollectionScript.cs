using UnityEngine;
using UnityEngine.UI; // Legacy UI Text use karne ke liye zaroori hai

public class CollectionScript : MonoBehaviour
{
    [Header("UI Reference")]
    public Text diamondText; // Unity Inspector mein aapka Legacy Text yahan aayega

    [Header("Audio Setup")]
    public AudioSource audioSource; // Inspector mein Player ka AudioSource yahan drag hoga
    public AudioClip coinCollectSound; // Diamond collect hone ki sound (.mp3 ya .wav) yahan drag hogi

    [Header("Level Lock Setup (New)")]
    public GameObject invisibleWall; // New: Inspector mein aapka Invisible Wall wala cube yahan aayega

    private int diamondsCollected = 0;
    private int totalDiamonds = 5;

    void Start()
    {
        // Game shuru hote hi screen par 0/5 show karne ke liye
        UpdateUI();

        // Game ke shuru mein ensure karna ke invisible wall active (deewar khadi) ho
        if (invisibleWall != null)
        {
            invisibleWall.SetActive(true);
        }
    }

    // Jab bhi Player kisi Diamond (Trigger) ke andar se guzre ga
    private void OnTriggerEnter(Collider other)
    {
        // Check karo ke jis cheez se takraye hain, kya uska tag "Diamond" hai?
        if (other.CompareTag("Diamond"))
        {
            diamondsCollected++; // Score ko 1 barha do
            UpdateUI();          // Screen par text ko badlo

            // --- AUDIO LOGIC ---
            // Diamond collect hone par sound play hogi
            if (audioSource != null && coinCollectSound != null)
            {
                audioSource.PlayOneShot(coinCollectSound);
            }

            Destroy(other.gameObject); // Diamond ko map se delete kar do
            Debug.Log("Diamond Collected! Total: " + diamondsCollected);

            // --- NEW INVISIBLE WALL DISABLE LOGIC ---
            // Check karo ke kya saare (5) diamonds collect ho chuke hain?
            if (diamondsCollected >= totalDiamonds)
            {
                if (invisibleWall != null)
                {
                    invisibleWall.SetActive(false); // Invisible wall ko gayab kar do!
                    Debug.Log("Path Unlocked! Invisible Wall removed. Go hunt the Boss!");
                }
                else
                {
                    Debug.LogError("Bhai, CollectionScript par Invisible Wall ka object assign nahi kiya hua!");
                }
            }
        }
    }

    // UI Text ko refresh karne ka function
    void UpdateUI()
    {
        if (diamondText != null)
        {
            // Text ko "0/5", "1/5" wagera mein convert karega
            diamondText.text = diamondsCollected + "/" + totalDiamonds;
        }
    }
}