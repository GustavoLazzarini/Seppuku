//Made by Galactspace Studios

using UnityEngine;

namespace Util
{
    public class CycleOffsetRandomizer : MonoBehaviour
    {
        [SerializeField] private string property = "CycleOffset";

        private Animator GetAnimator() => GetComponent<Animator>();
        private void SetOffset(Animator arg) => arg.SetFloat(property, Random.Range(0, 1));

        private void Start() 
        {
            SetOffset(GetAnimator());
            Destroy(this);
        }
    }
}
