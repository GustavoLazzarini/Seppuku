//Made by Galactspace Studios

using UnityEngine;
using Core.UIElements;
using Scriptable.Generic;
using UnityEngine.UIElements;

namespace Core.Controllers.UI
{
    public class MenuController : UIController
    {
        private UIButton _startButton;
        private UIButton _optionsButton;
        private UIButton _quitButton;

        [Space]
        [SerializeField] private ChannelSo playChannel;
        [SerializeField] private ChannelSo optionsChannel;
        [SerializeField] private ChannelSo quitChannel;

        protected override void OnEnable()
        {
            base.OnEnable();

            LoadElements();
            LinkElements();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnlinkElements();
        }

        private void LoadElements()
        {
            _startButton = GetButton("StartButton");
            _optionsButton = GetButton("OptionsButton");
            _quitButton = GetButton("QuitButton");
        }

        private void LinkElements()
        {
            _startButton.clicked += playChannel.Invoke;
            _optionsButton.clicked += optionsChannel.Invoke;
            _quitButton.clicked += quitChannel.Invoke;
        }

        private void UnlinkElements()
        {
            _startButton.clicked -= playChannel.Invoke;
            _optionsButton.clicked -= optionsChannel.Invoke;
            _quitButton.clicked -= quitChannel.Invoke;
        }

        private void Play(NavigationSubmitEvent eventData) => playChannel.Invoke();
        private void Settings(NavigationSubmitEvent eventData) => optionsChannel.Invoke();
        private void Quit(NavigationSubmitEvent eventData) => quitChannel.Invoke();
    }
}
