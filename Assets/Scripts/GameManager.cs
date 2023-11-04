using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraControl cameraControl;

    public GameObject[] cameraTargets;
    public PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        player.Setup();
    }

    public void GameStart()
    {
        player.StartGame();
    }

    public void GameEnd()
    {

    }
}
