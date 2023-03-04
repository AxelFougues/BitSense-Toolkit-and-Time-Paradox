using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviour : MonoBehaviour{

    public static float animationSpeed = 10;
    public static float distance = 100;

    private void OnEnable() {
        transform.position += Vector3.down * distance;
    }

    private void FixedUpdate() {
        if (transform.position.y < 0) {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, animationSpeed);
        }
    }
}
