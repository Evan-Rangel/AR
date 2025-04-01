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
    [field: SerializeField]public string playerName { get; private set; } 
    private void Awake()
    {
        currentPokeball= pokeballHolder.GetComponentsInChildren<Image>();
        characterCards = new List<GameObject>();
    }
    public void LosePokeball()
    {
        pokeballs--;
        currentPokeball[pokeballs].sprite = pokeballXSprite;
        if (pokeballs <= 0)
        {

            Debug.Log("Player Lose");
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
            card.GetComponent<CharacterUIManager>().recieveDamageButton.SetActive(true);
        }
    } 
    public void DisableDamageableCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().recieveDamageButton.SetActive(false);
        }
    }
    public void ActiveSelectedCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().selectCardButton.SetActive(true);
        }
    }public void DisableSelectedCards()
    {
        foreach (GameObject card in characterCards)
        {
            card.GetComponent<CharacterUIManager>().selectCardButton.SetActive(false);
        }
    }
}
public enum ElementType {
Fuego,Agua,Planta, Normal
}