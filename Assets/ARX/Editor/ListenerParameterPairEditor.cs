using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ListenerParameterPair<bool>))]
public class ListenerParameterPairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw targetObject property
        EditorGUI.PropertyField(position, property.FindPropertyRelative("targetObject"), true);
        position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetObject"));

        // Draw methodName property
        EditorGUI.PropertyField(position, property.FindPropertyRelative("methodName"));
        position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("methodName"));

        // Draw variableType property
        EditorGUI.PropertyField(position, property.FindPropertyRelative("variableType"));
        position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("variableType"));

        // Draw dynamicOptions property based on variableType
        VariableType variableType = (VariableType)property.FindPropertyRelative("variableType").enumValueIndex;
        switch (variableType)
        {
            case VariableType.Bool:
                DrawBoolOptions(position, property.FindPropertyRelative("dynamicOptions"));
                break;
            case VariableType.Float:
                DrawFloatOptions(position, property.FindPropertyRelative("dynamicOptions"));
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate total height of the properties
        float totalHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetObject")) +
                            EditorGUI.GetPropertyHeight(property.FindPropertyRelative("methodName")) +
                            EditorGUI.GetPropertyHeight(property.FindPropertyRelative("variableType"));

        // Add height for dynamicOptions property based on variableType
        VariableType variableType = (VariableType)property.FindPropertyRelative("variableType").enumValueIndex;
        switch (variableType)
        {
            case VariableType.Bool:
                totalHeight += GetBoolOptionsHeight(property.FindPropertyRelative("dynamicOptions"));
                break;
            case VariableType.Float:
                totalHeight += GetFloatOptionsHeight(property.FindPropertyRelative("dynamicOptions"));
                break;
        }

        return totalHeight;
    }

    private void DrawBoolOptions(Rect position, SerializedProperty dynamicOptions)
    {
        EditorGUI.LabelField(position, "Bool Options");

        // Draw each bool option
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            SerializedProperty option = dynamicOptions.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = option.FindPropertyRelative("name");
            SerializedProperty targetValueProp = option.FindPropertyRelative("targetValue");

            // Draw name and targetValue properties
            position.y += EditorGUI.GetPropertyHeight(nameProp);
            EditorGUI.PropertyField(position, nameProp);
            position.y += EditorGUI.GetPropertyHeight(targetValueProp);
            EditorGUI.PropertyField(position, targetValueProp);
        }
    }

    private float GetBoolOptionsHeight(SerializedProperty dynamicOptions)
    {
        float height = EditorGUIUtility.singleLineHeight * 2; // Height for label and each property
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("targetValue"));
        }
        return height;
    }

    private void DrawFloatOptions(Rect position, SerializedProperty dynamicOptions)
    {
        EditorGUI.LabelField(position, "Float Options");

        // Draw each float option
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            SerializedProperty option = dynamicOptions.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = option.FindPropertyRelative("name");
            SerializedProperty targetValueProp = option.FindPropertyRelative("targetValue");

            // Draw name and targetValue properties
            position.y += EditorGUI.GetPropertyHeight(nameProp);
            EditorGUI.PropertyField(position, nameProp);
            position.y += EditorGUI.GetPropertyHeight(targetValueProp);
            EditorGUI.PropertyField(position, targetValueProp);
        }
    }

    private float GetFloatOptionsHeight(SerializedProperty dynamicOptions)
    {
        float height = EditorGUIUtility.singleLineHeight * 2; // Height for label and each property
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("targetValue"));
        }
        return height;
    }

    private void DrawIntOptions(Rect position, SerializedProperty dynamicOptions)
    {
        EditorGUI.LabelField(position, "Int Options");

        // Draw each int option
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            SerializedProperty option = dynamicOptions.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = option.FindPropertyRelative("name");
            SerializedProperty targetValueProp = option.FindPropertyRelative("targetValue");

            // Draw name and targetValue properties
            position.y += EditorGUI.GetPropertyHeight(nameProp);
            EditorGUI.PropertyField(position, nameProp);
            position.y += EditorGUI.GetPropertyHeight(targetValueProp);
            EditorGUI.PropertyField(position, targetValueProp);
        }
    }

    private float GetIntOptionsHeight(SerializedProperty dynamicOptions)
    {
        float height = EditorGUIUtility.singleLineHeight * 2; // Height for label and each property
        for (int i = 0; i < dynamicOptions.arraySize; i++)
        {
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
            height += EditorGUI.GetPropertyHeight(dynamicOptions.GetArrayElementAtIndex(i).FindPropertyRelative("targetValue"));
        }
        return height;
    }
}
