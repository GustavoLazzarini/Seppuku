//Copyright Galactspace Studios 2022

//References
using UnityEngine;
using Scriptable.Generic;

namespace Scriptable.Entities
{
	[CreateAssetMenu(menuName = "Game/Entities/Entity Configuration")]
	public class EntityConfigurationSo : ScriptableObject
	{
		//Variables
		[Header("Channels")]
		[SerializeField] private ChannelSo _impulseChannel;
		[SerializeField] private Vector2ChannelSo _damagePlayerChannel;
		[SerializeField] private Vector2ChannelSo _playerPosChannel;

		[Header("HP")]
		[SerializeField] private int _maxLife = 100;
		[SerializeField] [Range(0.1f, 1)] private float _damageAbsorsion = 1;

		[Header("Movement")]
		[SerializeField] private int _maxStamina = 100;
		[SerializeField] private float _walkSpeed = 5;
		[SerializeField] private float _climbSpeed = 5;
		[SerializeField] private float _crouchSpeed = 1.5f;
		[SerializeField] private float _runSpeed = 7;
		[SerializeField] private float _jumpSpeed = 5;
		[SerializeField] private float _airSpeedMod = 1.1f;
		[SerializeField] private float _slideSpeedMod = 1.1f;
		[SerializeField] private float _dashSpeed = 5;

		[Header("Aceleration")]
		[SerializeField] private float _acelerationRate;
		[SerializeField] private float _deacelerationRate;
		[SerializeField] private float _crouchDeacelerationRate;

		public ChannelSo ImpulseChannel => _impulseChannel;
		public Vector2ChannelSo DamagePlayerChannel => _damagePlayerChannel;
		public Vector2ChannelSo PlayerPositionChannel => _playerPosChannel;

		public int MaxLife => _maxLife;
		public float DamageAbsorsion => _damageAbsorsion;

		public int MaxStamina => _maxStamina;
		public float WalkSpeed => _walkSpeed;
		public float ClimbSpeed => _climbSpeed;
		public float CrouchSpeed => _crouchSpeed;
		public float RunSpeed => _runSpeed;
		public float JumpSpeed => _jumpSpeed;
		public float AirSpeedModifier => _airSpeedMod;
		public float SlideSpeedModifier => _slideSpeedMod;
		public float DashSpeed => _dashSpeed;

		public float AcelerationRate => _acelerationRate;
		public float DeacelerationRate => _deacelerationRate;
		public float CrouchDeacelerationRate => _crouchDeacelerationRate;
	}
}