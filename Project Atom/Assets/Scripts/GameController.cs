using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    PullController pullController;
    public GameObject Particles;
    public int numberOfParticles = 15;
    public int iPullParticles = 0;
    public List<GameObject> pullParticles = new List<GameObject>();
    void Start()
    {
        pullController = new PullController();
        pullController.CreatePull(pullParticles, Particles, numberOfParticles);
    }


}
