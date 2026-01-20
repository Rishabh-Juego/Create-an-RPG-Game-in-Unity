using UnityEngine;

public abstract class UniqueScriptable : ScriptableObject, IUniqueId
{
    [Space(10), Header("Implement UniqueId"), Space(5)]
    public string itemName;
    public UniqueType itemType = UniqueType.Undefined;
    
    
    public string UniqueID => itemName;
    public UniqueType UniqueType => itemType;
}
