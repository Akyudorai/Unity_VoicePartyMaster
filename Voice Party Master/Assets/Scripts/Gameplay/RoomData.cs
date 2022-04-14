using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public string RoomName;
    public List<GameObject> ObjectsInRoom = new List<GameObject>();
    public List<GameObject> PlayersInRoom = new List<GameObject>();
    public List<GameObject> EnemiesInRoom = new List<GameObject>();

    public List<string> info;

    public MeshRenderer renderable;

    public List<GameObject> triggerEnabledObjects = new List<GameObject>();

    public Vector3 cameraOverviewPos;

    private void Awake()
    {
        renderable.enabled = false;

        LoadObjects();
        RefreshRoomRenderables();
    }

    private void Start() 
    {
        
    }

    public void RemoveEnemy(GameObject enemy) {
        
        if (EnemiesInRoom.Contains(enemy)) {
            EnemiesInRoom.Remove(enemy);
        }        
    }

    public void RemovePlayer(GameObject player) {
        if (PlayersInRoom.Contains(player)) {
            PlayersInRoom.Remove(player);
        }
    }

    private void LoadObjects() {
        for (int i = 0; i < transform.childCount; i++) 
        {
            if (transform.GetChild(i).GetComponent<InteractionData>() != null) 
            {
                ObjectsInRoom.Add(transform.GetChild(i).gameObject);
            }
        }   
    }

    public List<GameObject> GetUndiscoveredObjects() 
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < ObjectsInRoom.Count; i++) {
            if (!ObjectsInRoom[i].GetComponent<InteractionData>().hasBeenInteractedWith) {
                result.Add(ObjectsInRoom[i]);
            }            
        }

        return result;
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

        // Toggle Trigger Enabled Objects
        foreach (GameObject obj in triggerEnabledObjects) {
            obj.SetActive((PlayersInRoom.Count > 0) ? true : false);
        }
    }

    private void OnTriggerEnter(Collider c) 
    {                
        // If a player character steps into a room, set its current room state to this
        if (c.tag == "Player" && !PlayersInRoom.Contains(c.gameObject)) {            
            
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

            if (c.gameObject.GetComponent<EnemyController>() != null) {
                c.gameObject.GetComponent<EnemyController>().SetRoom(this);
            }
            
            else if (c.gameObject.GetComponent<BossController>() != null) {
                c.gameObject.GetComponent<BossController>().SetRoom(this);
            }

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
