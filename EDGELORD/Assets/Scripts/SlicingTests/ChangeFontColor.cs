using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFontColor : MonoBehaviour {

    public Text exitText;
    public Color color;

    public void ChangeTextColor()
    {
        exitText.color = Color.red;
    }

}
