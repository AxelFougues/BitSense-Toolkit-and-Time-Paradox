using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BitSenseOutput))]
[RequireComponent(typeof(BitSenseInput))]
public class BitSenseManager : MonoBehaviour{

    [HideInInspector]
    public GeometryManager geometryManager;

    void Start() {
        geometryManager = GetComponentInChildren<GeometryManager>();
        geometryManager.running = false;
    }

    public void setMap(Material map) {
        geometryManager.setMap(map);
    }

    public void run() {
        geometryManager.running = true;
        //bitSenseOutput.startOutput();
    }

    public void pause() {
        geometryManager.running = false;
        //bitSenseOutput.stopOutput();
    }

    public void setStereoProjectionAngle(float angle) {
        geometryManager.stereoProjectionAngle = angle;
    }

    public void setVisualDebug(bool state) {
        geometryManager.visualDebug = state;
    }

}
