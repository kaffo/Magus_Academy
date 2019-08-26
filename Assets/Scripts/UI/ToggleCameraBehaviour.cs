using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCameraBehaviour : MonoBehaviour
{
    public FlyCamera cameraScript;

    private void Start()
    {
        if (cameraScript == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (cameraScript)
            cameraScript.enabled = false;
    }

    private void OnDisable()
    {
        if (cameraScript)
            cameraScript.enabled = true;
    }
}
