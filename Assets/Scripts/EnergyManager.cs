using System;
using System.Linq;
using UnityEngine;

public class EnergyManager: Draggable
{
    [SerializeField] int energyCount = 1;
    [SerializeField] ElementType elementType;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void Start()
    {

        elementType = (ElementType)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(ElementType)).Cast<ElementType>().Max());
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
        }            }
    private void LateUpdate()
    {
        //transform.position= GameManager.instance.energySpawn.position;
    }
    public override void DropInCard()
    {
        base.DropInCard();
        targetCharacter.AddEnergy(energyCount, elementType);
    }
}
