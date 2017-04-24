using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;
using UnityEngine.Events;

namespace EDGELORD
{
	public class ColorFlashUntilGameStart : MonoBehaviour
	{
	    public PlayerID playerID;
        public CharacterActionScript myCharacter;
	    public Renderer myRenderer;
	    public Color color1;
	    public Color color2;
	    public UnityEvent OnPlayerReady;

	    void Start()
	    {
	        var characters = FindObjectsOfType<CharacterActionScript>();
	        foreach (CharacterActionScript c in characters)
	        {
                if (c.OwningPlayer == playerID)
                {
                    myCharacter = c;
                    break;
                }
            }
	        if (myCharacter != null)
	        {
	            StartCoroutine(CoWaitForReady());
	        }
	    }

	    IEnumerator CoWaitForReady()
	    {
            yield return new WaitForEndOfFrame();
            float timer = 0f;
	        Color startColor = myRenderer.material.color;
            while (!myCharacter.isReady)
            {
                timer += Time.deltaTime;
                myRenderer.material.color = Color.Lerp(color1, color2, (Mathf.Sin(Mathf.PI * 2 * timer)/2f) + 0.5f);
	            yield return null;
	        }
	        myRenderer.material.color = startColor;
            OnPlayerReady.Invoke();
	    }
	}
}