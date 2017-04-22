using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {
    private Button button;

	// Use this for initialization
	void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadMainGame);
	}

    void LoadMainGame () {
        SceneManager.LoadScene("GameManager"); // TODO: change this to name of the final main-game scene
    }
}
