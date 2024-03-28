/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            20-Dec-17
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
    [CustomEditor(typeof(GraphyManager))]
    internal class GraphyManagerEditor : Editor
    {
        #region Methods -> Unity Callbacks

        private void OnEnable()
        {
            m_target = (GraphyManager)target;

            var serObj = serializedObject;

            #region Section -> Settings

            m_graphyMode = serObj.FindProperty("m_graphyMode");

            m_enableOnStartup = serObj.FindProperty("m_enableOnStartup");

            m_keepAlive = serObj.FindProperty("m_keepAlive");

            m_background = serObj.FindProperty("m_background");
            m_backgroundColor = serObj.FindProperty("m_backgroundColor");

            m_enableHotkeys = serObj.FindProperty("m_enableHotkeys");

            m_toggleModeKeyCode = serObj.FindProperty("m_toggleModeKeyCode");

            m_toggleModeCtrl = serObj.FindProperty("m_toggleModeCtrl");
            m_toggleModeAlt = serObj.FindProperty("m_toggleModeAlt");

            m_toggleActiveKeyCode = serObj.FindProperty("m_toggleActiveKeyCode");

            m_toggleActiveCtrl = serObj.FindProperty("m_toggleActiveCtrl");
            m_toggleActiveAlt = serObj.FindProperty("m_toggleActiveAlt");

            m_graphModulePosition = serObj.FindProperty("m_graphModulePosition");

            m_graphModuleOffset = serObj.FindProperty("m_graphModuleOffset");

            #endregion

            #region Section -> FPS

            m_fpsModuleState = serObj.FindProperty("m_fpsModuleState");

            m_goodFpsColor = serObj.FindProperty("m_goodFpsColor");
            m_goodFpsThreshold = serObj.FindProperty("m_goodFpsThreshold");

            m_cautionFpsColor = serObj.FindProperty("m_cautionFpsColor");
            m_cautionFpsThreshold = serObj.FindProperty("m_cautionFpsThreshold");

            m_criticalFpsColor = serObj.FindProperty("m_criticalFpsColor");

            m_fpsGraphResolution = serObj.FindProperty("m_fpsGraphResolution");

            m_fpsTextUpdateRate = serObj.FindProperty("m_fpsTextUpdateRate");

            #endregion

            #region Section -> RAM

            m_ramModuleState = serObj.FindProperty("m_ramModuleState");

            m_allocatedRamColor = serObj.FindProperty("m_allocatedRamColor");
            m_reservedRamColor = serObj.FindProperty("m_reservedRamColor");
            m_monoRamColor = serObj.FindProperty("m_monoRamColor");

            m_ramGraphResolution = serObj.FindProperty("m_ramGraphResolution");

            m_ramTextUpdateRate = serObj.FindProperty("m_ramTextUpdateRate");

            #endregion

            #region Section -> Audio

            m_findAudioListenerInCameraIfNull = serObj.FindProperty("m_findAudioListenerInCameraIfNull");

            m_audioListener = serObj.FindProperty("m_audioListener");

            m_audioModuleState = serObj.FindProperty("m_audioModuleState");

            m_audioGraphColor = serObj.FindProperty("m_audioGraphColor");

            m_audioGraphResolution = serObj.FindProperty("m_audioGraphResolution");

            m_audioTextUpdateRate = serObj.FindProperty("m_audioTextUpdateRate");

            m_FFTWindow = serObj.FindProperty("m_FFTWindow");

            m_spectrumSize = serObj.FindProperty("m_spectrumSize");

            #endregion

            #region Section -> Advanced Settings

            m_advancedModulePosition = serObj.FindProperty("m_advancedModulePosition");

            m_advancedModuleOffset = serObj.FindProperty("m_advancedModuleOffset");

            m_advancedModuleState = serObj.FindProperty("m_advancedModuleState");

            #endregion
        }

        #endregion

        #region Methods -> Public Override

        public override void OnInspectorGUI()
        {
            if (m_target == null && target == null)
            {
                base.OnInspectorGUI();
                return;
            }

            var defaultLabelWidth = EditorGUIUtility.labelWidth;
            var defaultFieldWidth = EditorGUIUtility.fieldWidth;

            //===== CONTENT REGION ========================================================================

            GUILayout.Space(20);

            #region Section -> Logo

            if (GraphyEditorStyle.ManagerLogoTexture != null)
            {
                GUILayout.Label
                (
                    GraphyEditorStyle.ManagerLogoTexture,
                    new GUIStyle(GUI.skin.GetStyle("Label"))
                    {
                        alignment = TextAnchor.UpperCenter
                    }
                );

                GUILayout.Space(10);
            }
            else
            {
                EditorGUILayout.LabelField
                (
                    "[ GRAPHY - MANAGER ]",
                    GraphyEditorStyle.HeaderStyle1
                );
            }

            #endregion

            GUILayout.Space(5); //Extra pixels added when the logo is used.

            #region Section -> Settings

            EditorGUIUtility.labelWidth = 130;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                m_graphyMode,
                new GUIContent
                (
                    "Graphy Mode",
                    "LIGHT mode increases compatibility with mobile and older, less powerful GPUs, but reduces the maximum graph resolutions to 128."
                )
            );

            GUILayout.Space(10);

            m_enableOnStartup.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Enable On Startup",
                    "If ticked, Graphy will be displayed by default on startup, otherwise it will initiate and hide."
                ),
                m_enableOnStartup.boolValue
            );

            // This is a neat trick to hide Graphy in the Scene if it's going to be deactivated in play mode so that it doesn't use screen space.
            if (!Application.isPlaying) m_target.GetComponent<Canvas>().enabled = m_enableOnStartup.boolValue;

            m_keepAlive.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Keep Alive",
                    "If ticked, it will survive scene changes.\n\nCAREFUL, if you set Graphy as a child of another GameObject, the root GameObject will also survive scene changes. If you want to avoid that put Graphy in the root of the Scene as its own entity."
                ),
                m_keepAlive.boolValue
            );

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            m_background.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Background",
                    "If ticked, it will show a background overlay to improve readability in cluttered scenes."
                ),
                m_background.boolValue
            );

            m_backgroundColor.colorValue = EditorGUILayout.ColorField(m_backgroundColor.colorValue);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            m_enableHotkeys.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Enable Hotkeys",
                    "If ticked, it will enable the hotkeys to be able to modify Graphy in runtime with custom keyboard shortcuts."
                ),
                m_enableHotkeys.boolValue
            );

            if (m_enableHotkeys.boolValue)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 130;
                EditorGUIUtility.fieldWidth = 35;

                EditorGUILayout.PropertyField
                (
                    m_toggleModeKeyCode,
                    new GUIContent
                    (
                        "Toggle Mode Key",
                        "If ticked, it will require clicking this key and the other ones you have set up."
                    )
                );

                EditorGUIUtility.labelWidth = 30;
                EditorGUIUtility.fieldWidth = 35;

                m_toggleModeCtrl.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        "Ctrl",
                        "If ticked, it will require clicking Ctrl and the other keys you have set up."
                    ),
                    m_toggleModeCtrl.boolValue
                );

                m_toggleModeAlt.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        "Alt",
                        "If ticked, it will require clicking Alt and the other keys you have set up."
                    ),
                    m_toggleModeAlt.boolValue
                );

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 130;
                EditorGUIUtility.fieldWidth = 35;

                EditorGUILayout.PropertyField
                (
                    m_toggleActiveKeyCode,
                    new GUIContent
                    (
                        "Toggle Active Key",
                        "If ticked, it will require clicking this key and the other ones you have set up."
                    )
                );

                EditorGUIUtility.labelWidth = 30;
                EditorGUIUtility.fieldWidth = 35;

                m_toggleActiveCtrl.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        "Ctrl",
                        "If ticked, it will require clicking Ctrl and the other kesy you have set up."
                    ),
                    m_toggleActiveCtrl.boolValue
                );

                m_toggleActiveAlt.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        "Alt",
                        "If ticked, it will require clicking Alt and the other keys you have set up."
                    ),
                    m_toggleActiveAlt.boolValue
                );

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(15);

            EditorGUIUtility.labelWidth = 155;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                m_graphModulePosition,
                new GUIContent
                (
                    "Graph modules position",
                    "Defines in which corner the modules will be located."
                )
            );

            EditorGUILayout.PropertyField
            (
                m_graphModuleOffset,
                new GUIContent
                (
                    "Graph modules offset",
                    "Defines how far from the corner the module will be located."
                )
            );

            #endregion

            GUILayout.Space(20);

            #region Section -> FPS

            m_fpsModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_fpsModuleInspectorToggle,
                " [ FPS ]",
                style: GraphyEditorStyle.FoldoutStyle,
                toggleOnLabelClick: true
            );

            GUILayout.Space(5);

            if (m_fpsModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_fpsModuleState,
                    new GUIContent
                    (
                        "Module state",
                        "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.LabelField("Fps thresholds and colors:");

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_goodFpsThreshold.intValue = EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        "- Good",
                        "When FPS rise above this value, this color will be used."
                    ),
                    m_goodFpsThreshold.intValue
                );

                m_goodFpsColor.colorValue = EditorGUILayout.ColorField(m_goodFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                if (m_goodFpsThreshold.intValue <= m_cautionFpsThreshold.intValue && m_goodFpsThreshold.intValue > 1)
                    m_cautionFpsThreshold.intValue = m_goodFpsThreshold.intValue - 1;
                else if (m_goodFpsThreshold.intValue <= 1) m_goodFpsThreshold.intValue = 2;

                EditorGUILayout.BeginHorizontal();

                m_cautionFpsThreshold.intValue = EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        "- Caution",
                        "When FPS falls between this and the Good value, this color will be used."
                    ),
                    m_cautionFpsThreshold.intValue
                );

                m_cautionFpsColor.colorValue = EditorGUILayout.ColorField(m_cautionFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                if (m_cautionFpsThreshold.intValue >= m_goodFpsThreshold.intValue)
                    m_cautionFpsThreshold.intValue = m_goodFpsThreshold.intValue - 1;
                else if (m_cautionFpsThreshold.intValue <= 0) m_cautionFpsThreshold.intValue = 1;

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        "- Critical",
                        "When FPS falls below the Caution value, this color will be used. (You can't have negative FPS, so this value is just for reference, it can't be changed)."
                    ),
                    0
                );

                m_criticalFpsColor.colorValue = EditorGUILayout.ColorField(m_criticalFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;

                if (m_fpsModuleState.intValue == 0)
                    m_fpsGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent
                        (
                            "Graph resolution",
                            "Defines the amount of points in the graph"
                        ),
                        m_fpsGraphResolution.intValue,
                        20,
                        m_graphyMode.intValue == 0 ? 300 : 128
                    );

                m_fpsTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        "Text update rate",
                        "Defines the amount times the text is updated in 1 second."
                    ),
                    m_fpsTextUpdateRate.intValue,
                    1,
                    60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> RAM

            m_ramModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_ramModuleInspectorToggle,
                " [ RAM ]",
                style: GraphyEditorStyle.FoldoutStyle,
                toggleOnLabelClick: true
            );

            GUILayout.Space(5);

            if (m_ramModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_ramModuleState,
                    new GUIContent
                    (
                        "Module state",
                        "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.LabelField("Graph colors:");

                EditorGUI.indentLevel++;

                m_allocatedRamColor.colorValue = EditorGUILayout.ColorField
                (
                    "- Allocated",
                    m_allocatedRamColor.colorValue
                );

                m_reservedRamColor.colorValue = EditorGUILayout.ColorField
                (
                    "- Reserved",
                    m_reservedRamColor.colorValue
                );

                m_monoRamColor.colorValue = EditorGUILayout.ColorField
                (
                    "- Mono",
                    m_monoRamColor.colorValue
                );

                EditorGUI.indentLevel--;

                if (m_ramModuleState.intValue == 0)
                    m_ramGraphResolution.intValue = EditorGUILayout.IntSlider(
                        new GUIContent
                        (
                            "Graph resolution",
                            "Defines the amount of points are in the graph"
                        ),
                        m_ramGraphResolution.intValue,
                        20,
                        m_graphyMode.intValue == 0 ? 300 : 128
                    );

                m_ramTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        "Text update rate",
                        "Defines the amount times the text is updated in 1 second."
                    ),
                    m_ramTextUpdateRate.intValue,
                    1,
                    60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> Audio

            m_audioModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_audioModuleInspectorToggle,
                " [ AUDIO ]",
                style: GraphyEditorStyle.FoldoutStyle,
                toggleOnLabelClick: true
            );

            GUILayout.Space(5);

            if (m_audioModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_audioModuleState,
                    new GUIContent
                    (
                        "Module state",
                        "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.PropertyField
                (
                    m_findAudioListenerInCameraIfNull,
                    new GUIContent
                    (
                        "Find audio listener",
                        "Tries to find the AudioListener in the Main camera in the scene. (if AudioListener is null)"
                    )
                );

                EditorGUILayout.PropertyField
                (
                    m_audioListener,
                    new GUIContent
                    (
                        "Audio Listener",
                        "Graphy will take the data from this Listener. If none are specified, it will try to get it from the Main Camera in the scene."
                    )
                );

                if (m_audioModuleState.intValue == 0)
                {
                    m_audioGraphColor.colorValue = EditorGUILayout.ColorField
                    (
                        "Graph color",
                        m_audioGraphColor.colorValue
                    );

                    m_audioGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent
                        (
                            "Graph resolution",
                            "Defines the amount of points that are in the graph."
                        ),
                        m_audioGraphResolution.intValue,
                        20,
                        m_graphyMode.intValue == 0 ? 300 : 128
                    );

                    // Forces the value to be a multiple of 3, this way the audio graph is painted correctly
                    if (m_audioGraphResolution.intValue % 3 != 0 && m_audioGraphResolution.intValue < 300)
                        m_audioGraphResolution.intValue += 3 - m_audioGraphResolution.intValue % 3;
                    //TODO: Figure out why a static version of the ForceMultipleOf3 isnt used.
                }

                EditorGUILayout.PropertyField
                (
                    m_FFTWindow,
                    new GUIContent
                    (
                        "FFT Window",
                        "Used to reduce leakage between frequency bins/bands. Note, the more complex window type, the better the quality, but reduced speed. \n\nSimplest is rectangular. Most complex is BlackmanHarris"
                    )
                );

                m_spectrumSize.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        "Spectrum size",
                        "Has to be a power of 2 between 128-8192. The higher sample rate, the less precision but also more impact on performance. Careful with mobile devices"
                    ),
                    m_spectrumSize.intValue,
                    128,
                    8192
                );

                var closestSpectrumIndex = 0;
                var minDistanceToSpectrumValue = 100000;

                for (var i = 0; i < m_spectrumSizeValues.Length; i++)
                {
                    var newDistance = Mathf.Abs
                    (
                        m_spectrumSize.intValue - m_spectrumSizeValues[i]
                    );

                    if (newDistance < minDistanceToSpectrumValue)
                    {
                        minDistanceToSpectrumValue = newDistance;
                        closestSpectrumIndex = i;
                    }
                }

                m_spectrumSize.intValue = m_spectrumSizeValues[closestSpectrumIndex];

                m_audioTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        "Text update rate",
                        "Defines the amount times the text is updated in 1 second"
                    ),
                    m_audioTextUpdateRate.intValue,
                    1,
                    60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> Advanced Settings

            m_advancedModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_advancedModuleInspectorToggle,
                " [ ADVANCED DATA ]",
                style: GraphyEditorStyle.FoldoutStyle,
                toggleOnLabelClick: true
            );

            GUILayout.Space(5);

            if (m_advancedModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_advancedModulePosition);

                EditorGUILayout.PropertyField
                (
                    m_advancedModuleOffset,
                    new GUIContent
                    (
                        "Advanced modules offset",
                        "Defines how far from the corner the module will be located."
                    )
                );

                EditorGUILayout.PropertyField
                (
                    m_advancedModuleState,
                    new GUIContent
                    (
                        "Module state",
                        "FULL -> Text \nOFF -> Turned off"
                    )
                );
            }

            #endregion;

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Variables -> Private

        private GraphyManager m_target;

        private readonly int[] m_spectrumSizeValues =
        {
            128,
            256,
            512,
            1024,
            2048,
            4096,
            8192
        };

        #region Section -> Settings

        private SerializedProperty m_graphyMode;

        private SerializedProperty m_enableOnStartup;

        private SerializedProperty m_keepAlive;

        private SerializedProperty m_background;
        private SerializedProperty m_backgroundColor;

        private SerializedProperty m_enableHotkeys;

        private SerializedProperty m_toggleModeKeyCode;
        private SerializedProperty m_toggleModeCtrl;
        private SerializedProperty m_toggleModeAlt;

        private SerializedProperty m_toggleActiveKeyCode;
        private SerializedProperty m_toggleActiveCtrl;
        private SerializedProperty m_toggleActiveAlt;


        private SerializedProperty m_graphModulePosition;
        private SerializedProperty m_graphModuleOffset;

        #endregion

        #region Section -> FPS

        private bool m_fpsModuleInspectorToggle = true;

        private SerializedProperty m_fpsModuleState;

        private SerializedProperty m_goodFpsColor;
        private SerializedProperty m_goodFpsThreshold;

        private SerializedProperty m_cautionFpsColor;
        private SerializedProperty m_cautionFpsThreshold;

        private SerializedProperty m_criticalFpsColor;

        private SerializedProperty m_fpsGraphResolution;

        private SerializedProperty m_fpsTextUpdateRate;

        #endregion

        #region Section -> RAM

        private bool m_ramModuleInspectorToggle = true;

        private SerializedProperty m_ramModuleState;

        private SerializedProperty m_allocatedRamColor;
        private SerializedProperty m_reservedRamColor;
        private SerializedProperty m_monoRamColor;

        private SerializedProperty m_ramGraphResolution;

        private SerializedProperty m_ramTextUpdateRate;

        #endregion

        #region Section -> Audio

        private bool m_audioModuleInspectorToggle = true;

        private SerializedProperty m_findAudioListenerInCameraIfNull;

        private SerializedProperty m_audioListener;

        private SerializedProperty m_audioModuleState;

        private SerializedProperty m_audioGraphColor;

        private SerializedProperty m_audioGraphResolution;

        private SerializedProperty m_audioTextUpdateRate;

        private SerializedProperty m_FFTWindow;

        private SerializedProperty m_spectrumSize;

        #endregion

        #region Section -> Advanced Settings

        private bool m_advancedModuleInspectorToggle = true;

        private SerializedProperty m_advancedModulePosition;

        private SerializedProperty m_advancedModuleOffset;

        private SerializedProperty m_advancedModuleState;

        #endregion

        #endregion

        #region Methods -> Private

        #endregion
    }
}