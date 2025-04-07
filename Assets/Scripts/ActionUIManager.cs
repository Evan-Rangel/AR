using UnityEngine;

public class ActionUIManager:MonoBehaviour
{
    [field:SerializeField] public int energyCost { get; private set; }  
    [field:SerializeField] public ElementType elementType { get; private set; }  

    public void SetActionData(ActionData actionData)
    {
        energyCost = actionData.energyCost;
        elementType = actionData.elementType;
    }
}
