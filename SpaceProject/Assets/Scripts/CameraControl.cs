using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{

	[SerializeField] float minOrthographicSize = 1;
	[SerializeField] float maxOrthographicSize = 10;
	[SerializeField] float minHigh = 10;
	[SerializeField] float maxHigh = 30;
	
	[SerializeField] float wheelSpeed = 2;
	
	void Update ()
	{
		UpdatePosition();
		
		UpdateZoom();
	}
	
	private void UpdatePosition()
	{
		Vector3 currentPosition = transform.position;
		Vector3 movingVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		transform.position = currentPosition + movingVector;
	}
	
	private void UpdateZoom()
	{
		float scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
		if(scrollWheelDelta != 0)
		{
			if(GetComponent<Camera>().orthographic)
			{
				
				float tempOrthographicSize = GetComponent<Camera>().orthographicSize;
				tempOrthographicSize -= wheelSpeed * scrollWheelDelta;
				tempOrthographicSize = Mathf.Clamp(tempOrthographicSize, minOrthographicSize, maxOrthographicSize);
				GetComponent<Camera>().orthographicSize = tempOrthographicSize;
			}
			else 
			{
				Vector3 tempPosition = GetComponent<Camera>().transform.position + transform.forward * wheelSpeed * scrollWheelDelta;
				if(((scrollWheelDelta > 0)&&(tempPosition.y < minHigh))|| ((scrollWheelDelta < 0)&&(tempPosition.y > maxHigh)))
				{
					return;
				}
				GetComponent<Camera>().transform.position = tempPosition;

				
			}
		}
	}
}
