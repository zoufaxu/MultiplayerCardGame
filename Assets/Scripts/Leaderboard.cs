using System;
using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public Transform contentTransform;
    public LeaderboardData leaderboardDataObj;
    List<LeaderboardData> allEntries = new List<LeaderboardData>();

    private void Start() {
        NetworkClient.Instance.onLeaderboardRefresh += OnLeaderboardRefresh;
    }

    private void OnLeaderboardRefresh(LeaderboardEntry[] entries)
    {
        foreach (var entry in entries)
        {
            LeaderboardData leaderboardData = Instantiate(leaderboardDataObj , contentTransform);
            leaderboardData.ShowData(entry.ConnectUserId , entry.Score.ToString());
            allEntries.Add(leaderboardData);
        }
    }
}
