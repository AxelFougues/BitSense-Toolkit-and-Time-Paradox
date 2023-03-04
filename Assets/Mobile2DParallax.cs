using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile2DParallax : MonoBehaviour{

    static int smoothing = 25;

    public float speed;
    public float verticalClamp;
    public float horizontalClamp;
    
    Vector2 offset = Vector2.zero;
    Vector2 initial;
    Queue<Vector3> smoother;

    private void Start() {
        initial = transform.position;
        smoother = new Queue<Vector3>();
    }

    void Update() {
        if (Input.acceleration != null) {
            Vector3 acceleration = getSmoothedAcceleration(Input.acceleration.normalized);
            offset.x = acceleration.x * speed * horizontalClamp;
            offset.y = (-acceleration.y - 0.7f) * speed * verticalClamp;
            offset.x = -Mathf.Clamp(offset.x, -horizontalClamp, horizontalClamp) + initial.x;
            offset.y = Mathf.Clamp(offset.y, -verticalClamp, verticalClamp) + initial.y;
            float step = speed * Time.deltaTime;
            transform.position = new Vector3(offset.x, offset.y, transform.position.z); //Vector3.MoveTowards(transform.position, new Vector3(offset.x, offset.y, transform.position.z), step);
        }
    }

    Vector3 getSmoothedAcceleration(Vector3 raw) {
        smoother.Enqueue(raw);
        if (smoother.Count > smoothing) {
            smoother.Dequeue();
        }
        Vector3 average = Vector3.zero;
        foreach (Vector3 vec in smoother) average += vec;
        return average / smoother.Count;
    }

}
