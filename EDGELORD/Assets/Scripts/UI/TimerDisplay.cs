using UnityEngine;
using TMPro;
using System;

public class TimerDisplay : MonoBehaviour {
    private TMP_Text textBox;

    void Awake () {
        textBox = GetComponent<TMP_Text>();
    }

    public void UpdateTime(float timeLeftInSeconds) {
        int mins = (int)(timeLeftInSeconds / 60);
        float secs = (timeLeftInSeconds % 60);
        textBox.text = string.Format("{0}:{1,00:00.0}", mins, secs);
    }

    public void ResetTime() {
        textBox.text = "0:00.0";
    }
}