using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyManager: Draggable
{
    [SerializeField] int energyCount = 1;
    [SerializeField] ElementType elementType;
    SpriteRenderer spriteRenderer;
    public PlayerController target;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void Start()
    {
        elementType= GameManager.instance.currentPlayerTurn.elementsEnergy[UnityEngine.Random.Range(0, GameManager.instance.currentPlayerTurn.elementsEnergy.Length)];
        switch (elementType)
        {           case ElementType.Fuego:
               spriteRenderer.color = Color.red;
               break;
           case ElementType.Agua:
               spriteRenderer.color = Color.blue;
               break;
           case ElementType.Planta:
               spriteRenderer.color = Color.green;
               break;
           case ElementType.Normal:
               spriteRenderer.color = Color.white;
               break;
        }         
    }
    
    public override void DropInCard()
    {
        base.DropInCard();
        if (GameManager.instance.GetControllerByCharacter(targetCharacter.data.charName) == target)
        {
            targetCharacter.AddEnergy(energyCount, elementType);
            GameObject.Destroy(gameObject);
        }
    }
}
