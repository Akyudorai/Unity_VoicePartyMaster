using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public VoiceController vc;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);
    public float cameraSpeed = 1.0f;

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, vc.selectedCharacter.transform.position + cameraOffset, cameraSpeed * Time.deltaTime);
    }
}
