using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraControl cameraControl;

    public GameObject[] cameraTargets;
    public PlayerManager player;

    public SpawnManagers spawners;
    public int gameDifficulty = 0;
    public int maxDifficulty = 9;
    public float diffStepUpTime = 10f; //Time interval between each difficulty increase
    private float timer = 0;

    // Player scores
    private bool gameActive = false;
    private float gameTime = 0f;
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
            gameTime += Time.deltaTime;

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
                spawners.IncreaseDifficulty(gameDifficulty);
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

    public void GameSetup()
    {
        cameraControl.StartingScreenSize();
        player.Setup();
    }

    public void GameIntro()
    {
        //Game Introduction Cutscene
        cameraControl.IngameScreenSize();
    }

    //Scene goes back to menu
    public void GameOutro()
    {
        cameraControl.StartingScreenSize();
        player.ResetPlayer();

        //WHERE I WOULD RESET SCORES.
    }

    //Results are shown
    public void GameEnd()
    {
        player.EndGame();
        gameActive = false;
        spawners.ResetSpawningManager();
        //Stop game here
    }

    public void GameStart()
    {
        //Game Start Point
        cameraControl.IngameDampTime();
        player.StartGame();
        gameActive = true;
        spawners.EnableSpawning();
    }
}
