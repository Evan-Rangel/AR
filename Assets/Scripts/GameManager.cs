using UnityEngine;

public class GameManager:MonoBehaviour
{
    [SerializeField] PlayerController playerOne;
    [SerializeField] PlayerController playerTwo;
    [SerializeField] GameObject holder;
    [SerializeField] GameObject characterUIPrefab;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowCharacter()
    { 
        GameObject tempChar= Instantiate(characterUIPrefab, holder.transform);
        tempChar.GetComponent<CharacterUIManager>().LoadActions(playerOne.character[0]);
    }
}
