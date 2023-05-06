using PlayerIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public Transform turnIndicator, turnPanel;
    public TMP_Text buttomTextView, topTextView;
    private NetworkClient client;
    public CardPlaceholder cardPlaceholder;
    public ChatBox chatBubble;
    private void Start()
    {
        client = NetworkClient.Instance;
    }
    public void AcceptCard(Message message)
    {
        GetComponent<CardManager>().SpawnCards(message);
    }
    public void IsMyTurn(bool value)
    {
        turnIndicator.gameObject.SetActive(value);

        if (turnPanel != null)
        {
            turnPanel.gameObject.SetActive(value);
        }
    }
    public void SetChips(int amount)
    {
        topTextView.text = amount.ToString();
        
    }
    public void SetUsername(string username)
    {
        topTextView.text = username;
    }
    public void SetStatus(string status){
        buttomTextView.text = status;
    }
    public void OnRoundOver()
    {
        IsMyTurn(false);
        GetComponent<CardManager>().DestroyCards();
        cardPlaceholder?.RemoveCardPlaceholder();
    }
    public void OnPackCard()
    {
        GetComponent<CardManager>().DestroyCards();
    }
    public void Chaal()
    {
        client.ActionChaal();
    }
    public void Pack()
    {
        client.ActionPack();
    }
    public void Show()
    {
        client.ActionShow();
    }
    public void ActionCardShowEvent(List<string> cards)
    {
        cardPlaceholder?.SetCards(cards);
    }
    public void ShowChatMessage(string message){
        ChatBox chatMsg = Instantiate(chatBubble , transform);
        chatMsg.SetMessage(message);
    }
}
