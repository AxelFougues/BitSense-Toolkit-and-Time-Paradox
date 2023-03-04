using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
public class PlayerOutput : MonoBehaviour{

    [Header("Signal generators references")]
    public GameObject signalGeneratorLeft;
    public GameObject signalGeneratorRight;
    public GameObject signalGeneratorMono;

    public GameObject initialiser = null;

    [Header("Output parameters")]
    public bool centerOnly = true;
    public bool directional = false;
    public bool volumeBased = true;
    public bool ampModBased = false;
    public bool freqModBased = false;

    bool isStarted = false;
    SignalGenerator signalLeft;
    SignalGenerator signalRight;
    SignalGenerator signalCenter;
    PlayerInput playerInput;

    MultiChannelSphericalSample currentIn;

    void Start(){
        playerInput = GetComponent<PlayerInput>();
        signalLeft = signalGeneratorLeft.GetComponent<SignalGenerator>();
        signalRight = signalGeneratorRight.GetComponent<SignalGenerator>();
        signalCenter = signalGeneratorMono.GetComponent<SignalGenerator>();

        //GENERATOR INIT
        //DEFAULT with no initialiser LA note on sine
        if (initialiser == null) {                                          
            signalLeft.sinusAudioWaveIntensity = 1;
            signalRight.sinusAudioWaveIntensity = 1;
            signalCenter.sinusAudioWaveIntensity = 1;
            signalLeft.mainFrequency = 440;
            signalRight.mainFrequency = 440;
            signalCenter.mainFrequency = 440;
        //INITIALIZE with initializer
        } else {
            SignalGenerator init = initialiser.GetComponent<SignalGenerator>();
            signalLeft = init;
            signalRight = init;
            signalCenter = init;
        }
        //Assign channels 
        signalLeft.stereoPan = -1;
        signalRight.stereoPan = 1;
        signalCenter.stereoPan = 0;
    }


    void Update(){
        if (isStarted) {
            updateInput();
            if (centerOnly) { //Center UPDATES
                updateGenerator(signalCenter, currentIn.getChannel("center"));
            } else { // Left Right UPDATES
                updateGenerator(signalLeft, currentIn.getChannel("left"));
                updateGenerator(signalRight, currentIn.getChannel("right"));
            }
        }
    }

    void updateInput() {
        currentIn = playerInput.getLastSample();
    }

    void updateGenerator(SignalGenerator generator, MultiLayerSphericalSample channel) {//mono of blue for now;
        if (directional) signalCenter.masterVolume = (channel.getDirectionOfLayer(2).x / 360); //* channel.getDirectionOfLayer(2).y;
        else signalCenter.masterVolume = channel.getIntensityOfLayer(2);
        Debug.Log("Grad " + channel.getDirectionOfLayer(2).x + " slope " + channel.getDirectionOfLayer(2).y);
    }

    public void startOutput() {
        if (centerOnly) signalCenter.enabled = true;
        else { signalLeft.enabled = true; signalRight.enabled = true; }
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
}
