using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardIdentity : MonoBehaviour
{
    public string cardFace { get; set; }

    public void SetCardFace(string cardFace)
    {
        this.cardFace = cardFace;

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + cardFace);
    }
}
