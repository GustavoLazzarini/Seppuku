//Made by Galactspace Studios

using UnityEngine;

namespace Core.Triggers
{
    public class ColliderExitTrigger : Trigger
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.IsPlayer()) Call();
        }
    }
}