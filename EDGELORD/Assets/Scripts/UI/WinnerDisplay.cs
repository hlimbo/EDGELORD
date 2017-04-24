using UnityEngine;

public class WinnerDisplay : MonoBehaviour {
    public GameObject[] targets;

    void Awake () {
        // textBox = GetComponent<TMP_Text>();
        // hide display
        HideWindow();
    }

    public void ShowMessage (string text) {
        // textBox.text = text;

        if (text.Equals("PLAYER1")) {
            targets[0].SetActive(true);
        } else if (text.Equals("PLAYER2")) {
            targets[1].SetActive(true);
        } else {
            targets[2].SetActive(true);
        }
    }

    public void HideWindow () {
        foreach (var target in targets) {
            target.SetActive(false);
        }
    }
}