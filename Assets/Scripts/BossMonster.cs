using UnityEngine;
using UnityEngine.UI; // Legacy UI Elements ke liye zaroori hai
using UnityEngine.SceneManagement; // Scenes control karne ke liye yeh sabse zaroori hai
using System.Collections; // ZAROORI LINE: IEnumerator/Coroutines chalane ke liye yeh chahiye!

public class BossMonster : MonoBehaviour
{
    public int bossHealth = 5; // 5 shots requirement

    [Header("UI Victory Setup")]
    public Text victoryText;   // Inspector mein hamara VictoryText (Legacy) yahan aayega

    [Header("New Action Buttons")]
    public GameObject restartButton;   // New: RestartButton yahan aayega
    public GameObject mainMenuButton;  // New: MainMenuButton yahan aayega

    [Header("Gameplay UI Components to Disable")]
    public GameObject shootButton;
    public GameObject MovementJoystick;
    public GameObject CameraJoystick;
    public Text DiamondText;
    public GameObject Crosshair;
    public GameObject Crosshair1;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        // Fix: Agar Animator child object par bhi hoga, toh yeh use dhoond lega
        anim = GetComponentInChildren<Animator>();

        if (anim == null)
        {
            Debug.LogError("Boss Animator Component not found!");
        }

        // Game ke shuru mein ensure karna ke victory text band (disabled) ho
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }

        // --- NEW: Shuru mein dono naye buttons ko bhi chupa (disable) do ---
        if (restartButton != null) restartButton.SetActive(false);
        if (mainMenuButton != null) mainMenuButton.SetActive(false);
    }

    // Yeh function hamari shooting script call karegi jab goli lagegi
    public void TakeDamage()
    {
        if (isDead) return; // Agar pehle se mar chuka hai toh kuch na karo

        bossHealth--;
        Debug.Log("Boss Hit! Remaining Health: " + bossHealth);

        if (bossHealth <= 0)
        {
            // NEW CHANGED LOGIC: Ab hum direct function nahi, balki Coroutine start karenge timer ke liye
            StartCoroutine(BossDeathRoutine());
        }
    }

    // --- NEW: IEnumerator Coroutine for 2 Seconds Delay ---
    IEnumerator BossDeathRoutine()
    {
        isDead = true;

        // 1. Animator ko trigger bhejo taake death animation chale aur woh neeche gir jaye
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        Debug.Log("Boss Monster is dying... Waiting for 2 seconds delay.");

        // 2. TIMING DELAY: Yahan code pure 2 seconds ke liye ruk jaye ga
        yield return new WaitForSeconds(2.0f);

        // 3. Game Win Logic - 2 seconds ke delay ke baad yeh UI active/inactive hogi
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(true);

            // --- NEW: Jeetne par dono buttons ko screen par active (show) kar do ---
            if (restartButton != null) restartButton.SetActive(true);
            if (mainMenuButton != null) mainMenuButton.SetActive(true);

            // Gameplay components ko turn off karna
            if (shootButton != null) shootButton.SetActive(false);
            if (MovementJoystick != null) MovementJoystick.SetActive(false);
            if (CameraJoystick != null) CameraJoystick.SetActive(false);
            if (DiamondText != null) DiamondText.gameObject.SetActive(false);
            if (Crosshair != null) Crosshair.SetActive(false);
            if (Crosshair1 != null) Crosshair1.SetActive(false);
        }

        Debug.Log(" VICTORY! Boss Monster Defeated. GAME WON!");
    }

    // --- NEW ACTION FUNCTIONS FOR BUTTONS ---

    // Yeh function chalega jab player Restart Game button dabaye ga
    public void RestartGame()
    {
        Debug.Log("Restarting Level...");
        // Yeh line automatic current active scene ko dobara fresh reload kar degi
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Yeh function chalega jab player Main Menu button dabaye ga
    public void BackToMainMenu()
    {
        Debug.Log("Going back to Main Menu...");
        // Yahan apne Main Menu scene ka exact naam likhein (Jaise humne 'MainMenu' banaya tha)
        SceneManager.LoadScene("MainMenu");
    }
}