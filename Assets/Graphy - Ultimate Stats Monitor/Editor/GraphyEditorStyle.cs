/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            02-Jan-18
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEditor;
using UnityEngine;

namespace Tayx.Graphy
{
    internal static class GraphyEditorStyle
    {
        #region Static Constructor

        static GraphyEditorStyle()
        {
            var managerLogoGuid =
                AssetDatabase.FindAssets($"Manager_Logo_{(EditorGUIUtility.isProSkin ? "White" : "Dark")}")[0];
            var debuggerLogoGuid =
                AssetDatabase.FindAssets($"Debugger_Logo_{(EditorGUIUtility.isProSkin ? "White" : "Dark")}")[0];
            var guiSkinGuid = AssetDatabase.FindAssets("GraphyGUISkin")[0];

            ManagerLogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                AssetDatabase.GUIDToAssetPath(managerLogoGuid)
            );

            DebuggerLogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                AssetDatabase.GUIDToAssetPath(debuggerLogoGuid)
            );

            Skin = AssetDatabase.LoadAssetAtPath<GUISkin>
            (
                AssetDatabase.GUIDToAssetPath(guiSkinGuid)
            );

            if (Skin != null)
            {
                HeaderStyle1 = Skin.GetStyle("Header1");
                HeaderStyle2 = Skin.GetStyle("Header2");

                SetGuiStyleFontColor
                (
                    HeaderStyle2,
                    EditorGUIUtility.isProSkin ? Color.white : Color.black
                );
            }
            else
            {
                HeaderStyle1 = EditorStyles.boldLabel;
                HeaderStyle2 = EditorStyles.boldLabel;
            }

            FoldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                font = HeaderStyle2.font,
                fontStyle = HeaderStyle2.fontStyle,
                contentOffset = Vector2.down * 3f
            };

            SetGuiStyleFontColor
            (
                FoldoutStyle,
                EditorGUIUtility.isProSkin ? Color.white : Color.black
            );
        }

        #endregion

        #region Methods -> Private

        private static void SetGuiStyleFontColor(GUIStyle guiStyle, Color color)
        {
            guiStyle.normal.textColor = color;
            guiStyle.hover.textColor = color;
            guiStyle.active.textColor = color;
            guiStyle.focused.textColor = color;
            guiStyle.onNormal.textColor = color;
            guiStyle.onHover.textColor = color;
            guiStyle.onActive.textColor = color;
            guiStyle.onFocused.textColor = color;
        }

        #endregion

        #region Variables -> Private

        private static string path;

        #endregion

        #region Properties -> Public

        public static Texture2D ManagerLogoTexture { get; }

        public static Texture2D DebuggerLogoTexture { get; }

        public static GUISkin Skin { get; }

        public static GUIStyle HeaderStyle1 { get; }

        public static GUIStyle HeaderStyle2 { get; }

        public static GUIStyle FoldoutStyle { get; }

        #endregion
    }
}