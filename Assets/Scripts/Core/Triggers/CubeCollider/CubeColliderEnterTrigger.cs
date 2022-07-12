//Copyright Galactspace 2022

using UnityEngine;
using Core.Controllers;

namespace Core.Triggers
{
    [RequireComponent(typeof(CubeCollider))]
    public class CubeColliderEnterTrigger : Trigger
    {
        private CubeCollider _cubeCollider;
        private CubeCollider Collider => _cubeCollider ??= GetComponent<CubeCollider>();

        private void OnEnable()
        {
            Collider.OnEnter += Call;
        }

        private void OnDisable()
        {
            Collider.OnEnter -= Call;
        }
    }
}
