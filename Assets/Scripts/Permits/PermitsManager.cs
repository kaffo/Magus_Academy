using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PERMITS
{
    FIRE_MAGE_ONE,
    DRUID_ONE,
    STUDENT_CAPACITY_ONE
}

[Serializable]
public class PermitsGameObjectDictionary : SerializableDictionary<PERMITS, GameObject> { }

[Serializable]
public class PermitsBoolDictionary : SerializableDictionary<PERMITS, bool> { }

[Serializable]
public class PermitsDependancyDictionary : SerializableDictionary<PERMITS, UpgradeDependancy> { }

public class PermitsManager : Singleton<PermitsManager>
{
    public PermitsGameObjectDictionary permitsGameObjectDict;
    public PermitsBoolDictionary enabledPermitsDict;
    public PermitsDependancyDictionary permitsDependancyScriptDict;

    public void EnablePermit(PERMITS permitToEnable)
    {
        permitsDependancyScriptDict[permitToEnable].CheckDependancies();
        if (enabledPermitsDict[permitToEnable])
        {
            Debug.LogWarning(permitToEnable.ToString() + " already enabled!");
        } else if (permitsDependancyScriptDict[permitToEnable].dependanciesResolved)
        {
            enabledPermitsDict[permitToEnable] = true;
            Instantiate(permitsGameObjectDict[permitToEnable], transform);
            permitsDependancyScriptDict[permitToEnable].PurchaseUpgrade();
        }
    }
}
