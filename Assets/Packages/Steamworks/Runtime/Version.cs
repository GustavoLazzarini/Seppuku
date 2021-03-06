// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2021 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

namespace Steamworks {
	public static class Version {
		public const string SteamworksNETVersion = "20.1.0";
		public const string SteamworksSDKVersion = "1.53a";
		public const string SteamAPIDLLVersion = "06.91.21.57";
		public const int SteamAPIDLLSize = 263080;
		public const int SteamAPI64DLLSize = 295336;
	}
}

#endif // !DISABLESTEAMWORKS
