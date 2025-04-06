using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerOne;
    [SerializeField] PlayerController playerTwo;
    public PlayerController currentPlayerTurn;
    [SerializeField] GameObject holder;
    [SerializeField] GameObject characterUIPrefab;
    public static GameManager instance;
    [SerializeField] GameObject energyPrefab;
    [SerializeField] TMP_Text messageTxt;
    [SerializeField] TMP_Text Player01Txt;
    [SerializeField] TMP_Text Player02Txt;
    string player1Name, player2Name;
    int currentCharsAlive = 0;
    public int currentDamage;
    public ElementType elementDamage;
    IEnumerator showText;
    [SerializeField] Dictionary<string, PlayerController> pokemonsActives;
    public PlayerController GetPlayerByCharacterName(string characterName)
    {
        if (!pokemonsActives.ContainsKey(characterName)) return null;
        return pokemonsActives[characterName];
    
    }
    public Color normalColor;
    public Color highlightColor;
    Dictionary< string, Transform> characterPositions;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        showText = ShowText("Start");
        pokemonsActives = new Dictionary<string, PlayerController>();
        characterPositions= new Dictionary<string, Transform>();
    }
    private void Start()
    {
        Player01Txt.text= playerOne.playerName; 
        Player02Txt.text= playerTwo.playerName;
        StartText(playerOne.playerName + " muestra tu Pokemon");
        StartCoroutine(PlayerTwoCharacter());
    }
    IEnumerator PlayerTwoCharacter()
    {
        yield return new WaitUntil(()=> pokemonsActives.ContainsValue(playerOne));
        StartText(playerTwo.playerName + " muestra tu Pokemon");
    }
    public void ActiveAction(int _damage, string characterName, ElementType elementAttack)
    {
        currentDamage = _damage;
        elementDamage = elementAttack;
        if (pokemonsActives[characterName] != null)
        {
            if (pokemonsActives[characterName] == playerOne)
            {
                playerTwo.ActiveDamageableCards(); 
                playerOne.DisableSelectedCards();
            }
            if (pokemonsActives[characterName] == playerTwo)
            { 
                playerOne.ActiveDamageableCards();
                playerTwo.DisableSelectedCards();
            }
        }
    }
    public void DamageDeal(string _charName)
    {
        currentDamage = 0;
        pokemonsActives[_charName].DisableDamageableCards();
        if (currentPlayerTurn != null) currentPlayerTurn.DisableSelectedCards();
    }
    //Funcion para obtener la posicion del centro de las cartas
    public Vector3 CenterPosition()
    {
        if (characterPositions.Count == 0) return Vector3.zero;
        Vector3 center = Vector3.zero;
        foreach (Transform pos in characterPositions.Values)
        {
            center += pos.position;
        }
        return (center / characterPositions.Count) + Vector3.up * 0.1f;
    }
    //Funcion para instanciar el prefab de las cartas. OnTargetFound
    public void LinkCharacterToPlayer(GameObject characterCard)
    {
        if (characterCard == null) return;
        string pokemonName = characterCard.GetComponent<CharacterUIManager>().data.charName;
        if (pokemonsActives.ContainsKey(pokemonName)) return;
        if (!characterPositions.ContainsKey(pokemonName))
        {
            characterPositions[pokemonName] = characterCard.transform;
        }
        if (currentCharsAlive < 4)
        {
            currentCharsAlive++;
            if (currentCharsAlive % 2 != 0)
            {
                playerOne.AddCharacterToPlayer(characterCard);
                characterCard.GetComponent<CharacterUIManager>().SetPlayerName(playerOne.playerName);
                pokemonsActives[pokemonName] = playerOne;

            }
            else
            {
                characterCard.GetComponent<CharacterUIManager>().SetPlayerName(playerTwo.playerName);
                playerTwo.AddCharacterToPlayer(characterCard);
                pokemonsActives[pokemonName] = playerTwo;

                if (currentPlayerTurn == null)
                {

                    StartCoroutine(DelayTurn());

                }
            }
        }
    }
    IEnumerator DelayTurn()
    {
        yield return new WaitForSeconds(0.05f);
        Turn(playerOne);
    }
    void Turn(PlayerController _newPlayerTurn)
    {

        currentPlayerTurn = _newPlayerTurn;
        currentPlayerTurn.ActiveSelectedCards();
        SpawnEnergy();
        StartText("turno de " + currentPlayerTurn.playerName);
    }
    public void NextTurn()
    {
        currentPlayerTurn.DisableSelectedCards();
        Turn(currentPlayerTurn==playerOne?playerTwo:playerOne);
    }
    public void CharacterDeath(string pokemonName)
    {
        pokemonsActives[pokemonName].LosePokeball();
        pokemonsActives[pokemonName].DisableDamageableCards();
        currentCharsAlive--;
    }
    public void StartText(string _text) { StopCoroutine(showText); showText = ShowText(_text); StartCoroutine(showText); }
    IEnumerator ShowText(string text)
    {
        messageTxt.text = text;
        messageTxt.maxVisibleCharacters = 0;
        foreach (char c in text)
        {
            messageTxt.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void SpawnEnergy() 
    {
        GameObject energy = Instantiate(energyPrefab, CenterPosition() , Quaternion.Euler(90, 0, 0));//, Camera.main.transform.rotation, Camera.main.transform);
        energy.GetComponent<EnergyManager>().target= currentPlayerTurn;
    }
}
