using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class MouseHoverRelay : MonoBehaviour
    {
        public GameObject RelayTarget;

        public void OnMouseDown()
        {
            RelayTarget.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            Debug.Log("OnMouseDown");
        }
        public void OnMouseEnter()
        {
            RelayTarget.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
            Debug.Log("OnMouseEnter");
        }
        public void OnMouseHover()
        {
            RelayTarget.SendMessage("OnMouseHover", SendMessageOptions.DontRequireReceiver);
            Debug.Log("OnMouseHover");
        }
        public void OnMouseExit()
        {
            RelayTarget.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
        }
    }
}