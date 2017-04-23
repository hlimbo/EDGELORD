using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuitButton : MonoBehaviour {
    private Button button;

    void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitProgram);
    }

    void QuitProgram () {
        StartCoroutine(StopMusicThenQuit());
    }

    private IEnumerator StopMusicThenQuit () {
        MusicPlayer musicPlayer = (MusicPlayer)FindObjectOfType(typeof(MusicPlayer));
        musicPlayer.FadeOutAndStop(0.9f);
        yield return new WaitForSeconds(1.0f);
        Application.Quit();
    }
}