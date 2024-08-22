using UnityEngine;

public class VerticalDoorController : MonoBehaviour
{
    private bool isOpen = false;
    private bool hasObject = false; 
    
    public void SetObject(bool value)
    {
        hasObject = value;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && hasObject)
        {
            if (!isOpen)
            {
                isOpen = true;
                
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                isOpen = false;
            }
        }
    }
}
