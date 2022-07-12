//Made by Galactspace Studios

using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(Renderer))]
    public class TextureOffsetter : MonoBehaviour
    {
        private Material _mat;
        private Vector2 _current;

        [SerializeField] private string property = "_Offset";

        [Space]
        [SerializeField] private Vector2 offset;
        [SerializeField] private float multiplier = 1;

        private bool HasMaterial => _mat != null;
        
        private Vector2 GetNext() => _current + (offset * multiplier * Time.deltaTime);

        private void SetOffset(Vector2 arg) => _mat.SetVector(property, arg);

        private void Start() => _mat = GetComponent<Renderer>().material;

        private void LateUpdate() 
        {
            if (!HasMaterial) return;
            SetOffset(GetNext());
        }
    }
}
