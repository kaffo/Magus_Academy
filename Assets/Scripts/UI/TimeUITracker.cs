using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUITracker : MonoBehaviour
{
    public GameObject timeNodePrefab;
    public RectTransform pointerTransform;

    private GameTime gameTime;
    private float pointerIncrementDistance = 1f;

    private void Start()
    {
        if (timeNodePrefab == null || pointerTransform == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        gameTime = TimeManager.Instance.currentTime;
        gameTime.OnMinuteIncrement += OnMinuteChange;
        gameTime.OnDayIncrement += OnDayChange;

        // Insert a node for each time slot
        int numOfTimeslots = TimeManager.Instance.timeslotsSetup.Count;
        for (int i = 0; i < numOfTimeslots; i++)
        {
            GameObject timeNode = Instantiate(timeNodePrefab, transform);
            TimeNodeFiller timeNodeFillerScript = timeNode.GetComponent<TimeNodeFiller>();
            if (timeNodeFillerScript)
            {
                timeNodeFillerScript.myTimeslot = TimeManager.Instance.timeslotsSetup[i];
            }
        }

        // Sets the width of the parent container so there's no whitespace between nodes
        RectTransform parentTransform = ((RectTransform)transform.parent);
        RectTransform timeNodeTransform = ((RectTransform)timeNodePrefab.transform);
        float timeNodeWidth = timeNodeTransform.sizeDelta.x;
        float parentWidth = timeNodeWidth * numOfTimeslots;
        parentTransform.sizeDelta = new Vector2(parentWidth, parentTransform.sizeDelta.y);

        // Workout the increment we need to move the needle each minute
        pointerIncrementDistance = parentTransform.sizeDelta.x / (GameTime.MINUTES_IN_HOUR * numOfTimeslots);
        float startPos =  (parentWidth / numOfTimeslots * gameTime.hours) - (pointerTransform.sizeDelta.x / 2);
        pointerTransform.anchoredPosition = new Vector2(startPos, pointerTransform.anchoredPosition.y);
    }

    private void OnMinuteChange()
    {
        pointerTransform.anchoredPosition = new Vector2(pointerTransform.anchoredPosition.x + pointerIncrementDistance, pointerTransform.anchoredPosition.y);
    }

    private void OnDayChange()
    {
        pointerTransform.anchoredPosition = new Vector2(-(pointerTransform.sizeDelta.x / 2), pointerTransform.anchoredPosition.y);
    }
}
