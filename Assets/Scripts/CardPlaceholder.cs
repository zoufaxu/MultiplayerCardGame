using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPlaceholder : MonoBehaviour
{
    public List<Image> cardPlaceholders;
    public Sprite defaultSprite;

    public void SetCards(List<string> cards)
    {
        print("ÏÔÊ¾¿¨ÅÆ");
        for (int i = 0; i < cardPlaceholders.Count; i++)
        {
            cardPlaceholders[i].sprite = Resources.Load<Sprite>("Cards/" + cards[i]);
        }
    }
    public void RemoveCardPlaceholder()
    {
        print("Òþ²Ø¿¨ÅÆ");
        for (int i = 0; i < cardPlaceholders.Count; i++)
        {
            cardPlaceholders[i].sprite = defaultSprite;
        }
    }
}
