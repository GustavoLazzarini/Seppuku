//Copyright Galactspace Studios 2022

//References
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Scriptable.Font
{
	[CreateAssetMenu(menuName = "Game/UI/Font Pack")]
	public class FontPackSo : ScriptableObject
	{
		[Space]
		[SerializeField] private TMP_FontAsset tmpFont;
		[SerializeField] private FontAsset unityFont;

		public TMP_FontAsset TextMeshFont => tmpFont;
		public FontAsset UnityFont => unityFont;
	}
}
