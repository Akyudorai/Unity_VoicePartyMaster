using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public string RoomName;
    public List<GameObject> ObjectsInRoom = new List<GameObject>();

    private Dictionary<string, GameObject> objectDict = new Dictionary<string, GameObject>();
    public List<string> info;


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

    private void OnTriggerEnter(Collider c) 
    {        
        // If a player character steps into a room, set its current room state to this
        if (c.tag == "Player") {
            c.gameObject.GetComponent<PlayerController>().SetRoom(this);
        }
    }
}
