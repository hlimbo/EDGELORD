using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownDisplay : MonoBehaviour {
    public int countdownLengthInSecs = 3;
    public GameObject target;

    private TMP_Text textBox;
    private SfxPlayer sfxPlayer;

	void Awake () {
        textBox = target.GetComponentInChildren<TMP_Text>();
        textBox.text = System.Convert.ToString(countdownLengthInSecs);
        target.SetActive(false);

        sfxPlayer = GetComponent<SfxPlayer>();
	}

    public void ShowCountdown () {
        target.SetActive(true);
    }

    public void HideCountdown () {
        target.SetActive(false);
    }

    public void DoFullCountdown (int countDownInSeconds = 3) {
        StartCoroutine(DoFullCountdownCoroutine(countDownInSeconds));
    }

    private IEnumerator DoFullCountdownCoroutine(int countDownInSeconds = 3) {
        yield return StartCountdownCoroutine(false, countDownInSeconds);
        yield return new WaitForSeconds(1);
        HideCountdown();
    }

    public IEnumerator StartCountdownCoroutine (bool startGame = true, int countDownInSeconds = 3) {
        ShowCountdown();
        int countdown = countDownInSeconds; //countdownLengthInSecs;
        do {
            textBox.text = System.Convert.ToString(countdown);
            if(!startGame)
                sfxPlayer.PlaySoundEffect("siren");
            --countdown;
            yield return new WaitForSeconds(1);
        } while (countdown > 0);


        if (startGame) {
            textBox.text = "Go!";
        } else {
            sfxPlayer.PlaySoundEffect("siren"); // play for 0 as well
            textBox.text = System.Convert.ToString(0);
        }

        // HideCountdown() is called in GameManager so that it can start the timer
        // *before* hiding the countdown
    }
}
