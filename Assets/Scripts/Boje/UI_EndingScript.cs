﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_EndingScript : MonoBehaviour
{
    [SerializeField] private Canvas winCanvas;
    [SerializeField] private Canvas looseCanvas;

    public void WIN()
    {
        transform.gameObject.SetActive(true);
        winCanvas.gameObject.SetActive(true);
    }

    public void LOOSE()
    {
        transform.gameObject.SetActive(true);
        looseCanvas.gameObject.SetActive(true);
    }
    
    public void toMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
