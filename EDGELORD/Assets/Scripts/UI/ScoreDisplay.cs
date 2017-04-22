using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour {
    public static string scoreString = "Score ";

    private TMP_Text textBox;

    void Start () {
        textBox = GetComponent<TMP_Text>();
        textBox.text = scoreString + "0";
    }

    public void UpdateScore(float newValue) {
        textBox.text = scoreString + Convert.ToString(newValue);
    }

    public void ResetScore () {
        textBox.text = scoreString + "0";
    }
}
