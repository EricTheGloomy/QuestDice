// Scripts/Editor/TileEditorCustomEditor.cs
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileEditor))]
public class TileEditorCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TileEditor tileEditor = (TileEditor)target;

        EditorGUILayout.LabelField("Tile Editing Options", EditorStyles.boldLabel);

        // Update to use `selectedTerrainID` instead of `selectedTileType`
        tileEditor.selectedTerrainID = EditorGUILayout.IntField("Terrain ID", tileEditor.selectedTerrainID);
        tileEditor.radius = EditorGUILayout.IntSlider("Radius", tileEditor.radius, 0, 10);

        if (GUILayout.Button("Select Tile"))
        {
            tileEditor.SelectTile();
        }

        if (GUILayout.Button("Apply Tile Type"))
        {
            tileEditor.ApplyTileType();
        }

        // Directly call ShowFogOfWar and HideFogOfWar without null checks
        if (GUILayout.Button("Show Fog of War"))
        {
            tileEditor.ShowFogOfWar();
        }

        if (GUILayout.Button("Hide Fog of War"))
        {
            tileEditor.HideFogOfWar();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tileEditor);
        }
    }
}
