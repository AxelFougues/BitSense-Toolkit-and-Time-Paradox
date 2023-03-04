using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level ScriptableObject")]
public class Level : ScriptableObject {
    public static int obstacleLayer = 2;
    public static int goalLayer = 1;

    public Material map;
    public Reward reward;
    public int number;
    public string descriptionText;

    [Range(0, 1)]
    [Tooltip("0: invincible, 1: death on presence of layer")]
    public float difficulty = 0;

    public int completed = 0;
    public int played = 0;


    public void reset() {
        completed = played = 0;
    }

}

    
