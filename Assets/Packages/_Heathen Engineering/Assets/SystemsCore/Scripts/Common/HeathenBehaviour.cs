﻿using UnityEngine;

namespace HeathenEngineering
{
    /// <summary>
    /// Used as the base class for traditional Transform based behaviours
    /// </summary>
    public class HeathenBehaviour : MonoBehaviour
    {
        private Transform _selfTransform;
        public Transform SelfTransform
        {
            get
            {
                if (_selfTransform == null)
                    _selfTransform = GetComponent<Transform>();
                return _selfTransform;
            }
        }
    }
}
