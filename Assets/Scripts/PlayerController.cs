using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> characterCards { get; private set; }
    [SerializeField] GameObject pokeballHolder;
    Image[] currentPokeball;
    [SerializeField] int pokeballs = 3;
    [SerializeField] Sprite pokeballXSprite;
    [field: SerializeField] public PlayerController enemyPlayer { get; private set; }
    bool activeEffect;
    [field: SerializeField]public string playerName { get; private set; }
    public ElementType[] elementsEnergy; 
    private void Awake()
    {
        currentPokeball= pokeballHolder.GetComponentsInChildren<Image>();
        characterCards = new List<GameObject>();
        activeEffect = false; 
    }
    public void LosePokeball()
    {
        pokeballs--;
        currentPokeball[pokeballs].sprite = pokeballXSprite;
        if (pokeballs <= 0)
        {
            GameManager.instance.StartText(enemyPlayer.playerName + " Gano");
        }
    }
    public void AddCharacterToPlayer(GameObject _characterCard)
    {
        characterCards.Add(_characterCard);
    }
    public void ActiveDamageableCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().ActiveDamage(true);
        }
    } 
    public void DisableDamageableCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().ActiveDamage(false);
        }
    }
    public void ActiveSelectedCards()
    {
        Debug.Log(playerName);
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().ActiveCard(true);
        }
    }
    public void DisableSelectedCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().ActiveCard(false);
        }
    }
   
}
public enum ElementType {
Fuego,Agua,Planta, Normal
}