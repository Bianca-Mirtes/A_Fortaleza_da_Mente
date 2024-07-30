using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectController : MonoBehaviour
{
    private Transform feedbackCanva;
    private GameObject inventario;
    private void Start()
    {
        feedbackCanva = GameObject.FindGameObjectWithTag("Feedback").transform;
        inventario = GameObject.FindGameObjectWithTag("Inventario");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            feedbackCanva.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " coletado!";
            int slotsCount = inventario.transform.GetChild(0).childCount;
            for(int ii=0; ii < slotsCount; ii++)
            {
                Transform slot = inventario.transform.GetChild(0).GetChild(ii);
                if (!slot.GetChild(0).gameObject.activeSelf)
                {
                    slot.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
                    slot.GetChild(0).gameObject.SetActive(true);
                    break;
                }
            }
            FindObjectOfType<FadeController>().FadeIn();
            gameObject.SetActive(false);
            Invoke("FadeOut", 3f);
            //feedbackCanva.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            feedbackCanva.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " coletado!";
            FindObjectOfType<FadeController>().FadeIn();
            gameObject.SetActive(false);
            Invoke("FadeOut", 3f);
            //feedbackCanva.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void FadeOut()
    {
        FindObjectOfType<FadeController>().FadeOut();
        Destroy(gameObject);
    }
}
