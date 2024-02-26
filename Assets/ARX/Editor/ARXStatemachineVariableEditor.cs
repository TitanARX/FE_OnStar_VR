using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ARXStatemachineVariable))]
public class ARXStatemachineVariableEditor : Editor
{
    SerializedProperty modelProp;
    SerializedProperty selectedVariableTypeProp;
    SerializedProperty boolDataTargetsProp;
    SerializedProperty vect2DataTargetsProp;
    SerializedProperty vect3DataTargetsProp;
    SerializedProperty floatDataTargetsProp;
    SerializedProperty intDataTargetsProp;
    SerializedProperty colorDataTargetsProp;

    SerializedProperty boolEventProp;
    SerializedProperty vect2EventProp;
    SerializedProperty vect3EventProp;
    SerializedProperty floatEventProp;
    SerializedProperty intEventProp;
    SerializedProperty colorEventProp;

    void OnEnable()
    {
        modelProp = serializedObject.FindProperty("model");
        selectedVariableTypeProp = serializedObject.FindProperty("selectedVariableType");

        boolDataTargetsProp = serializedObject.FindProperty("BoolDataTargets");
        vect2DataTargetsProp = serializedObject.FindProperty("Vect2DataTargets");
        vect3DataTargetsProp = serializedObject.FindProperty("Vect3DataTargets");
        floatDataTargetsProp = serializedObject.FindProperty("FloatDataTargets");
        intDataTargetsProp = serializedObject.FindProperty("IntDataTargets");
        colorDataTargetsProp = serializedObject.FindProperty("ColorDataTargets");

        boolEventProp = serializedObject.FindProperty("_boolEvent");
        vect2EventProp = serializedObject.FindProperty("_vect2Event");
        vect3EventProp = serializedObject.FindProperty("_vect3Event");
        floatEventProp = serializedObject.FindProperty("_floatEvent");
        intEventProp = serializedObject.FindProperty("_intEvent");
        colorEventProp = serializedObject.FindProperty("_colorEvent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(modelProp);
        EditorGUILayout.PropertyField(selectedVariableTypeProp);

        VariableType selectedType = (VariableType)selectedVariableTypeProp.enumValueIndex;

        // Draw the appropriate data targets list based on the selected variable type
        switch (selectedType)
        {
            case VariableType.Bool:
                EditorGUILayout.PropertyField(boolDataTargetsProp, true);
                EditorGUILayout.PropertyField(boolEventProp);
                break;
            case VariableType.Vect2:
                EditorGUILayout.PropertyField(vect2DataTargetsProp, true);
                EditorGUILayout.PropertyField(vect2EventProp);
                break;
            case VariableType.Vect3:
                EditorGUILayout.PropertyField(vect3DataTargetsProp, true);
                EditorGUILayout.PropertyField(vect3EventProp);
                break;
            case VariableType.Float:
                EditorGUILayout.PropertyField(floatDataTargetsProp, true);
                EditorGUILayout.PropertyField(floatEventProp);
                break;
            case VariableType.Integer:
                EditorGUILayout.PropertyField(intDataTargetsProp, true);
                EditorGUILayout.PropertyField(intEventProp);
                break;
            case VariableType.Color:
                EditorGUILayout.PropertyField(colorDataTargetsProp, true);
                EditorGUILayout.PropertyField(colorEventProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
