using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class CharacterUIManager : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] GameObject actionUIPrefab;
    [SerializeField] GameObject actionPrefab;
    [SerializeField] Image background;
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text characterHealthTxt;
    [SerializeField] List<GameObject> energyImages;
    [SerializeField] GameObject energyTxt;
    public CharacterData data;
    Dictionary<ElementType, int> energyElements;
    public UnityEvent deathEvent;
    float currentHealth;
    [field: SerializeField]public GameObject  selectCardButton{ get; private set; }
    [field: SerializeField] public GameObject recieveDamageButton { get; private set; }
    //[field: SerializeField] public GameObject cancelCardButton { get; private set; }
    private void Start() 
    {
        LoadActions(data);
        energyElements = new Dictionary<ElementType, int>();
        foreach (ElementType elemnts in Enum.GetValues(typeof(ElementType)))
        {
            energyElements.Add(elemnts, 0);
        }
        recieveDamageButton.GetComponent<Button>().onClick.AddListener(() => RecieveDamage(GameManager.instance.currentDamage));
        selectCardButton.GetComponent<Button>().onClick.AddListener(()=>SelectCard());
    }
    public void SelectCard()
    { 
        grid.SetActive(true);
        selectCardButton.SetActive(false);
        
    }
    public void RecieveDamage(int damage)
    {
        recieveDamageButton.SetActive(false);
        currentHealth -= damage;
        if(currentHealth>data.health) currentHealth = data.health;
        if (currentHealth<=0)
        {
            deathEvent.Invoke();
        }
        characterHealthTxt.text = currentHealth.ToString() + "ps";

    }
    public void AddEnergy(int _energy, ElementType element)
    {
        energyElements[element] += _energy;
        Debug.Log("AddEnergy");
        LoadEnergyImages(energyElements[element],element);
    }
    public void LoadEnergyImages(int _energy, ElementType element)
    {
        foreach (GameObject img in energyImages)
        {
            img.SetActive(false);
        }
        if (_energy <= 4)
        {

            for (int i = 0; i < _energy; i++)
            {
                energyImages[i].SetActive(true);
                SetCardColor(energyImages[i].GetComponent<Image>(), element);
            }
            energyTxt.SetActive(false);
        }
        else
        {
            Debug.Log("Text Ennergty");
            energyImages[0].SetActive(true);
            SetCardColor(energyImages[0].GetComponent<Image>(), element);
            energyTxt.SetActive(true);
            energyTxt.GetComponent<TMP_Text>().text = _energy.ToString();
        }
    }
    public void LoadActions(CharacterData data)
    {
        currentHealth = data.health;
        characterHealthTxt.text = currentHealth.ToString() +"ps";
        foreach (ActionData action in data.actions)
        {
            GameObject actionUI = Instantiate(actionUIPrefab, grid.transform);
            ActionUIManager actionUIManager = actionUI.GetComponent<ActionUIManager>();
            actionUIManager.SetActionData(action);
            SetCardColor(background, data.affinity);
            characterName.text = data.charName;
            actionUI.GetComponentInChildren<Button>().onClick.AddListener(() => ActionSelected(action));
        }
        grid.SetActive(false);
    }

    void ActionSelected(ActionData data)
    {
        if (energyElements[data.elementType]>=data.energyCost)
        {

            Debug.Log("Action");
            return;
        }
        Debug.Log("No energy");
    }
    void SetCardColor(Image _img, ElementType _type)
    {
        switch (_type)
        {
            case ElementType.Fuego:
                _img.color = Color.red;
                break;
            case ElementType.Agua:
                _img.color = Color.blue;
                break;
            case ElementType.Planta:
                _img.color = Color.green;
                break;
            case ElementType.Normal:
                _img.color = Color.white;
                break;
        }
    }
}
