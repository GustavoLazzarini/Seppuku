//Made by Galactspace Studios

using Core.Types;
using UnityEngine;
using Scriptable.Dialogue;

namespace Core.Callers
{
    public class DialogueCaller : Caller
    {
        [SerializeField] private DialogueCallerSo callerSo;

        [Space]
        [SerializeField] private LocalizedDialogueSo dialogAsset;

        public override void Call()
        {
            DialogueSo ds = dialogAsset.LoadAsset();
            Debug.Log(ds == null);

            DebugManager.Engine($"[DialogueCaller] Called {dialogAsset.TableEntryReference.KeyId}!");
            callerSo.StartDialogChannel.Invoke(dialogAsset);
        }
    }
}
