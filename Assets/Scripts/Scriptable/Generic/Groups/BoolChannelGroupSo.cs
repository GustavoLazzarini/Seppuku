//Copyright Galactspace Studios 2022

//References
using System.Linq;
using UnityEngine;
using Scriptable.Abstract;

namespace Scriptable.Generic.Groups
{
	[CreateAssetMenu(menuName = "Game/Generic/Groups/Bool Channel Group")]
	public class BoolChannelGroupSo : GroupSo<BoolChannelSo>
	{
		public bool IsAllTrue() => !GroupArray.Any(x => !x.Baked);
		public bool HasAnyTrue() => GroupArray.Any(x => x.Baked);
	}
}
