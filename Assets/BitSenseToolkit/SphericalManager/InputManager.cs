using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gyroscope))]
[RequireComponent(typeof(Accelerometer))]
public class InputManager : MonoBehaviour { 

    [Header("SpericalManager References")]
    //Input Manager Refs
    public GameObject sphere;
    public GameObject plane;

    public GameObject positionIndicatorLeft;
    public GameObject positionIndicatorCenter;
    public GameObject positionIndicatorRight;

    public GameObject rotationIndicatorLeft;
    public GameObject rotationIndicatorCenter;
    public GameObject rotationIndicatorRight;

    public GameObject sphericalManager;

    //OUT values
    //center
    [HideInInspector]
    public Vector2 positionCenter = Vector2.zero;
    [HideInInspector]
    public Vector3 rotationCenter = Vector3.zero;
    //left right
    [HideInInspector]
    public Vector2 positionLeft = Vector2.zero;
    [HideInInspector]
    public Vector2 positionRight = Vector2.zero;
    [HideInInspector]
    public Vector3 rotationLeft = Vector2.zero;
    [HideInInspector]
    public Vector3 rotationRight = Vector2.zero;
    //Smaple
    public MultiChannelSphericalSample lastSample;

    //Visuals and indicator updates
    [HideInInspector]
    public bool indicatorUpdates = true;
    [HideInInspector]
    Material currentMap;
    Texture2D currentMapTexture;
    float stereoAngle = 10;
    int layer;


    private void Start() {
        layer = (1 << gameObject.layer);
        currentMap = sphere.GetComponent<MeshRenderer>().material;
        currentMapTexture = currentMap.mainTexture as Texture2D;
        lastSample = new MultiChannelSphericalSample(0);
    }

    private void Update() {
        if (sphericalManager != null) updateFromSphericalManager();
        if (Gyroscope.Instance.gyroEnabled) {
            //Caster follows Gyro rotation
            transform.localRotation = Gyroscope.Instance.rotation; //NO INITIAL RECENTERING, DOESN'T MATTER JUST RECENTER WHEN MAPPING
        }
    }

    void FixedUpdate() {
        castToSphere();
        if (indicatorUpdates) {
            //Update mono indicators
            if (positionIndicatorCenter != null) positionIndicatorCenter.transform.position = plane.transform.position + new Vector3(0.5f - positionCenter.x, 0f, 0.5f - positionCenter.y);
            if (rotationIndicatorCenter != null) {
                rotationIndicatorCenter.transform.position = rotationCenter;
            }
            //Update stereo indicators
            if (positionIndicatorLeft != null) positionIndicatorLeft.transform.position = plane.transform.position + new Vector3(0.5f - positionLeft.x, 0f, 0.5f - positionLeft.y);
            if (rotationIndicatorLeft != null) {
                rotationIndicatorLeft.transform.position = rotationLeft;
            }
            if (positionIndicatorRight != null) positionIndicatorRight.transform.position = plane.transform.position + new Vector3(0.5f - positionRight.x, 0f, 0.5f - positionRight.y);
            if (rotationIndicatorRight != null) {
                rotationIndicatorRight.transform.position = rotationRight;
            }
        }
    }

    void castToSphere() {
        MultiChannelSphericalSample outChannels = new MultiChannelSphericalSample(transform.rotation.eulerAngles.z);
        //center
        RaycastHit hit;
        if (Physics.Raycast(sphere.transform.position, transform.forward, out hit, 10, layer)) {
            Debug.DrawRay(sphere.transform.position, transform.forward, Color.green);
            positionCenter = hit.textureCoord;
            rotationCenter = hit.point;
            lastSample.addChannel("center", currentMapTexture, positionToPixel(positionCenter));
        } else {
            positionCenter = Vector2.zero;
            rotationCenter = Vector3.zero;
        }

        //Left Right
        RaycastHit hitLeft, hitRight;
        if (Physics.Raycast(sphere.transform.position, Quaternion.AngleAxis(-stereoAngle/2, transform.up) * transform.forward , out hitLeft, 10, layer) && Physics.Raycast(sphere.transform.position, Quaternion.AngleAxis(stereoAngle / 2, transform.up) * transform.forward, out hitRight, 10, layer)) {
            Debug.DrawRay(sphere.transform.position, Quaternion.AngleAxis(-stereoAngle / 2, transform.up) * transform.forward, Color.blue);//Debug ray for Left
            Debug.DrawRay(sphere.transform.position, Quaternion.AngleAxis(stereoAngle / 2, transform.up) * transform.forward, Color.red);// Debug ray for right
            positionLeft = hitLeft.textureCoord;
            positionRight = hitRight.textureCoord;
            rotationLeft = hitLeft.point;
            rotationRight = hitRight.point;
            lastSample.addChannel("left", currentMapTexture, positionToPixel(positionLeft));
            lastSample.addChannel("right", currentMapTexture, positionToPixel(positionRight));
        } else {
            positionLeft = positionRight = Vector2.zero;
            rotationRight = rotationLeft = Vector3.zero;
        }
        
    }

    void updateFromSphericalManager() {
        stereoAngle = sphericalManager.GetComponent<PlayerInput>().currentStereoAngle;
        indicatorUpdates = sphericalManager.GetComponent<PlayerInput>().visual;
    }

    Vector2 positionToPixel(Vector2 position) {
        int height = currentMapTexture.height;
        int width = currentMapTexture.width;
        return new Vector2((int)(width * position.x), (int)(height * position.y));
    }

    public void setMap(Material map) {
        currentMap = map;
        currentMapTexture = currentMap.mainTexture as Texture2D;
        sphere.GetComponent<MeshRenderer>().material = map;
        plane.GetComponent<MeshRenderer>().material = map;
    }

    
}
