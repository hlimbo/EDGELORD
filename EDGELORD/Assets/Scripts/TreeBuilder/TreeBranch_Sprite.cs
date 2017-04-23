using System.Collections.Generic;
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


        public void HandleSlice(List<SpriteSlicer2DSliceInfo> sliceInfo)
        {
            //SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(sliceInfo);
            //GameObject closestChild = GetSlicedObjectClosestToBase(info, OwnerTreeBranch.gameObject);
            //foreach (GameObject go in info.ChildObjects)
            //{
            //    var rb = go.GetComponent<Rigidbody2D>();
            //    if (rb) rb.isKinematic = false;
            //}
            //if (closestChild != null)
            //{
            //    Debug.Log(closestChild.name);
            //    closestChild.GetComponent<Rigidbody2D>().isKinematic = true;
            //}

        }

        public void OnSpriteSliced(SpriteSlicer2DSliceInfo info)
        {
            OwnerTreeBranch.OnSpriteSliced(info);
        }

        //TODO: Create a helper function that obtains the most recently sliced game object.
        public SpriteSlicer2DSliceInfo GetMostRecentSlicedObject(List<SpriteSlicer2DSliceInfo> slicedObjectInfo)
        {
            if (slicedObjectInfo == null)
            {
                Debug.Log("GetMostRecentSlicedObject slicedObjectInfo is null");
                return null;
            }

            if (slicedObjectInfo.Count == 1)
            {
                return slicedObjectInfo[0];
            }

            //GET THE last object in the array is the most recently sliced.
            int recentlySlicedIndex = slicedObjectInfo.Count - 1;
            return slicedObjectInfo[recentlySlicedIndex];
        }
        public GameObject GetSlicedObjectClosestToBase(SpriteSlicer2DSliceInfo branch, GameObject baseObject)
        {
            if (branch == null)
                return null;

            //Note: there should be only 2 sliced parts in the list
            List<GameObject> slicedParts = branch.ChildObjects;

            //gets the worldspace coordinates for the centerpoint of the sliced sprite.
            Vector3 slicedPartCenter1 = slicedParts[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;
            Vector3 slicedPartCenter2 = slicedParts[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center;

            float d1 = Vector3.Distance(baseObject.transform.position, slicedPartCenter1);
            float d2 = Vector3.Distance(baseObject.transform.position, slicedPartCenter2);

            GameObject closestObjectToBase = d1 < d2 ? slicedParts[0] : slicedParts[1];

            return closestObjectToBase;
        }

        public void SetTint(Color tint)
        {
            var r = GetComponent<Renderer>();
            if (r)
            {
                r.material.color = tint;

            }
            else
            {
                Debug.Log("No Renderer!");
            }
        }
        public void SetTint(Color tint, int order)
        {
            var r = GetComponent<Renderer>();
            if (r)
            {
                r.material.color = tint;
                r.sortingOrder = order;
            }
            else
            {
                Debug.Log("No Renderer!");
            }
        }

    }
}