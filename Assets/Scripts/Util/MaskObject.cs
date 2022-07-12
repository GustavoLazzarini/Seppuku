//Created by Galactspace

using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MaskObject : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }
}