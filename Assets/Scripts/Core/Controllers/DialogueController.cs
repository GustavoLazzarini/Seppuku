//Made by Galactspace Studios

using TMPro;
using Core.Types;
using UnityEngine;
using System.Collections;
using Scriptable.Dialogue;
using UnityEngine.Localization;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;

namespace Core.Controllers
{
    public class DialogueController : MonoBehaviour
    {
        private bool _isLinked;
        private bool _isWritting;

        private float _speed;
        private int _currentDialogIndex;

        private Dialog _currentDialog;
        private LocalizedAsset<DialogueSo> _currentDialogueSo;

        private Animator _dialogAnimator;

        private Queue<Dialog> _dialogues = new Queue<Dialog>();

        [SerializeField] private DialogueCallerSo caller;

        [Space]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private GameObject nextButton;

        private bool HasDialogue => _dialogues.Count > 0;
        private bool HasDialogueSo => _currentDialogueSo != null;

        private Dialog GetCurrent() => _dialogues.Dequeue();
        //private DialogueSo GetCurrentSo()
        //{
        //    try
        //    {
        //        DialogueSo target = _currentDialogueSo.LoadAssetAsync();
        //        DebugManager.Info($"[DialogueController] Got CurrentSo, targetIsNull: {target == null}");
        //        return target;
        //    }
        //    catch (System.Exception e)
        //    {
        //        DebugManager.Error($"[DialogueController] [GetCurrentSo] {e.Message}");
        //        return default;
        //    }
        //}

        private void SetText(string text) => dialogText.text = text;
        private void AddText(string text) => SetText(dialogText.text + text);

        private void OnEnable() 
        {
            caller.PassDialogChannel.Link(PassDialogue);
        }
        
        private void OnDisable() 
        {
            caller.PassDialogChannel.Unlink(PassDialogue);
        }

        private void LoadStrings(DialogueSo dialogueSo)
        {
            DebugManager.Info(dialogueSo.name);

            try
            {
                _dialogues.Clear();

                for (int i = 0; i < dialogueSo.Length; i++)
                {
                    if (i > _currentDialogIndex)
                    {
                        _dialogues.Enqueue(dialogueSo.Dialogue[i]);
                        DebugManager.Engine($"[DialogueController] Enqueued {dialogueSo.Dialogue[i]}");
                    }
                }

                DebugManager.Info($"[DialogueController] Loaded Strings");
            }
            catch (System.Exception e)
            {
                DebugManager.Error($"[DialogueController] [LoadStrings] {e.Message}");
            }
        }

        private void PassDialogue()
        {
            if (_isWritting)
            {
                _speed = caller.Configuration.FastDialogueSpeed;
                return;
            }

            if (!HasDialogue)
            {
                EndDialogue();
                return;
            }

            _dialogAnimator.SetTrigger("Next");
            
            caller.DialogPassedChannel.Invoke();
            ShowDialogue(GetCurrent(), true);
        }

        private void EndDialogue()
        {
            _dialogAnimator.SetTrigger("End");

            caller.DialogEndedChannel.Invoke();
            Destroy(gameObject, .5f);
        }

        public async void StartDialog(LocalizedDialogueSo dialogSo)
        {
            if (!LocalizationSettings.InitializationOperation.IsDone) await LocalizationSettings.InitializationOperation.Task;

            DebugManager.Info("Starting Dialog Loading");

            try
            {
                _dialogues = new Queue<Dialog>();
                _dialogAnimator = GetComponent<Animator>();

                _currentDialogIndex = -1;
                _speed = caller.Configuration.DialogueSpeed;

                _currentDialogueSo = dialogSo;
                DebugManager.Engine($"[DialogueController] Current dialog key: {dialogSo.TableEntryReference.KeyId}");
                _currentDialogueSo.WaitForCompletion = true;

                caller.DialogStartedChannel.Invoke();
            }
            catch (System.Exception e)
            {
                DebugManager.Error($"[DialogueController] [StartDialog] {e.Message}");
            }

            DebugManager.Info(LocalizationSettings.Instance.GetSelectedLocale().LocaleName);

            try
            {
                _currentDialogueSo.AssetChanged += OnDialogueLoaded;
            }
            catch (System.Exception e)
            {                
                DebugManager.Error($"[DialogueController] {e.Message}");
                DebugManager.Error($"[DialogueController] {e.StackTrace}");
            }
        }

        private void OnDialogueLoaded(DialogueSo dialogue)
        {
            _currentDialogueSo.AssetChanged -= OnDialogueLoaded;
            
            try
            {
                DebugManager.Engine($"[DialogueController] Is null: {dialogue == null}");
                LoadStrings(dialogue);
                if (_dialogues.Count > 0) ShowDialogue(GetCurrent(), true);
            }
            catch (System.Exception e)
            {
                DebugManager.Error($"[DialogueController] {e.Message}");
                DebugManager.Error($"[DialogueController] {e.StackTrace}");
            }
        }

        private void ShowDialogue(Dialog arg, bool pass)
        {
            StopAllCoroutines();

            DebugManager.Engine($"[DialogueController] Loading: {arg}");

            if (pass) _currentDialogIndex++;

            _currentDialog = arg;
            _speed = caller.Configuration.DialogueSpeed;
            
            StartCoroutine(ShowDialogueCoroutine());

            IEnumerator ShowDialogueCoroutine()
            {
                nextButton.SetActive(false);

                string dialogue = _currentDialog.Phrase;
                string dialogueName = _currentDialog.Speaker.SpeakerName;

                SetText("");
                _isWritting = true;

                nameText.text = dialogueName;

                for (int c = 0; c < dialogue.Length; c ++)
                {
                    AddText(dialogue[c].ToString());
                    yield return new WaitForSeconds(_speed);
                }

                nextButton.SetActive(true);
                _isWritting = false;
            }
        }
    }
}
