using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings ScriptableObject")]
public class GameSettings : ScriptableObject{

    [Header("Output")]

    public bool monoEnabled = true;
    public bool stereoEnabled = false;


    [Range(0, 20)]
    public float stereoProjectionAngle = 10;
    [Range(0, 90)]
    public float stereoOutwardAngle = 20;

    public bool directionBased = false;
    
    [Header("Input")]

    public bool visualDebug = true;

    
}
