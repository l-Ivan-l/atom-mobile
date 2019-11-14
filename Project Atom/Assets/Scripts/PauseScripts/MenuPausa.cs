using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {

    }

    public void Resume(){
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false;
    }

    public void Pause (){
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0f;
      GameIsPaused = true;
    }

    public void LoadMenu(string nombredeescena){
      Debug.Log("Loading menu...");
      Time.timeScale = 1f;
      SceneManager.LoadScene(0);
    }

    public void QuitGame(){
      Debug.Log("Loading exit...");
      Application.Quit();
    }

}
