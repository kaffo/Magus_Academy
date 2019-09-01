using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LecturerInfoFrameFiller : MonoBehaviour
{
    [Header("External References")]
    public LecturerStats myLecturerStatsReference;
    public Sprite loyalSprite;
    public Sprite indifferentSprite;
    public Sprite disloyalSprite;

    [Header("Internal References")]
    public Image profileImage;
    public Text lecturerNameText;
    public Text lecturerDesiresText;
    public Text lecturerTraitsText;
    public Image loyaltyImage;
    public List<GameObject> enrolledObjectsToDisable;

    private void Start()
    {
        if (myLecturerStatsReference == null || loyalSprite == null || indifferentSprite == null || disloyalSprite == null ||
            profileImage == null || lecturerNameText == null || lecturerDesiresText == null || lecturerTraitsText == null || loyaltyImage == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        if (myLecturerStatsReference.isHired)
        {
            foreach (GameObject objectToDisable in enrolledObjectsToDisable)
            {
                objectToDisable.SetActive(false);
            }
        }

        profileImage.sprite = myLecturerStatsReference.profilePicture;
        lecturerNameText.text = myLecturerStatsReference.lecturerName;

        string desireText;
        switch (myLecturerStatsReference.lecturerDesire)
        {
            case LECTURER_DESIRES.RESEARCH:
                desireText = "research";
                break;
            case LECTURER_DESIRES.TEACHING:
                desireText = "teach";
                break;
            default:
                desireText = "None";
                break;
        }

        string desiresText = "Desires to " + desireText + " magic";
        List<MAGIC_SCHOOL> proficentList = new List<MAGIC_SCHOOL>();
        List<MAGIC_SCHOOL> excelList = new List<MAGIC_SCHOOL>();

        foreach (MAGIC_SCHOOL lecturerMagic in myLecturerStatsReference.lecturerSkills.Keys)
        {
            if (myLecturerStatsReference.lecturerSkills[lecturerMagic] > 0.3 && myLecturerStatsReference.lecturerSkills[lecturerMagic] < 0.65)
            {
                proficentList.Add(lecturerMagic);
            } else if (myLecturerStatsReference.lecturerSkills[lecturerMagic] >= 0.65)
            {
                proficentList.Add(lecturerMagic);
            }
        }

        if (proficentList.Count > 0)
        {
            desiresText = desiresText + "\nIs proficient in ";
            foreach (MAGIC_SCHOOL lecturerMagic in proficentList)
            {
                desiresText = desiresText + lecturerMagic.ToString() + " & ";
            }
            desiresText = desiresText.Remove(desiresText.Length - 3, 2) + "magic";
        }

        if (excelList.Count > 0)
        {
            desiresText = desiresText + "\nnExcels in ";
            foreach (MAGIC_SCHOOL lecturerMagic in excelList)
            {
                desiresText = desiresText + lecturerMagic.ToString() + " & ";
            }
            desiresText = desiresText.Remove(desiresText.Length - 3, 2) + "magic";
        }

        lecturerDesiresText.text = desiresText;

        string traitsText = "";
        for (int i = 0; i < myLecturerStatsReference.lecturerTraits.Count; i++)
        {
            string traitText;
            switch (myLecturerStatsReference.lecturerTraits[i])
            {
                case LECTURER_TRAITS.HAPPY:
                    traitText = "Happy";
                    break;
                case LECTURER_TRAITS.HARDWORKING:
                    traitText = "Hardworking";
                    break;
                case LECTURER_TRAITS.LAZY:
                    traitText = "Lazy";
                    break;
                case LECTURER_TRAITS.SAD:
                    traitText = "Sad";
                    break;
                default:
                    traitText = "Empty";
                    break;
            }

            if (i + 1 < myLecturerStatsReference.lecturerTraits.Count)
            {
                traitsText += traitText + ", ";
            }
            else
            {
                traitsText += traitText;
            }
        }
        lecturerTraitsText.text = traitsText;

        switch (myLecturerStatsReference.lecturerLoyalty)
        {
            case LECTURER_LOYALTY.LOYAL:
                loyaltyImage.sprite = loyalSprite;
                break;
            case LECTURER_LOYALTY.INDIFFERENT:
                loyaltyImage.sprite = indifferentSprite;
                break;
            case LECTURER_LOYALTY.DISLOYAL:
                loyaltyImage.sprite = disloyalSprite;
                break;
            default:
                loyaltyImage.sprite = indifferentSprite;
                break;
        }
    }

    public void HireLecturer()
    {
        LecturerManager.Instance.HireLecturer(myLecturerStatsReference);
    }
}
