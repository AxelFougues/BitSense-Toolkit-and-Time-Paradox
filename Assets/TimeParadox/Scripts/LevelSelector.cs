using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour{

    public Level level;
    public GameObject completeOverlay;
    public GameObject lockedOverlay;
    public bool locked = true;

    Game game;

    private void Start() {
        game = FindObjectOfType<Game>();
        if (level != null) locked = false;
        lockedOverlay.SetActive(locked);
        if (!locked) {
            GetComponentInChildren<Text>().text = level.number.ToString();
            completeOverlay.GetComponent<Image>().sprite = level.reward.sprite;
        } else GetComponentInChildren<Text>().text = "X";
    }

    public void launch() {
        if (!locked) game.play(level);
    }

    private void Update() {
        if (!locked) {
            completeOverlay.SetActive(level.completed > 0);
        }
    }

}
