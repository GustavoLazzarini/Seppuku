//Made by Galactspace Studios

using UnityEngine;
using Core.Controllers.UI;
using UnityEngine.UIElements;

namespace Core.Callers
{
    public class UISelectCaller : Caller
    {
        private VisualElement _uiRoot;

        public T GetElement<T>(VisualElement root, string elementName) where T : VisualElement => root.Q<T>(elementName);

        [Space]
        [SerializeField] private UIController controller;
        [SerializeField] private string element;

        public override void Call() 
        {
            DebugManager.Engine($"[UISelectCaller] Called in object {gameObject.name}");
            controller.SelectElement(element);
        }
    }
}
