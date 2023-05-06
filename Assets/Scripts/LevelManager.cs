using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoSingleton<LevelManager>
{
    public void OnAuthenticationComplete()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void OnJoinGameRoom()
    {
        SceneManager.LoadScene("Game Room");
    }
}
