using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggleButton : MonoBehaviour{
    public Sprite checkedSprite;
    public Sprite uncheckedSprite;
    public bool state;

    public void onToggle() {
        state = !state;
        if (state) GetComponent<Image>().sprite = checkedSprite;
        else GetComponent<Image>().sprite = uncheckedSprite;
    }
}
