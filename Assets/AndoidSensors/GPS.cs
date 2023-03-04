using UnityEngine;
using System.Collections;
using System;

public class GPS : MonoBehaviour {

    public static GPS Instance { set; get; }
    public float desiredAccuracyInMeters, updateDistanceInMeters;
    public double latitude;
    public double longitude;
    public double initialLat;
    public double initialLon;
    public double zeroedLat;
    public double zeroedLon;

    public double nTlatitude = 0;
    public double nTlongitude = 0;
    public double nTinitialLat = 0;
    public double nTinitialLon = 0;
    public double nTzeroedLat = 0;
    public double nTzeroedLon = 0;

    private void Start() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService() {
        if (!Input.location.isEnabledByUser) {
            Debug.Log("Location not enabled.");
            //yield break;
        }

        Input.location.Start(1f, 0.1f);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait <= 0) {
            Debug.Log("Location initialization timed out.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.Log("Location initialization failed.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Running) {
            initialLat = ((40007863.0 / 180.0) * (90.0 - Input.location.lastData.latitude));
            initialLon = ((40075.017 / 360.0) * (180.0 + Input.location.lastData.longitude));
            //initialPosition.y = Input.location.lastData.altitude;
        }
        NativeToolkit.StartLocation();
        
    }

    private void Update() {
        if (nTinitialLon == 0 || nTinitialLat == 0) {
            nTinitialLat = ((40007863.0 / 180.0) * (90.0 - NativeToolkit.GetLatitude()));
            nTinitialLon = ((40075.017 / 360.0) * (180.0 + NativeToolkit.GetLongitude()));
        } else {
            nTlatitude = ((40007863.0 / 180.0) * (90.0 - NativeToolkit.GetLatitude()));
            nTlongitude = ((40075.017 / 360.0) * (180.0 + NativeToolkit.GetLongitude()));
            nTzeroedLat = nTlatitude - nTinitialLat;
            nTzeroedLon = nTlongitude - nTinitialLon;
        }

        if (Input.location.status == LocationServiceStatus.Running) {
            latitude = ((40007863.0/180.0) * (90.0 - Input.location.lastData.latitude));
            longitude = ((40075.017/360.0) * (180.0 + Input.location.lastData.longitude));
            //position.y = Input.location.lastData.altitude;
        }
        zeroedLat = latitude - initialLat;
        zeroedLon = longitude - initialLon;
    }
}