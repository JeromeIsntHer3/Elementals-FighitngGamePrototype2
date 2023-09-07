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

        var animTypeEnum = new PropertyField(property.FindPropertyRelative("Type"));
        var canChange = new PropertyField(property.FindPropertyRelative("CanChangeFaceDirection"));
        var animationClip = new PropertyField(property.FindPropertyRelative("Clip"));
        

        var clipInspector = new Box();
        root.Add(clipInspector);

        var fold = new Foldout()
        {
            viewDataKey = "dateKeyCharacterAnimationEditor"
        };
        fold.Add(animTypeEnum);
        fold.Add(canChange);
        fold.Add(animationClip);
        //fold.Add(new PropertyField(property.FindPropertyRelative("AnimationCondition")));
        fold.Add(new PropertyField(property.FindPropertyRelative("IsFullyAnimated")));
        //fold.Add(clipInspector);
        root.Add(fold);


        //animationClip.RegisterCallback<ChangeEvent<Object>, VisualElement>(ClipChanged, clipInspector);
        animTypeEnum.RegisterCallback<ChangeEvent<string>, Foldout>(TypeChanged, fold);


        return root;
    }

    void ClipChanged(ChangeEvent<Object> evt,  VisualElement clipInspector)
    {
        clipInspector.Clear();

        var t = evt.newValue;
        if (t == null) return;

        clipInspector.Add(new InspectorElement(t));
    }

    void TypeChanged(ChangeEvent<string> evt, Foldout fold)
    {
        fold.text = "";
        fold.text = evt.newValue;
    }
}