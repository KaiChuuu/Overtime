using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public PlayerGates[] stage3Gates;
    public Material[] stage3Walls;

    public PlayerGates[] stage2Gates;
    public Material[] stage2LeftWalls;
    public Material[] stage2RightWalls;
    public Material[] stage2MiddleWalls;
    private bool stage2Enabled = false;

    public Material[] leftWalls;
    public Material[] rightWalls;
    public Material[] middleWalls;

    public MaterialColorSO maxHP;
    public MaterialColorSO lowHP;
    public MaterialColorSO freeze;
    public MaterialColorSO defaultColor;
    public MaterialColorSO inactiveColor;

    private int clockTimes = 60; //Assuming clocks start 60

    void Start()
    {
        ResetStages();
    }

    public void DefaultWalls()
    {
        foreach (Material left in leftWalls)
        {
            left.SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
        }
        foreach (Material right in rightWalls)
        {
            right.SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
        }
        foreach (Material middle in middleWalls)
        {
            middle.SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
        }
    }

    public void UpdateLeftWalls(float time)
    {
        foreach (Material left in leftWalls)
        {
            left.SetColor("_EmissionColor",
                    Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
        }

        if (stage2Enabled)
        {
            foreach (Material left2 in stage2LeftWalls)
            {
                left2.SetColor("_EmissionColor",
                        Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
            }
        }
    }

    public void UpdateRightWalls(float time)
    {
        foreach (Material right in rightWalls)
        {
            right.SetColor("_EmissionColor",
                    Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
        }

        if (stage2Enabled)
        {
            foreach (Material right2 in stage2RightWalls)
            {
                right2.SetColor("_EmissionColor",
                        Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
            }
        }
    }

    public void FreezeColor()
    {
        foreach (Material left in leftWalls)
        {
            left.SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
        }
        foreach (Material right in rightWalls)
        {
            right.SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
        }

        if (stage2Enabled)
        {
            foreach (Material left2 in stage2LeftWalls)
            {
                left2.SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
            }
            foreach (Material right2 in stage2RightWalls)
            {
                right2.SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
            }
        }
    }

    public void OpenStages(int difficulty)
    {
        switch (difficulty) 
        {
            case 2:
                foreach(PlayerGates gates in stage2Gates)
                {
                    gates.DeactivateGate();
                }
                StartCoroutine(WarmupStage2Walls());
                //Remove fog
                break;
            case 4:
                foreach(PlayerGates gates in stage3Gates)
                {
                    gates.DeactivateGate();
                }
                break;

        }
    }

    IEnumerator WarmupStage2Walls()
    {
        //Wall wake up glow
        float emissionValue = 0f;
        for (int i = 0; i < 100; i++)
        {
            emissionValue += 0.01f;
            foreach (Material left in stage2LeftWalls)
            {
                left.SetColor("_EmissionColor",
                    Color.Lerp(inactiveColor.emissionColor * inactiveColor.emissionStrength, 
                        leftWalls[0].GetColor("_EmissionColor"), emissionValue));
            }
            foreach (Material right in stage2RightWalls)
            {
                right.SetColor("_EmissionColor",
                    Color.Lerp(inactiveColor.emissionColor * inactiveColor.emissionStrength, 
                        rightWalls[0].GetColor("_EmissionColor"), emissionValue));
            }
            foreach (Material mid in stage2MiddleWalls)
            {
                mid.SetColor("_EmissionColor",
                    Color.Lerp(inactiveColor.emissionColor * inactiveColor.emissionStrength, 
                        defaultColor.emissionColor * defaultColor.emissionStrength, emissionValue));
            }
            yield return new WaitForSeconds(0.1f); //Tweak this to modify warmup timing, current duration is 10 seconds (5 sec is 0.05)
        }

        //Sign up left and right walls accordingly
        stage2Enabled = true;
    }

    public void ResetStages()
    {
        foreach (Material left in stage2LeftWalls)
        {
            left.SetColor("_EmissionColor", inactiveColor.emissionColor * inactiveColor.emissionStrength);
        }
        foreach (Material right in stage2RightWalls)
        {
            right.SetColor("_EmissionColor", inactiveColor.emissionColor * inactiveColor.emissionStrength);
        }
        foreach (Material mid in stage2MiddleWalls)
        {
            mid.SetColor("_EmissionColor", inactiveColor.emissionColor * inactiveColor.emissionStrength);
        }
        stage2Enabled = false;

        foreach (PlayerGates gates in stage2Gates)
        {
            gates.ReactivateGate();
        }
       
    }
}
