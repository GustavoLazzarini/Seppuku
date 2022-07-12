//Copyright Galactspace Studios 2022

//References
using Core.Types;
using UnityEngine;

namespace Scriptable.Dialogue
{
	[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue")]
	public class DialogueSo : ScriptableObject
	{
		[SerializeField] private Dialog[] dialogues;
		public Dialog[] Dialogue => dialogues;
		public int Length => dialogues.Length;
	}
}
