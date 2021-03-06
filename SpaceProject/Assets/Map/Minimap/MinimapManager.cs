﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MapNamespace
{
	public class MinimapManager : MonoBehaviour 
	{
		public enum MinimapObjectType {Ship, Planet};

		public enum MapState { MiniMap, FullScreenMap, NoMap };
		private MapState _mapState = MapState.MiniMap;

		[System.Serializable] public class MapObjectProperties
		{
			public MinimapObjectType type;
			public Sprite sprite;
		}

		[SerializeField] public List<MapObjectProperties> mapObjectList;
		[SerializeField] public Sprite selectedSprite;

		[SerializeField] public Camera mainCamera;

		[SerializeField] public GameObject mapFullScreen;
		[SerializeField] public GameObject mapMini;

		[SerializeField] public RawImage mapFullScreenImage;
		[SerializeField] public RawImage mapMiniImage;
		
		[SerializeField] public RectTransform mapMiniRectTransform;
		
		[SerializeField] public int mapIconSize = 10;
		[SerializeField] public float minimapSize = 0.2f;

		[HideInInspector] public List<DisplayOnMap> displayOnMapObjectList;
		[HideInInspector] public List<Transform> mapObjectTransformList;
		[HideInInspector] public List<Transform> mapSpriteTransformList;

		
		private DisplayOnMap _selectedObject = null;

		private RenderTexture _renderTexture;

		private Camera _mapCamera;

		private static MinimapManager _instance;

		private void Awake()
		{
			_instance = this;

			if(mainCamera == null)
			{
				mainCamera = Camera.main;
			}
		}

		private void Start()
		{
			mainCamera.cullingMask &=  ~(1 << LayerMask.NameToLayer(StaticVariables.mapSpriteLayerName));
			CreateMapCamera();
			_renderTexture = new RenderTexture((int)_mapCamera.pixelWidth, (int)_mapCamera.pixelHeight, 0);
			_mapCamera.targetTexture = _renderTexture;

			mapMiniRectTransform.sizeDelta = new Vector2( _renderTexture.width * minimapSize, _renderTexture.height * minimapSize); 

			mapMiniImage.texture = _renderTexture;
			mapFullScreenImage.texture = _renderTexture;

			SetMapState(MapState.MiniMap);


		}

		private void Update()
		{
			if(Input.GetKeyUp(KeyCode.M))
			{
				ChangeMapState();
			}

			UpdateSpritesPosition();

		}

		private void UpdateSpritesPosition()
		{
			for (int i = 0; i < mapObjectTransformList.Count; i++)
			{
				mapSpriteTransformList[i].position = mapObjectTransformList[i].position;
	//			Vector3 eulerAngles = mapObjectTransformList[i].eulerAngles;
				mapSpriteTransformList[i].eulerAngles = new Vector3(90, mapObjectTransformList[i].eulerAngles.y, 0);
			}
		}

		public static MinimapManager GetInstance()
		{
			return _instance;
		}


		private void CreateMapCamera()
		{
			GameObject minimapCameraObject = new GameObject("MapCamera");
			minimapCameraObject.transform.position = Vector3.up * 100;
			minimapCameraObject.transform.Rotate(90, 0, 0);
			_mapCamera = minimapCameraObject.AddComponent<Camera>();
			_mapCamera.orthographic = true;
			_mapCamera.orthographicSize = 100;
			_mapCamera.cullingMask = (1 << LayerMask.NameToLayer(StaticVariables.mapSpriteLayerName));
		}

		public void OnClickMinimap(Vector2 clickPosition)
		{

			Vector3 worldClickPoint = _mapCamera.ScreenToWorldPoint(Input.mousePosition);
			foreach(Transform tr in mapObjectTransformList)
			{ 

				Vector3 currentPosition = new Vector3(tr.position.x, 0, tr.position.z);
				worldClickPoint = new Vector3(worldClickPoint.x, 0, worldClickPoint.z);
				if((currentPosition - worldClickPoint).sqrMagnitude < mapIconSize)
				{
					DisplayOnMap objectDisplayOnMap =  tr.GetComponent<DisplayOnMap>();
					if(objectDisplayOnMap != null)
					{
						if(_selectedObject != null)
						{
							_selectedObject.SetSelected(false);
						}
						objectDisplayOnMap.SetSelected(true);
						_selectedObject = objectDisplayOnMap;
					}
				}
			}

		}

		public void ChangeMapState()
		{
			int nextStateNumber = (int)_mapState + 1;
			if(nextStateNumber >= Enum.GetNames(typeof(MapState)).Length)
			{
				nextStateNumber = 0;
			}
			
			SetMapState((MapState)nextStateNumber);
		}

		public void CloseMap()
		{
			mapFullScreen.SetActive(false);
			mapMini.SetActive(true);
		}

		private void SetMapState(MapState newMapState)
		{
			_mapState = newMapState;

			switch (_mapState)
			{
				case MapState.FullScreenMap:
					mapFullScreen.SetActive(true);
					mapMini.SetActive(false);
					break;
				case MapState.MiniMap:
					mapFullScreen.SetActive(false);
					mapMini.SetActive(true);
					break;
				case MapState.NoMap:
					mapFullScreen.SetActive(false);
					mapMini.SetActive(false);
					break;
			}
		}

	}
}
