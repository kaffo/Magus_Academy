using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerLayoutRebuildOnResize : MonoBehaviour
{
    public RectTransform layoutToUpdate;

    private void OnRectTransformDimensionsChange()
    {
        if (layoutToUpdate)
            LayoutRebuilder.MarkLayoutForRebuild(layoutToUpdate);
    }

}
