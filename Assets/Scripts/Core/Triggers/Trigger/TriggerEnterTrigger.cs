//Made by Galactspace Studios

using UnityEngine;

namespace Core.Triggers
{
    public class TriggerEnterTrigger : Trigger
    {
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.IsPlayer()) Call();
        }
    }
}