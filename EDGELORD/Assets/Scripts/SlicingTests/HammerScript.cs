using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour {

    public GameObject hammer;

    //in DEGREES
    public float minAngle = -45, maxAngle = 45;
    public float speedPercent = 0.25f;


    void Start()
    {
        hammer.transform.eulerAngles = new Vector3(0.0f, 0.0f, minAngle);

        float lerpValue = Mathf.LerpAngle(minAngle, maxAngle, Time.deltaTime * speedPercent);

    }

	void Update ()
    {
        hammer.transform.Rotate(Vector3.forward, minAngle);
       // hammer.transform.eulerAngles = new Vector3(hammer.transform.eulerAngles.x, hammer.transform.eulerAngles.y, );
	}
}
