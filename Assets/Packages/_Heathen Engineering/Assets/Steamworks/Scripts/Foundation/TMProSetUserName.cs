#if !DISABLESTEAMWORKS
using UnityEngine;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration.UI
{
    /// <summary>
    /// Applies the name of the indicated user to the attached label
    /// </summary>
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    public class TMProSetUserName : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI label;
        [SerializeField]
        [Tooltip("Should the component load the local user's name on Start.\nIf false you must call SetName and provide the ID of the user to load")]
        private bool useLocalUser;

        private void Start()
        {
            label = GetComponent<TMPro.TextMeshProUGUI>();

            if (useLocalUser)
            {
                var user = API.User.Client.Id;
                SetName(user);
            }
        }

        public void SetName(UserData user)
        {
            label.text = user.Name;
        }

        public void SetName(CSteamID user)
        {
            label.text = UserData.Get(user).Name;
        }

        public void SetName(ulong user)
        {
            label.text = UserData.Get(user).Name;
        }
    }
}
#endif