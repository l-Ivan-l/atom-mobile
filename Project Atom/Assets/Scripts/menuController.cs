using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    private void Start()
    {
        Singleton.Instance.PlayerLife = 450;
        Singleton.Instance.ActualBullet = 0;
        Singleton.Instance.ActualWeapon = 0;
    }
    public void Play()
    {
        SceneManager.LoadScene("DemoV1.1");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
