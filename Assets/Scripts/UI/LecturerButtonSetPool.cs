using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LecturerButtonSetPool : MonoBehaviour
{
    public LecturerInfoFrameManager lecturerInfoFrameManager;
    public bool usePooledLecturers = true;
    private Button myButton;

    private void Start()
    {
        myButton = this.GetComponent<Button>();
        if (myButton == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
        myButton.onClick.AddListener(MyButtonOnClick);
    }

    private void MyButtonOnClick()
    {
        lecturerInfoFrameManager.usePooledLecturers = usePooledLecturers;
    }
}
