using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

namespace Plawius
{
    public class ControllerIcons : ScriptableObject
    {
#if UNITY_EDITOR
        [Serializable]
        public struct PlatformDefault
        {
            public BuildTarget platform;
            public TMP_FontAsset fontAsset;
        }

        [SerializeField]
        private List<PlatformDefault> m_platformDefaults;
#endif

        [SerializeField]
        private TMP_FontAsset m_activeFont;

        /// <summary>
        /// Force refresh / apply icon font
        /// </summary>
        public void ForceInit()
        {
            if (TMP_Settings.instance == null || m_activeFont == null) return;

            var font = m_activeFont;
            m_activeFont = null;
            SetActiveFont(font);
        }

        /// <summary>
        /// Returns current active icon set font that was added to Text Mesh Pro Settings' fallback list
        /// </summary>
        /// <returns>Font asset that is currently 'active' - means added to Text Mesh Pro Settings' fallback list</returns>
        public TMP_FontAsset GetActiveFont()
        {
            return m_activeFont;
        }

        /// <summary>
        /// Sets current active icon set font. Adds it to Text Mesh Pro Settings' fallback list, removing previous active from there. Refreshes all TMP_Text objects
        /// </summary>
        /// <param name="newFontToActivate">New active icon set font</param>
        /// <exception cref="Exception">If TMP_Settings.instance is null (no Text Mesh Pro installed) will throw Exception</exception>
        public void SetActiveFont(TMP_FontAsset newFontToActivate)
        {
            if (m_activeFont == newFontToActivate)
                return;

            var tmpSettings = TMP_Settings.instance;
            if (tmpSettings == null)
                throw new Exception("Cannot find TMPSettings (TMP_Settings.instance is null)! Do you have Text Mesh Pro installed / added to the packages?");

            if (m_activeFont == null)
            {
                // there was no active font, just add new one
                TMP_Settings.fallbackFontAssets.Add(newFontToActivate);
            }
            else if (newFontToActivate == null)
            {
                // there was an active font, but now there is none - remove previous one
                TMP_Settings.fallbackFontAssets.RemoveAll(f => f == m_activeFont);
            }
            else
            {
                // there was an active font, now there should be another - replace
                var replaced = false;
                for (var i = 0; i < TMP_Settings.fallbackFontAssets.Count; i++)
                {
                    if (TMP_Settings.fallbackFontAssets[i] == m_activeFont)
                    {
                        TMP_Settings.fallbackFontAssets[i] = newFontToActivate;
                        replaced = true;
                    }
                }
                if (replaced == false)
                    TMP_Settings.fallbackFontAssets.Add(newFontToActivate);
            }

            m_activeFont = newFontToActivate;

            foreach (var text in FindObjectsOfType<TMP_Text>())
            {
                text.ForceMeshUpdate(false, true);
            }
        }

        private static void RefreshAllTexts()
        {
            foreach (var text in FindObjectsOfType<TMP_Text>())
            {
                // this call is needed to apply fallback font changes
                text.ForceMeshUpdate(false, true);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Build Preprocessor that changes active icon set font according to ControllerIcon settings and current active platform.
        /// </summary>
        private class Preprocessor : IPreprocessBuildWithReport
        {
            public int callbackOrder => 0;

            public void OnPreprocessBuild(BuildReport report)
            {
                ControllerIcons instance = null;
                var guids = AssetDatabase.FindAssets($"t:{nameof(ControllerIcons)}");
                if (guids.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    instance = AssetDatabase.LoadAssetAtPath<ControllerIcons>(path);
                }

                if (instance != null && instance.m_platformDefaults != null)
                {
                    foreach (var platformDefault in instance.m_platformDefaults)
                    {
                        if (platformDefault.platform == EditorUserBuildSettings.activeBuildTarget)
                        {
                            instance.SetActiveFont(platformDefault.fontAsset);
                            Debug.Log($"[ControllerIcons] <{instance.GetActiveFont()}> set as a default icon set for {EditorUserBuildSettings.activeBuildTarget} build target", instance);
                            break;
                        }
                    }
                }
            }
        }
#endif
    }
}