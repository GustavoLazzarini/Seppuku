//Copyright Galactspace Studios 2022

//References
using UnityEngine;

namespace Scriptable.Item
{
	[CreateAssetMenu(menuName = "Game/Item/Consumable")]
	public class ConsumableSo : ItemSo
	{
		//Variables
		[Space]
		[SerializeField] private float _lifeRecover;
		public float LifeRecover => _lifeRecover;

		[SerializeField] private float _manaRecover;
		public float ManaRecover => _manaRecover;

		[SerializeField] private float _staminaRecover;
		public float StaminaRecover => _staminaRecover;
	}
}
