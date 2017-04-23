using System;
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
        //public Dictionary<TreeBranch, float> ChildDictionary = new Dictionary<TreeBranch, float>();
        public float ProjectedDistanceFromParent;
        //public 
        [Space]
        public GameObject SpriteHolder; 
        public TreeBranch_Sprite branchSprite;
        public TreeBranchData BranchData;

        public TreeRoot MyRoot;
        [Header("Default Propoportions")]
        public float defaultLength = 3.2f;
        public float defaultWidth = 0.5f;

        [Space]
        public float Area;

        public Action OnSliceBranch = delegate { };


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

        public void AddChildBranch(TreeBranch childBranch)
        {
            var projectedDistance = Vector3.Project(childBranch.BranchData.LocalBasePoint, Vector3.up);
            childBranch.ProjectedDistanceFromParent = projectedDistance.magnitude;
            DirectChildBranches.Add(childBranch);
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
            SendCutEvents(this.transform.position, this.transform.position + (Vector3)data.GrowDirection*data.Length);
            if (doCoroutine)
            {
                StartCoroutine(CoLerpGenerate(this.SpriteHolder.transform, targetScale));
            }
            else
            {
                this.SpriteHolder.transform.localScale = targetScale;
            }

        }

        private void SendCutEvents(Vector3 startPoint, Vector3 endPoint)
        {
            Debug.DrawLine(startPoint, endPoint, Color.red);
            Vector3 toEndPoint = endPoint - startPoint;
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, toEndPoint.normalized, toEndPoint.magnitude);
            foreach (RaycastHit2D hit in hits)
            {
                if(hit.transform.root == this.transform.root) continue;
                if (hit.rigidbody)
                {
                    var sp = hit.rigidbody.GetComponent<TreeBranch_Sprite>();
                    if (sp)
                    {
                        sp.OwnerTreeBranch.SliceThisBranch(startPoint, endPoint);
                        //SpriteSlicer2D.SliceAllSprites(startPoint, endPoint);
                    }
                }
            }
        }


        private Vector3 GetNewProportions(float targetLength, float targetWidth)
        {
            float newLength = targetLength / defaultLength;
            float newWidth = targetWidth / defaultWidth;

            Vector3 newScale = new Vector3(newWidth,  newLength, 1f);
            return newScale;
        }

        public void OnSpriteSliced(SpriteSlicer2DSliceInfo info)
        {
            List<SpriteSlicer2DSliceInfo> infoList = new List<SpriteSlicer2DSliceInfo>();
            infoList.Add(info); 
            HandleSliceReparenting(infoList);
            OnSliceBranch();
        }
        public void SliceThisBranch(Vector3 worldStartPoint, Vector3 worldEndPoint)
        {
            List<SpriteSlicer2DSliceInfo> sliceInfoList = new List<SpriteSlicer2DSliceInfo>();
            SpriteSlicer2D.SliceSprite(worldStartPoint, worldEndPoint, branchSprite.gameObject, false, ref sliceInfoList);
            
            //HandleSliceReparenting(sliceInfoList);

        }

        public void HandleSliceReparenting(List<SpriteSlicer2DSliceInfo> sliceInfo)
        {
            branchSprite.HandleSlice(sliceInfo);

            SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(sliceInfo);
            if (info == null)
            {
                
            }
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
                    var spriteCenterPos = sp.GetComponent<Renderer>().bounds.center;
                    var projectedDist = Vector3.Project(this.transform.InverseTransformPoint(spriteCenterPos), Vector3.up);
                }
            }
            Debug.Log(childObjects.Count);
            if(childObjects.Count < 1) return;
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
            var projectedEnterDist = Vector3.Project(this.transform.InverseTransformPoint(info.SliceEnterWorldPosition), Vector3.up);
            var projectedExitDist = Vector3.Project(this.transform.InverseTransformPoint(info.SliceExitWorldPosition), Vector3.up);
            float projectedCutOffDistance = Mathf.Min(projectedEnterDist.magnitude, projectedExitDist.magnitude);
            foreach (GameObject child in childObjects)
            {
                if (child != closestGameObject)
                {
                    child.GetComponent<Rigidbody2D>().isKinematic = false;

                    var spriteCenterPos = child.GetComponent<Renderer>().bounds.center;
                    var projectedDist = Vector3.Project(this.transform.InverseTransformPoint(spriteCenterPos), Vector3.up).magnitude;
                    //if (projectedDist > projectedCutOffDistance)
                    //{

                        foreach (TreeBranch branch in DirectChildBranches)
                        {
                            if (branch.ProjectedDistanceFromParent > projectedCutOffDistance)
                            {
                                branch.transform.parent = child.transform;
                            }
                        }
                    //}
                    //var projectedDistanceFromRoot = 

                    child.transform.parent = null;
                    child.gameObject.layer = 9; // "NoCollision" Layer.
                }
            }
            closestGameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            branchSprite = closestGameObject.GetComponent<TreeBranch_Sprite>();
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
            if (slicedObjectInfo.Count < 1)
                return null;

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
/*
 * 
 * 
 *      SpriteSlicer2D.SliceAllSprites(startPoint, endPoint, canDestroyParent, ref slicedObjectInfo, slicableMask);

            //Case 2 reparent branches to proper slices if main part of the sword is cut in half
            //DONE: Reparent the children branches attached to the base of the sliced object.
            SpriteSlicer2DSliceInfo info = GetMostRecentSlicedObject(slicedObjectInfo);
            GameObject slicedObject = info.SlicedObject;
            List<GameObject> slicedPieces = info.ChildObjects;

            //obtain all the branches from the slicedObject.
            List<GameObject> branches = new List<GameObject>();
            for (int i = 0; i < slicedObject.transform.childCount; ++i)
                branches.Add(slicedObject.transform.GetChild(i).gameObject);

            //detach branches from deactivated parent object
            slicedObject.transform.DetachChildren();

            foreach(GameObject branch in branches)
            {
                //obtain which branch is closer of the 2 slicedPiece objects
                float d1 = Vector3.Distance(slicedPieces[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center, branch.transform.position);
                float d2 = Vector3.Distance(slicedPieces[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center, branch.transform.position);

                //float d1 = Vector3.Distance(branch.transform.position, slicedPieces[0].GetComponent<SlicedSprite>().MeshRenderer.bounds.center);
                //float d2 = Vector3.Distance(branch.transform.position, slicedPieces[1].GetComponent<SlicedSprite>().MeshRenderer.bounds.center);

                //if slicedPieces[0] is further away from slicedPieces[1] -> attach branch to slicedPieces[1]
                if (d1 > d2)
                    branch.transform.SetParent(slicedPieces[1].transform, true);
                else
                    branch.transform.SetParent(slicedPieces[0].transform, true);
            }

 * 
 * 
 * */
