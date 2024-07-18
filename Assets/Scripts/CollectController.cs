using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectController : MonoBehaviour
{
    private Transform feedbackCanva;
    private void Start()
    {
        feedbackCanva = GameObject.FindGameObjectWithTag("Feedback").transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            feedbackCanva.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " coletado!";
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
