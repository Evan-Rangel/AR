using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;
enum GamePhase
{ 
    Start,
    FirstCharacterSelection,
    SecondCharacterSelection,
    thirdCharacterSelection,
    fourthCharacterSelection,
    Player01Turn,
    Player02Turn,
    End

}
public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerOne;
    [SerializeField] PlayerController playerTwo;
    [SerializeField] GameObject holder;
    [SerializeField] GameObject characterUIPrefab;
    public static GameManager instance;
    [SerializeField] GameObject energyPrefab;
    [SerializeField] TMP_Text messageTxt;
    [SerializeField] TMP_Text Player01Txt;
    [SerializeField] TMP_Text Player02Txt;
    Dictionary<GamePhase, string> mesaggesPhases;
    string player1Name, player2Name;
    GamePhase currentPhase;
    int currentCharsAlive = 0;
    public int currentDamage;
    [SerializeField] Dictionary<string, PlayerController> pokemonsActives;
    UnityEvent clickEvent;
    [field: SerializeField] public Transform energySpawn { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        pokemonsActives = new Dictionary<string, PlayerController>();
    }
    private void Start()
    {
        
        mesaggesPhases = new Dictionary<GamePhase, string>();
        mesaggesPhases.Add(GamePhase.Start, "Start");
        mesaggesPhases.Add(GamePhase.FirstCharacterSelection, "Player 01 Select Character");
        mesaggesPhases.Add(GamePhase.SecondCharacterSelection, "Player 02 Select Character");
        mesaggesPhases.Add(GamePhase.thirdCharacterSelection, "Player 01 Select Character");
        mesaggesPhases.Add(GamePhase.fourthCharacterSelection, "Player 02 Select Character");
        mesaggesPhases.Add(GamePhase.Player01Turn, "Player 01 Turn");
        mesaggesPhases.Add(GamePhase.Player02Turn, "Player 02 Turn");
        mesaggesPhases.Add(GamePhase.End, "End");
        currentPhase = GamePhase.Start;

        Player01Txt.text= playerOne.playerName; 
        Player02Txt.text= playerTwo.playerName; 
        // ChangeMessage(mesaggesPhases[currentPhase]);
    }
    public void ActiveAction(int _damage, string characterName)
    {
        currentDamage = _damage;
        if (pokemonsActives[characterName] != null)
        {
            if (pokemonsActives[characterName] == playerOne)
            {
                playerTwo.ActiveDamageableCards();    
            }
            if (pokemonsActives[characterName] == playerTwo)
            { 
                playerOne.ActiveDamageableCards();
            }
        }
    }
    public void DamageDeal(string _charName)
    {
        pokemonsActives[_charName].DisableDamageableCards();
        pokemonsActives[_charName].ActiveSelectedCards();
        currentDamage = 0;
        //currentPhase = currentPhase == GamePhase.Player01Turn ? GamePhase.Player02Turn : GamePhase.Player01Turn;
        //ChangeMessage(mesaggesPhases[currentPhase]);
    }
    void ChangeTurn()
    {

    }
    public void LinkCharacterToPlayer(GameObject characterCard)
    {
        if (characterCard == null) return;
        string pokemonName = characterCard.GetComponent<CharacterUIManager>().data.charName;
        if (pokemonsActives.ContainsKey(pokemonName)) return;

        if (currentCharsAlive < 4)
        {
            currentCharsAlive++;
            if (currentCharsAlive % 2 != 0)
            {
                Debug.Log(pokemonName + " Linked To " + playerOne.playerName);
                playerOne.AddCharacterToPlayer(characterCard);
                characterCard.GetComponent<CharacterUIManager>().selectCardButton.SetActive(true);
                characterCard.GetComponent<CharacterUIManager>().SetPlayerName(playerOne.playerName);
                pokemonsActives[pokemonName] = playerOne;
            }
            else
            {
                Debug.Log(pokemonName + " Linked To " + playerTwo.playerName);
                characterCard.GetComponent<CharacterUIManager>().SetPlayerName(playerTwo.playerName);

                playerTwo.AddCharacterToPlayer(characterCard);
                pokemonsActives[pokemonName] = playerTwo;
            }
        }
    }
    public bool FirstTurn(string name)
    {
        Debug.Log(name + "  " + pokemonsActives[name]);

        if(pokemonsActives[name]== playerOne)
        {
            return true;
        }
        return false;

    }
    public void CharacterDeath(string pokemonName)
    {
        pokemonsActives[pokemonName].LosePokeball();
        currentCharsAlive--;
       
    }
    void ChangeMessage(string message)
    {
        messageTxt.gameObject.SetActive(true);
        messageTxt.text = message;

    }
   
    IEnumerator WaitForInteraction()
    { 
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0)|| Input.touchCount==1);
        clickEvent.Invoke();
    }
    public void SpawnEnergy() //Calls In Button
    {
        GameObject energy = Instantiate(energyPrefab, energySpawn.position, Quaternion.Euler(90, 0, 0));//, Camera.main.transform.rotation, Camera.main.transform);
    }
}
