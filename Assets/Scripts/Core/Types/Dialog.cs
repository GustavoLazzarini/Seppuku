//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Dialogue;
using UnityEngine.Localization;

namespace Core.Types
{
    [System.Serializable]
    public class LocalizedDialogueSo : LocalizedAsset<DialogueSo> { }

    [System.Serializable]
    public class Dialog
    {
        [SerializeField] private DialogueSpeakerSo speaker;
        [SerializeField] [TextArea(3, 10)] private string phrase;

        public DialogueSpeakerSo Speaker => speaker;
        public string Phrase => phrase;

        public override string ToString()
        {
            return $"{speaker}: {phrase}";
        }
    }
}
