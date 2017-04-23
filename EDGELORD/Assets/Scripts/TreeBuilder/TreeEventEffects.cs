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
            sfxSource.pitch += (Random.value - 0.5f) * 0.2f;
        }

        private void OnBranchCutEffects(Vector3 worldPos)
        {
            //ToDo: Branch Cutting Events.
            sfxSource.PlayOneShot(sound, 0.5f);
        }
    }
}