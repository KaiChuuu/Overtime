using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Material[] leftWalls;
    
    public Material[] rightWalls;
    
    public Material[] middleWalls;

    public MaterialColorSO maxHP;
    public MaterialColorSO lowHP;
    public MaterialColorSO freeze;
    public MaterialColorSO defaultColor;

    private int clockTimes = 60; //Assuming clocks start 60

    public void DefaultWalls()
    {
        leftWalls[0].SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
        leftWalls[1].SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);

        rightWalls[0].SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
        rightWalls[1].SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);

        middleWalls[0].SetColor("_EmissionColor", defaultColor.emissionColor * defaultColor.emissionStrength);
    }

    public void UpdateLeftWalls(float time)
    {
        leftWalls[0].SetColor("_EmissionColor",
            Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);

        leftWalls[1].SetColor("_EmissionColor",
            Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
    }

    public void UpdateRightWalls(float time)
    {
        rightWalls[0].SetColor("_EmissionColor",
            Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);

        rightWalls[1].SetColor("_EmissionColor",
            Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
    }

    public void FreezeColor()
    {
        leftWalls[0].SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
        leftWalls[1].SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);

        rightWalls[0].SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
        rightWalls[1].SetColor("_EmissionColor", freeze.emissionColor * freeze.emissionStrength);
    }
}
