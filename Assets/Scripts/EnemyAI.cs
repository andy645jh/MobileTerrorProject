﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

#region public properties
	public Transform player;	
	public Transform patrolEnemy;
	public Transform rayTarget;
	public float distanceToDetectplayer = 100;
	public float followTime= 5;
#endregion

#region private properties
	private Transform[] _positions;
	private NavMeshAgent _agent;
	private int _counter = 0;
	private bool _isRotating = false;
	private float _rotationSpeed = 300f;	
	private RaycastHit _hitFront;
	private RaycastHit _hitBack;
	private bool _isFollowPlayer = true;
	private float _speed = 5;
	private Vector3 _pos;
	private bool _isWaiting = false;
	private Rigidbody _playerRigi;

#endregion

	void Start () {
		_positions = patrolEnemy.GetComponentsInChildren<Transform>();
		_agent = GetComponent<NavMeshAgent>();			
		_agent.SetDestination(_positions[_counter].position);		
		_playerRigi = player.GetComponent<Rigidbody>();		
	}
	
	IEnumerator RotateAgent(Quaternion currentRotation, Quaternion targetRotation) {
		_isRotating = true;
		while(currentRotation != targetRotation) {
			transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
			yield return 1;
		}
		_isRotating = false;
	}

	private void rotate(){

		if(_isRotating){
			Vector3 targetDir = _pos - transform.position;
			// The step size is equal to speed times frame time.
			float step = _speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);	
			
			Debug.DrawRay(transform.position, newDir, Color.red);

			// Move our position a step closer to the target.
			transform.rotation = Quaternion.LookRotation(newDir);		
			Debug.Log("Target: "+targetDir);	
			Debug.Log("Dir: "+newDir);
			if(targetDir==newDir || _isFollowPlayer){
				_isRotating = false;
			}
		}		
	}

	private void moverPlayer(){
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");	
	
		_playerRigi.velocity = new Vector3(10 * h, 0, 10 * v);
		Debug.Log("Vel: "+_playerRigi.velocity);
	}

	// Update is called once per frame
	void Update () {

		moverPlayer();		
		rotate();

		float dist=_agent.remainingDistance;
		
		bool detectPlayerFront = Physics.Raycast(rayTarget.position, transform.forward, out _hitFront, distanceToDetectplayer);
		bool detectPlayerBack = Physics.Raycast(rayTarget.position, -transform.forward, out _hitBack, distanceToDetectplayer);

		if(_isWaiting){
			_agent.SetDestination(player.position);		
		}
		
		if ((detectPlayerBack && _hitBack.transform.CompareTag("Player")) || (detectPlayerFront && _hitFront.transform.CompareTag("Player")))
		{
			if(!_isFollowPlayer){
				//_agent.speed += 10; 
				_agent.SetDestination(player.position);
				_isFollowPlayer = true;
				if(!_isWaiting){
					_isWaiting = true;
					Invoke("checkPlayer",followTime);
				}
				
			}
			              
			Debug.Log("Se encontro al Player");
			Debug.DrawRay(rayTarget.position, transform.forward * Vector3.Distance(_hitFront.point,transform.position), Color.green);		
			
		}else{				
				
			Debug.Log("esta colisionando con Muros");
			//_agent.speed = 3.5f; 
			if(_isFollowPlayer){				
				//_agent.SetDestination(_positions[_counter].position);
				_isFollowPlayer = false;
			}
			
			
			Debug.DrawRay(rayTarget.position, transform.forward * Vector3.Distance(_hitFront.point,transform.position), Color.yellow);
			Debug.DrawRay(rayTarget.position, -transform.forward * Vector3.Distance(_hitBack.point,transform.position), Color.yellow);			
        }

		if (dist!=Mathf.Infinity && _agent.pathStatus==NavMeshPathStatus.PathComplete && _agent.remainingDistance==0) {			
			_counter++;
			if(_counter>=_positions.Length){
				_counter=0;
			}
			_agent.SetDestination(_positions[_counter].position);
		}
	}

	public void checkPlayer(){
		Debug.Log("checkPlayer");
		if(!_isFollowPlayer){			
			_agent.SetDestination(_positions[_counter].position);			
		}
		_isWaiting = false;
	}
}
