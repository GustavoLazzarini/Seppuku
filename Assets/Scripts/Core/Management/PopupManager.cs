//Made by Galactspace Studios

using UnityEngine;
using Core.Popups;
using Scriptable.Popup;
using System.Threading.Tasks;
using UnityEngine.Localization;

namespace Core.Management
{
    public class PopupManager : MonoBehaviour
    {
        private Popup _currentPopup;
        public bool HasInstance => _currentPopup.NotNull();

        [SerializeField] private PopupCallerSo popupCaller;
        
        [Space]
        [SerializeField] private GameObject _infoPopup;

        private Popup InstantiatePopup(GameObject popup) => Instantiate(popup, Vector3.zero, Quaternion.identity, transform).GetComponent<Popup>();

        public void ShowInfoPopup(LocalizedString mainText) => ShowInfoPopup(mainText.GetLocalizedString());

        public async void ShowInfoPopup(string mainText)
        {
            if (HasInstance) await HidePopup();

            _currentPopup = InstantiatePopup(_infoPopup);
            _currentPopup.SetText(mainText);
        }

        public void Hide() => _ = HidePopup();

        private async Task HidePopup()
        {
            if (!HasInstance) return;

            await _currentPopup.HidePopup();
            Destroy(_currentPopup.gameObject);
            _currentPopup = null;
        }
    }
}
