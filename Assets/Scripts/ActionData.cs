using UnityEngine;

[CreateAssetMenu(fileName = "ActionData", menuName = "Action Data", order = 51)]
public class ActionData : ScriptableObject
{
    public ElementType elementType;
    public string actionName;
    [Range(1,10)]public int energyCost;
    [Range(0, 500)] public int actionValue;
}
