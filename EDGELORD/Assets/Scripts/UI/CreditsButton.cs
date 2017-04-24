using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour {
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

    private Phases swingState = CreditsButton.Phases.PHASE1;

    void Start () {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        //button.onClick.AddListener(QuitProgram); ToDo: Show credits (popup or new scene?)
    }

    void FixedUpdate()
    {
        int currentX = (int)rect.eulerAngles.x;
        if (isSwinging)
        {
            updateTime--;
            if (swingState == CreditsButton.Phases.PHASE1)
            {
                if (updateTime == 0)
                {
                    currentX += 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX >= 30f)
                {
                    swingState = CreditsButton.Phases.PHASE2;
                }
            }

            else if (swingState == CreditsButton.Phases.PHASE2)
            {
                if (updateTime == 0)
                {
                    currentX -= 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX <= 345f)
                {
                    swingState = CreditsButton.Phases.PHASE3;
                }
            }
            else if (swingState == CreditsButton.Phases.PHASE3)
            {
                if (updateTime == 0)
                {
                    currentX += 3;
                    rect.eulerAngles = new Vector3((float)currentX, 0, 0);
                    updateTime = 1;
                }
                if (currentX >= 357)
                {
                    swingState = CreditsButton.Phases.PHASE1;
                    rect.eulerAngles = new Vector3(0, 0, 0);
                    isSwinging = false;
                }
            }

        }
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
