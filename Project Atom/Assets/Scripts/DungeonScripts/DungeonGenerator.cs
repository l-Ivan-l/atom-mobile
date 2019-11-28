using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    private Vector3 initPosition = new Vector3(0, 0, 0);
    public GameObject initRoomPrefab;
    public GameObject[] roomPrefabs = new GameObject[7];
    public GameObject preBossRoom;
    public GameObject bossRoom;
    public GameObject treasureRoom;
    public GameObject[] hallPrefabs = new GameObject[3];
    private GameObject InitRoom;
    private int roomIndex;
    private int hallIndex;
    private int exitIndex;
    //private int iterations;
    public int nRooms;
    private List<GameObject> modulesInDungeon = new List<GameObject>();
    private List<GameObject> pendingExits = new List<GameObject>();
    private List<KeyValuePair<GameObject, GameObject>> connectedExits = new List<KeyValuePair<GameObject, GameObject>>();

    private float sphereRadius;
    public LayerMask roomLayer;

    private Vector3 newModulePos;
    private Vector3 newModuleSize;
    //private Vector3 newModuleCenter;
    private Quaternion newModuleRotation;
    public GameObject loadingScreen;
    public GameObject hud;
    public Minimap miniMap;
    public GameObject portalPrefab;

    private GameObject preBossRoomExit;

    private int nCollisions;

    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
      nCollisions = 0;
      roomIndex = 0;
      hallIndex = 0;
      exitIndex = 0;
      //nRooms = 12;
      StartCoroutine(GenerateDungeon());
    }

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.R)){
        RestartScene();
      }
    }

    IEnumerator GenerateDungeon()
    {
      InitRoom = (GameObject)Instantiate(initRoomPrefab, this.transform.position, Quaternion.identity, this.transform);
      modulesInDungeon.Add(InitRoom);
      pendingExits.AddRange(InitRoom.GetComponent<RoomScript>().GetExits());
      //for (int i = 0; i < iterations; i++) { //Esto puede cambiar por un while hasta que haya n Rooms.
      while(nRooms > 0) {
        exitIndex = Random.Range(0, pendingExits.Count);
        var currentExit = pendingExits[exitIndex];
        if(nRooms > 3) {
          //Generate Hall
          if(currentExit.CompareTag(MyTags.EXIT_ROOM_TAG)) {
            GenerateHall(currentExit);
          //GenerateRoom
          } else if(currentExit.CompareTag(MyTags.EXIT_HALL_TAG)) {
            GenerateRoom(currentExit);
            nRooms -= 1;
          }
        } else if(nRooms == 3 && currentExit.CompareTag(MyTags.EXIT_HALL_TAG)) {
          GenerateTreasureRoom(currentExit);
          nRooms -= 1;
        } else if(nRooms == 2 && currentExit.CompareTag(MyTags.EXIT_ROOM_TAG)) {
          GeneratePreBossRoom(currentExit);
          nRooms -= 1;
        } else if(nRooms == 1) {
          Debug.Log("Generate Boss Room");
          GenerateBossRoom(preBossRoomExit);
          nRooms -= 1;
        }
        if(nCollisions > 35) {
          RestartScene();
        }
        yield return new WaitForSeconds(0.02f);
      }

      EraseDeadEnds();
      ReEvaluateConnections();
      FixRooms();
      GenerateEnemies();
      GeneratePortals();
      Debug.Log("DUNGEON GENERADO!");
      Debug.Log("Conexiones: " + pendingExits.Count);
      Debug.Log("Modulos: " + modulesInDungeon.Count);
      //DeactivateRooms();
      LoadComplete();
    }

    void LoadComplete()
    {
      loadingScreen.SetActive(false);
      hud.SetActive(true);
      StartCoroutine(LightOn(4f));
    }

    IEnumerator LightOn(float _timer)
    {
      yield return new WaitForSeconds(_timer);
      light.SetActive(true);
    }

    void GeneratePreBossRoom(GameObject _currentExit)
    {
      Debug.Log("Pre BossRoom generated");
      GameObject _preBossRoom = Instantiate(preBossRoom, this.transform);
      modulesInDungeon.Add(_preBossRoom);
      var preBossModuleExits = _preBossRoom.GetComponent<Module>().GetExits();
      pendingExits.AddRange(preBossModuleExits);
      int randomExit = Random.Range(0, preBossModuleExits.Count);
      var preBossExit = preBossModuleExits[randomExit];
      preBossRoomExit = _preBossRoom.GetComponent<PreBossRoom>().topConnection;
      ConnectExits(_currentExit, preBossExit, preBossModuleExits);
    }

    void GenerateBossRoom(GameObject _currentExit)
    {
      GameObject _bossRoom = Instantiate(bossRoom, this.transform);
      modulesInDungeon.Add(_bossRoom);
      var bossModuleExits = _bossRoom.GetComponent<Module>().GetExits();
      pendingExits.AddRange(bossModuleExits);
      int randomExit = Random.Range(0, bossModuleExits.Count);
      var bossExit = bossModuleExits[randomExit];
      ConnectExits(_currentExit, bossExit, bossModuleExits);
    }

    void GenerateTreasureRoom(GameObject _currentExit)
    {
      GameObject _treasureRoom = Instantiate(treasureRoom, this.transform);
      modulesInDungeon.Add(_treasureRoom);
      var treasureModuleExits = _treasureRoom.GetComponent<Module>().GetExits();
      pendingExits.AddRange(treasureModuleExits);
      int randomExit = Random.Range(0, treasureModuleExits.Count);
      var treasureExit = treasureModuleExits[randomExit];
      ConnectExits(_currentExit, treasureExit, treasureModuleExits);
    }

    void GenerateRoom(GameObject _currentExit)
    {
      //Room Distribution
      /*
      if(nRooms >= 5) {
        roomIndex = Random.Range(0, (int)(roomPrefabs.Length/2));
      } else if(nRooms < 5) {
        roomIndex = Random.Range((int)(roomPrefabs.Length/2), roomPrefabs.Length);
      }
      */
      roomIndex = Random.Range(0, roomPrefabs.Length);
      GameObject newRoom = Instantiate(roomPrefabs[roomIndex], this.transform);
      modulesInDungeon.Add(newRoom);
      var newModuleExits = newRoom.GetComponent<Module>().GetExits();
      pendingExits.AddRange(newModuleExits);
      int randomExit = Random.Range(0, newModuleExits.Count);
      var newExit = newModuleExits[randomExit];
      ConnectExits(_currentExit, newExit, newModuleExits);
    }

    void GenerateHall(GameObject _currentExit)
    {
      hallIndex = Random.Range(0, hallPrefabs.Length);
      GameObject newHall = Instantiate(hallPrefabs[hallIndex], this.transform);
      modulesInDungeon.Add(newHall);
      var newModuleExits = newHall.GetComponent<Module>().GetExits();
      pendingExits.AddRange(newModuleExits);
      int randomExit = Random.Range(0, newModuleExits.Count);
      var newExit = newModuleExits[randomExit];
      ConnectExits(_currentExit, newExit, newModuleExits);
    }

    void ConnectExits(GameObject _currentExit, GameObject _newExit, List<GameObject> _newModuleExits)
    {
      var newModule = _newExit.transform.parent;
  		var forwardVectorToMatch = -_currentExit.transform.forward;
  		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(_newExit.transform.forward);
  		newModule.RotateAround(_newExit.transform.position, Vector3.up, correctiveRotation);
  		var correctiveTranslation = _currentExit.transform.position - _newExit.transform.position;
  		newModule.transform.position += correctiveTranslation;
      if(!newModule.gameObject.CompareTag(MyTags.PRE_BOSS_ROOM_TAG)) {
        if(!CheckSpace(newModule.gameObject, _newModuleExits)) {
          //Remove connected exits

          pendingExits.Remove(_newExit);
          _newExit.SetActive(false);
          pendingExits.Remove(_currentExit);
          _currentExit.SetActive(false);

          connectedExits.Add(new KeyValuePair<GameObject, GameObject>(_currentExit, _newExit));
          newModule.gameObject.layer = 10;
        }
      } else if(newModule.gameObject.CompareTag(MyTags.PRE_BOSS_ROOM_TAG)) {
        if(CheckRotation(newModule.gameObject, _newModuleExits) && !CheckSpace(newModule.gameObject, _newModuleExits)) {
          pendingExits.Remove(_newExit);
          _newExit.SetActive(false);
          pendingExits.Remove(_currentExit);
          _currentExit.SetActive(false);

          connectedExits.Add(new KeyValuePair<GameObject, GameObject>(_currentExit, _newExit));
          newModule.gameObject.layer = 10;
        }
      }

    }

    bool CheckRotation(GameObject _newModule, List<GameObject> _newModuleExits)
    {
      Quaternion moduleRotation = _newModule.transform.rotation;
      if(moduleRotation.y == 0) {
        return true;
      } else {
        modulesInDungeon.Remove(_newModule);
        pendingExits.RemoveRange(pendingExits.Count - _newModuleExits.Count, _newModuleExits.Count);
        Destroy(_newModule);
        nRooms+=1;
        return false;
      }
    }

    bool CheckSpace(GameObject _newModule, List<GameObject> _newModuleExits) //<-Pasar las exits del newModule
    {
      newModulePos = transform.TransformPoint(_newModule.GetComponent<Module>().GetCollisionPosition());
      newModuleSize = (_newModule.GetComponent<Module>().collisionSize / 2);
      newModuleRotation = _newModule.transform.rotation;

      if(Physics.CheckBox(newModulePos, newModuleSize, newModuleRotation, roomLayer)) {
        Debug.Log("COLLISION!");
        nCollisions += 1;
        modulesInDungeon.Remove(_newModule);
        pendingExits.RemoveRange(pendingExits.Count - _newModuleExits.Count, _newModuleExits.Count);
        Destroy(_newModule);
        if(_newModule.CompareTag(MyTags.ROOM_TAG)
        || _newModule.CompareTag(MyTags.BOSS_ROOM_TAG)
        || _newModule.CompareTag(MyTags.TREASURE_ROOM_TAG)
        || _newModule.CompareTag(MyTags.PRE_BOSS_ROOM_TAG)) {
          nRooms+=1;
        }
        return true;
      } else {
        return false;
      }
    }

    private static float Azimuth(Vector3 vector)
    {
    	return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }

    void EraseDeadEnds()
    {
      for(int i = 0; i < pendingExits.Count; i++) {
        if(pendingExits[i].CompareTag(MyTags.EXIT_HALL_TAG)) {
          modulesInDungeon.Remove(pendingExits[i].transform.parent.gameObject);
          pendingExits[i].transform.parent.gameObject.GetComponent<HallGenerator>().Autodestroy();
        }
      }
    }

    void FixRooms()
    {
      for(int i = 0; i < modulesInDungeon.Count; i++) {
        if(modulesInDungeon[i].CompareTag(MyTags.ROOM_TAG)) {
          modulesInDungeon[i].GetComponent<RoomScript>().EvaluateConnections();
          modulesInDungeon[i].GetComponent<RoomScript>().GenerateWalls();
        }
      }
    }

    void ReEvaluateConnections()
    {
      //Debug.Log("Connected Exits: ");
      foreach (var exit in connectedExits)
      {
      	//Debug.Log(exit.Key + "=>" + exit.Value);
        if(exit.Key.CompareTag(MyTags.EXIT_HALL_TAG)) {
          var hallExits = exit.Key.transform.parent.gameObject.GetComponent<HallGenerator>().GetExits();
          for(int i = 0; i < hallExits.Count; i++) {
            if(hallExits[i].activeInHierarchy) {
              exit.Value.SetActive(true);
              break;
            }
          }
        } else if (exit.Value.CompareTag(MyTags.EXIT_HALL_TAG)) {
          var hallExits = exit.Value.transform.parent.gameObject.GetComponent<HallGenerator>().GetExits();
          for(int i = 0; i < hallExits.Count; i++) {
            if(hallExits[i].activeInHierarchy) {
              exit.Key.SetActive(true);
              break;
            }
          }
        }

      }
    }

    void GenerateEnemies()
    {
      for(int i = 1; i < modulesInDungeon.Count - 1; i++) {
        if(modulesInDungeon[i].CompareTag(MyTags.ROOM_TAG) || modulesInDungeon[i].CompareTag(MyTags.TREASURE_ROOM_TAG)) {
          modulesInDungeon[i].GetComponent<EnemiesGenerator>().SpawnEnemies();
        }
      }
    }

    void GeneratePortals()
    {
      List<GameObject> roomsInDungeon = new List<GameObject>();
      for(int i = 1; i < modulesInDungeon.Count - 1; i++) {
        if(modulesInDungeon[i].CompareTag(MyTags.ROOM_TAG)) {
          roomsInDungeon.Add(modulesInDungeon[i]);
        }
      }
      Debug.Log("Rooms in dungeon: " + roomsInDungeon.Count);
      for(int i = 0; i < roomsInDungeon.Count; i+=3) {
        Transform portalSpawn = roomsInDungeon[i].GetComponent<RoomScript>().portalPoint.transform;
        GameObject portal = Instantiate(portalPrefab, portalSpawn.position, portalPrefab.transform.localRotation, roomsInDungeon[i].transform);
        roomsInDungeon[i].GetComponent<RoomScript>().HavePortal = true;
        portal.SetActive(false);
      }
      miniMap.GetPortalIcons();
    }

    void DeactivateRooms()
    {
      for(int i = 1; i < modulesInDungeon.Count; i++) {
        if(modulesInDungeon[i].CompareTag(MyTags.ROOM_TAG)) {
          Debug.Log("Deactivate module " + i);
          modulesInDungeon[i].GetComponent<Module>().DeactivateModule();
        }
      }
    }

    void RestartScene()
    {
      Scene scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
    }

}//class



















//space
