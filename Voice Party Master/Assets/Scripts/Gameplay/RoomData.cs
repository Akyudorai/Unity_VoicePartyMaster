using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public string RoomName;
    public List<GameObject> ObjectsInRoom = new List<GameObject>();
    public List<GameObject> PlayersInRoom = new List<GameObject>();
    public List<GameObject> EnemiesInRoom = new List<GameObject>();



    private Dictionary<string, GameObject> objectDict = new Dictionary<string, GameObject>();
    public List<string> info;

    public MeshRenderer renderable;

    private void Awake()
    {
        renderable.enabled = false;
    }

    private void Start() 
    {
        LoadObjects();
    }

    private void LoadObjects() {
        for (int i = 0; i < transform.childCount; i++) 
        {
            ObjectsInRoom.Add(transform.GetChild(i).gameObject);
            
            if (transform.GetChild(i).GetComponent<InteractionData>() != null) 
            {
                objectDict.Add(transform.GetChild(i).GetComponent<InteractionData>().ObjectName, transform.GetChild(i).gameObject);
            }
        }

        
    }

    public Dictionary<string, GameObject> GetObjects() 
    {
        return objectDict;
    }

    public string GetRoomInfo() 
    {
        string output = "";
        foreach (string s in info) {
            output += s + "\n";
        }

        return output;
    }

    private void RefreshRoomRenderables()
    {
        // Toggle the room renderable
        renderable.enabled = (PlayersInRoom.Count > 0) ? true : false;

        // Toggle all enemy renderables
        foreach (GameObject obj in EnemiesInRoom) {
            foreach (SkinnedMeshRenderer mr in obj.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                mr.enabled = (PlayersInRoom.Count > 0) ? true : false;
            }                    
        }

        // Toggle all object renderables
        foreach (GameObject obj in ObjectsInRoom) {
            obj.GetComponent<MeshRenderer>().enabled = (PlayersInRoom.Count > 0) ? true : false;
        }
    }

    private void OnTriggerEnter(Collider c) 
    {        
        // If a player character steps into a room, set its current room state to this
        if (c.tag == "Player" && !PlayersInRoom.Contains(c.gameObject)) {
            
            // Set Room for Player
            c.gameObject.GetComponent<PlayerController>().SetRoom(this);
            
            // Add Player to Room
            PlayersInRoom.Add(c.gameObject);

            // Toggle Renderables
            RefreshRoomRenderables();

            // Updates Room Data
            GameObject.Find("UI").GetComponent<InterfaceManager>().UpdateRoomData();
        }

        // If an enemy character enters the room, add it to list
        if (c.tag == "Enemy" && !EnemiesInRoom.Contains(c.gameObject))
        {
            // Add the enemy to room list
            EnemiesInRoom.Add(c.gameObject);

            // Toggle Renderables
            RefreshRoomRenderables();

            // Updates Enemy List
            GameObject.Find("UI").GetComponent<InterfaceManager>().UpdateRoomData();
        }
    }

    private void OnTriggerExit(Collider c) 
    {
        // if a player character leaves a room, remove it from the entities list
        if (c.tag == "Player") {
            PlayersInRoom.Remove(c.gameObject);
        }   

        // Toggle Renderables
        RefreshRoomRenderables();        
    }
}
