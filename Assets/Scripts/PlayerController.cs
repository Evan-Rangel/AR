using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> characterCards { get; private set; }
    public ElementType[] elementsEnergy; 
    [field: SerializeField] public PlayerController enemyPlayer { get; private set; }
    [field: SerializeField]public string playerName { get; private set; }
    [SerializeField] string playerNumber;

    [SerializeField] GameObject pokeballHolder;
    [SerializeField] int pokeballs = 3;
    [SerializeField] Sprite pokeballXSprite;

    
    Image[] currentPokeball;
    
    private void Awake()
    {
        currentPokeball= pokeballHolder.GetComponentsInChildren<Image>();
        characterCards = new List<GameObject>();
    }
    private void Start()
    {
        playerName = PlayerPrefs.GetString("NombreJugador" + playerNumber);
    }
    public void LosePokeball()
    {
        pokeballs--;
        currentPokeball[pokeballs].sprite = pokeballXSprite;

        if (pokeballs <= 0)
        {
            StartCoroutine(DelayTextDeath());
        }
    }
    IEnumerator DelayTextDeath()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.StartText(enemyPlayer.playerName + " Gano");
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