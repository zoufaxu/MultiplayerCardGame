using PlayerIOClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkClient : MonoSingleton<NetworkClient>
{
    public string gameId;
    private Client client;
    public Action<Message> onPlayerSpawned, onPlayerLeave, onRoundStarted, onTurnnChanged, onTransaction, onRoundOver, onActionShow, onStatUptate , onChatMesssgeReceived;
    public Action onActionPack;
    private List<Connection> allConnections = new List<Connection>();
    private Connection connection { get { return allConnections[allConnections.Count - 1]; } }
    public string GetID { get { return client.ConnectUserId; } }
    private string username;
    public Action<LeaderboardEntry[]> onLeaderboardRefresh;

    //注册
    public void RegisterUser(string userName, string password) {
        username = userName;
        if (string.IsNullOrEmpty(gameId))
        {
            print("gameid为空");
            return;
        }
        PlayerIO.Authenticate(gameId, "public", new Dictionary<string, string>
        {
            { "register", "true" },
            { "username", userName },
            { "password",password },
        },null,delegate (Client client) {
            print("注册成功");
        },
        delegate (PlayerIOError error) {
            print($"注册失败{error.Message}");

        });
    }
    //登录
    public void LoginUser(string userName, string password)
    {
        if (string.IsNullOrEmpty(gameId))
        {
            print("gameid为空");
            return;
        }
        PlayerIO.Authenticate(gameId, "public", new Dictionary<string, string>
        {
            { "username", userName },
            { "password",password },
        }, null, delegate (Client client) {
            print("登录成功");
            this.client = client;
            JoinLobbyRoom();
            client.BigDB.LoadMyPlayerObject((DatabaseObject databaseObj) =>
            {
                try
                {
                    string useName = databaseObj.GetString(NetworkConstant.DATABASE_USERNAME);
                }
                catch (Exception e)
                {

                    print($"数据库出错{e.Message}");
                    connection.Send(Message.Create(NetworkConstant.FIRST_TIME_LOGIN , username));
                }
            });
        },
        delegate (PlayerIOError error) {
            print($"登录失败{error.Message}");

        });
    }
    //进入大厅
    private void JoinLobbyRoom()
    {
        client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        client.Multiplayer.CreateJoinRoom("$service-room$", "Lobby", true ,null , null,                               
        (Connection connection) => 
        {
            print("加入大厅成功");
            allConnections.Add(connection);
            LevelManager.Instance.OnAuthenticationComplete();
        },
        (PlayerIOError error) => 
        {
            print($"加入大厅失败{error.Message}");
        });
    }
    public void JoinGameRoom()
    {
        string roomId = client.ConnectUserId + "" + DateTime.Now.ToString();

        client.Multiplayer.ListRooms("GameRoom", null, 5, 0 ,
            (RoomInfo[] rooms) =>
            {
                if (rooms.Length > 0)
                {
                    client.Multiplayer.JoinRoom(rooms[0].Id , null , 
                        (Connection connection) =>
                        {
                            print("加入房间成功");
                            allConnections.Add(connection);
                            connection.OnMessage += GameRoomMessageHanlder;
                            connection.OnDisconnect += DisconnectFromGameRoom;
                            LevelManager.Instance.OnJoinGameRoom();
                        });
                }
                else
                {
                    client.Multiplayer.CreateJoinRoom(roomId, "GameRoom", true, null, null,
                        (Connection connection) =>
                        {
                            print("加入房间成功");
                            allConnections.Add(connection);
                            connection.OnMessage += GameRoomMessageHanlder;
                            connection.OnDisconnect += DisconnectFromGameRoom;
                            LevelManager.Instance.OnJoinGameRoom();
                        });
                }
            } , (PlayerIOError error) =>{
                // client.Multiplayer.CreateJoinRoom(roomId, "GameRoom", true, null, null,
                //     (Connection connection) =>
                //     {
                //         print("加入房间成功");
                //         allConnections.Add(connection);
                //         connection.OnMessage += GameRoomMessageHanlder;
                //         connection.OnDisconnect += DisconnectFromGameRoom;
                //         LevelManager.Instance.OnJoinGameRoom();
                //     });
            });
        
    }



    private void GameRoomMessageHanlder(object sender, Message e)
    {
        switch (e.Type)
        {
            case NetworkConstant.SPAWN_LOCAL_PLAYER:
                onPlayerSpawned?.Invoke(e);
                break;
            case NetworkConstant.SPAWN_FOREIGN_PLAYER:
                onPlayerSpawned?.Invoke(e);
                break;
            case NetworkConstant.PLAYER_LEFT:
                onPlayerLeave?.Invoke(e);
                break;
            case NetworkConstant.ROUND_STARTED:
                onRoundStarted?.Invoke(e);
                break;
            case NetworkConstant.CHANGE_TURN:
                onTurnnChanged?.Invoke(e);
                break;
            case NetworkConstant.TRANSACTION:
                onTransaction?.Invoke(e);
                break;
            case NetworkConstant.ROUND_OVER:
                onRoundOver?.Invoke(e);
                break;
            case NetworkConstant.ACTION_PACK:
                onActionPack?.Invoke();
                break;
            case NetworkConstant.ACTION_SHOW:
                onActionShow?.Invoke(e);
                break;
            case NetworkConstant.ACTION_SHOW_FAILED:
                print("显示失败");
                break;
            case NetworkConstant.STATUS_UPDATE:
                onStatUptate(e);
                break;
            case NetworkConstant.CHAT_MESSAGE:
                onChatMesssgeReceived(e);
                break;
        }
    }
    private void DisconnectFromGameRoom(object sender, string message)
    {
        print("OnDisconnect");
        allConnections.RemoveAt(allConnections.Count - 1);
        LevelManager.Instance.OnAuthenticationComplete();
    }
    public void LeaveRoom()
    {
        connection.Disconnect();
    }
    public void DisconnectAll(){
        foreach (var conn in allConnections)
        {
            conn.Disconnect();
        }
    }
    public void ActionChaal()
    {
        connection.Send(Message.Create(NetworkConstant.ACTION_CHAAL));
    }
    public void ActionPack()
    {
        connection.Send(Message.Create(NetworkConstant.ACTION_PACK));
    }
    public void ActionShow()
    {
        connection.Send(Message.Create(NetworkConstant.ACTION_SHOW));
    }
    public void SendChatMessage(string message){
        connection.Send(NetworkConstant.CHAT_MESSAGE , message);
    }
    public void GetLeaderboardEntries()
    {
        client.Leaderboards.GetTop("chip" , null , 0 , 10 , null , (LeaderboardEntry[] entries) =>{
            onLeaderboardRefresh?.Invoke(entries);
        });
    } 
}
