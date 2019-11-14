using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : Module
{
    public GameObject[] weaponPrefabs;
    public Transform[] gunSpawn;
    private int[] weaponIndex = new int[2];

    void Awake()
    {
      collisionPosition = colliderTransform.position;
      exitTag = MyTags.EXIT_ROOM_TAG;
    }

    // Start is called before the first frame update
    void Start()
    {
      for(int i = 0; i < weaponIndex.Length; i++) {
        weaponIndex[i] = 0;
      }
      SpawnWeapons();
    }

    void SpawnWeapons()
    {
      weaponIndex[0] = Random.Range(0, weaponPrefabs.Length);
      weaponIndex[1] = Random.Range(0, weaponPrefabs.Length);
      while(weaponIndex[1] == weaponIndex[0]){
        weaponIndex[1] = Random.Range(0, weaponPrefabs.Length);
      }
      //Spawn Weapons
      for(int i = 0; i < 2; i++) {
        Vector3 weaponPosition = gunSpawn[i].position;
        GameObject weapon = Instantiate(weaponPrefabs[weaponIndex[i]], weaponPosition, Quaternion.identity, this.transform);
      }

    }

}//class
