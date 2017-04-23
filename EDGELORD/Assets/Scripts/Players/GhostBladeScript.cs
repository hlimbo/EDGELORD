using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBladeScript : MonoBehaviour {

    public float defaultLength;
    public float defaultWidth;

	public void setScale(Vector2 scale) {
        transform.localScale = new Vector3(scale.x*defaultWidth, scale.y*defaultLength, 1);
    }

    public void setRotation(Vector2 rotation) {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rotation);
    }
}
