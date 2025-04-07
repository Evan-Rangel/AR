using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> characterCards { get; private set; }
    public List<ElementType> elementsEnergy; 


    [field: SerializeField] public PlayerController enemyPlayer { get; private set; }
    [field: SerializeField]public string playerName { get; private set; }
    [SerializeField] string playerNumber;

    [SerializeField] GameObject pokeballHolder;
    [field:SerializeField]public int pokeballs = 3;
    [SerializeField] Sprite pokeballXSprite;
    
    Image[] currentPokeball;
    
    private void Awake()
    {
        currentPokeball= pokeballHolder.GetComponentsInChildren<Image>();
        characterCards = new List<GameObject>();
    }
    private void Start()
    {
        string data = PlayerPrefs.GetString("ElementoJugador" + playerNumber);
        string[] elements = data.Split(',');
        foreach(string element in elements)
        {
            if (System.Enum.TryParse(element, out ElementType elementType))
            {
                elementsEnergy.Add(elementType);
            }
        }
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
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.mainMenuButton.SetActive(true);
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
