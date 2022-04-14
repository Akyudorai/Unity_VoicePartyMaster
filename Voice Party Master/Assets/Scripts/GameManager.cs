using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    /////////////////////////////////////////
    // Variables
    /////////////////////////////////////////

    public GameObject bossRef;
    public GameObject bossHealthPanel;
    public Image bossHealthImg;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject lossPanel;

    [SerializeField] private List<GameObject> rooms = new List<GameObject>(); 
    public Dictionary<string, Vector3> roomNav = new Dictionary<string, Vector3>();

    /////////////////////////////////////////
    // MonoBehaviour
    /////////////////////////////////////////

    private void Awake() {

        // Populate room navigation map
        for (int i = 0; i < rooms.Count; i++)
        {
            roomNav.Add(rooms[i].name.ToLower(), rooms[i].transform.position);
        }
    }

    private void Start()
    {
        // Randomly select a room to place items
        // 0 == Hallway, 1 == Boss (hence they are excluded)
        List<int> randomRooms = new List<int>() { 2, 3, 4, 5, 6, 7, 8 };        

        int keyRoom = randomRooms[Random.Range(0, randomRooms.Count - 1)];
        randomRooms.Remove(keyRoom);        

        int swordRoom = randomRooms[Random.Range(0, randomRooms.Count - 1)];
        randomRooms.Remove(swordRoom);

        int shieldRoom = randomRooms[Random.Range(0, randomRooms.Count - 1)];
        randomRooms.Remove(shieldRoom);

        int gemRoom = randomRooms[Random.Range(0, randomRooms.Count - 1)];
        randomRooms.Remove(gemRoom);
        
        int glovesRoom = randomRooms[Random.Range(0, randomRooms.Count - 1)];
        randomRooms.Remove(glovesRoom);

        // Populate room navigation map
        for (int i = 1; i < rooms.Count; i++)
        {
            if (i == keyRoom + 1) {

                // Randomly select an object within the room to hide the key in.
                RoomData keyRoomData = rooms[i].GetComponent<RoomData>();
                int objectCount = keyRoomData.ObjectsInRoom.Count;
                int randomObject = Random.Range(0, objectCount-1);                

                for (int j = 0; j < objectCount; j++) 
                {
                    if (j == randomObject) {
                        rooms[i].GetComponent<RoomData>().ObjectsInRoom[j].GetComponent<InteractionData>().InspectionResult[0] = "You found a dusty old key!";
                        Debug.Log("Key was hidden in " + rooms[i].name);
                    }                    
                }
            }

            if (i == swordRoom + 1) {
               
                // Randomly select an object within the room to hide the key in.
                RoomData keyRoomData = rooms[i].GetComponent<RoomData>();
                int objectCount = keyRoomData.ObjectsInRoom.Count;
                int randomObject = Random.Range(0, objectCount-1);                
                
                for (int j = 0; j < objectCount; j++) 
                {
                    if (j == randomObject) {
                        rooms[i].GetComponent<RoomData>().ObjectsInRoom[j].GetComponent<InteractionData>().InspectionResult[0] = "You found a dull sword!";
                        Debug.Log("Sword was hidden in " + rooms[i].name);
                    }                    
                }
            }

            if (i == shieldRoom + 1) {
                
                // Randomly select an object within the room to hide the key in.
                RoomData keyRoomData = rooms[i].GetComponent<RoomData>();
                int objectCount = keyRoomData.ObjectsInRoom.Count;
                int randomObject = Random.Range(0, objectCount-1);                

                for (int j = 0; j < objectCount; j++) 
                {
                    if (j == randomObject) {
                        rooms[i].GetComponent<RoomData>().ObjectsInRoom[j].GetComponent<InteractionData>().InspectionResult[0] = "You found an old shield!";
                        Debug.Log("Shield was hidden in " + rooms[i].name);
                    }                    
                }
            }

            if (i == gemRoom + 1) {
                
                // Randomly select an object within the room to hide the key in.
                RoomData keyRoomData = rooms[i].GetComponent<RoomData>();
                int objectCount = keyRoomData.ObjectsInRoom.Count;
                int randomObject = Random.Range(0, objectCount-1);                

                for (int j = 0; j < objectCount; j++) 
                {
                    if (j == randomObject) {
                        rooms[i].GetComponent<RoomData>().ObjectsInRoom[j].GetComponent<InteractionData>().InspectionResult[0] = "You found a magic gem!";
                        Debug.Log("Gem was hidden in " + rooms[i].name);
                    }                    
                }
            }

            if (i == glovesRoom + 1) {
                
                // Randomly select an object within the room to hide the key in.
                RoomData keyRoomData = rooms[i].GetComponent<RoomData>();
                int objectCount = keyRoomData.ObjectsInRoom.Count;
                int randomObject = Random.Range(0, objectCount-1);                

                for (int j = 0; j < objectCount; j++) 
                {
                    if (j == randomObject) {
                        rooms[i].GetComponent<RoomData>().ObjectsInRoom[j].GetComponent<InteractionData>().InspectionResult[0] = "You found a pair of gloves!";
                        Debug.Log("Gloves was hidden in " + rooms[i].name);
                    }                    
                }
            }
        }
    }

    private void Update() {
         
        // Update Boss Healthbar
        if (bossHealthPanel != null)
        { 
            if (bossHealthImg != null) {
                bossHealthImg.fillAmount = bossRef.GetComponent<BossController>().entity.GetCurrentHealth() / bossRef.GetComponent<BossController>().entity.GetMaxHealth();
            }        

            if (bossRef.GetComponent<BossController>().entity.GetCurrentHealth() <= 0) {
                bossHealthPanel.SetActive(false);
                WinEvent();
            }
        }
        

    }

    public void LoseEvent()
    {
        lossPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void WinEvent() 
    {
        winPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    /////////////////////////////////////////
    // Custom Functions
    /////////////////////////////////////////

    public RoomData GetRoom(string name) 
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].GetComponent<RoomData>().RoomName == name) {
                return rooms[i].GetComponent<RoomData>();
            }
        }

        return null;
    }

    /////////////////////////////////////////
    // Commands
    /////////////////////////////////////////
    public static void Quit(int order, string prevKeyword) {

        // Exit the application
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 

        Application.Quit();
    }

}
