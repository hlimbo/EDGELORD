using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class TreeBranch_Sprite : MonoBehaviour
    {
        public TreeBranch OwnerTreeBranch;

        private void Awake()
        {
            if (!OwnerTreeBranch)
            {
                Transform currentParent = transform.parent;
                OwnerTreeBranch = currentParent.GetComponent<TreeBranch>();
                int failsafeBreak = 0;
                while (OwnerTreeBranch == null && currentParent != null)
                {
                    currentParent = currentParent.parent;
                    OwnerTreeBranch = currentParent.GetComponent<TreeBranch>();
                    failsafeBreak++;
                    if (failsafeBreak > 10)
                    {
                        break;
                    }
                }
            }
        }
    }
}