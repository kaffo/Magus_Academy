using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActiveToggle : MonoBehaviour
{
    public GameObject gameObjectToToggle;

    private Button myButton;
  
    private void Start()
    {
        myButton = this.GetComponent<Button>();
        if (myButton == null || gameObjectToToggle == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
        myButton.onClick.AddListener(MyButtonOnClick);
    }

    private void MyButtonOnClick()
    {
        gameObjectToToggle.SetActive(!gameObjectToToggle.activeInHierarchy);
    }
}
