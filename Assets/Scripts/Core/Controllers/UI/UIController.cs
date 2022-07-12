//Made by Galactspace Studios

using Core.Types;
using System.Linq;
using UnityEngine;
using Core.UIElements;
using Scriptable.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Core.Controllers.UI
{
    public abstract class UIController : MonoBehaviour
    {
        private bool _cursorRegistered;

        protected VisualElement _root;
        protected IBaseUIFocusable _focusedElement;

        private ScrollView _queriedScrollView;
        private List<UIButton> _queriedButtons = new();
        private List<UILabel> _queriedLabels = new();
        private List<UISlider> _queriedSliders = new();
        private List<UISliderInt> _queriedSlidersInt = new();
        private List<UIToggle> _queriedToggles = new();
        private List<VisualElement> _queriedElements = new();
        private List<UIStaticSlider> _queriedStaticSliders = new();

        [SerializeField] protected UIDocument document;
        [SerializeField] protected GameInputSo gameInput;

        [Space]
        [SerializeField] protected string fistSelected;

        [Space]
        [SerializeField] protected string scrollView;
        [SerializeField] protected float scrollSpeed;

        [Space]
        [SerializeField] private UIElement[] queryElements;

        protected virtual UIButton[] GetButtons() => _queriedButtons.ToArray();
        protected virtual UIButton GetButton(string elementName) => _queriedButtons.First(x => x.name == elementName);

        protected virtual UILabel[] GetLabels() => _queriedLabels.ToArray();
        protected virtual UILabel GetLabel(string elementName) => _queriedLabels.First(x => x.name == elementName);

        protected virtual UISlider[] GetSliders() => _queriedSliders.ToArray();
        protected virtual UISlider GetSlider(string elementName) => _queriedSliders.First(x => x.name == elementName);

        protected virtual UISliderInt[] GetSlidersInt() => _queriedSlidersInt.ToArray();
        protected virtual UISliderInt GetSliderInt(string elementName) => _queriedSlidersInt.First(x => x.name == elementName);

        protected virtual UIToggle[] GetToggles() => _queriedToggles.ToArray();
        protected virtual UIToggle GetToggle(string elementName) => _queriedToggles.First(x => x.name == elementName);

        protected virtual VisualElement[] GetVisualElements() => _queriedElements.ToArray();
        protected virtual VisualElement GetVisualElement(string elementName) => _queriedElements.First(x => x.name == elementName);

        protected virtual UIStaticSlider[] GetStaticSliders() => _queriedStaticSliders.ToArray();
        protected virtual UIStaticSlider GetStaticSlider(string elementName) => _queriedStaticSliders.First(x => x.name == elementName);

        protected virtual bool FocusUpdated => DocFocusedElement == (VisualElement)_focusedElement;
        protected virtual bool HasElement(string elementName) => queryElements.Any(x => x.elementName == elementName);

        protected virtual bool HasDocFocusedElement => _root.focusController.focusedElement != null;
        protected virtual VisualElement DocFocusedElement => _root.focusController.focusedElement as VisualElement;

        protected virtual T GetElementOnDoc<T>(string elementName) where T : VisualElement => _root.Q<T>(elementName);

        protected virtual void OnEnable()
        {
            UpdateUI();
            gameInput.InputDeviceChannel.Link(UpdateUI);

            gameInput.LinkNavs(OnUp, OnDown, OnLeft, OnRight);
            gameInput.ButtonInteractChannel.Link(OnSubmit);

            if (_queriedScrollView != null)
                gameInput.RightStickChannel.Link(OnRightStick);

            if (!string.IsNullOrEmpty(fistSelected))
                SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(fistSelected));
        }

        protected virtual void OnDisable()
        {
            if (_focusedElement != null) _focusedElement.UnregisterFocus(gameInput);

            if (_cursorRegistered)
            {
                _cursorRegistered = false;
                UnregisterMouseOverCallbacks();
            }

            gameInput.UnlinkNavs(OnUp, OnDown, OnLeft, OnRight);
            gameInput.ButtonInteractChannel.Unlink(OnSubmit);

            _queriedScrollView = null;
            gameInput.RightStickChannel.Unlink(OnRightStick);
            gameInput.InputDeviceChannel.Unlink(UpdateUI);
        }

        protected virtual void Update()
        {
            if (HasDocFocusedElement && !FocusUpdated && queryElements.Any(x => x.elementName == DocFocusedElement.name)) SelectElement((IBaseUIFocusable)DocFocusedElement);
            if (_focusedElement != null) SelectElement(_focusedElement);
        }

        private void UpdateUI(InputDeviceType deviceType) => UpdateUI();

        private void UpdateUI()
        {
            _root = document.rootVisualElement;

            if (!string.IsNullOrEmpty(scrollView))
                _queriedScrollView = GetElementOnDoc<ScrollView>(scrollView);

            if (_cursorRegistered)
            {
                UnregisterMouseOverCallbacks();
                _cursorRegistered = false;
            }

            UpdateButtons(queryElements.Where(x => x.elementType == InterfaceElementType.Button).ToArray());
            UpdateLabels(queryElements.Where(x => x.elementType == InterfaceElementType.Label).ToArray());
            UpdateSliders(queryElements.Where(x => x.elementType == InterfaceElementType.Slider).ToArray());
            UpdateSlidersInt(queryElements.Where(x => x.elementType == InterfaceElementType.SliderInt).ToArray());
            UpdateToggles(queryElements.Where(x => x.elementType == InterfaceElementType.Toggle).ToArray());
            UpdateVisualElements(queryElements.Where(x => x.elementType == InterfaceElementType.VisualElement).ToArray());
            UpdateStaticSliders(queryElements.Where(x => x.elementType == InterfaceElementType.StaticSlider).ToArray());

            if (!_cursorRegistered && gameInput.InputDeviceChannel.Baked == InputDeviceType.Keyboard)
            {
                RegisterMouseOverCallbacks();
                _cursorRegistered = true;
            }
        }

        private void RegisterMouseOverCallbacks()
        {
            foreach (UIButton btn in GetButtons()) btn.RegisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UISlider slider in GetSliders()) slider.RegisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UISliderInt sliderInt in GetSlidersInt()) sliderInt.RegisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UIToggle toggle in GetToggles()) toggle.RegisterCallback<MouseOverEvent>(HoveredElement);
        }

        private void UnregisterMouseOverCallbacks()
        {
            foreach (UIButton btn in GetButtons()) btn.UnregisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UISlider slider in GetSliders()) slider.UnregisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UISliderInt sliderInt in GetSlidersInt()) sliderInt.UnregisterCallback<MouseOverEvent>(HoveredElement);
            foreach (UIToggle toggle in GetToggles()) toggle.UnregisterCallback<MouseOverEvent>(HoveredElement);
        }

        private void UpdateButtons(UIElement[] elements)
        {
            _queriedButtons.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UIButton button = GetElementOnDoc<UIButton>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)button).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedButtons.Add(button);
            }
        }

        private void UpdateLabels(UIElement[] elements)
        {
            _queriedLabels.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UILabel label = GetElementOnDoc<UILabel>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)label).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedLabels.Add(label);
            }
        }

        private void UpdateSliders(UIElement[] elements)
        {
            _queriedSliders.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UISlider slider = GetElementOnDoc<UISlider>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)slider).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedSliders.Add(slider);
            }
        }

        private void UpdateSlidersInt(UIElement[] elements)
        {
            _queriedSlidersInt.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UISliderInt sliderInt = GetElementOnDoc<UISliderInt>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)sliderInt).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedSlidersInt.Add(sliderInt);
            }
        }

        private void UpdateToggles(UIElement[] elements)
        {
            _queriedToggles.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UIToggle toggle = GetElementOnDoc<UIToggle>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)toggle).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedToggles.Add(toggle);
            }
        }

        private void UpdateVisualElements(UIElement[] elements)
        {
            _queriedElements.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                VisualElement velement = GetElementOnDoc<VisualElement>(elements[i].elementName);
                _queriedElements.Add(velement);
            }
        }

        private void UpdateStaticSliders(UIElement[] elements)
        {
            _queriedStaticSliders.Clear();
            for (int i = 0; i < elements.Length; i++)
            {
                UIStaticSlider slider = GetElementOnDoc<UIStaticSlider>(elements[i].elementName);

                if (elements[i].elementText != null)
                    ((IBaseUIElement)slider).SetText(gameInput.ButtonLink.GetString(elements[i].elementText));

                _queriedElements.Add(slider);
            }
        }

        protected virtual void HoveredElement(MouseOverEvent eventData) => SelectElement((IBaseUIFocusable)eventData.target);

        public void SelectElement(string elementName)
        {
            if (string.IsNullOrEmpty(elementName)) return;
            SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(elementName));
        }

        protected virtual void SelectElement(IBaseUIFocusable element)
        {
            if (_focusedElement != null) _focusedElement.UnregisterFocus(gameInput);
            
            _focusedElement = element;
            _focusedElement.RegisterFocus(gameInput);
        }

        protected virtual void OnUp()
        {
            if (_focusedElement == null) return;

            string navigation = _focusedElement.Up;
            if (string.IsNullOrEmpty(navigation)) return;

            SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(navigation));
        }

        protected virtual void OnDown()
        {
            if (_focusedElement == null) return;

            string navigation = _focusedElement.Down;
            if (string.IsNullOrEmpty(navigation)) return;

            SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(navigation));
        }

        protected virtual void OnLeft()
        {
            if (_focusedElement == null) return;

            string navigation = _focusedElement.Left;
            if (string.IsNullOrEmpty(navigation)) return;

            SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(navigation));
        }

        protected virtual void OnRight()
        {
            if (_focusedElement == null) return;

            string navigation = _focusedElement.Right;
            if (string.IsNullOrEmpty(navigation)) return;

            SelectElement((IBaseUIFocusable)GetElementOnDoc<VisualElement>(navigation));
        }

        protected virtual void OnSubmit()
        {
            if (_focusedElement == null) return;
            ((VisualElement)_focusedElement).SendEvent(NavigationSubmitEvent.GetPooled());
        }

        protected virtual void OnRightStick(Vector2 value)
        {
            if (_queriedScrollView == null) return;
            _queriedScrollView.verticalScroller.value = Mathf.Clamp(_queriedScrollView.verticalScroller.value - (value.y * scrollSpeed * Time.deltaTime), _queriedScrollView.verticalScroller.lowValue, _queriedScrollView.verticalScroller.highValue);
        }
    }
}
