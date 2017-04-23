using UnityEngine;

public class WinnerDisplay : MonoBehaviour {

    void Start () {
        // textBox = GetComponent<TMP_Text>();
        // hide display
        gameObject.setActive(false);
    }

    void ShowMessage (string text) {
        // textBox.text = text;

        gameObject.setActive(true);
    }

    void Hide () {
        gameObject.setActive(false);
    }
}