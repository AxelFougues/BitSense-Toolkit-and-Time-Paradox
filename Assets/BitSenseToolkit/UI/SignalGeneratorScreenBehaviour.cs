using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignalGeneratorScreenBehaviour : MonoBehaviour{

    public GameObject titleScreen;
    public GameObject frequencySlider;
    public GameObject panSlider;
    public GameObject sineSlider;
    public GameObject sawSlider;
    public GameObject sqareSlider;
    public GameObject amplitudeModulationFrequencySlider;
    public GameObject frequencyModulationFrequencySlider;
    public GameObject frequencyModulationIntensitySlider;

    SignalGenerator signalGenerator;

    private void Start() {
        signalGenerator = GameObject.Find("SignalGenerator").GetComponent<SignalGenerator>();
    }

    public void onBackButtonClick() {
        titleScreen.SetActive(true);
        gameObject.SetActive(false);
    }

    public void onToggleSound() {
        signalGenerator.enabled = !signalGenerator.enabled;
    }

    public void onSineChange() {
        signalGenerator.sinusAudioWaveIntensity = sineSlider.GetComponent<Slider>().value;
    }

    public void onSawChange() {
        signalGenerator.sawAudioWaveIntensity = sawSlider.GetComponent<Slider>().value;
    }

    public void onSquareChange() {
        signalGenerator.squareAudioWaveIntensity = sqareSlider.GetComponent<Slider>().value;
    }

    public void onFrequencyChange() {
        signalGenerator.mainFrequency = frequencySlider.GetComponent<Slider>().value;
    }

    public void onPanChange() {
        signalGenerator.stereoPan = panSlider.GetComponent<Slider>().value;
    }


    public void onAmplitudeModulationFrequencyChange() {
        signalGenerator.amplitudeModulationOscillatorFrequency = amplitudeModulationFrequencySlider.GetComponent<Slider>().value;
    }

    public void onFrequencyModulationFrequencyChange() {
        signalGenerator.frequencyModulationOscillatorFrequency = frequencyModulationFrequencySlider.GetComponent<Slider>().value;
    }

    public void onFrequencyModulationIntensityChange() {
        signalGenerator.frequencyModulationOscillatorIntensity = frequencyModulationIntensitySlider.GetComponent<Slider>().value;
    }

    public void onToggleAmpMod() {
        signalGenerator.useAmplitudeModulation = !signalGenerator.useAmplitudeModulation;
    }

    public void onToggleFreqMod() {
        signalGenerator.useFrequencyModulation = !signalGenerator.useFrequencyModulation;
    }
}
