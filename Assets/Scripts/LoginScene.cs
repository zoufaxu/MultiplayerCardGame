using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour {
    public TMP_InputField userName;
    public TMP_InputField password;
    public void RegisterUser(){
        if (string.IsNullOrEmpty(password.text) || password.text.Length < 6)
        {
            Debug.Log("密码不得少于6位");
            return;
        }
        NetworkClient.Instance.RegisterUser(userName.text , password.text);
    }
    public void LoginUser()
    {
        if (string.IsNullOrEmpty(password.text) || password.text.Length < 6)
        {
            Debug.Log("密码不得少于6位");
            return;
        }
        NetworkClient.Instance.LoginUser(userName.text, password.text);
    }
}
