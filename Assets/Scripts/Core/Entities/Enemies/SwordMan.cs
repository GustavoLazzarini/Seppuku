//Made by Galactspace

using UnityEngine;

namespace Core.Entities
{
    public class SwordMan : Enemy
    {
        public override void Attack()
        {
            _entityAnimator.SetTrigger("Attack");
        }
    }
}
