//Made by Galactspace Studios

using Core.Types;
using UnityEngine;
using Core.Controllers;
using Scriptable.Dialogue;
using UnityEngine.Localization;
using System.Collections.Generic;

namespace Core.Management
{
    public class DialogueManager : MonoBehaviour
    {
        private bool _isDialogueRunning;
        private Queue<LocalizedDialogueSo> dialogues = new Queue<LocalizedDialogueSo>();    

        private void Enqueue(LocalizedDialogueSo dialog)
        {
            DebugManager.Engine(dialog.ToString());
            dialogues.Enqueue(dialog);
        }

        [SerializeField] private DialogueCallerSo dialogCaller;

        private void OnEnable() 
        {
            dialogCaller.StartDialogChannel.Link(StartDialog);
            dialogCaller.DialogEndedChannel.Link(DialogueEnded);
        }

        private void OnDisable() 
        {
            dialogCaller.StartDialogChannel.Unlink(StartDialog);
            dialogCaller.DialogEndedChannel.Unlink(DialogueEnded);
        }

        private void StartDialog(LocalizedDialogueSo dialog)
        {
            Enqueue(dialog);
            
            if (_isDialogueRunning) return;
            _isDialogueRunning = true;

            InstantiateDialogue(dialogues.Dequeue());
        }

        private void DialogueEnded()
        {
            if (!_isDialogueRunning) return;

            _isDialogueRunning = false;
        }

        private void InstantiateDialogue(LocalizedDialogueSo dialog)
        {
            DebugManager.Engine($"[DialogueManager] {dialog.TableEntryReference.KeyId}");
            GameObject obj = Instantiate(dialogCaller.Prefab, Vector3.zero, Quaternion.identity, null);
            obj.GetComponent<DialogueController>().StartDialog(dialog);
        }
    }
}
