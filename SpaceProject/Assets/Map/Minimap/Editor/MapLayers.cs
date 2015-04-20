using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class MapLayers
{
	static MapLayers()
	{
		CreateLayer();
	}

	static void CreateLayer()
	{
		string mapSpriteLayerName = StaticVariables.mapSpriteLayerName;

		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		
		SerializedProperty it = tagManager.GetIterator();

		bool showChildren = true;
		while (it.NextVisible(showChildren))
		{
			if (it.name.Contains("User Layer") && (it.stringValue == "" || it.stringValue == mapSpriteLayerName ))
			{

				it.stringValue = mapSpriteLayerName;
				break;
			}
		}
		tagManager.ApplyModifiedProperties();

		int mapSpriteLayer = LayerMask.NameToLayer(mapSpriteLayerName);
		for( int i = 0; i <= 31; i++)
		{
			Physics.IgnoreLayerCollision(mapSpriteLayer, i);
		}

	}
}
