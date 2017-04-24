using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MichaelWolf
{
	public class RestartScene : MonoBehaviour
	{
	    public KeyCode RestartKey = KeyCode.Escape;
        void Update()
	    {
	        if (Input.GetKeyDown(RestartKey))
	        {
	            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	        }
	    }
	}
}