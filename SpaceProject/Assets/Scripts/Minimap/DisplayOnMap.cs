using UnityEngine;
using System.Collections;
using System.Linq;

public class DisplayOnMap : MonoBehaviour 
{
	[SerializeField] public MinimapManager.MinimapObjectType type;

	private MinimapManager _minimapManager;

	private GameObject _iconObject;
	private GameObject _selectedSpriteObject;
	
	private void Start () 
	{
		 _minimapManager = MinimapManager.GetInstance();

		_iconObject = CreateMapSprite("MapIcon", _minimapManager.mapObjectList.Find(f => f.type == type).sprite);
		_iconObject.AddComponent<SphereCollider>();

		_selectedSpriteObject = CreateMapSprite("MapSelectedIcon", _minimapManager.selectedSprite);
		_selectedSpriteObject.transform.SetParent(_iconObject.transform);
		_selectedSpriteObject.SetActive(false);

		_minimapManager.displayOnMapObjectList.Add(this);

	}

	private GameObject CreateMapSprite(string name, Sprite sprite)
	{
		GameObject mapSpriteObject = new GameObject(name);
		mapSpriteObject.transform.position = transform.position;
		mapSpriteObject.transform.SetParent(gameObject.transform);
		mapSpriteObject.transform.Rotate(90, 0, 0);
		mapSpriteObject.AddComponent<SpriteRenderer>().sprite = sprite;
		mapSpriteObject.layer = LayerMask.NameToLayer(StaticVariables.mapSpriteLayerName);

		mapSpriteObject.transform.localScale = Vector3.one * _minimapManager.mapIconSize;

		return mapSpriteObject;
	}

	public void SetSelected(bool isSelected)
	{
		_selectedSpriteObject.SetActive(isSelected);
	}

	public void SetIconSize(int newSize)
	{
		_iconObject.transform.localScale = Vector3.one * newSize;
		_selectedSpriteObject.transform.localScale = Vector3.one * newSize;
	}

	private void OnDestroy()
	{
		_minimapManager.displayOnMapObjectList.Remove(this);
	}
}
