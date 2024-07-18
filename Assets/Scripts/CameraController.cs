using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x, -133.34f, -77.6f), Mathf.Clamp(player.position.y, 16.565f, 34.82f), transform.position.z);
    }
}
