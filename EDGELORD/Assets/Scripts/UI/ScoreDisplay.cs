using System.Collections;
using System;
using EDGELORD.TreeBuilder;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour {
    public string scoreString = "Score ";

    private TMP_Text textBox;
    

    void Awake () {
        textBox = GetComponent<TMP_Text>();
        textBox.text = scoreString + "0";
    }

    public void UpdateScore(float newValue) {
        textBox.text = string.Format("Score\n{0:0.00}", newValue);
    }

    public void ResetScore () {
        textBox.text = scoreString + "0";
    }
}
