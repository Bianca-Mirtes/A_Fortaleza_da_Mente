using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 

   private Transform player;

    private void Start()
    {
        if(transform.parent.name == "PlayerOne")
        {
            player = GameObject.FindGameObjectWithTag("Elizabeth").transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Anthony").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x, -133.34f, -77.6f), Mathf.Clamp(player.position.y, 16.565f, 34.82f), transform.position.z);
    }
}
