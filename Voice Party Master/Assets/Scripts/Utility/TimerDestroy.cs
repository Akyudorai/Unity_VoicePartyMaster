using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    public float time;

    private void Update()
    {
        if (time > 0) time -= Time.deltaTime;
        if (time <= 0) Destroy(gameObject);
    }
}
