using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BitSenseInput))]
public class BitSenseOutput : MonoBehaviour{

    //BitSenseManager BSManager;

    [Header("Signal generators references")]
    public SignalGenerator signalLeft;
    public SignalGenerator signalRight;
    public SignalGenerator signalCenter;

    //bool isStarted = false;



    //BitSenseInput BSInput;

    //MultiChannelSphericalSample currentIn;

    void Start() {
        //BSManager = GetComponent<BitSenseManager>();
        //BSInput = GetComponent<BitSenseInput>();

        //GENERATOR INIT
        //DEFAULT with no initialiser LA note on sine
        signalLeft.sinusAudioWaveIntensity = 1;
        signalRight.sinusAudioWaveIntensity = 1;
        signalCenter.sinusAudioWaveIntensity = 1;
        signalLeft.mainFrequency = 440;
        signalRight.mainFrequency = 440;
        signalCenter.mainFrequency = 440;
        //Assign channels 
        signalLeft.stereoPan = -1;
        signalRight.stereoPan = 1;
        signalCenter.stereoPan = 0;
    }

    /*
    void updateGenerator(SignalGenerator generator, MultiLayerSphericalSample channel) {//mono of blue for now;
        float value = 0;
        if (BSManager.gameSettings.directionBased) {//direction based
            value = (180 - channel.getDirectionOfLayer(2).x) /180; //using volume as direcion
            if (channel.getDirectionOfLayer(2).y == 0) value = 0;//check for slope
        } else value = channel.getIntensityOfLayer(2); //intensityBased

        signalCenter.masterVolume = value;

        //Debug.Log("Grad " + channel.getDirectionOfLayer(2).x + " slope " + channel.getDirectionOfLayer(2).y);
    }

    public void startOutput() {
        if (BSManager.gameSettings.monoEnabled) signalCenter.enabled = true;
        if (BSManager.gameSettings.stereoEnabled) { signalLeft.enabled = true; signalRight.enabled = true; }
        isStarted = true;
    }

    public void stopOutput() {
        signalCenter.enabled = false;
        signalLeft.enabled = false;
        signalLeft.enabled = false;
        isStarted = false;
    }

    public void toggleOutput() {
        if (isStarted) stopOutput();
        else startOutput();
    }
    */
    public void initialise(string channel, SignalGenerator initializer) {
        switch (channel) {
            case "left": signalLeft = initializer; ; break;
            case "right": signalRight = initializer; break;
            case "center": signalCenter = initializer; break;
            default: signalCenter = signalRight = signalLeft = initializer; break;
        }
        //Assign channels 
        signalLeft.stereoPan = -1;
        signalRight.stereoPan = 1;
        signalCenter.stereoPan = 0;
    }

    public void set(float value, string channel = "center", string modulationType = "volume") {
        SignalGenerator toSet;
        switch (channel) {
            case "left": toSet = signalLeft; break;
            case "right": toSet = signalRight; break;
            default: toSet = signalCenter; break;
        }
        switch (modulationType) {
            case "ampMod": toSet.amplitudeModulationOscillatorFrequency = value; break;
            case "freqMod": toSet.frequencyModulationOscillatorFrequency = value; break;
            default: toSet.masterVolume = value; break;
        }
    }

    public void startStop(string channel = "all", bool state = false) {
        switch (channel) {
            case "left": signalLeft.enabled = state; ; break;
            case "right": signalRight.enabled = state; break;
            case "center": signalCenter.enabled = state; break;
            default: signalCenter.enabled = signalRight.enabled = signalLeft.enabled = state; break;
        }
    }

}
