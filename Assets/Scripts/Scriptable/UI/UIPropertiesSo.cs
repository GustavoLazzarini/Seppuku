//Copyright Galactspace Studios 2022

//References
using Plawius;
using UnityEngine;
using Scriptable.Configuration;

namespace Scriptable.UI
{
	[CreateAssetMenu(menuName = "Game/UI/UI Properties")]
	public class UIPropertiesSo : ScriptableObject
	{
		[SerializeField] private ButtonTextLinkSo _linker;
		[SerializeField] private ControllerIconsSo _controllerIcons;

		public ButtonTextLinkSo Linker => _linker;
		public ControllerIconsSo Icons => _controllerIcons;
	}
}
