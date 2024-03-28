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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tayx.Graphy
{
    [CustomEditor(typeof(GraphyDebugger))]
    internal class GraphyDebuggerEditor : Editor
    {
        #region Methods -> Unity Callbacks

        private void OnEnable()
        {
            m_target = (GraphyDebugger)target;
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

            if (GraphyEditorStyle.DebuggerLogoTexture != null)
            {
                GUILayout.Label
                (
                    GraphyEditorStyle.DebuggerLogoTexture,
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
                    "[ GRAPHY - DEBUGGER ]",
                    GraphyEditorStyle.HeaderStyle1
                );
            }

            #endregion

            GUILayout.Space(5); //Extra pixels added when the logo is used.

            #region Section -> Settings

            var serObj = serializedObject;

            var
                debugPacketList =
                    serObj.FindProperty("m_debugPackets"); // Find the List in our script and create a refrence of it

            //Update our list
            serObj.Update();

            EditorGUILayout.LabelField("Current [Debug Packets] list size: " + debugPacketList.arraySize);

            EditorGUIUtility.fieldWidth = 32;
            EditorGUILayout.BeginHorizontal();


            m_newDebugPacketListSize = EditorGUILayout.IntField
            (
                "Define a new list size",
                m_newDebugPacketListSize
            );

            if (GUILayout.Button("Resize List"))
                if (EditorUtility.DisplayDialog
                    (
                        "Resize List",
                        "Are you sure you want to resize the entire List?\n\n" +
                        "Current List Size -> " +
                        debugPacketList.arraySize +
                        "\n" +
                        "New List Size -> " +
                        m_newDebugPacketListSize +
                        "\n" +
                        "This will add default entries if the value is greater than the list size, or erase the bottom values until the new size specified.",
                        "Resize",
                        "Cancel")
                   )
                {
                    m_currentlySelectedDebugPacketIndex = 0;

                    if (m_newDebugPacketListSize != debugPacketList.arraySize)
                    {
                        while (m_newDebugPacketListSize > debugPacketList.arraySize)
                        {
                            debugPacketList.InsertArrayElementAtIndex(debugPacketList.arraySize);
                            SetDefaultDebugPacketValues(debugPacketList);
                        }

                        while (m_newDebugPacketListSize < debugPacketList.arraySize)
                            debugPacketList.DeleteArrayElementAtIndex(debugPacketList.arraySize - 1);
                    }
                }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("NOT RECOMMENDED (Only use for first initialization)",
                EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (debugPacketList.arraySize < 1)
            {
                m_previouslySelectedDebugPacketIndex = 0;
                m_currentlySelectedDebugPacketIndex = 0;
                m_selectedDebugPacketCondition = 0;

                serializedObject.ApplyModifiedProperties();
                return;
            }

            GraphyEditorStyle.HeaderStyle2.contentOffset = Vector2.down * 3f;

            EditorGUILayout.LabelField("Selected debug packet:");

            EditorGUILayout.BeginHorizontal();

            var debugPacketNames = new List<string>();
            for (var i = 0; i < debugPacketList.arraySize; i++)
            {
                var listItem = debugPacketList.GetArrayElementAtIndex(i);
                // NOTE: If the Popup detects two equal strings, it just paints 1, that's why I always add the "i"
                var checkMark = listItem.FindPropertyRelative("Active").boolValue ? '\u2714' : '\u2718';
                debugPacketNames.Add
                (
                    i + 1 +
                    " (" +
                    checkMark +
                    ") " +
                    " - ID: " +
                    listItem.FindPropertyRelative("Id").intValue +
                    " (Conditions: " +
                    listItem.FindPropertyRelative("DebugConditions").arraySize +
                    ")"
                );
            }

            m_currentlySelectedDebugPacketIndex =
                EditorGUILayout.Popup(m_currentlySelectedDebugPacketIndex, debugPacketNames.ToArray());

            if (m_currentlySelectedDebugPacketIndex != m_previouslySelectedDebugPacketIndex)
            {
                m_selectedDebugPacketCondition = 0;

                m_previouslySelectedDebugPacketIndex = m_currentlySelectedDebugPacketIndex;
            }

            var defaultGUIColor = GUI.color;

            GUI.color = new Color(0.7f, 1f, 0.0f, 1f);

            //Or add a new item to the List<> with a button

            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                debugPacketList.InsertArrayElementAtIndex(debugPacketList.arraySize);
                SetDefaultDebugPacketValues(debugPacketList);
            }

            GUI.color = new Color(1f, 0.7f, 0.0f, 1f);

            //Remove this index from the List

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                debugPacketList.DeleteArrayElementAtIndex(m_currentlySelectedDebugPacketIndex);
                if (m_currentlySelectedDebugPacketIndex > 0) m_currentlySelectedDebugPacketIndex--;

                if (debugPacketList.arraySize < 1)
                {
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
            }

            GUI.color = defaultGUIColor;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //Display our list to the inspector window

            var listItemSelected =
                debugPacketList.GetArrayElementAtIndex(m_currentlySelectedDebugPacketIndex);

            var Active = listItemSelected.FindPropertyRelative("Active");
            var Id = listItemSelected.FindPropertyRelative("Id");
            var ExecuteOnce = listItemSelected.FindPropertyRelative("ExecuteOnce");
            var InitSleepTime = listItemSelected.FindPropertyRelative("InitSleepTime");
            var ExecuteSleepTime = listItemSelected.FindPropertyRelative("ExecuteSleepTime");
            var ConditionEvaluation = listItemSelected.FindPropertyRelative("ConditionEvaluation");
            var DebugConditions = listItemSelected.FindPropertyRelative("DebugConditions");
            var MessageType = listItemSelected.FindPropertyRelative("MessageType");
            var Message = listItemSelected.FindPropertyRelative("Message");
            var TakeScreenshot = listItemSelected.FindPropertyRelative("TakeScreenshot");
            var ScreenshotFileName = listItemSelected.FindPropertyRelative("ScreenshotFileName");
            var DebugBreak = listItemSelected.FindPropertyRelative("DebugBreak");
            var UnityEvents = listItemSelected.FindPropertyRelative("UnityEvents");

            #endregion

            EditorGUILayout.LabelField
            (
                "[ PACKET ] - ID: " +
                Id.intValue +
                " (Conditions: " +
                DebugConditions.arraySize +
                ")",
                GraphyEditorStyle.HeaderStyle2
            );

            EditorGUIUtility.labelWidth = 150;
            EditorGUIUtility.fieldWidth = 35;

            Active.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Active",
                    "If false, it will not be checked"
                ),
                Active.boolValue
            );

            Id.intValue = EditorGUILayout.IntField
            (
                new GUIContent
                (
                    "ID",
                    "Optional Id. It's used to get or remove DebugPackets in runtime"
                ),
                Id.intValue
            );

            ExecuteOnce.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Execute once",
                    "If true, once the actions are executed, this DebugPacket will delete itself"
                ),
                ExecuteOnce.boolValue
            );

            InitSleepTime.floatValue = EditorGUILayout.FloatField
            (
                new GUIContent
                (
                    "Init sleep time",
                    "Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game)"
                ),
                InitSleepTime.floatValue
            );

            ExecuteSleepTime.floatValue = EditorGUILayout.FloatField
            (
                new GUIContent
                (
                    "Sleep time after execute",
                    "Time to wait before checking if conditions are met again (once they have already been met and if ExecuteOnce is false)"
                ),
                ExecuteSleepTime.floatValue
            );


            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("[ CONDITIONS ] (" + DebugConditions.arraySize + ")",
                GraphyEditorStyle.HeaderStyle2);

            EditorGUILayout.PropertyField
            (
                ConditionEvaluation,
                new GUIContent("Condition evaluation")
            );

            EditorGUILayout.Space();

            if (DebugConditions.arraySize < 1)
            {
                DebugConditions.InsertArrayElementAtIndex(DebugConditions.arraySize);
                m_selectedDebugPacketCondition = 0;
            }

            EditorGUILayout.BeginHorizontal();

            var debugPacketConditionNames = new List<string>();
            for (var i = 0; i < DebugConditions.arraySize; i++)
            {
                var listItem = DebugConditions.GetArrayElementAtIndex(i);
                // NOTE: If the Popup detects two equal strings, it just paints 1, that's why I always add the "i"

                var conditionName = i + 1 + " - ";
                conditionName +=
                    GetComparerStringFromDebugVariable(
                        (GraphyDebugger.DebugVariable)listItem.FindPropertyRelative("Variable").intValue) + " ";
                conditionName +=
                    GetComparerStringFromDebugComparer(
                        (GraphyDebugger.DebugComparer)listItem.FindPropertyRelative("Comparer").intValue) + " ";
                conditionName += listItem.FindPropertyRelative("Value").floatValue.ToString();

                debugPacketConditionNames.Add(conditionName);
            }

            m_selectedDebugPacketCondition =
                EditorGUILayout.Popup(m_selectedDebugPacketCondition, debugPacketConditionNames.ToArray());

            GUI.color = new Color(0.7f, 1f, 0.0f, 1f);

            if (GUILayout.Button("Add", GUILayout.Width(60)))
                DebugConditions.InsertArrayElementAtIndex(DebugConditions.arraySize);

            if (DebugConditions.arraySize > 1)
                GUI.color = new Color(1f, 0.7f, 0.0f, 1f);
            else
                GUI.color = new Color(1f, 0.7f, 0.0f, 0.5f);

            //Remove this index from the List
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
                if (DebugConditions.arraySize > 1)
                {
                    DebugConditions.DeleteArrayElementAtIndex(m_selectedDebugPacketCondition);
                    if (m_selectedDebugPacketCondition > 0) m_selectedDebugPacketCondition--;
                }

            GUI.color = defaultGUIColor;

            EditorGUILayout.EndHorizontal();

            var conditionListItemSelected =
                DebugConditions.GetArrayElementAtIndex(m_selectedDebugPacketCondition);

            var Variable = conditionListItemSelected.FindPropertyRelative("Variable");
            var Comparer = conditionListItemSelected.FindPropertyRelative("Comparer");
            var Value = conditionListItemSelected.FindPropertyRelative("Value");

            EditorGUILayout.PropertyField
            (
                Variable,
                new GUIContent("Variable")
            );

            EditorGUILayout.PropertyField
            (
                Comparer,
                new GUIContent("Comparer")
            );

            EditorGUILayout.PropertyField
            (
                Value,
                new GUIContent("Value")
            );

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("[ ACTIONS ]", GraphyEditorStyle.HeaderStyle2);

            EditorGUIUtility.labelWidth = 140;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                MessageType,
                new GUIContent("Message type")
            );

            EditorGUILayout.PropertyField(Message);

            TakeScreenshot.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Take screenshot",
                    "If true, it takes a screenshot and stores it. The location where the image is written to can include a directory/folder list. With no directory/folder list the image will be written into the Project folder. On mobile platforms the filename is appended to the persistent data path."
                ),
                TakeScreenshot.boolValue
            );

            if (TakeScreenshot.boolValue)
                EditorGUILayout.PropertyField
                (
                    ScreenshotFileName,
                    new GUIContent
                    (
                        "Screenshot file name",
                        "Avoid this characters: * . \" / \\ [ ] : ; | = , \n\nIt will have the date appended at the end to avoid overwriting."
                    )
                );

            DebugBreak.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    "Debug Break",
                    "If true, it pauses the editor"
                ),
                DebugBreak.boolValue
            );

            EditorGUILayout.PropertyField(UnityEvents);

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Variables -> Private

        private GraphyDebugger m_target;

        private int m_newDebugPacketListSize;

        private int m_previouslySelectedDebugPacketIndex;
        private int m_currentlySelectedDebugPacketIndex;

        private int m_selectedDebugPacketCondition;

        #endregion

        #region Methods -> Private

        private void SetDefaultDebugPacketValues(SerializedProperty debugPacketSerializedProperty)
        {
            var debugPacket = new GraphyDebugger.DebugPacket();

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("Active")
                .boolValue = debugPacket.Active;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("Id")
                .intValue = debugPacketSerializedProperty.arraySize;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("ExecuteOnce")
                .boolValue = debugPacket.ExecuteOnce;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("InitSleepTime")
                .floatValue = debugPacket.InitSleepTime;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("ExecuteSleepTime")
                .floatValue = debugPacket.ExecuteSleepTime;
        }

        private string GetComparerStringFromDebugVariable(GraphyDebugger.DebugVariable debugVariable)
        {
            switch (debugVariable)
            {
                case GraphyDebugger.DebugVariable.Fps:
                    return "FPS Current";
                case GraphyDebugger.DebugVariable.Fps_Min:
                    return "FPS Min";
                case GraphyDebugger.DebugVariable.Fps_Max:
                    return "FPS Max";
                case GraphyDebugger.DebugVariable.Fps_Avg:
                    return "FPS Avg";

                case GraphyDebugger.DebugVariable.Ram_Allocated:
                    return "Ram Allocated";
                case GraphyDebugger.DebugVariable.Ram_Reserved:
                    return "Ram Reserved";
                case GraphyDebugger.DebugVariable.Ram_Mono:
                    return "Ram Mono";

                case GraphyDebugger.DebugVariable.Audio_DB:
                    return "Audio DB";

                default:
                    return null;
            }
        }

        private string GetComparerStringFromDebugComparer(GraphyDebugger.DebugComparer debugComparer)
        {
            switch (debugComparer)
            {
                case GraphyDebugger.DebugComparer.Less_than:
                    return "<";
                case GraphyDebugger.DebugComparer.Equals_or_less_than:
                    return "<=";
                case GraphyDebugger.DebugComparer.Equals:
                    return "==";
                case GraphyDebugger.DebugComparer.Equals_or_greater_than:
                    return ">=";
                case GraphyDebugger.DebugComparer.Greater_than:
                    return ">";

                default:
                    return null;
            }
        }

        #endregion
    }
}