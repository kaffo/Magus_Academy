﻿using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{

    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    public Camera cameraObject;
    public float mainSpeed = 100.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    public float maxZoom = 0.5f;
    public float minZoom = 20f;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    private Transform cameraTransform;

    private void Start()
    {
        if (cameraObject == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
        cameraTransform = cameraObject.transform;
    }

    void Update()
    {
        /*lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(cameraTransform.eulerAngles.x + lastMouse.x, cameraTransform.eulerAngles.y + lastMouse.y, 0);
        cameraTransform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;*/
        //Mouse  camera angle done.  

        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");           //This little peece of code is written by JelleWho https://github.com/jellewie
        if (ScrollWheelChange != 0)
        {                                            //If the scrollwheel has changed
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            cameraObject.orthographicSize -= R;
            cameraObject.orthographicSize = Mathf.Clamp(cameraObject.orthographicSize, maxZoom, minZoom);
        }

        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = cameraTransform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            cameraTransform.Translate(p);
            newPosition.x = cameraTransform.position.x;
            newPosition.z = cameraTransform.position.z;
            cameraTransform.position = newPosition;
        }
        else
        {
            cameraTransform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}