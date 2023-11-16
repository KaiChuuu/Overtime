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
    }

    public void UpdateRightWalls(float time)
    {
        foreach (Material right in rightWalls)
        {
            right.SetColor("_EmissionColor",
                    Color.Lerp(lowHP.emissionColor, maxHP.emissionColor, time / clockTimes) * defaultColor.emissionStrength);
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
    }
}
