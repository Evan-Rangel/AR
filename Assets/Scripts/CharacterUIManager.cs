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
    [SerializeField] TMP_Text playerNameTxt;
    [SerializeField] List<GameObject> energyImages;
    [SerializeField] List<GameObject> energyTxt;

    public CharacterData data;
    Dictionary<ElementType, int> energyElements;
    public UnityEvent deathEvent;
    float currentHealth;
    [field: SerializeField]public GameObject  selectCardButton{ get; private set; }
    [field: SerializeField] public GameObject recieveDamageButton { get; private set; }
    private void Start() 
    {
        LoadActions(data);
        energyElements = new Dictionary<ElementType, int>();
        foreach (ElementType elements in Enum.GetValues(typeof(ElementType)))
        {
            energyElements[elements]= 0;
        }
        recieveDamageButton.GetComponent<Button>().onClick.AddListener(() => RecieveDamage(ref GameManager.instance.currentDamage));
        selectCardButton.GetComponent<Button>().onClick.AddListener(()=>SelectCard());
        deathEvent.AddListener(() => GameManager.instance.CharacterDeath(data.charName));
        recieveDamageButton.SetActive(false);
        selectCardButton.SetActive(false);

    }
    public void SelectCard()
    { 
        grid.SetActive(true);
        selectCardButton.SetActive(false);
        
    }
    public void SetPlayerName(string _name)
    {
        playerNameTxt.text = _name;
    }
    public void RecieveDamage(ref int damage)
    {
        currentHealth -= damage;
        if(currentHealth>data.health) currentHealth = data.health;
        if (currentHealth<=0)
        {
            deathEvent.Invoke();
            transform.root.gameObject.SetActive(false); 
        }
        characterHealthTxt.text = currentHealth.ToString() + "ps";
        GameManager.instance.DamageDeal(data.charName);
    }
    public void AddEnergy(int _energy, ElementType element)
    {
        energyElements[element] += _energy;
      
        int count = 0;
        Debug.Log(Enum.GetValues(typeof(ElementType)).Length);

        foreach (ElementType e in Enum.GetValues(typeof(ElementType)))
        {
            if (energyElements[e] > 0)
            {
                energyImages[count].SetActive(true);
                energyTxt[count].SetActive(true);
                energyTxt[count].GetComponent<TMP_Text>().text = energyElements[e].ToString();

                SetCardColor(energyImages[count].GetComponent<Image>(), e);

                count++;
            }
        }
    }
    public void LoadEnergyImages(int _energy, ElementType element)
    {
        foreach (GameObject img in energyImages)
        {
            img.SetActive(false);
        }
        int count = 0;
        foreach (ElementType e in Enum.GetValues(typeof(ElementType)))
        {
            if (energyElements[e]>0)
            {
                energyImages[count].SetActive(true);
                energyImages[count].GetComponentInChildren<TMP_Text>().gameObject.SetActive(true);
                energyImages[count].GetComponentInChildren<TMP_Text>().text= energyElements[e].ToString();
                SetCardColor(energyImages[count].GetComponent<Image>(), e);

                count++;
            }
        }
        /*
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
            energyImages[0].SetActive(true);
            SetCardColor(energyImages[0].GetComponent<Image>(), element);
            energyTxt.SetActive(true);
            energyTxt.GetComponent<TMP_Text>().text = _energy.ToString();
        }*/
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

    void ActionSelected(ActionData actionData)
    {
        if (energyElements[actionData.elementType]>= actionData.energyCost)
        {
            GameManager.instance.ActiveAction(actionData.actionValue,data.charName );
            grid.SetActive(false);
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
