using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
    private Button button;

    void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitProgram);
    }

    void QuitProgram () {
        Application.Quit();
    }
}