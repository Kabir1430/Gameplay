using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Script")]
    public AI AI_Script;
    public RagdollEnabler Player;
    [Header("ArrayData")]
    public Transform[] Store;

    public Transform[] Player_WayPoitn_Store;


    public GameObject pauseMenu; // Reference to the pause menu UI
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;
    public void TogglePause()
    {
            Time.timeScale = 0; // Pause the game
            
    }
    public void Resume()
    {
            Time.timeScale = 1; // Resume the game
     }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // Resume the game
    }
}
