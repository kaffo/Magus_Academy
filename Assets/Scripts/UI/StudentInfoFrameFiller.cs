using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentInfoFrameFiller : MonoBehaviour
{
    [Header("External References")]
    public StudentStats myStudentStatsReference;
    public Sprite loyalSprite;
    public Sprite indifferentSprite;
    public Sprite disloyalSprite;

    [Header("Internal References")]
    public Image profileImage;
    public Text studentNameText;
    public Text studentDesiresText;
    public Text studentTraitsText;
    public Image loyaltyImage;

    private void Start()
    {
        if (myStudentStatsReference == null || loyalSprite == null || indifferentSprite == null || disloyalSprite == null ||
            profileImage == null || studentNameText == null || studentDesiresText == null || studentTraitsText == null || loyaltyImage == null)
        {
            Debug.LogError(this.name + " on " + this.gameObject + " has not been setup correctly!");
            this.enabled = false;
            return;
        }

        profileImage.sprite = myStudentStatsReference.profilePicture;
        studentNameText.text = myStudentStatsReference.studentName;

        string desireText;
        switch (myStudentStatsReference.studentDesire)
        {
            case STUDENT_DESIRES.ELEMENTAL:
                desireText = "Elemental";
                break;
            case STUDENT_DESIRES.NATURE:
                desireText = "Nature";
                break;
            default:
                desireText = "None";
                break;
        }
        studentDesiresText.text = "Aspires to study " + desireText + " magic";

        string traitsText = "";
        for (int i = 0; i < myStudentStatsReference.studentTraits.Count; i++)
        {
            string traitText;
            switch(myStudentStatsReference.studentTraits[i])
            {
                case STUDENT_TRAITS.HAPPY:
                    traitText = "Happy";
                    break;
                case STUDENT_TRAITS.HARDWORKING:
                    traitText = "Hardworking";
                    break;
                case STUDENT_TRAITS.LAZY:
                    traitText = "Lazy";
                    break;
                case STUDENT_TRAITS.SAD:
                    traitText = "Sad";
                    break;
                default:
                    traitText = "Empty";
                    break;
            }

            if (i + 1 < myStudentStatsReference.studentTraits.Count)
            {
                traitsText += traitText + ", ";
            } else
            {
                traitsText += traitText;
            }
        }
        studentTraitsText.text = traitsText;

        switch(myStudentStatsReference.studentLoyalty)
        {
            case STUDENT_LOYALTY.LOYAL:
                loyaltyImage.sprite = loyalSprite;
                break;
            case STUDENT_LOYALTY.INDIFFERENT:
                loyaltyImage.sprite = indifferentSprite;
                break;
            case STUDENT_LOYALTY.DISLOYAL:
                loyaltyImage.sprite = disloyalSprite;
                break;
            default:
                loyaltyImage.sprite = indifferentSprite;
                break;
        }
    }
}
