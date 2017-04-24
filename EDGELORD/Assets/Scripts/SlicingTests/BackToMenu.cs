using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

    public void GoBackToMenu()
    {
        //Load MainMenu
        SceneManager.LoadSceneAsync(0);
    }

}
