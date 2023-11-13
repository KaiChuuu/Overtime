using UnityEngine;

[CreateAssetMenu(fileName = "MaterialColorTypes", menuName = "ScriptableObjects/MaterialColorSO")]
public class MaterialColorSO : ScriptableObject
{
    public Color baseColor;

    public Color emissionColor;
    public float emissionStrength;
}
