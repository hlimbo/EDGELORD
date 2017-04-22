using UnityEngine;

namespace EDGELORD.SliceSystem
{
    public struct SliceEventArgs
    {
        public Vector3 worldStartPos;
        public Vector3 worldEndPos;
    }

    public delegate void SliceEvent(object sender, SliceEventArgs e);
}