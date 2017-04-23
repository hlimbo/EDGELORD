using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    /// <summary>
    /// Branch Component that goes onto each branch. 
    /// Each Branch should have it's root transform at the base of the branch.
    /// 
    /// - Creates Child Branches
    /// - Handles Slicing
    /// - Sends Slice to enemy branches.
    /// </summary>
    public class TreeBranch : MonoBehaviour, IBranchable
    {
        public PlayerID OwningPlayer;
        public List<TreeBranch> DirectChildBranches = new List<TreeBranch>();
        //public 
        [Space]
        public GameObject SpriteHolder; 
        public TreeBranch_Sprite branchSprite;
        public TreeBranchData BranchData;

        public TreeRoot MyRoot;
        [Header("Default Propoportions")]
        public float defaultLength = 3.2f;
        public float defaultWidth = 0.5f;

        private void Start()
        {
            GetTreeRoot();
        }

        private void GetTreeRoot()
        {
            var roots = FindObjectsOfType<TreeRoot>();
            if (roots.Length < 1)
            {
                Debug.LogWarning("[TreeBranch] No Roots found!");
                return;
            }
            foreach (TreeRoot root in roots)
            {
                if (root.OwningPlayer == OwningPlayer) MyRoot = root;
            }
            if (MyRoot == null)
            {
                Debug.LogWarning("[TreeBranch] Matching Root not found!");
                MyRoot = roots[0];
            }
        }

        public TreeBranch CreateChildBranch(TreeBranchData data, GameObject branchPrefab)
        {
            var go = GameObject.Instantiate(branchPrefab, data.LocalBasePoint, Quaternion.identity);
            var branch = go.GetComponent<TreeBranch>();

            return branch;
        }
        // Call this when the Branch is created.
        public void Generate(TreeBranchData data, bool doCoroutine = true)
        {
            BranchData = data;

            this.transform.parent = data.ParentBranch.transform;
            this.transform.localPosition = data.LocalBasePoint;
            var rot = Quaternion.LookRotation(Vector3.forward, (Vector3)data.GrowDirection.normalized);
            this.transform.rotation = rot;
            var targetScale = GetNewProportions(data.Length, data.Width);
            if (doCoroutine)
            {
                StartCoroutine(CoLerpGenerate(this.SpriteHolder.transform, targetScale));
            }
            else
            {
                this.SpriteHolder.transform.localScale = targetScale;
            }
        }

        private Vector3 GetNewProportions(float targetLength, float targetWidth)
        {
            float newLength = targetLength / defaultLength;
            float newWidth = targetWidth / defaultWidth;

            Vector3 newScale = new Vector3(newWidth,  newLength, 1f);
            return newScale;
        }

        public void SliceBranch(Vector3 worldStartPoint, Vector3 worldEndPoint, GameObject cutGameObject)
        {
            List<SpriteSlicer2DSliceInfo> sliceInfoList = new List<SpriteSlicer2DSliceInfo>();
            SpriteSlicer2D.SliceSprite(worldStartPoint, worldEndPoint, cutGameObject, false, ref sliceInfoList);
            
            HandleSliceReparenting(sliceInfoList);

        }

        public void HandleSliceReparenting(List<SpriteSlicer2DSliceInfo> sliceInfo)
        {
            branchSprite.HandleSlice(sliceInfo);

            //SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(sliceInfo);
            //GameObject closestChild = GetSlicedObjectClosestToBase(info, this.gameObject);

            //foreach (Transform child in transform)
            //{
            //    var ss = child.GetComponent<SlicedSprite>();
            //    if (ss)
            //    {

            //    }
            //}



            //SpriteSlicer2DSliceInfo info = sliceInfo[0];
            List<GameObject> childObjects = new List<GameObject>();
            foreach (Transform child in SpriteHolder.transform)
            {
                if (child.GetComponent<SlicedSprite>())
                {
                    childObjects.Add(child.gameObject);
                    var rb = child.GetComponent<Rigidbody2D>();
                    if (rb) rb.isKinematic = true;
                    TreeBranch_Sprite sp = child.GetComponent<TreeBranch_Sprite>();
                    if (!sp)
                    {
                        sp = child.gameObject.AddComponent<TreeBranch_Sprite>();
                        sp.OwnerTreeBranch = this;
                    }
                }
            }
            Debug.Log(childObjects.Count);
            GameObject closestGameObject = childObjects[0];
            float closestDist = Vector3.Distance(transform.position, closestGameObject.GetComponent<SlicedSprite>().MeshRenderer.bounds.center);
            foreach (GameObject child in childObjects)
            {
                var newDist = Vector3.Distance(transform.position, child.GetComponent<SlicedSprite>().MeshRenderer.bounds.center);
                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closestGameObject = child;
                }
            }
            foreach (GameObject child in childObjects)
            {
                if (child != closestGameObject)
                {
                    child.GetComponent<Rigidbody2D>().isKinematic = false;
                    //var projectedDistanceFromRoot = 
                }
            }
            closestGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

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

            //GameObject[] slicedPieces = 
            //float sliceDistanceFromRoot = 
            //Todo: Rechild children to each appropriate part.

            //Todo: Find sliced piece to remain attached, and replace the old one it with the new one.

            //ToDo: Add Rigidbody to unattached piece, and destroy it after a period of time. 
        }


        private IEnumerator CoLerpGenerate(Transform targetTransform, Vector3 targetScale, float lerpTime = 0.1f)
        {
            Vector3 currentScale = targetScale;
            currentScale.y = 0f;
            targetTransform.localScale = currentScale;
            //Vector3 startLocalPos = targetTransform.localPosition;
            //Vector3 targetLocalPos = targetTransform.localPosition += Vector3.up * targetScale.y;
            float timer = 0f;
            while (timer < lerpTime)
            {
                timer += Time.deltaTime;
                currentScale.y = Mathf.Lerp(0f, targetScale.y, timer / lerpTime);
                targetTransform.localScale = currentScale;
                //targetTransform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, timer / lerpTime);
                yield return null;
            }
            targetTransform.localScale = targetScale;
            
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

    }
}