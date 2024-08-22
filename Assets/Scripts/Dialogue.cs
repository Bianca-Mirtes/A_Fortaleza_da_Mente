using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public Person character;

    [TextArea]
    public string[] messages;

    // Update is called once per frame
    void Update()
    {
        
    }
}
