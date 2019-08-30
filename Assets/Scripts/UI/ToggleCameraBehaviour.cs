using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCameraBehaviour : MonoBehaviour
{
    public FlyCamera cameraScript;
    public bool onlyToggleScroll = false;

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
        {
            if (onlyToggleScroll)
            {
                cameraScript.disableScroll = true;
            } else
            {
                cameraScript.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        if (cameraScript)
        {
            if (onlyToggleScroll)
            {
                cameraScript.disableScroll = false;
            }
            else
            {
                cameraScript.enabled = true;
            }
        }
    }
}
