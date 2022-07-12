//Copyright Galactspace Studios 2022

//References
using TMPro;
using System;
using UnityEngine;
using Scriptable.Font;
using UnityEngine.TextCore.Text;

namespace Scriptable.Configuration
{
	[CreateAssetMenu(menuName = "Game/Configuration/Controller Icons")]
	public class ControllerIconsSo : ScriptableObject
	{
		[Space]
		[SerializeField] private FontAsset mainFont;
		[SerializeField] private FontPackSo activeFont;

		[Space]
		[SerializeField] private FontPackSo pcFont;
		[SerializeField] private FontPackSo xboxFont;
		[SerializeField] private FontPackSo playstationFont;

		public FontPackSo PcFont => pcFont;
		public FontPackSo XboxFont => xboxFont;
		public FontPackSo PlaystationFont => playstationFont;

		public FontAsset MainFont => mainFont;
		public FontPackSo ActiveFont => activeFont;

		public Action OnFontUpdate { get; set; } = delegate { };

		private bool HasActiveFont => activeFont != null;
		private bool HasTMPInstace => TMP_Settings.instance != null;

		private bool IsSameFont(FontPackSo font) => ActiveFont == font;

		public void ForceInit()
		{
			if (!HasTMPInstace || !HasActiveFont)
			{
				DebugManager.Engine($"[ControllerIconsSo] Could not force init");
				return;
			}

			FlushActiveFont();
		}

		private void FlushActiveFont()
		{
			FontPackSo current = activeFont;
			activeFont = null;
			SetActiveFont(current);
		}

		public void SetActiveFont(FontPackSo target)
		{
			if (IsSameFont(target) || !HasTMPInstace)
			{
				return;
			}

			TMP_Settings settings = TMP_Settings.instance;			

			if (activeFont == null)
			{
				MainFont.fallbackFontAssetTable.Clear();
				TMP_Settings.fallbackFontAssets.Clear();

				mainFont.fallbackFontAssetTable.Add(target.UnityFont);
				TMP_Settings.fallbackFontAssets.Add(target.TextMeshFont);
			}
			else if (target.TextMeshFont == null || target.UnityFont == null)
			{
				mainFont.fallbackFontAssetTable.RemoveAll(x => x == activeFont.UnityFont);
				TMP_Settings.fallbackFontAssets.RemoveAll(x => x == activeFont.TextMeshFont);
			}
			else
			{
				bool replaced = false;
				bool replaced1 = false;

				for (int i = 0; i < TMP_Settings.fallbackFontAssets.Count; i++)
				{
					if (TMP_Settings.fallbackFontAssets[i] == activeFont.TextMeshFont)
					{
						TMP_Settings.fallbackFontAssets[i] = target.TextMeshFont;
						replaced = true;
					}
				}

				for (int i = 0; i < mainFont.fallbackFontAssetTable.Count; i++)
				{
					if (mainFont.fallbackFontAssetTable[i] == activeFont.UnityFont)
					{
						mainFont.fallbackFontAssetTable[i] = target.UnityFont;
						replaced1 = true;
					}
				}

				if (!replaced1) mainFont.fallbackFontAssetTable.Add(target.UnityFont);
				if (!replaced) TMP_Settings.fallbackFontAssets.Add(target.TextMeshFont);
			}

			activeFont = target;
			RefreshAllTexts();
			OnFontUpdate?.Invoke();
            
			DebugManager.Engine($"[ControllerIconsSO] Updated font to: {activeFont.name}");
		}

		private void RefreshAllTexts()
        {
			foreach (var text in FindObjectsOfType<TMP_Text>())
				text.ForceMeshUpdate(false, true);
		}
	}
}
