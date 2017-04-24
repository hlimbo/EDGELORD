using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton : MonoBehaviour {
    private Button button;
    public string sceneName;
    public bool stopMusic = true;

	// Use this for initialization
	void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadMainGame);
	}

    void LoadMainGame () {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene () {
        if (stopMusic) {
            MusicPlayer musicPlayer = (MusicPlayer)FindObjectOfType(typeof(MusicPlayer));
            musicPlayer.FadeOutAndStop(0.9f);
            yield return new WaitForSeconds(1.0f);
        }
        SceneManager.LoadScene(sceneName);
    }
}
