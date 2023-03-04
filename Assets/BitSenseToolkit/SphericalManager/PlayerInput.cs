using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool visual;
    public Material[] maps;

    int currentMap = 0;
    [HideInInspector]
    public float currentStereoAngle = 10;

    InputManager inputManager;

    void Start(){
        inputManager = GetComponentInChildren<InputManager>();
        if (maps.Length > 0) inputManager.setMap(maps[0]);
        if (!visual) {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) renderer.enabled = false;
        }
    }

    public Vector2 getPlaneProjectionCenter() {
        return inputManager.positionCenter;
    }

    public Vector2 getSphereProjectionCenter() {
        return inputManager.rotationCenter;
    }

    public Vector2 getPlaneProjectionLeft() {
        return inputManager.positionLeft;
    }

    public Vector2 getPlaneProjectionRight() {
        return inputManager.positionRight;
    }

    public MultiChannelSphericalSample getLastSample() {
        return inputManager.lastSample;
    }

    public void nextMap() {
        if (currentMap < maps.Length - 1) currentMap++;
        else currentMap = 0;
        inputManager.setMap(maps[currentMap]);
    }

    public void setStereoAngle(float angle) {
        currentStereoAngle = angle;
    }
}
