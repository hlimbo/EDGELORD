using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MichaelWolf
{
	public class StartMusicPlayer : MonoBehaviour 
	{
	    private void Start()
	    {
	        MusicPlayer.Instance.StartMusic();
	    }
	}
}