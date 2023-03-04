using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour{

    public static Game instance;

    public GameSettings gameSettings;

    public BitSenseManager BSManager = null;

    public GameObject mainCamera;
    public GameObject inLevelCamera;

    public GameObject UI;
    public GameObject mainPanel;
    public GameObject levelPanel;
    private Texture2D texture = null;  
    
    private BitSenseInput BSInput = null;
    private BitSenseOutput BSOutput = null;
    public bool playing = false;
    public bool paused = false;

    Level currentlyPlayed;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        BSInput = BSManager.GetComponent<BitSenseInput>();
        BSOutput = BSManager.GetComponent<BitSenseOutput>();
        updateBSManagerSettings();
    }

    //###########GAME LOOP################################################

    private void Update() {
        if (playing) run();
    }

    public void updateBSManagerSettings() {
        BSManager.setStereoProjectionAngle(gameSettings.stereoProjectionAngle);
        BSManager.setVisualDebug(gameSettings.visualDebug);
    }

    public void play(Level level) {
        if (!playing) {
            currentlyPlayed = level;
            playing = true;
            BSManager.setMap(currentlyPlayed.map);
            if (gameSettings.stereoEnabled) {
                BSOutput.startStop("right", true);
                BSOutput.startStop("left", true);
            }
            if (gameSettings.monoEnabled)
                BSOutput.startStop("center", true);
            updateBSManagerSettings();
            BSManager.run();

            UI.SetActive(false);
            mainCamera.SetActive(false);
            inLevelCamera.SetActive(true);
        }
    }

    public void stop() {
        if (playing) {
            playing = false;
            currentlyPlayed.played++;
            BSOutput.startStop("all", false);
            BSManager.pause();
            UI.SetActive(false);
            mainCamera.SetActive(true);
            inLevelCamera.SetActive(false);
        }
    }

    public void pause() {
        if (playing && !paused) {
            paused = true;
            BSOutput.startStop("all", false);
            BSManager.pause();
        }
    }

    public void unpause() {
        if (playing && paused) {
            paused = false;
            BSOutput.startStop("all", true);
            BSManager.run();
        }
    }

    public void run() {
        if (testDeath()) {
            Debug.Log("Game Over");
            stop();
        }
        if (testVictory()) {
            Debug.Log("Game Won");
            currentlyPlayed.completed++;
            stop();
        }
        produceFeedback();

    }

    public void produceFeedback() {
        if (gameSettings.directionBased && gameSettings.stereoEnabled) {
            Vector2 output = calculateDirectionalBasedStereo("left", "right", 2);
            BSOutput.set(output.x, "left");
            BSOutput.set(output.y, "right");
        } else if (gameSettings.directionBased && gameSettings.monoEnabled) {
            float output = calculateDirectionalBasedMono("center", 2);
            BSOutput.set(output, "center");
        } else {
            float output = calculateIntensityBasedOut("center", 2);
            BSOutput.set(output);
        }
    }

    //#####################################################################

    private bool testDeath() {
        if (texture == null) texture = currentlyPlayed.map.mainTexture as Texture2D;
        if (BSInput.getLastSample().getChannel("center") != null) {
            float pixel = BSInput.getLastSample().getChannel("center").getIntensityOfLayer(Level.obstacleLayer);
            return pixel > 1 - currentlyPlayed.difficulty;
        }
        return false;
    }

    private bool testVictory() {
        if (texture == null) texture = currentlyPlayed.map.mainTexture as Texture2D;
        if (BSInput.getLastSample().getChannel("center") != null) {
            float pixel = BSInput.getLastSample().getChannel("center").getIntensityOfLayer(Level.goalLayer);
            return pixel == 1;
        }
        return false;
    }

    private float calculateIntensityBasedOut(string channel, int layer) {
        MultiLayerSphericalSample ch = BSInput.getLastSample().getChannel(channel);
        if (ch == null) return 0;
        return ch.getIntensityOfLayer(layer);
    }

    private float calculateDirectionalBasedMono(string channel, int layer) {
        MultiLayerSphericalSample ch = BSInput.getLastSample().getChannel(channel);
        if (ch == null ) return 0;
        if (ch.getDirectionOfLayer(layer).y == 0) return 0;//check for slope
        float value = (180 - Mathf.Abs(ch.getDirectionOfLayer(layer).x)) / 180; //using volume as direcion
        return value * ch.getDirectionOfLayer(layer).y;
    }

    private Vector2 calculateDirectionalBasedStereo(string left, string right, int layer) {
        MultiLayerSphericalSample ch1 = BSInput.getLastSample().getChannel(left);
        MultiLayerSphericalSample ch2 = BSInput.getLastSample().getChannel(right);
        if (ch1 == null || ch2 == null) return Vector2.zero;
        Vector2 result = Vector2.zero;
        if (ch1.getDirectionOfLayer(layer).y > 0) {
            result.x = (180 - Mathf.Abs(Mathf.DeltaAngle(ch1.getDirectionOfLayer(layer).x, -gameSettings.stereoOutwardAngle))) / 180;
        }
        if (ch2.getDirectionOfLayer(layer).y > 0) {
            result.y = (180 - Mathf.Abs(Mathf.DeltaAngle(ch2.getDirectionOfLayer(layer).x, gameSettings.stereoOutwardAngle))) / 180;
        }
        //Debug.Log(result);
        return result;
    }

    //###########UI EVENTS#################################

    public void mainPanelPlayButton() {
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    public void levelPanelBackButton() {
        mainPanel.SetActive(true);
        levelPanel.SetActive(false);
    }
}
