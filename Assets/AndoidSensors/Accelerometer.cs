using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour{

    public static Accelerometer Instance;
    public Vector3 acceleration;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        acceleration = Input.acceleration;
    }
}
