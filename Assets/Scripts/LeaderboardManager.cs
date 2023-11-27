using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    private const string leaderboardId = "Overtime_Highscores";
    private const int rangeLimit = 5;

    private List<Score> scoreboard;
    public struct Score
    {
        public Score(int rank, string playerName, int score)
        {
            this.rank = rank;
            this.playerName = playerName;
            this.score = score;
        }

        public int rank;
        public string playerName;
        public int score;
    }

    private bool newHighscore = false;
    private Score highscore;

    private async void OnApplicationQuit()
    {
       await AddScore();
    }

    private async void Awake()
    {
        highscore = new Score(-1, "NUL", -1);
        scoreboard = new List<Score>();
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await GetScores();
    }

    private async Task GetScores()
    {
        scoreboard.Clear();

        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = 5 });

        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            if (i >= 5)
            {
                break;
            }
            scoreboard.Add(new Score(scoresResponse.Results[i].Rank+1,
                                     scoresResponse.Results[i].PlayerName,
                                     (int) scoresResponse.Results[i].Score));
        }

        return;
    }

    public async Task AddScore()
    {
        if (!newHighscore) return;
        int rank = await GetPlayerLeaderboardRank(highscore.score);
        if (rank == -1) return;

        if (!newHighscore) return;

        if (scoreboard.Count < 5 || highscore.score > scoreboard[scoreboard.Count-1].score)
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(highscore.playerName.ToUpper());
            var playerEntry = await LeaderboardsService.Instance
                .AddPlayerScoreAsync(leaderboardId, highscore.score);
            //Debug.Log(JsonConvert.SerializeObject(playerEntry));
        }

        highscore.score = 0;
        newHighscore = false;
    }

    public async Task<List<Score>> GetLeaderboard()
    {
        await GetScores();
        return scoreboard;
    }

    public async Task<int> GetPlayerLeaderboardRank(int playerScore)
    {
        await GetScores();

        for(int i=0; i<scoreboard.Count; i++)
        {
            if(playerScore > scoreboard[i].score)
            {
                newHighscore = true;
                highscore.score = playerScore;
                highscore.rank = scoreboard[i].rank;
                return highscore.rank;
            }
        }

        if (scoreboard.Count < 5)
        {
            newHighscore = true;
            highscore.score = playerScore;
            highscore.rank = scoreboard.Count+1;
            return highscore.rank;
        }

        newHighscore = false;
        return -1;
    }

    public void UpdateCurrentHighscoreName(string name)
    {
        highscore.playerName = name;
    }

    public int GetPredictedHighscore()
    {
        if (scoreboard.Count == 0) return -1;
        return scoreboard[scoreboard.Count - 1].score;
    }
}
