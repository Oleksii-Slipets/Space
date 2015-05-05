using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotentialField
{
	public Vector3 target_position;

	// -------------------------------------
	public PotentialField(Ship control)
	{
		shipControl = control;
	}

	public bool isArrived
	{
		get {return arrived;}
	}

	public void AllowCollisionWith(GameObject target)
	{
		allowedToCollideWith = target;
	}

	// -------------------------------------
	// PRIVATE
	// -------------------------------------

	struct ObjInField
	{
		public GameObject game_object;
		public ObjInField(RaycastHit hitInfo)
		{
			this.game_object = hitInfo.collider.gameObject;
		}
	}

	Ship shipControl;
	// -------------------------------------
	Vector3 repulso = Vector3.zero;
	Vector3 toTarget = Vector3.zero;
	List<ObjInField> objectsInField = new List<ObjInField>();

	GameObject allowedToCollideWith = null;
	// -------------------------------------

	bool arrived = false;

	// -------------------------------------
	void RefreshObjectsInField()
	{
		objectsInField.Clear ();

		RaycastHit[] hits = Physics.SphereCastAll (shipControl.transform.position, shipControl.fieldRadius, shipControl.transform.forward, 0.1f);
//		Debug.Log(hits.Length);
		foreach(RaycastHit hit in hits)
		{
			if (hit.collider.gameObject == allowedToCollideWith) continue;
//			if (hit.collider.gameObject.GetComponentInChildren<Projectile>() != null) continue;

			objectsInField.Add ( new ObjInField(hit) );
		}

	}

	// -------------------------------------
	void UpdateFollow()
	{
		RefreshObjectsInField ();
		
		repulso = Vector3.zero;
		foreach(var obj in objectsInField)
		{
			Vector3 vectorToObj = shipControl.transform.position - obj.game_object.transform.position;
			float distance = vectorToObj.magnitude;
			repulso += vectorToObj.normalized * shipControl.CurrentSpeed * (3.0f + 3.0f * (distance / shipControl.fieldRadius));
		}
		if(repulso != Vector3.zero)
		shipControl.LookAt((toTarget + repulso).normalized);

	}

	// -------------------------------------
	void UpdateDoneConditions()
	{
//		arrived = true;
//
		toTarget = target_position - shipControl.transform.position;
//		arrived = toTarget.magnitude < shipControl.acceleration * 3.0f;
	}

	// -------------------------------------
	public void DoUpdate ()
	{
		UpdateDoneConditions ();

//		if (arrived)
//			shipControl.TargetSpeed = 0.0f;
//		else
			UpdateFollow();
	}
}
