using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraControl cameraControl;

    public GameObject[] targets;
    public GameObject player;

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
        Transform[] gameTargets = new Transform[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            //Set targets to each tank transform
            gameTargets[i] = targets[i].transform;
        }

        //Set targets camera will follow
        cameraControl.targets = gameTargets;
    }

    public void GameStart()
    {

    }

    public void GameEnd()
    {

    }
}
