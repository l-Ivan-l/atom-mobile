using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private void Awake()
    {
        //Singleton.Instance.ActualBullet = 0;
        //Singleton.Instance.ActualWeapon = 0;
    }
    public void RestartGame(string _sceneName)
    {
      SceneManager.LoadScene(_sceneName);
    }

}//class
