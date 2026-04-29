using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleUI_MS : MonoBehaviour
{
    private Renderer consoleRenderer;
    public RenderTexture consoleUI;

    void Start()
    {
        consoleRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            consoleRenderer.material.mainTexture = consoleUI;
        }
    }

}
