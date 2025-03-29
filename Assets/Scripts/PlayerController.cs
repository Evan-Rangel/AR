using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public CharacterData[] character { get; private set; }
    
}
public enum ElementType {
Fuego,Agua,Rayo,Aire,Planta
}