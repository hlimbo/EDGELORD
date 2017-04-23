using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour {
    private TMP_Text countdownText;

	// Use this for initialization
	void Start () {
        countdownText = GetComponent<TextMeshPro>();
        if (countdownText == null) {
            Debug.Log("could not GetComponent<TMP_Text>");
        }
        countdownText.text = "3";
	}

    public void StartCountdown () {
        StartCoroutine(doCountdown());
    }

    private IEnumerator doCountdown () {
        yield return new WaitForSeconds(1.0F);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.0F);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.0F);
        countdownText.text = "Start!";
        yield return new WaitForSeconds(1.0F);
        countdownText.enabled = false;
    }

    public void Reset () {
        countdownText.enabled = true;
        countdownText.text = "3";
    }
}
