using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarSpacer : MonoBehaviour
{
    public GenericTriggers scrollbarTriggers;
    public RectTransform spacerToResize;

    public float widthWithoutScrollbar = 100f;
    public float widthWtihScrollbar = 120f;

    private void Start()
    {
        if (scrollbarTriggers == null || spacerToResize == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        scrollbarTriggers.OnGameObjectEnable += OnScrollbarEnable;
        scrollbarTriggers.OnGameObjectDisable += OnScrollbarDisable;
    }

    private void OnScrollbarEnable()
    {
        spacerToResize.sizeDelta = new Vector2(widthWtihScrollbar, spacerToResize.sizeDelta.y);
        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }

    private void OnScrollbarDisable()
    {
        spacerToResize.sizeDelta = new Vector2(widthWithoutScrollbar, spacerToResize.sizeDelta.y);
        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }
}
