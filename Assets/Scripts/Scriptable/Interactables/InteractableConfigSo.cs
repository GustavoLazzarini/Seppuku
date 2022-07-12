//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Generic;

namespace Scriptable.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactable/Interactable Configuration")]
    public class InteractableConfigSo : ScriptableObject
    {
        [Space]
        [SerializeField] private ChannelSo _interactChannel;
        public ChannelSo InteractChannel => _interactChannel;
        
        [SerializeField] private Vector2ChannelSo _playerPosChannel;
        public Vector2ChannelSo PlayerPositionChannel => _playerPosChannel;

        [Space]
        [SerializeField] private float _interactDistance = 2;
        public float InteractableDistance => _interactDistance;

        [Space]
        [Tooltip("The popup button that appears when the player is within the Interactable Distance")]
        [SerializeField] private GameObject _buttonPrefab;
        public GameObject ButtonPrefab => _buttonPrefab;
    }
}
