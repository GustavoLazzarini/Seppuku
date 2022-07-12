//Made by Galactspace Studios

using UnityEngine;
using System.Threading.Tasks;

namespace Core.Popups
{
    public class TutorialPopup : Popup
    {
        [SerializeField] private Animator animator;

        public override async Task HidePopup()
        {
            animator.SetTrigger("Out");
            await Taskf.WaitSeconds(0.8f);
        }
    }
}
