#if !DISABLESTEAMWORKS
using UnityEngine;
using UnityEngine.Events;
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration.UI
{
    /// <summary>
    /// Applies the avatar of the indicated user to the attached RawImage
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.RawImage))]
    public class SetUserAvatar : MonoBehaviour
    {
        private UnityEngine.UI.RawImage image;
        [SerializeField]
        [Tooltip("Should the component load the local user's avatar on Start.\nIf false you must call LoadAvatar and provide the ID of the user to load")]
        private bool useLocalUser;
        public UnityEvent evtLoaded;

        private void Start()
        {
            image = GetComponent<UnityEngine.UI.RawImage>();

            if (useLocalUser)
            {
                var user = API.User.Client.Id;
                LoadAvatar(user);
            }
        }

        public void LoadAvatar(UserData user) => user.LoadAvatar((r) =>
        {
            image.texture = r;
            evtLoaded?.Invoke();
        });

        public void LoadAvatar(CSteamID user) => UserData.Get(user).LoadAvatar((r) =>
        {
            image.texture = r;
            evtLoaded?.Invoke();
        });

        public void LoadAvatar(ulong user) => UserData.Get(user).LoadAvatar((r) =>
        {
            image.texture = r;
            evtLoaded?.Invoke();
        });
    }
}
#endif