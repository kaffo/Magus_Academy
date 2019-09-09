using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeNodeFiller : MonoBehaviour
{
    [Header("Internal References")]
    public Image myTimeslotImage;
    [Header("External References")]
    public Color sleepingColour;
    public Color eatingColour;
    public Color teachingColour;

    public TIMESLOT myTimeslot
    {
        get
        {
            return myTimeslot;
        }
        set
        {
            SetSlotColour(value);
        }
    }

    private TIMESLOT m_MyTimeSlot = TIMESLOT.NONE;

    private void Start()
    {
        if (myTimeslotImage == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }
    }

    private void SetSlotColour(TIMESLOT newTimeslot)
    {
        m_MyTimeSlot = newTimeslot;

        switch(newTimeslot)
        {
            case TIMESLOT.SLEEPING:
                myTimeslotImage.color = sleepingColour;
                break;
            case TIMESLOT.EATING:
                myTimeslotImage.color = eatingColour;
                break;
            case TIMESLOT.TEACHING:
                myTimeslotImage.color = teachingColour;
                break;
            default:
                Color clearColour = Color.white;
                //clearColour.a = 1;
                myTimeslotImage.color = clearColour;
                break;
        }
    }
}
