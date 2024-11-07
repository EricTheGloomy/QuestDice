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

        // Dropdown to select tile type
        tileEditor.selectedTileType = (TileType)EditorGUILayout.EnumPopup("Tile Type", tileEditor.selectedTileType);

        // Slider to set radius
        tileEditor.radius = EditorGUILayout.IntSlider("Radius", tileEditor.radius, 0, 10);

        // Button to select the tile
        if (GUILayout.Button("Select Tile"))
        {
            tileEditor.SelectTile();
        }

        // Button to apply the selected tile type to the selected cell and nearby cells
        if (GUILayout.Button("Apply Tile Type"))
        {
            tileEditor.ApplyTileType();
        }

        // Button to enable fog of war on all tiles
        if (GUILayout.Button("Show Fog of War"))
        {
            tileEditor.ShowFogOfWar();
        }

        // Button to disable fog of war on all tiles
        if (GUILayout.Button("Hide Fog of War"))
        {
            tileEditor.HideFogOfWar();
        }

        // Make sure any changes in the editor are saved to the object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(tileEditor);
        }
    }
}
