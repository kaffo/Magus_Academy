using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDependancy : MonoBehaviour
{
    [Header("Dependancy List")]
    public List<UpgradeDependancy> myUpgradeDependancies = new List<UpgradeDependancy>();

    [Header("Parameters")]
    public bool initialPurchaseState = false;

    //[HideInInspector()]
    public List<UpgradeDependancy> upgradeDependanciesOnMe = new List<UpgradeDependancy>();
    public bool dependanciesResolved = false;
    private bool upgradePurchased = false;

    private void Awake()
    {
        upgradePurchased = initialPurchaseState;
        foreach (UpgradeDependancy dependancy in myUpgradeDependancies)
        {
            dependancy.upgradeDependanciesOnMe.Add(this);
        }
    }

    private void Start()
    {
        CheckDependancies();
    }

    public void CheckDependancies()
    {
        bool readyToResolve = true;
        foreach (UpgradeDependancy dependancy in myUpgradeDependancies)
        {
            if (!dependancy.IsUpgradePurchased())
            {
                readyToResolve = false;
                break;
            }
        }

        dependanciesResolved = readyToResolve;
    }

    public void PurchaseUpgrade()
    {
        if (dependanciesResolved)
        {
            Debug.Log("Purchased " + gameObject.name);
            upgradePurchased = true;

            foreach (UpgradeDependancy dependancy in upgradeDependanciesOnMe)
            {
                dependancy.CheckDependancies();
            }
        } else
        {
            Debug.Log("Cannot purchase " + gameObject.name);
        }
    }

    public bool IsUpgradePurchased()
    {
        return upgradePurchased;
    }
}
