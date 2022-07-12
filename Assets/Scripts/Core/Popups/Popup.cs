//Made by Galactspace Studios

using TMPro;
using UnityEngine;
using System.Threading.Tasks;

namespace Core.Popups
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _mainText;

        public void SetText(string text) => _mainText.text = text;
        public abstract Task HidePopup();
    }
}
