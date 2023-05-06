using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameRoom : MonoBehaviour
{
    public Transform chatPanel;
    public void OnLeaveRoom()
    {
        NetworkClient.Instance.LeaveRoom();
    }
    public void ShowChatPanel(){
        chatPanel.gameObject.SetActive(true);
    }
    public void SendChat(TMP_Text text){
        NetworkClient.Instance.SendChatMessage(text.text);
        chatPanel.gameObject.SetActive(false);

    }
}
