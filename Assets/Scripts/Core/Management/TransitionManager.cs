//Made by Galactspace Studios

using System;
using UnityEngine;
using Core.Transitions;
using Scriptable.Transition;

namespace Core.Management
{
    public class TransitionManager : MonoBehaviour
    {
        private Transition _instance;
        private Transition Instance
        {
            get => _instance ??= Instantiate(transitionPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Transition>();
            set => _instance = value;
        }

        [SerializeField] private TransitionCallerSo transitionCaller;
        [SerializeField] private GameObject transitionPrefab;

        private void OnEnable()
        {
            transitionCaller.Link(FadeIn, FadeOut);
        }

        private void OnDisable()
        {
            transitionCaller.Unlink(FadeIn, FadeOut);
        }

        private void FadeIn(Action onComplete)
        {
            Instance.In(onComplete);
        }

        private void FadeOut(Action onComplete)
        {
            Instance.Out(onComplete);
            Instance = null;
        }
    }
}
