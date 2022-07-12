//Copyright Galactspace Studio

using UnityEngine;

namespace Scriptable.Entities
{
    [CreateAssetMenu(menuName = "Game/Entities/Player/Stair Affection")]
    public class StairAffectionSo : ScriptableObject
    {
        public AnimationCurve VerticalAffection;
        public AnimationCurve HorizontalAffection;

        [Space]
        public float LerpMultiplier;
        public float HorizontalRotation;
        public float VerticalPosition;

        [Space]
        public Vector3 CenterOffset;
        public float VerticalDistance;
        public float HorizontalDistance;
    }
}