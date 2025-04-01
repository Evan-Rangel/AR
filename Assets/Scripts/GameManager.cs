using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    Dictionary<GamePhase, string> mesaggesPhases;
    GamePhase currentPhase;
    int currentCharsAlive = 0;
    public int currentDamage;
    [SerializeField] Dictionary<string, PlayerController> pokemonsActives;
    [field: SerializeField] public Transform energySpawn { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        pokemonsActives= new Dictionary<string, PlayerController>();
    }
    void ChangePhase(GamePhase newPhase)
    { 
        currentPhase = newPhase;
    }
    public void LinkCharacterToPlayer(GameObject characterCard)
    {
        if (characterCard == null) return;
        string pokemonName = characterCard.GetComponent<CharacterUIManager>().data.charName;
        if (pokemonsActives.ContainsKey(pokemonName)) return;

        if (currentCharsAlive<4)
        {
            currentCharsAlive++;
            if (currentCharsAlive % 2 != 0)
            {
                Debug.Log(pokemonName + " Linked To player 1");
                playerOne.AddCharacterToPlayer(characterCard);
                pokemonsActives[pokemonName]= playerOne;
            }
            else
            {
                Debug.Log(pokemonName + " Linked To player 2");
                playerTwo.AddCharacterToPlayer(characterCard);
                pokemonsActives[pokemonName]= playerTwo;
            
            }
        }
    }
    public void CharacterDeath(string pokemonName)
    {
        pokemonsActives[pokemonName].LosePokeball();
        currentCharsAlive--;
    }

    public void SpawnEnergy() 
    { 
        GameObject energy=Instantiate(energyPrefab, energySpawn.position, Camera.main.transform.rotation, Camera.main.transform);
    }
}
