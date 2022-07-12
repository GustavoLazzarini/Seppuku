//Copyright Galactspace Studios 2022

//References
using UnityEngine;

namespace Scriptable.Dialogue
{
	[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue Configuration")]
	public class DialogueConfigurationSo : ScriptableObject
	{
		//Variables
		[Space]
		[SerializeField] private float _dialogueSpeed;
		public float DialogueSpeed => _dialogueSpeed;

		[SerializeField] private float _fastDialogueSpeed;
		public float FastDialogueSpeed => _fastDialogueSpeed;
	}
}
