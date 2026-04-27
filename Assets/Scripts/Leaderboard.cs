using UnityEngine;
using LootLocker.Requests;
using System.Collections;

public class Leaderboard : MonoBehaviour
{
    private string leaderboardKey = "swarm_leaderboard";

    public void SubmitScore(string playerName, int score, System.Action<bool> callback)
    {
        StartCoroutine(SubmitScoreRoutine(playerName, score, callback));
    }

    IEnumerator SubmitScoreRoutine(string playerName, int score, System.Action<bool> callback)
    {
        bool sessionStarted = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            sessionStarted = response.success;
        });

        yield return new WaitUntil(() => sessionStarted);

        bool done = false;
        bool success = false;

        LootLockerSDKManager.SubmitScore(playerName, score, leaderboardKey, (response) =>
        {
            success = response.success;
            done = true;
        });

        yield return new WaitUntil(() => done);
        callback(success);
    }

    public void GetTopScores(System.Action<LootLockerLeaderboardMember[]> callback)
    {
        StartCoroutine(GetScoresRoutine(callback));
    }

    IEnumerator GetScoresRoutine(System.Action<LootLockerLeaderboardMember[]> callback)
    {
        bool sessionStarted = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            sessionStarted = response.success;
        });

        yield return new WaitUntil(() => sessionStarted);

        LootLockerSDKManager.GetScoreList(leaderboardKey, 5, (response) =>
        {
            if (response.success)
                callback(response.items);
            else
                callback(null);
        });
    }
}