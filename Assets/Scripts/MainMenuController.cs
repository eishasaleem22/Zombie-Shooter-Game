using UnityEngine;
using UnityEngine.SceneManagement; // Scenes switch karne ke liye yeh line sabse zaroori hai

public class MainMenuController : MonoBehaviour
{
    // Yeh function hamara Start Game button call karega
    public void StartGame()
    {
        Debug.Log("Start Button Clicked! Loading Game Scene...");

        // "GameScene" ki jagah apne asli game scene ka exact naam likhein (spelling same honi chahiye)
        SceneManager.LoadScene("GameScene");
    }
}