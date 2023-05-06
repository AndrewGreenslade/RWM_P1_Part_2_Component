using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AnimationManager))]
public class AnimationSystemMenuManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationManager myScript = (AnimationManager)target;

        if (GUILayout.Button("Export Animations"))
        {
            myScript.ExportAnims(myScript.gameObject.name.ToString());
        }

        if (GUILayout.Button("Import Animations"))
        {
            string[] myFilters = {"Scriptable Object files", "asset"};
            string path = EditorUtility.OpenFilePanelWithFilters("Open animation asset File to import", "", myFilters);
            string dataPath = Application.dataPath;

            dataPath = dataPath.Replace("/Assets", "");
            path = path.Replace(dataPath + "/", "");

            myScript.LoadExportedAnims(path);
        }
    }
}
