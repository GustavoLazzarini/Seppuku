//Made by Galactspace Studios

using UnityEngine.UIElements;

namespace Core.UIElements
{
    public class UILabel : Label, IBaseUIElement
    {
        public new class UxmlFactory : UxmlFactory<UILabel, UxmlTraits> { }

        public string ElementText { get => text; set => text = value; }
    }
}
