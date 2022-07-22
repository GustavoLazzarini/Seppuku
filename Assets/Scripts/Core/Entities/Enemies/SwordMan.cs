//Made by Galactspace

using UnityEngine;

namespace Core.Entities
{
    public class SwordMan : Enemy
    {
        public override void Attack()
        {
            EAnimator.SetTrigger("Attack");
        }
    }
}
