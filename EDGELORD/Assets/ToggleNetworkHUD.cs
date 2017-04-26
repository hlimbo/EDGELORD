using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNetworkHUD : MonoBehaviour {

    public UnityEngine.Networking.NetworkManagerHUD hud;
	public void DoToggleHUD()
    {
        if(hud == null)
        {
            hud = FindObjectOfType<UnityEngine.Networking.NetworkManagerHUD>();
        }
        hud.enabled = !hud.enabled;
    }
}
