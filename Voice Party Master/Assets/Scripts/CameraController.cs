using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public VoiceController vc;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);
    public float cameraSpeed = 1.0f;

    public enum TargetMode { 
        FollowTarget, TargetPosition
    }

    public TargetMode tMode = TargetMode.FollowTarget;
    public Vector3 targetPosition = new Vector3(0, 0, 0);    
    public GameObject targetCharacter = null;

    private void Update() {

        if (tMode == TargetMode.FollowTarget) {
            transform.position = Vector3.Lerp(transform.position, targetCharacter.transform.position + cameraOffset, cameraSpeed * Time.deltaTime);
        } 
        else if (tMode == TargetMode.TargetPosition) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        }
        
    }
}
