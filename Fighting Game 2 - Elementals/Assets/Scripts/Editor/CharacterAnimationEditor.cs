using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

[CustomPropertyDrawer(typeof(CharacterAnimation))]
public class CharacterAnimationEditor : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        root.Add(new PropertyField(property.FindPropertyRelative("Type")));
        root.Add(new PropertyField(property.FindPropertyRelative("canChangeFaceDirection")));
        var animationClip = new PropertyField(property.FindPropertyRelative("Clip"));
        root.Add(animationClip);

        var clipInspector = new Box();
        root.Add(clipInspector);

        animationClip.RegisterCallback<ChangeEvent<Object>, VisualElement>(ClipChanged, clipInspector);

        return root;
    }

    void ClipChanged(ChangeEvent<Object> evt,  VisualElement clipInspector)
    {
        clipInspector.Clear();

        var t = evt.newValue;
        if (t == null) return;

        clipInspector.Add(new InspectorElement(t));
    }
}