using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Events;

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
    IEnumerator showText;
    [SerializeField] Dictionary<string, PlayerController> pokemonsActives;
    public PlayerController GetControllerByCharacter(string name) { return pokemonsActives[name]; }
    public Color normalColor;
    public Color highlightColor;
    Dictionary< string, Transform> characterPositions;
    UnityEvent clickEvent;
    [field: SerializeField] public Transform energySpawn { get; private set; }
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
    private void Update()
    {
        energySpawn.position = CenterPosition()+Vector3.up*0.1f;    
    }
    public void ActiveAction(int _damage, string characterName)
    {
        currentDamage = _damage;

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
        StartText(_charName + " recive " + currentDamage + " de daño");
        currentDamage = 0;
        if (currentPlayerTurn != null) currentPlayerTurn.DisableSelectedCards();
    }
    public Vector3 CenterPosition()
    {
        if (characterPositions.Count == 0) return Vector3.zero;
        Vector3 center = Vector3.zero;
        foreach (Transform pos in characterPositions.Values)
        {
            center += pos.position;
        }
        return center / characterPositions.Count;
    }
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
                Debug.Log(pokemonName + " Linked To " + playerOne.playerName);
                playerOne.AddCharacterToPlayer(characterCard);
                characterCard.GetComponent<CharacterUIManager>().SetPlayerName(playerOne.playerName);
                pokemonsActives[pokemonName] = playerOne;

            }
            else
            {
                Debug.Log(pokemonName + " Linked To " + playerTwo.playerName);
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

   
    IEnumerator WaitForInteraction()
    { 
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0)|| Input.touchCount==1);
        clickEvent.Invoke();
    }
    public void SpawnEnergy() //Calls In Button
    {
        GameObject energy = Instantiate(energyPrefab, energySpawn.position, Quaternion.Euler(90, 0, 0));//, Camera.main.transform.rotation, Camera.main.transform);
        energy.GetComponent<EnergyManager>().target= currentPlayerTurn;
    }
}
