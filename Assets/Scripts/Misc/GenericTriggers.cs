using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTriggers : MonoBehaviour
{
    public delegate void OnGameObjectEnableDelegate();
    public event OnGameObjectEnableDelegate OnGameObjectEnable;

    public delegate void OnGameObjectDisableDelegate();
    public event OnGameObjectDisableDelegate OnGameObjectDisable;

    private void OnEnable()
    {
        if (OnGameObjectEnable != null)
        {
            OnGameObjectEnable();
        }
    }

    private void OnDisable()
    {
        if (OnGameObjectDisable != null)
        {
            OnGameObjectDisable();
        }
    }
}
