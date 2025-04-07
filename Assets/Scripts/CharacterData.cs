using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character Data", order = 51)]
public class CharacterData: ScriptableObject
{
    [Range(10, 200)] public float health;
    public string charName;
    public ElementType affinity;
    public ElementType debility;
    public List<ActionData> actions; 
}
