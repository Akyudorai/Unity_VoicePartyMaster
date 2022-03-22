using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    /////////////////////////////////////////
    // Variables
    /////////////////////////////////////////

    [SerializeField] private List<GameObject> rooms = new List<GameObject>(); 
    public Dictionary<string, Vector3> roomNav = new Dictionary<string, Vector3>();

    /////////////////////////////////////////
    // MonoBehaviour
    /////////////////////////////////////////

    private void Awake() {

        // Populate room navigation map
        foreach (GameObject o in rooms) {
            roomNav.Add(o.name.ToLower(), o.transform.position);
        }
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
