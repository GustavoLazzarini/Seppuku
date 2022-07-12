//Made by Galactspace Studios

using UnityEngine;

namespace Core.Triggers
{
    public class TriggerExitTrigger : Trigger
    {
        private void OnTriggerExit(Collider collision)
        {
            if (collision.IsPlayer()) Call();
        }
    }
}