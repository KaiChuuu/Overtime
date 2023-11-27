using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LeaderboardManager leaderboardmanager;

    public CanvasManager canvasManager;
    public EnvironmentManager envManager;
    public CameraControl cameraControl;

    public GameObject[] cameraTargets;
    public PlayerManager player;

    public SpawnManagers spawners;
    public int gameDifficulty = 0;
    public int maxDifficulty = 9;
    public float diffStepUpTime = 10f; //Time interval between each difficulty increase
    private float timer = 0;

    public GameTimeManager timeManager;

    private bool predicationActivated = false;

    private bool gameActive = false;

    // Player scores
    private int totalKills = 0;
    private int totalScore = 0;
    //

    // Start is called before the first frame update
    void Start()
    {
        SetCameraTargets();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            UpdateDifficulty();
        }
    }

    void UpdateDifficulty()
    {
        if (gameDifficulty < maxDifficulty)
        {
            timer += Time.deltaTime;

            if (timer > diffStepUpTime)
            {
                timer = 0;
                gameDifficulty++;

                //**These methods are sensitive to difficulty change**
                spawners.IncreaseDifficulty(gameDifficulty);
                envManager.OpenStages(gameDifficulty);
            }
        }
    }

    private void SetCameraTargets()
    {
        //Create collection of transforms for each tank
        Transform[] gameTargets = new Transform[cameraTargets.Length];

        for (int i = 0; i < cameraTargets.Length; i++)
        {
            //Set targets to each tank transform
            gameTargets[i] = cameraTargets[i].transform;
        }

        //Set targets camera will follow
        cameraControl.targets = gameTargets;
    }

    //Only on game launch
    public void GameSetup()
    {
        cameraControl.StartingScreenSize();
        player.Setup(ref canvasManager);
        spawners.Setup(ref player.player);
        timeManager.Setup(ref canvasManager, ref player.player);
    }

    public void GameIntro()
    {
        //Game Introduction Cutscene
        cameraControl.IngameScreenSize();
    }

    //Scene goes back to menu
    public void GameOutro()
    {
        envManager.ResetStages();
        cameraControl.StartingScreenSize();
        player.ResetPlayer();

        //Reset game stats
        gameDifficulty = 0;
        timer = 0f;
        timeManager.ResetTimes();
        totalKills = 0;
        totalScore = 0;
        canvasManager.UpdateGameScore(totalScore);
        canvasManager.UpdateGameScoreColor("White");
        predicationActivated = false;
    }

    //Player endscreen delay; Game stops here
    public async void GameEnd()
    {
        //Determine if its a top 5 highscore
        int rank = await leaderboardmanager.GetPlayerLeaderboardRank(totalScore);
        bool isHighscore = false;
        if(rank != -1)
        {
            isHighscore = true;
        }

        canvasManager.GameEndScores(totalScore, totalKills, isHighscore, rank);
        player.EndGame();
        gameActive = false;
        timeManager.UpdateGameStatus(false);
        spawners.DisableSpawning();
    }

    //Results are shown
    public void EndScreen()
    {
        spawners.ResetSpawningManager();
    }

    public void GameStart()
    {
        //Game Start Point
        cameraControl.IngameDampTime();
        player.StartGame();
        gameActive = true;
        spawners.EnableSpawning();
        timeManager.UpdateGameStatus(true);
    }

    public void UpdateKillCount(int enemyScore)
    {
        totalScore += enemyScore;
        totalKills++;

        if (!predicationActivated && totalScore > leaderboardmanager.GetPredictedHighscore())
        {
            canvasManager.UpdateGameScoreColor("Blue");
            predicationActivated = true;
        }

        canvasManager.UpdateGameScore(totalScore);
    }

    public void UpdateLeftGameColor(float time)
    {
        envManager.UpdateLeftWalls(time);
    }

    public void UpdateRightGameColor(float time)
    {
        envManager.UpdateRightWalls(time);
    }

    public void UpdateBothGameColor(string name)
    {
        switch (name)
        {
            case "freeze":
                envManager.FreezeColor();
                spawners.FreezeEnemies();
                break;
            case "default":
                envManager.DefaultWalls();
                break;
        }
    }

    public void Unfreeze()
    {
        spawners.UnfreezeEnemies();
    }
}
