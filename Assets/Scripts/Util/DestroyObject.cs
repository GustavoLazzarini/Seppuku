//Made by Galactspace Studios

using UnityEngine;

namespace Util
{
    public class DestroyObject : MonoBehaviour
    {
        public void DoDestroy() 
        {
            Destroy(gameObject);
        }

        public void DoDestroy(GameObject arg)
        {
            Destroy(arg);
        }
    }
}
