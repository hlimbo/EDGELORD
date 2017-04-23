using UnityEngine;

namespace EDGELORD.TreeBuilder
{
    public class TreeEventEffects : MonoBehaviour
    {
        public AudioClip sound;

        private TreeRoot _treeRoot;
        private AudioSource sfxSource;

        private void Start()
        {
            _treeRoot = GetComponent<TreeRoot>();
            _treeRoot.OnBranchCutAction += OnBranchCutEffects;

            sfxSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        }

        private void OnBranchCutEffects()
        {
            //ToDo: Branch Cutting Events.
            sfxSource.PlayOneShot(sound);
        }
    }
}