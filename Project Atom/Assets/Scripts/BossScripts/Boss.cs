using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
  protected float bossLife;
  protected float maxBossLife;
  public int bossPhase;
  protected Animator bossAnim;
  protected float secondPhasePercentage;
  protected float thirdPhasePercentage;
  protected float secondPhaseLife;
  protected float thirdPhaseLife;
  protected Image bossBar;
  public string sceneToGo;

  public AudioClip bossMusic;

  public void BossPhaseState(float _bossLife, float _secondPhaseLife, float _thirdPhaseLife)
  {
    if(_bossLife <= _secondPhaseLife && _bossLife > _thirdPhaseLife) {
      bossPhase = 2;
    } else if(_bossLife <= _thirdPhaseLife && _bossLife > 0f) {
      bossPhase = 3;
    } else if(_bossLife <= 0f) {
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
