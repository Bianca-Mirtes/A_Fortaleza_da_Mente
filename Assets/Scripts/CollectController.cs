using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectController : MonoBehaviour
{
    private GameObject[] feedbackCanvas;
    private Transform inventario;
    private Transform feedbackCurrent;
    private void Start()
    {
        feedbackCanvas = GameObject.FindGameObjectsWithTag("Feedback");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (collision.gameObject.name == "Elizabeth")
            {
                inventario = GameObject.Find("PlayerOne").transform.GetChild(1);
                feedbackCurrent = feedbackCanvas[0].transform.parent.name == "PlayerOne" ? feedbackCanvas[0].transform : feedbackCanvas[1].transform;
            }
            else
            {
                inventario = GameObject.Find("PlayerTwo").transform.GetChild(1);
                feedbackCurrent = feedbackCanvas[0].transform.parent.name == "PlayerTwo" ? feedbackCanvas[0].transform : feedbackCanvas[1].transform;
            }

            int slotsCount = inventario.transform.GetChild(0).childCount;
            if (gameObject.name == "Relogio")
            {
                FindObjectOfType<VerticalDoorController>().SetObject(true);
                feedbackCurrent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = transform.GetChild(0).name + " coletado!";
                for (int ii = 0; ii < slotsCount; ii++)
                {
                    Transform slot = inventario.transform.GetChild(0).GetChild(ii);
                    if (!slot.GetChild(0).gameObject.activeSelf)
                    {
                        slot.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        slot.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                feedbackCurrent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " coletado!";
                for (int ii = 0; ii < slotsCount; ii++)
                {
                    Transform slot = inventario.transform.GetChild(0).GetChild(ii);
                    if (!slot.GetChild(0).gameObject.activeSelf)
                    {
                        slot.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
                        slot.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
            feedbackCurrent.gameObject.GetComponent<FadeController>().FadeIn();
            if(!gameObject.name.Equals("Relogio"))
            {
                gameObject.SetActive(false);
            }
            Invoke("FadeOut", 3f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            feedbackCurrent.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name + " coletado!";
            FindObjectOfType<FadeController>().FadeIn();
            gameObject.SetActive(false);
            Invoke("FadeOut", 3f);
        }
    }

    private void FadeOut()
    {
        feedbackCurrent.gameObject.GetComponent<FadeController>().FadeOut();
        Destroy(gameObject);
    }
}
