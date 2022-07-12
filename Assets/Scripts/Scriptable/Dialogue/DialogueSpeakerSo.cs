//Copyright Galactspace Studios 2022

//References
using UnityEngine;

namespace Scriptable.Dialogue
{
	[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue Speaker")]
	public class DialogueSpeakerSo : ScriptableObject
	{
		[SerializeField] private string _name;
		[SerializeField] private Sprite _portrait;
		
		public string SpeakerName => _name;
		public Sprite SpeakerPortrait => _portrait;

        public override string ToString()
        {
			return $"{_name}";
        }
    }
}
