using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MinimapManager : MonoBehaviour 
{
	public enum MinimapObjectType {Ship, Planet};

	public enum MinimapState { Expand, Closed };

	[System.Serializable] public class MinimapObjectProperties
	{
		public MinimapObjectType type;
		public Sprite sprite;
	}

	[SerializeField] public Sprite maskSprite;
	[SerializeField] public Sprite selectedSprite;

	[SerializeField] public RectTransform minimapRectTransform;
	[SerializeField] public List<MinimapObjectProperties> minimapObjectList;
	[SerializeField] public string mapSpriteLayerName = "MapSprite";
	[SerializeField] public int sizeMultiplier = 10;

	[SerializeField] public GameObject mapFullScreen;
	[SerializeField] public RawImage mapFullScreenImage;

	[SerializeField] public GameObject mapMini;

	private RenderTexture _renderTexture;
	private Camera _minimapCamera;

	private DisplayOnMap _selectedObject = null;

	private static MinimapManager _instance;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		CreateMinimapCamera();
		_renderTexture = new RenderTexture((int)_minimapCamera.pixelWidth, (int)_minimapCamera.pixelHeight, 0);
		_minimapCamera.targetTexture = _renderTexture;

		minimapRectTransform.sizeDelta = new Vector2( _renderTexture.width/3, _renderTexture.height/3); 
		minimapRectTransform.GetComponent<RawImage>().texture = _renderTexture;
		mapFullScreenImage.texture = _renderTexture;


	}

	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.E))
		{
			ExpandMap();
		}
		if(Input.GetKeyUp(KeyCode.C))
		{
			CloseMap();
		}
	}

	public static MinimapManager GetInstance()
	{
		return _instance;
	}


	private void CreateMinimapCamera()
	{
		GameObject minimapCameraObject = new GameObject("MinimapCamera");
		minimapCameraObject.transform.position = Vector3.up * 100;
		minimapCameraObject.transform.Rotate(90, 0, 0);
		_minimapCamera = minimapCameraObject.AddComponent<Camera>();
		_minimapCamera.orthographic = true;
		_minimapCamera.orthographicSize = 100;
		_minimapCamera.cullingMask = (1 << LayerMask.NameToLayer(mapSpriteLayerName));
	}

	public void OnClickMinimap(Vector2 clickPosition)
	{

		print("OnClickMinimap = " + clickPosition);
		LayerMask layerMask = 1 << LayerMask.NameToLayer (mapSpriteLayerName);
		RaycastHit hit;            
//		Ray ray = _minimapCamera.ScreenPointToRay(new Vector3(clickPosition.x, clickPosition.y, 0));
		Ray ray = _minimapCamera.ScreenPointToRay(Input.mousePosition);
		print(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, 1000f, layerMask)) 
		{
			DisplayOnMap objectDisplayOnMap = hit.collider.gameObject.GetComponentInParent<DisplayOnMap>();
			if(objectDisplayOnMap != null)
			{
				if(_selectedObject != null)
				{
					_selectedObject.SetSelected(false);
				}
				_selectedObject = objectDisplayOnMap;
				_selectedObject.SetSelected(true);
				print (objectDisplayOnMap.type.ToString());
			}


		}


	}

	public void ExpandMap()
	{
		mapFullScreen.SetActive(true);
		mapMini.SetActive(false);
	}

	public void CloseMap()
	{
		mapFullScreen.SetActive(false);
		mapMini.SetActive(true);
	}

}
