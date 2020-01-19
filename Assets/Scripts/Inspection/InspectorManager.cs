using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System.Reflection;

public class InspectorManager : Singleton<InspectorManager>
{
    [Header("References")]
    public GameObject familiarInspectorPrefab;

    [Header("Internal References")]
    public GameObject inspectorPool;

    private GameTime gameTime;

    private void Start()
    {
        if (familiarInspectorPrefab == null || inspectorPool == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTime = TimeManager.Instance.currentTime;
        gameTime.OnMonthIncrement += OnMonthChange;
    }

    public void SpawnInspector()
    {
        GameObject spawnedInspector = Instantiate(familiarInspectorPrefab, inspectorPool.transform);

        Debug.Log($"Spawned Familiar Inspector");
    }

    private void OnMonthChange()
    {
        SpawnInspector();
    }
}
