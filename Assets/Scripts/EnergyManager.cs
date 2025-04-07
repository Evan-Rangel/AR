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
        elementType= GameManager.instance.currentPlayerTurn.elementsEnergy[UnityEngine.Random.Range(0, GameManager.instance.currentPlayerTurn.elementsEnergy.Count)];
        spriteRenderer.sprite = GameManager.instance.elementSprites[elementType];
    }
    
    public override void DropInCard()
    {
        base.DropInCard();
        //Comparar el target con las cartas del jugador activo
        if (GameManager.instance.GetPlayerByCharacterName(targetCharacter.data.charName) == target)
        {
            targetCharacter.AddEnergy(energyCount, elementType);
            GameObject.Destroy(gameObject);
        }
    }
}
