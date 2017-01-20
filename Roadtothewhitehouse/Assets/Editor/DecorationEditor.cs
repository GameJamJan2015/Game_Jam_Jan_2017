using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineDecorator))]
public class DecorationEditor : Editor {

    private SplineDecorator decoration;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        decoration = target as SplineDecorator;
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Generate spline"))
        {
            Undo.RecordObject(decoration, "Generate spline");
            decoration.GenerateCurve();
            EditorUtility.SetDirty(decoration);
        }

        if (GUILayout.Button("Remove spline"))
        {
            Undo.RecordObject(decoration, "Remove spline");
            decoration.RemoveAll();
            EditorUtility.SetDirty(decoration);
        }
    }
}
