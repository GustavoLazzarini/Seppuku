//Copyright Galactspace Studios 2022

//References
using UnityEngine;
using UnityEngine.Localization;

namespace Scriptable.Item
{
	[CreateAssetMenu(menuName = "Game/Item/Item")]
	public class ItemSo : ScriptableObject
	{
		//Variables
		[Space]
		[SerializeField] private Sprite _itemSprite;
		public Sprite ItemSprite => _itemSprite;

		[Space]
		[SerializeField] private LocalizedString _itemName;
		public LocalizedString ItemNameString => _itemName;
		public string ItemName => _itemName.GetLocalizedString();

		[Space]
		[SerializeField] private float _price;
		public float Price => _price;
	}
}
