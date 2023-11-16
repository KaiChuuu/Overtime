using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public Transform mapDivider;

    private CanvasManager canvasManager;
    private GameObject player;

    //In seconds
    public float leftMaxTime = 60f;
    public float rightMaxTime = 60f;
    private float leftTimer = 0f;
    private float rightTimer = 0f;

    public float gameTime = 0f;

    private bool gameActive = false;

    public float freezeDuration = 5f; //Also the time gained within the duration
    private bool freezeTime = false;
    private float timer = 0f;

    public void Setup(ref CanvasManager canvas, ref GameObject gamePlayer)
    {
        canvasManager = canvas;
        player = gamePlayer;
        ResetTimes();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            if (!freezeTime)
            {
                if (player.transform.position.x < mapDivider.position.x)
                {
                    //Player is on left side of map
                    if (leftTimer > 0f)
                    {
                        leftTimer -= Time.deltaTime;
                        UpdateSideTimerUI(leftTimer, "left", true);
                    }
                    else
                    {
                        //Game Over
                    }

                    IncreaseRightTimer(true);
                }
                else
                {
                    //Player is on right side of map
                    if (rightTimer > 0f)
                    {
                        rightTimer -= Time.deltaTime;
                        UpdateSideTimerUI(rightTimer, "right", true);
                    }
                    else
                    {
                        //Game Over
                    }

                    IncreaseLeftTimer(true);
                }

                //Check if times are equal
                if (Mathf.FloorToInt(leftTimer) == Mathf.FloorToInt(rightTimer))
                {
                    UpdateFrozenGame();
                    freezeTime = true;
                }
            }
            else
            {
                //Freeze is active
                timer += Time.deltaTime;

                IncreaseLeftTimer(false);
                IncreaseRightTimer(false);

                if (timer > freezeDuration)
                {
                    //Unfreeze time
                    freezeTime = false;
                    timer = 0f;

                    //Update colors to remove frozen effect
                    UpdateSideTimerUI(rightTimer, "right", true);
                    UpdateSideTimerUI(leftTimer, "left", true);

                    canvasManager.Unfreeze();
                }
      
            }

            UpdateGameTimer(gameTime);
        }
    }

    void IncreaseRightTimer(bool display)
    {
        //Increase Right timer
        if (rightTimer <= 60f)
        {
            rightTimer += Time.deltaTime;
            UpdateSideTimerUI(rightTimer, "right", display);
        }
    }

    void IncreaseLeftTimer(bool display) 
    {
        //Increase Left timer
        if (leftTimer <= 60f)
        {
            leftTimer += Time.deltaTime;
            UpdateSideTimerUI(leftTimer, "left", display);
        }
    }

    void UpdateFrozenGame()
    {
        canvasManager.UpdateFreezeColor();
    }

    void UpdateSideTimerUI(float time, string side, bool display)
    {
        float currentTime = Mathf.FloorToInt(time);
        switch (side)
        {
            case "left":
                canvasManager.UpdateLeftTimer(currentTime, display);
                break;
            case "right":
                canvasManager.UpdateRightTimer(currentTime, display);
                break;
        }
    }

    void UpdateGameTimer(float time)
    {
        float minute = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00} : {1:00}", minute, seconds);
        canvasManager.UpdateGameTime(currentTime);
    }

    public void UpdateGameStatus(bool active)
    {
        gameActive = active;
    }
    

    public void ResetTimes()
    {
        leftTimer = leftMaxTime;
        rightTimer = rightMaxTime;

        //Update side timer UI
        canvasManager.UpdateLeftTimer(leftTimer, true);
        canvasManager.UpdateRightTimer(rightTimer, true);
        
        gameTime = 0f;

        //Update game UIs
        string currentTime = string.Format("{00:00} : {1:00}", 0, 0);
        canvasManager.UpdateGameTime(currentTime);
    }
}
