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


    [Header("UI_Anim")]
    public GameObject Menu;
    public Animator Panel_Anim;
    public Animator Menu_Anim;
    public string animationStateName;



    private bool isPaused = false;
    public void TogglePause()
    {
        //Panel_Anim.SetBool("Open", 
        Panel_Anim.SetBool("Open", false);
       // Menu_Anim.SetBool("MenuWhole_Entry", false);
        StartCoroutine(Wait());
    

    }
    public void Resume()
    {


        Menu_Anim.SetBool("MenuWhole_Entry", false);
        StartCoroutine(Wait_For_Anim());
        Time.timeScale = 1; // Resume the game
    }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // Resume the game
    }
    bool IsAnimationPlaying()
    {
        if (Menu_Anim == null)
        {
            Debug.LogWarning("Animator component is not set.");
            return false;
        }

        var currentState = Menu_Anim.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(animationStateName);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        Time.timeScale = 0; // Pause the game}
    }

    IEnumerator Wait_For_Anim()
    {
        yield return new WaitForSeconds(1f);
        Menu.SetActive(false);
      //  Time.timeScale = 0; // Pause the game}
    }
}
