using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public VisualTreeAsset m_UXML;

    public override VisualElement CreateInspectorGUI()
    {

        var root = new VisualElement();
        m_UXML.CloneTree(root);

        //InspectorElement.FillDefaultInspector(root, serializedObject, this);
        //return root;

        //Create a foldout for full inspector

        var foldout = new Foldout()
        {
            viewDataKey = "GameManagerFullInspectorFoldout",
            text = "Full Inspector",
        };

        InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
        root.Add(foldout);
        return root;
    }
}