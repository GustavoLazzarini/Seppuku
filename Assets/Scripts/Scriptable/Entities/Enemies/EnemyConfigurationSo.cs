//Copyright Galactspace Studios 2022

//References
using UnityEngine;

namespace Scriptable.Entities
{
	[CreateAssetMenu(menuName = "Game/Entities/Enemy Configuration")]
	public class EnemyConfigurationSo : EntityConfigurationSo
	{
		//Variables
		[Header("Pathfinding")]
		[SerializeField] private float _relaxTime = 2;
		[SerializeField] private float _pathUpdateRate = 0.5f;
		[SerializeField] private float _waypointDistance = 2;
		[SerializeField] private float _stopWalkDistance = 4;
		
		[Header("Attack")]
		[SerializeField] private float _shootDistance;

		[Header("Collision")]
		[SerializeField] private float _collisionDamage;
		[SerializeField] private float _collisionDistance;
		[SerializeField] private Vector3 _collisionOffset;

		[Header("Reaction")]
		[SerializeField] private float _reactionSpace;
		[SerializeField] private float _angryReactionSpace;

		public float RelaxTime => _relaxTime;
		public float PathUpdateRate => _pathUpdateRate;
		public float WaypointDistance => _waypointDistance;
		public float StopWalkDistance => _stopWalkDistance;

		public float ShootDistance => _shootDistance;

		public float CollisionDamage => _collisionDamage;
		public float CollisionDistance => _collisionDistance;
		public Vector3 CollisionOffset => _collisionOffset;

		public float ReactionSpace => _reactionSpace;
		public float AngryReactionSpace => _angryReactionSpace;
	}
}