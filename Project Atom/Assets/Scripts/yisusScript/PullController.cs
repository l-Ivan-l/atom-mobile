using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullController : MonoBehaviour
{
    public GameObject Particles;
    public float numberOfParticles;
    public int iPullParticles = 0;
    public List<GameObject> pullParticles;
    void Start()
    {
        pullParticles = new List<GameObject>();
        CreateParticlePull();
    }

    void CreateParticlePull()
    {
        for (int j = 0; j < numberOfParticles; j++)
        {
            GameObject particle = Instantiate(Particles);
            pullParticles.Add(particle);
        }
    }
}
