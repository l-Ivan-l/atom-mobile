using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawners;
    private List<GameObject> spawnPoints = new List<GameObject>();
    //public Vector3 spawnSize;
    public int minEnemies;
    public int maxEnemies;
    //private Vector3 spawnCenter;
    private int nEnemies;
    private int enemyIndex;
    private int spawnIndex;
    private List<GameObject> enemiesInRoom = new List<GameObject>();
    //public LayerMask obstacleLayer;
    //private float sphereRadius;

    public List<GameObject> EnemiesInRoom {
      get {return enemiesInRoom;}
      set {enemiesInRoom = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
      nEnemies = 0;
      enemyIndex  = 0;
      //sphereRadius = 0.5f;

      for(int i = 0; i < spawners.Length; i++) {
        for(int j = 0; j < spawners[i].transform.childCount; j++) {
          spawnPoints.Add(spawners[i].transform.GetChild(j).gameObject);
        }
      }
      Debug.Log("SPAWN POINTS: " + spawnPoints.Count);
    }

    /*
    public void SpawnEnemies()
    {
      spawnCenter = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
      nEnemies = Random.Range(minEnemies, maxEnemies + 1);
      for(int i = 0; i < nEnemies; i++) {
        Vector3 spawnPos = spawnCenter + new Vector3(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), spawnSize.y, Random.Range(-spawnSize.z / 2, spawnSize.z / 2));
        while(Physics.CheckSphere(spawnPos, sphereRadius, obstacleLayer)) {
          Debug.Log("ENEMY COLLISION");
          spawnPos = spawnCenter + new Vector3(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), spawnSize.y, Random.Range(-spawnSize.z / 2, spawnSize.z / 2));
        }
        enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPos, Quaternion.identity, this.transform);
        enemiesInRoom.Add(enemy);
        enemy.SetActive(false);
      }
    }
    */

    public void SpawnEnemies()
    {
      nEnemies = Random.Range(minEnemies, maxEnemies + 1);
      Debug.Log("Number of enemies: " + nEnemies);
      for(int i = 0; i < nEnemies; i++) {
        spawnIndex = Random.Range(0, spawnPoints.Count);
        GameObject spawnPoint = spawnPoints[spawnIndex];

        if(spawnPoint.transform.parent.gameObject.CompareTag(MyTags.SPAWN_01_TAG)) {
          enemyIndex = 0;
        } else if(spawnPoint.transform.parent.gameObject.CompareTag(MyTags.SPAWN_02_TAG)) {
          enemyIndex = 1;
        } else if(spawnPoint.transform.parent.gameObject.CompareTag(MyTags.SPAWN_03_TAG)) {
          enemyIndex = 2;
        }
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint.transform.position, Quaternion.identity, this.transform);
        //spawnPoints.Remove(spawnPoint);
        enemiesInRoom.Add(enemy);
        enemy.SetActive(false);
      }
    }

}//class

























//space
