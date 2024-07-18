using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorController : MonoBehaviour
{
    public BoxCollider2D colliderBoxTop;
    public BoxCollider2D collierBoxBottom;
    public PolygonCollider2D colliderPolygon;
    public GameObject wall;
    public GameObject edgeDoor;

    private bool isOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                isOpen = true;

                colliderBoxTop.enabled = false;
                colliderPolygon.enabled = true;
                collierBoxBottom.enabled = true;

                GetComponent<SpriteRenderer>().enabled = false;
                wall.GetComponent<TilemapRenderer>().sortingOrder = 5;
                edgeDoor.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
            else
            {
                colliderBoxTop.enabled = true;
                colliderPolygon.enabled = false;
                collierBoxBottom.enabled = false;
                GetComponent<SpriteRenderer>().enabled = true;
                wall.GetComponent<TilemapRenderer>().sortingOrder = 1;
                edgeDoor.GetComponent<SpriteRenderer>().sortingOrder = 1;
                isOpen = false;
            }
        }
    }
}
