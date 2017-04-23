using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class TreeEventEffects : MonoBehaviour
    {
        private TreeRoot _treeRoot;

        private void Start()
        {
            _treeRoot = GetComponent<TreeRoot>();
            _treeRoot.OnBranchCutAction += OnBranchCutEffects;
        }

        private void OnBranchCutEffects()
        {
            //ToDo: Branch Cutting Events.
        }
    }
}