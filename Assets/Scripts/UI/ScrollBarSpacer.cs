using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarSpacer : MonoBehaviour
{
    public GenericTriggers scrollbarTriggers;
    public RectTransform spacerToResize;
    public GameObject spacerToToggle;
    public bool addPaddingToParent = false;

    public float widthWithoutScrollbar = 100f;
    public float widthWtihScrollbar = 120f;
    public float paddingToAddToParent = 17f;

    private void Start()
    {
        if (scrollbarTriggers == null)
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
        if (spacerToResize)
            spacerToResize.sizeDelta = new Vector2(widthWtihScrollbar, spacerToResize.sizeDelta.y);

        if (spacerToToggle)
            spacerToToggle.SetActive(true);

        if (addPaddingToParent)
            ((RectTransform)transform).offsetMax = new Vector2(-paddingToAddToParent, ((RectTransform)transform).offsetMax.y);

        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }

    private void OnScrollbarDisable()
    {
        if (spacerToResize)
            spacerToResize.sizeDelta = new Vector2(widthWithoutScrollbar, spacerToResize.sizeDelta.y);

        if (spacerToToggle)
            spacerToToggle.SetActive(false);

        if (addPaddingToParent)
            ((RectTransform)transform).offsetMax = new Vector2(0f, ((RectTransform)transform).offsetMax.y);

        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }
}
