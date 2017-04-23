using UnityEngine;

public class WinnerDisplay : MonoBehaviour {

    void Awake () {
        // textBox = GetComponent<TMP_Text>();
        // hide display
        gameObject.SetActive(false);
    }

    public void ShowMessage (string text) {
        // textBox.text = text;

        gameObject.SetActive(true);
    }

    public void Hide () {
        gameObject.SetActive(false);
    }
}