using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    public void JionGameRoom()
    {
        NetworkClient.Instance.JoinGameRoom();
    }
    public void ShowLeaderboardPanel()
    {
        NetworkClient.Instance.GetLeaderboardEntries();
    }
}
