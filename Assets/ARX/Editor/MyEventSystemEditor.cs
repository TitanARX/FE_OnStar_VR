using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyEventSystem<bool>))]
public class MyEventSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MyEventSystem<bool> eventSystem = (MyEventSystem<bool>)target;

        // Allow the user to add listener-parameter pairs
        EditorGUILayout.LabelField("Listeners");
        for (int i = 0; i < eventSystem.listenerParameterPairs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            eventSystem.listenerParameterPairs[i].targetObject = EditorGUILayout.ObjectField(eventSystem.listenerParameterPairs[i].targetObject, typeof(UnityEngine.Object), true);
            eventSystem.listenerParameterPairs[i].methodName = EditorGUILayout.TextField(eventSystem.listenerParameterPairs[i].methodName);
            eventSystem.listenerParameterPairs[i].parameter = EditorGUILayout.Toggle(eventSystem.listenerParameterPairs[i].parameter);
            EditorGUILayout.EndHorizontal();
        }

        // Add button to add new listener-parameter pair
        if (GUILayout.Button("Add Listener"))
        {
            eventSystem.listenerParameterPairs.Add(new ListenerParameterPair<bool>());
        }

        // Apply changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
