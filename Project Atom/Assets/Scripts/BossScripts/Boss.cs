using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
  protected float bossLife;
  protected float maxBossLife;
  protected int bossPhase;
  protected Animator bossAnim;
  protected float secondPhasePercentage;
  protected float thirdPhasePercentage;
  protected float secondPhaseLife;
  protected float thirdPhaseLife;
  protected Image bossBar;
  public string sceneToGo;

  public AudioClip bossMusic;

  protected void BossPhaseState()
  {
    if(bossLife <= secondPhaseLife && bossLife > thirdPhaseLife) {
      bossPhase = 2;
    } else if(bossLife <= thirdPhaseLife && bossLife > 0f) {
      bossPhase = 3;
    } else if(bossLife <= 0f) {
      bossPhase = 4;
      StartCoroutine(WinLevel(3f));
    }
  }

  IEnumerator WinLevel(float _timer)
  {
    yield return new WaitForSeconds(_timer);
    SceneManager.LoadScene(sceneToGo);
  }

}//class
