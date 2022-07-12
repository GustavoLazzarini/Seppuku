//Copyright Galactspace Studios 2022

//References
using UnityEngine;

namespace Scriptable.Configuration
{
	[CreateAssetMenu(menuName = "Game/Configuration/Game Configuration")]
	public class GameConfigurationSo : ScriptableObject
	{
		//Variables
		[SerializeField] private int _targetFramerate = 60;
		public int TargetFramerate => _targetFramerate;
	}
}
