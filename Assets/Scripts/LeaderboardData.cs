using UnityEngine;
using TMPro;


public class LeaderboardData : MonoBehaviour {
    public TMP_Text nameText , chipsText;

    public void ShowData(string name , string chips)
    {
        nameText.text = name;
        chipsText.text = chips;
    }
}