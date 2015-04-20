using UnityEngine;
using System.Collections;
using System.Linq;

public class DisplayOnMap : MonoBehaviour 
{
	[SerializeField] public MinimapManager.MinimapObjectType type;

	private MinimapManager _minimapManager;

	private GameObject _iconObject;
	private GameObject _selectedSpriteObject;

	private Transform _transform;

	private void Awake()
	{
		_transform = transform;
	}

	private void Start () 
	{
		 _minimapManager = MinimapManager.GetInstance();

		if(_minimapManager != null)
		{
			_iconObject = CreateMapSprite("MapIcon", _minimapManager.mapObjectList.Find(f => f.type == type).sprite);
//			_iconObject.AddComponent<SphereCollider>().isTrigger = true;

			_minimapManager.displayOnMapObjectList.Add(this);
			_minimapManager.mapObjectTransformList.Add(transform);
		}

	}

	private GameObject CreateMapSprite(string name, Sprite sprite)
	{
		GameObject mapSpriteObject = new GameObject(name);
		Transform tr = mapSpriteObject.transform;

		tr.position = _transform.position;
		tr.rotation = _transform.rotation;
		tr.Rotate(90, 0, 0);
		tr.SetParent(_transform);

		mapSpriteObject.AddComponent<SpriteRenderer>().sprite = sprite;
		mapSpriteObject.layer = LayerMask.NameToLayer(StaticVariables.mapSpriteLayerName);

		tr.localScale = Vector3.one * _minimapManager.mapIconSize;

		return mapSpriteObject;
	}

	public void SetSelected(bool isSelected)
	{
		if(isSelected)
		{
			if(_selectedSpriteObject == null)
			{
				_selectedSpriteObject = CreateMapSprite("MapSelectedIcon", _minimapManager.selectedSprite);
				_selectedSpriteObject.transform.SetParent(_iconObject.transform);
			}

		}
		else
		{
			if(_selectedSpriteObject != null)
			{
				Destroy(_selectedSpriteObject);
			}

		}
	}

	public void SetIconSize(int newSize)
	{
		_iconObject.transform.localScale = Vector3.one * newSize;
		_selectedSpriteObject.transform.localScale = Vector3.one * newSize;
	}

	private void OnDestroy()
	{
		if(_minimapManager != null)
		{
			_minimapManager.displayOnMapObjectList.Remove(this);
			_minimapManager.mapObjectTransformList.Remove(transform);
		}

	}
}
