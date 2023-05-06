using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayerIOClient;

public class GameManager : MonoBehaviour
{
    public NetworkIdentity[] playerPrefabs;
    private List<NetworkIdentity> allPlayers;
    private NetworkClient client;
    private string playerId;


    private void Start()
    {
        client = NetworkClient.Instance;
        client.onPlayerSpawned += OnPlayerSpawned;
        client.onPlayerLeave += OnPlayerLeave;
        client.onRoundStarted += OnRoundStarted;
        client.onTurnnChanged += OnTurnnChanged;
        client.onTransaction += OnTransaction;
        client.onRoundOver += OnRoundOver;
        client.onActionPack += OnActionPack;
        client.onActionShow += OnActionShow;
        client.onStatUptate += OnStatUptate;
        client.onChatMesssgeReceived += OnChatMesssgeReceived;
        allPlayers = new List<NetworkIdentity>();
    }

    private void OnDestroy()
    {
        client.onPlayerSpawned -= OnPlayerSpawned;
        client.onPlayerLeave -= OnPlayerLeave;
        client.onRoundStarted -= OnRoundStarted;
        client.onTurnnChanged -= OnTurnnChanged;
        client.onTransaction -= OnTransaction;
        client.onRoundOver -= OnRoundOver;
        client.onActionPack -= OnActionPack;
        client.onActionShow -= OnActionShow;
        client.onStatUptate -= OnStatUptate;
        client.onChatMesssgeReceived -= OnChatMesssgeReceived;

    }
    private void OnApplicationQuit() {
        client.DisconnectAll();
    }

    //生成玩家
    private void OnPlayerSpawned(Message message)
    {
        NetworkIdentity identity;
        if (message.Type.Equals(NetworkConstant.SPAWN_LOCAL_PLAYER))
        {
            identity = playerPrefabs[0];
        }
        else
        {
            identity = playerPrefabs[allPlayers.Count];
        }
        identity.id = message.GetString(0);
        identity.GetComponent<PlayerManager>().SetUsername(message.GetString(1));
        identity.gameObject.SetActive(true);
        allPlayers.Add(identity);
    }
    //玩家离开
    private void OnPlayerLeave(Message message)
    {
        playerId = message.GetString(0);
        foreach (var item in allPlayers)
        {
            if (item.id.Equals(playerId))
            {
                item.gameObject.SetActive(false);
            }
        }
    }
    private void OnTurnnChanged(Message message)
    {
        print($"开始回合{message}");
        string playerId = message.GetString(0);

        foreach (NetworkIdentity nI in allPlayers)
        {
            nI.GetComponent<PlayerManager>().IsMyTurn(nI.id.Equals(playerId));
        }
    }
    private void OnRoundStarted(Message message)
    {
        print($"开始发牌{message.GetString(0)} , {message.GetString(1)} , {message.GetString(2)}");
        //foreach (NetworkIdentity item in allPlayers)
        //{
        //    item.GetComponent<CardManager>().DestroyCards();
        //    if (item.GetComponent<PlayerManager>().cardPlaceholder != null)
        //    {
        //        item.GetComponent<PlayerManager>().cardPlaceholder.RemoveCardPlaceholder();
        //    }

        //}
        foreach (NetworkIdentity nI in allPlayers)
        {
            
            if (nI.id.Equals(client.GetID))
            {
                nI.GetComponent<PlayerManager>().AcceptCard(message);
            }
        }
    }
    private void OnRoundOver(Message message)
    {
        print("回合结束");
        foreach (NetworkIdentity nI in allPlayers)
        {

            nI.GetComponent<PlayerManager>().OnRoundOver();
        }
    }

    private void OnTransaction(Message message)
    {
        int amt = message.GetInt(0);

        foreach (NetworkIdentity nI in allPlayers)
        {
            if (nI.id.Equals(client.GetID))
            {
                nI.GetComponent<PlayerManager>().SetChips(amt);
            }
        }
    }

    
    //弃牌
    private void OnActionPack()
    {
        foreach (NetworkIdentity nI in allPlayers)
        {
            if (nI.id.Equals(client.GetID))
            {
                nI.GetComponent<PlayerManager>().OnPackCard();
            }
        }
    }
    //显示卡牌 消息数组第 0 位是玩家id 后面才是牌
    private void OnActionShow(Message message)
    {
        playerId = message.GetString(0);

        List<string> cards = new List<string>();
        for (uint i = 1; i < message.Count; i++)
        {
            cards.Add(message.GetString(i));
        }
        foreach (NetworkIdentity nI in allPlayers)
        {
            if (nI.id.Equals(playerId))
            {
                nI.GetComponent<PlayerManager>().ActionCardShowEvent(cards);
            }
        }
    }

    private void OnStatUptate(Message message)
    {
        playerId = message.GetString(0);
        string playerState = message.GetString(1);

        foreach (NetworkIdentity nI in allPlayers)
        {
            if (nI.id.Equals(playerId))
            {
                nI.GetComponent<PlayerManager>().SetStatus(playerState);
            }
        }
    }
    
    private void OnChatMesssgeReceived(Message message)
    {
        playerId = message.GetString(0);
        string msg = message.GetString(1);

        foreach (NetworkIdentity nI in allPlayers)
        {
            if (nI.id.Equals(playerId))
            {
                nI.GetComponent<PlayerManager>().ShowChatMessage(msg);
            }
        }
    }

}
