using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour 
{
	private Ship _selectedShip = null;

	private void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			RaycastHit hit;            
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			LayerMask layerMask = ~(1 << LayerMask.NameToLayer (StaticVariables.mapSpriteLayerName));
			if (Physics.Raycast(ray, out hit, 100f, layerMask)) 
			{
				Ship newShip = hit.collider.GetComponent<Ship>();
				if(newShip != null)
				{
					SelectShip(newShip);
				}

			}
		}

		if(Input.GetMouseButtonUp(1) && _selectedShip != null)
		{
			RaycastHit hit;            
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			LayerMask layerMask = ~(1 << LayerMask.NameToLayer (StaticVariables.mapSpriteLayerName));
			if (Physics.Raycast(ray, out hit, 100f, layerMask)) 
			{
				if(hit.collider.name == "Floor")
				{

					_selectedShip.SetTargetTransform(SpawnTarget(hit.point));
				}
				
			}
		}
	}

	private Transform SpawnTarget(Vector3 position)
	{
		GameObject targetGO = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Target"), position, Quaternion.identity);
		targetGO.name = "Target";
		return targetGO.transform;
	}

	private void SelectShip(Ship newShip)
	{
		if(_selectedShip != null)
		{
			_selectedShip.SetSelected(false);
		}

		_selectedShip = newShip;
		_selectedShip.SetSelected(true);
	}

}
