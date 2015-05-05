using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour 
{
	[SerializeField] public GameObject selectionIndicator;
	[SerializeField] public List<ParticleSystem> particleSystemList;

	public enum State {MoveToTarget, Stay};
	private State _currentState = State.Stay;

	private Transform _targetTransform;

	[SerializeField] private float _commonSpeed = 20f;
	private float _maxMovingSpeed = 0.5f;
	private float _movingSpeed = 0f;
	private float _maxRotationSpeed = 0.3f;
	private float _rotationSpeed = 0f;
	
	private bool _isSelected = false;

	// --------------------------------------------------------------------
	PotentialField m_field;

	void init()
	{
		m_field = new PotentialField (this);
	}
	[Range(0.01f, 100.0f)]
	public float fieldRadius = 20.0f;
	public float CurrentSpeed
	{
		get { return _movingSpeed; }
	}
	
	public float TargetSpeed
	{
		get { return _maxMovingSpeed; }

	}
	
	public float acceleration = 0.2f;
	protected Vector3 direction = Vector3.forward;
	public void LookAt(Vector3 vector_forward)
	{
		direction = vector_forward.normalized;
	}
	// --------------------------------------------------------------------
	
	private void Start()
	{
	init();
		SetSelected(false);
	}

	private void FixedUpdate () 
	{
		Moving();
		AnimateParticles();
	}

	private void AnimateParticles()
	{
		foreach(ParticleSystem particleSystem in particleSystemList)
		{
			if(_movingSpeed > 0.1f)
			{
				particleSystem.enableEmission = true;
				particleSystem.startSpeed = -_movingSpeed * 2;
			}
			else
			{
				particleSystem.enableEmission = false;
			}

		}
	}

	private void Moving ()
	{
		Vector3 direction = Vector3.zero;
		Quaternion targetRotation = Quaternion.identity;

		if(_currentState == State.Stay)
		{

			_movingSpeed = Mathf.Lerp(_movingSpeed, 0, acceleration);
			_rotationSpeed = Mathf.Lerp(_rotationSpeed, 0, acceleration);

			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		else if(_targetTransform != null)
		{
//			m_field.target_position = _targetTransform.position;
//			m_field.DoUpdate();
			_rotationSpeed = Mathf.Lerp(_rotationSpeed, _maxRotationSpeed, acceleration);

			direction = (_targetTransform.position - transform.position);
			targetRotation = Quaternion.LookRotation(direction);
			_movingSpeed = Mathf.Lerp(_movingSpeed, _maxMovingSpeed, acceleration);

			if (IsCanReachToTarget(_targetTransform.position, 1))
			{
				SetState(State.Stay);
			}
		}
		
		

		GetComponent<Rigidbody>().MoveRotation(Quaternion.RotateTowards(GetComponent<Rigidbody>().rotation, targetRotation, _rotationSpeed * _commonSpeed));

		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * _movingSpeed * _commonSpeed * Time.deltaTime);
		
	}

	private bool IsCanReachToTarget(Vector3 targetPosition, float maxDistanceToReachTarget)
	{
		targetPosition = new Vector3(targetPosition.x, 1, targetPosition.z);
		Vector3 currentPosition = new Vector3(transform.position.x, 1, transform.position.z);
		return (Vector3.Distance(currentPosition, targetPosition) < maxDistanceToReachTarget);
	}


	private void SetState(State newState)
	{
		_currentState = newState;

		switch (_currentState)
		{
			case State.Stay:
			if(_targetTransform)
			{
				_targetTransform.GetComponentInChildren<SpriteRenderer>().enabled = false;
			}
				break;
			case State.MoveToTarget:

				break;

		}
	}

	public void SetTargetTransform(Transform newTargetTransform)
	{
		if(_targetTransform != null)
		{
			Destroy(_targetTransform.gameObject);
		}
		_targetTransform = newTargetTransform;
		SetState(State.MoveToTarget);
		
	}
	
	public void SetSelected(bool isSelected)
	{
		_isSelected = isSelected;
		selectionIndicator.SetActive(isSelected);
		if(_currentState != State.Stay)
		{
			_targetTransform.GetComponentInChildren<SpriteRenderer>().enabled = isSelected;
		}
	}

}
