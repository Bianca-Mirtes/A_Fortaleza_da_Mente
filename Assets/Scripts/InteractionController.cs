using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractionController : MonoBehaviour
{
    public Sprite open;
    public Sprite close;

    private bool isOpen = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!isOpen)
            {
                GetComponent<SpriteRenderer>().sprite = open;
                isOpen = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = close;
                isOpen = false;
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                GetComponent<SpriteRenderer>().sprite = open;
                isOpen = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = close;
                isOpen = false;
            }

        }
    }
}
