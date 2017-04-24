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

    public IEnumerator StartCountdownCoroutine () {
        ShowCountdown();
        int countdown = countdownLengthInSecs;
        do {
            textBox.text = System.Convert.ToString(countdown);
            --countdown;
            yield return new WaitForSeconds(1);
        } while (countdown > 0);

        textBox.text = System.Convert.ToString(0);

        // HideCountdown() is called in GameManager so that it can start the timer
        // *before* hiding the countdown
    }
}
