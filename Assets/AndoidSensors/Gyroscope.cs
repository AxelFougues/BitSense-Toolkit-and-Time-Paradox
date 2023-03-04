using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;


public class Gyroscope : MonoBehaviour{

    [HideInInspector]
    public static Gyroscope Instance;
    [HideInInspector]
    public Quaternion rotation;
    [HideInInspector]
    public bool gyroEnabled;


    private UnityEngine.Gyroscope gyro;

    private void Start() {
        Instance = this;
        gyroEnabled = EnableGyro();
    }

    private void Update() {
        if (gyroEnabled) {
            rotation = GyroToUnity(gyro.attitude);
        }
    }

    private bool EnableGyro() {
        if (SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    private static Quaternion GyroToUnity(Quaternion q) {
        return new Quaternion(q.x, q.z, q.y, -q.w);
    }

}
