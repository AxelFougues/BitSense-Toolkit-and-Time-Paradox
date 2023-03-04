using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphericalInputScreenBehaviour : MonoBehaviour{

    public GameObject titleScreen;
    public GameObject stereoParameters;
    public GameObject sphericalManager;
    public GameObject angleSlider;


    private void Start() {
        sphericalManager.GetComponent<PlayerInput>().setStereoAngle(angleSlider.GetComponent<Slider>().value);
    }

    public void onBackButtonClick() {
        titleScreen.SetActive(true);
        gameObject.SetActive(false);
        sphericalManager.SetActive(false);
    }

    public void onStereoButtonClick() {
        stereoParameters.SetActive(!stereoParameters.activeSelf);
        sphericalManager.GetComponent<PlayerOutput>().centerOnly = !sphericalManager.GetComponent<PlayerOutput>().centerOnly;
    }

    public void onMapButtonClick() {
        sphericalManager.GetComponent<PlayerInput>().nextMap();
    }

    public void onAngleChange() {
        sphericalManager.GetComponent<PlayerInput>().setStereoAngle(angleSlider.GetComponent<Slider>().value);
    }

    public void onToggleSound() {
        sphericalManager.GetComponent<PlayerOutput>().toggleOutput();
    }


}
