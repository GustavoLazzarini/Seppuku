//Made by Galactspace Studios

using UnityEngine.UIElements;
using UnityEngine.Localization;

namespace Core.UIElements
{
    public interface IBaseUIElement
    {
        public string ElementText { get; set; }

        public void SetText(string text) => ElementText = text;
        public void SetText(LocalizedString text) => SetText(text.GetLocalizedString());
    }

    public interface IBaseUIElement<TValue> : IBaseUIElement, INotifyValueChanged<TValue>
    {
        public TValue ElementValue { get; set; }
    }
}
