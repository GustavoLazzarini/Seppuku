//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Item;

namespace Core.Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private bool interactable;
        [SerializeField] private bool onTriggerEnter;

        [Space]
        [SerializeField] private ItemGroupChannelSo giveItemChannel;
        
        [Space]
        [SerializeField] private ItemSo[] itens;

        private void OnTriggerEnter(Collider other) 
        {
            if (!onTriggerEnter) return;
            if (other.IsPlayer()) Collect();
        }

        public void Collect()
        {
            ItemGroupSo itemGroup = ScriptableObject.CreateInstance<ItemGroupSo>();
            itemGroup.Populate(itens);

            giveItemChannel.Invoke(itemGroup);
            Destroy(gameObject);
        }
    }
}
