using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    public LeaderboardManager leaderboardmanager;

    public GameObject[] placements;
    public TMPro.TMP_Text[] scores;
    public TMPro.TMP_Text[] names;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<placements.Length; i++)
        {
            placements[i].SetActive(false);
        }
    }

    public async void DisplayLeaderboard()
    {
        List<LeaderboardManager.Score> leaderboard = await leaderboardmanager.GetLeaderboard();
        for(int i = 0; i<5; i++)
        {
            if (i < leaderboard.Count)
            {
                placements[i].SetActive(true);
                scores[i].text = leaderboard[i].score.ToString();
                names[i].text = leaderboard[i].playerName.Substring(0, 3);
            }
            else
            {
                placements[i].SetActive(false);
            }
        }
    }
}
