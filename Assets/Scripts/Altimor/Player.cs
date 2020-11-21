﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float GRAB_DISTANCE = 1.5f;
    private const float HANDLE_POSITION_X = 0.4f;
    private const float HANDLE_POSITION_Y = -0.2f;
    private const float HANDLE_POSITION_Z = 0.8f;

    public GameObject gm;
    public Canvas UI;
    private GameManager gmScript;
    private InteractionScript UIscript;

    private Vector3 handlePos;
    public Transform eyes;
    private GameObject handleObject;
    private RecipientBehaviour rb;
    private Boolean _calendrierOuvert = false;

    // Start is called before the first frame update
    void Start()
    {
        handleObject = null;
        handlePos = new Vector3(HANDLE_POSITION_X, HANDLE_POSITION_Y, HANDLE_POSITION_Z);
        UIscript = UI.GetComponent<InteractionScript>();
        gmScript = gm.GetComponent<GameManager>();
    }

    public void GrabObject()
    {
        RaycastHit spotedObject;
        if (handleObject == null && !_calendrierOuvert)
        {
            if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out spotedObject,
                GRAB_DISTANCE))
            {
                if (spotedObject.transform.CompareTag("Recipient"))
                {
                    UIscript.draw_take(spotedObject.transform.gameObject.GetComponent<RecipientBehaviour>()
                        .ContentName);
                    if (Input.GetButtonDown("Interact"))
                    {
                        handleObject = spotedObject.transform.gameObject;
                        handleObject.transform.parent = eyes;
                        handleObject.transform.localPosition = handlePos;
                        handleObject.transform.rotation = Quaternion.identity;
                        rb = handleObject.GetComponent<RecipientBehaviour>();
                    }
                }
                else if (spotedObject.transform.CompareTag("Calendrier"))
                {
                    if (!_calendrierOuvert)
                    {
                        UIscript.draw_open_calendrier();
                        if (Input.GetButtonDown("Interact"))
                        {
                            transform.Find("UI").GetComponent<CalendrierManager>()
                                .AfficheCalendrier();
                            _calendrierOuvert = true;
                            UIscript.clear_open_calendrier();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown("Interact"))
                        {
                            transform.Find("UI").GetComponent<CalendrierManager>().CacheCalendrier();
                            _calendrierOuvert = false;
                        }
                    }
                }
                else
                {
                    UIscript.clear_open_calendrier();
                }
            }
            else if (_calendrierOuvert && Input.GetButtonDown("Interact"))
            {
                transform.Find("UI").GetComponent<CalendrierManager>().CacheCalendrier();
                _calendrierOuvert = false;
            }
            else
            {
                UIscript.clear_UI();
            }
        }
        else if (_calendrierOuvert && Input.GetButtonDown("Interact"))
        {
            transform.Find("UI").GetComponent<CalendrierManager>().CacheCalendrier();
            _calendrierOuvert = false;
        }
        else
        {
            UIscript.clear_UI();
        }
    }

    public void DropObject()
    {
        if (handleObject)
        {
            RaycastHit spotedObject;
            if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out spotedObject,
                GRAB_DISTANCE))
            {
                if (spotedObject.transform.CompareTag("Furniture"))
                {
                    UIscript.draw_put();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        handleObject.transform.parent = null;
                        handleObject.transform.position = spotedObject.point;
                        handleObject.transform.rotation = Quaternion.identity;
                        handleObject = null;
                    }
                }
            }
        }
    }

    public void PourObject()
    {
        if (handleObject)
        {
            RaycastHit spotedObject;
            if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out spotedObject,
                GRAB_DISTANCE))
            {
                if (spotedObject.transform.CompareTag("Bowl"))
                {
                    UIscript.draw_add();
                    if (Input.GetButtonDown("Fire2"))
                    {
                        rb.Pouring(spotedObject.transform.gameObject);
                    }
                }
            }
        }
    }

    public void Sleep()
    {
        RaycastHit spotedObject;
        if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out spotedObject,
            GRAB_DISTANCE))
        {
            if (spotedObject.transform.CompareTag("Bed"))
            {
                UIscript.draw_sleep();
                if (Input.GetButtonDown("Interact"))
                {
                    gmScript.nextDay();
                }
            }
        }
    }

    public void Pet()
    {
        RaycastHit spotedObject;
        if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out spotedObject,
            GRAB_DISTANCE))
        {
            if (spotedObject.transform.CompareTag("Pet"))
            {
                UIscript.draw_pet();
                if (Input.GetButtonDown("Interact"))
                {
                    spotedObject.transform.gameObject.GetComponent<Cat>().Pet();
                }
            }
        }
    }
    
    private void Update()
    {
        GrabObject();
        DropObject();
        PourObject();
        Sleep();
        Pet();
    }
}