using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitSenseInput : MonoBehaviour{

    BitSenseManager BSManager;

    private void Start() {
        BSManager = GetComponent<BitSenseManager>();
    }

    public Vector2 getPlaneProjectionCenter() {
        return BSManager.geometryManager.positionCenter;
    }

    public Vector2 getSphereProjectionCenter() {
        return BSManager.geometryManager.rotationCenter;
    }

    public Vector2 getPlaneProjectionLeft() {
        return BSManager.geometryManager.positionLeft;
    }

    public Vector2 getPlaneProjectionRight() {
        return BSManager.geometryManager.positionRight;
    }

    public MultiChannelSphericalSample getLastSample() {
        return BSManager.geometryManager.lastSample;
    }

}
