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

		_iconObject = CreateMapSprite("MapIcon", _minimapManager.minimapObjectList.Find(f => f.type == type).sprite);
		_iconObject.AddComponent<SphereCollider>();

		_selectedSpriteObject = CreateMapSprite("MapSelectedIcon", _minimapManager.selectedSprite);
		_selectedSpriteObject.transform.SetParent(_iconObject.transform);
		_selectedSpriteObject.SetActive(false);

	}

	private GameObject CreateMapSprite(string name, Sprite sprite)
	{
		GameObject mapSpriteObject = new GameObject(name);
		mapSpriteObject.transform.position = transform.position;
		mapSpriteObject.transform.SetParent(gameObject.transform);
		mapSpriteObject.transform.Rotate(90, 0, 0);
		mapSpriteObject.AddComponent<SpriteRenderer>().sprite = sprite;
		mapSpriteObject.layer = LayerMask.NameToLayer(_minimapManager.mapSpriteLayerName);

		mapSpriteObject.transform.localScale = Vector3.one * _minimapManager.sizeMultiplier;

		return mapSpriteObject;
	}

	public void SetSelected(bool isSelected)
	{
		_selectedSpriteObject.SetActive(isSelected);
	}
}
