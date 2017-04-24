using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownDisplay : MonoBehaviour {
    public int countdownLengthInSecs = 3;
    public GameObject target;

    private TMP_Text textBox;

	void Awake () {
        textBox = target.GetComponentInChildren<TMP_Text>();
        textBox.text = System.Convert.ToString(countdownLengthInSecs);
        target.SetActive(false);
	}

    public void ShowCountdown () {
        target.SetActive(true);
    }

    public void HideCountdown () {
        target.SetActive(false);
    }

    public void DoFullCountdown () {
        StartCoroutine(DoFullCountdownCoroutine());
    }

    private IEnumerator DoFullCountdownCoroutine() {
        yield return StartCountdownCoroutine(false);
        yield return new WaitForSeconds(1);
        HideCountdown();
    }

    public IEnumerator StartCountdownCoroutine (bool startGame = true) {
        ShowCountdown();
        int countdown = countdownLengthInSecs;
        do {
            textBox.text = System.Convert.ToString(countdown);
            --countdown;
            yield return new WaitForSeconds(1);
        } while (countdown > 0);

        if (startGame) {
            textBox.text = "Go!";
        } else {
            textBox.text = System.Convert.ToString(0);
        }

        // HideCountdown() is called in GameManager so that it can start the timer
        // *before* hiding the countdown
    }
}
