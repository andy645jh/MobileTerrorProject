﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastDrag : MonoBehaviour 
{
	Ray ray;
	RaycastHit hit;
    UIManager uimanager;
    GameManager gamemanager;    
    private float _distance = 5;
    public bool rayCastState = false;

    private void Start()
    {
        uimanager = GameObject.Find("UI Manager").transform.GetComponent<UIManager>();
        gamemanager = GameObject.Find("GameManager").transform.GetComponent<GameManager>();
    }

    void Update()
    {
		Vector3 fwd = gameObject.transform.TransformDirection(Vector3.forward);

		/*if(rayCastState)
        {
            Debug.DrawRay(gameObject.transform.position, fwd * 5, Color.red);
        }
        else
        {
            Debug.DrawRay(gameObject.transform.position, fwd * 5, Color.green);
        }*/

		if (Physics.Raycast(gameObject.transform.position, fwd, out hit, _distance))
		{
            Debug.DrawRay(gameObject.transform.position, fwd * _distance, Color.green);
            /*if(rayCastState)
            {*/
                if (hit.transform.CompareTag("draggable"))
                {
                    Debug.Log("Draggable");
                    DraggableObj("draggable", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("doorLocked"))
                {
                    Debug.Log("doorLocked");
                    DraggableObj("doorLocked", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("DoorKey"))
                {
                    Debug.Log("DoorKey");
                    DraggableObj("DoorKey", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("doorAction"))
                {
                    Debug.Log("doorActionr");
                    DraggableObj("doorAction", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("LockedDoorClosed"))
                {
                    Debug.Log("LockedDoorClosed");
                    DraggableObj("LockedDoorClosed", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("LockedDoorOpened"))
                {
                    Debug.Log("LockedDoorOpened");
                    DraggableObj("LockedDoorOpened", hit.transform.gameObject);
                }
                else if (hit.transform.CompareTag("Interruptor"))
                {
                    Debug.Log("Interruptor");
                    DraggableObj("Interruptor", hit.transform.gameObject);
                }else{

                    disableWhenNotHit(fwd);                    
                }

                //rayCastState = false;
            //}
			
		}
        else
        {
            disableWhenNotHit(fwd);
        }
    }

    private void disableWhenNotHit(Vector3 fwd){
        Debug.Log("Did not Hit");
        uimanager.doorButton.SetActive(false);
        ///rayCastState = true;                
        Debug.DrawRay(gameObject.transform.position, fwd * _distance, Color.red);
    }

    void DraggableObj(string drag, GameObject obj)
    {
        switch (drag)
        {
            case "draggable":
                uimanager.DragAction(); 
                gamemanager.dragObj = obj;
                break;

            case "doorAction":
                uimanager.DoorAction();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.DoorAtionOpenClose);
                gamemanager.anim = obj.transform.GetComponent<Animator>();
                gamemanager.dragObj = obj;
                break;

            case "doorLocked":
                uimanager.DoorAction();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.CheckLock);
                gamemanager.anim = obj.transform.GetComponent<Animator>();
                gamemanager.dragObj = obj;
                break;

            case "DoorKey":
                uimanager.DoorKey();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.DoorDoorKeyPickUp);
                gamemanager.anim = obj.transform.GetComponent<Animator>();
                gamemanager.dragObj = obj;
                break;

            case "LockedDoorClosed":
                uimanager.LockerOpen();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.LockerDoorOpen);
                gamemanager.anim = obj.transform.GetComponent<Animator>();
                gamemanager.activeObj = obj;
                break;

            case "LockedDoorOpened":
                uimanager.LockerClose();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.LockerDoorClose);
                gamemanager.anim = obj.transform.GetComponent<Animator>();
                gamemanager.activeObj = obj;
                break;

            case "Interruptor":

                uimanager.toggleLucesManager();
                uimanager.doorButton.transform.GetComponent<Button>().onClick.AddListener(gamemanager.toggleLucesManager);
                gamemanager.SelectedSwitch = obj;

                break;

        }
    }
}
