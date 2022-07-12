//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Generic;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

namespace Core.Management
{
    public class EventSystemManager : MonoBehaviour
    {
        [SerializeField] private GameObjectChannelSo selectChannel;

        private void OnEnable() => selectChannel.Link(SelectObject);
        private void OnDisable() => selectChannel.Unlink(SelectObject);

        private void SelectObject(GameObject obj)
        {
            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(obj);
        }
    }
}
