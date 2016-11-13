using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BindingPropertyNameAttribute))]
public class BindingPropertyNameDrawer : PropertyDrawer
{
    const int helpHeight = 32;
    const int textHeight = 16;
    
    private BindingPropertyNameAttribute regexAttribute { get { return ((BindingPropertyNameAttribute)attribute); } }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        return base.GetPropertyHeight(prop, label) + (IsValid(prop) ? 0 : helpHeight);
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        var textFieldPosition = position;
        textFieldPosition.height = textHeight;
        DrawTextField(textFieldPosition, prop, label);
        
        var helpPosition = EditorGUI.IndentedRect(position);
        helpPosition.y += textHeight + 1;
        helpPosition.height = helpHeight;
        DrawHelpBox(helpPosition, prop);
    }

    private void DrawTextField(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        var value = EditorGUI.TextField(position, label, prop.stringValue);
        if (EditorGUI.EndChangeCheck())
        {
            prop.stringValue = value;

            if (EditorApplication.isPlaying)
            {
                var bindView = prop.serializedObject.targetObject as BindViewMonoBehaviour;
                if (bindView != null)
                {
                    bindView.SendMessage("OnDisable");
                    prop.serializedObject.ApplyModifiedProperties();
                    bindView.SendMessage("OnEnable");
                }
            }
        }
    }

    private void DrawHelpBox(Rect position, SerializedProperty prop)
    {
        if (IsValid(prop)) return;
        EditorGUI.HelpBox(position, regexAttribute.helpMessage, MessageType.Error);
    }

    private bool IsValid(SerializedProperty prop)
    {
        var parts = prop.stringValue.Split(regexAttribute.separator);
        for (int i = 0; i < parts.Length; i++)
            if (string.IsNullOrEmpty(parts[i])) return false;
        return true;
    }
}