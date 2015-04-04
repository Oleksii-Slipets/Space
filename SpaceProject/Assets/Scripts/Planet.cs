using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour 
{
	[SerializeField] private float _gravity = 4;

	private void OnTriggerStay(Collider otherCollider)
	{
		Vector3 direction = (transform.position - otherCollider.transform.position).normalized;
		otherCollider.rigidbody.MovePosition(otherCollider.transform.position + direction * _gravity * Time.deltaTime);
	}

}
