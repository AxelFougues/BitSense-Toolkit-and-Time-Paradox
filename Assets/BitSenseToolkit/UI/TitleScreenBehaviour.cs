using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenBehaviour : MonoBehaviour{

    public GameObject signalGeneratorScreen;
    public GameObject sphericalInputScreen;

    public GameObject sphericalManager;

    private void Start() {
    }

    public void onSignalGeneratorButtonClick() {
        gameObject.SetActive(false);
        signalGeneratorScreen.SetActive(true);
    }

    public void onSphericalInputButtonClick() {
        gameObject.SetActive(false);
        sphericalInputScreen.SetActive(true);
        sphericalManager.SetActive(true);
    }

    public void onGameVisualizerButtonClick() {
        SceneManager.LoadScene("Level");
    }
}
