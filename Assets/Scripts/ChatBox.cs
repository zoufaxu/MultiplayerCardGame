using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBox : MonoBehaviour
{
    public TMP_Text chatMessageDisplay;

    public void SetMessage(string message){
        chatMessageDisplay.text = message;
        Destroy(gameObject , 5f);
    }
}
