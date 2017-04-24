using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuitButton : MonoBehaviour {
    private Button button;
    private RectTransform rect;
    private bool isSwinging;
    private int updateTime = 1;

    private enum Phases
    {
        PHASE1,
        PHASE2,
        PHASE3
    }
    private Phases swingState = QuitButton.Phases.PHASE1;

    void Awake () {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        button.onClick.AddListener(QuitProgram);
    }

    void FixedUpdate()
    {
        int currentX = (int) rect.eulerAngles.x;
        if (isSwinging)
        {
            updateTime--;
            if (swingState == QuitButton.Phases.PHASE1)
            {
                if (updateTime == 0)
                {
                    currentX += 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX >= 30f)
                {
                    swingState = QuitButton.Phases.PHASE2;
                }
            }
            
            else if (swingState == QuitButton.Phases.PHASE2)
             {
                if (updateTime == 0)
                {
                    currentX -= 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX <= 345f)
                {
                    swingState = QuitButton.Phases.PHASE3;
                }
             }
             else if (swingState == QuitButton.Phases.PHASE3)
             {
                if (updateTime == 0)
                {
                    currentX += 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX >= 357)
                {
                    swingState = QuitButton.Phases.PHASE1;
                    rect.eulerAngles = new Vector3(0, 0, 0);
                    isSwinging = false;
                }
             }
             
        }
    }

    void QuitProgram () {
        StartCoroutine(StopMusicThenQuit());
    }

    private IEnumerator StopMusicThenQuit () {
        // MusicPlayer musicPlayer = MusicPlayer.Instance;
        MusicPlayer.Instance.FadeOutAndStop(0.9f);
        yield return new WaitForSeconds(1.0f);
        Application.Quit();
    }

    public void Swing()
    {
        if (!isSwinging)
        {
            rect.eulerAngles = new Vector3(-30, 0, 0);
            isSwinging = true;
        }
    }
}