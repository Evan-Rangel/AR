using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class CharacterUIManager : MonoBehaviour
{
    [field: SerializeField] public GameObject grid { get; private set; }
    [SerializeField] GameObject actionUIPrefab;
    [SerializeField] GameObject actionPrefab;
    [SerializeField] Image background;
    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text characterHealthTxt;
    [SerializeField] TMP_Text playerNameTxt;
    [SerializeField] List<GameObject> energyImages;
    [SerializeField] List<GameObject> energyTxt;

    [SerializeField] Animator effectAnimator;
    public CharacterData data;
    Dictionary<ElementType, int> energyElements;
    public UnityEvent deathEvent;
    float currentHealth;
    [field: SerializeField] public GameObject selectCardButton { get; private set; }
    [field: SerializeField] public GameObject recieveDamageButton { get; private set; }
    private void Start()
    {
        LoadActions(data);
        energyElements = new Dictionary<ElementType, int>();
        foreach (ElementType elements in Enum.GetValues(typeof(ElementType)))
        {
            energyElements[elements] = 0;
        }
        recieveDamageButton.GetComponent<Button>().onClick.AddListener(() => RecieveDamage(ref GameManager.instance.currentDamage, GameManager.instance.elementDamage));
        deathEvent.AddListener(() => GameManager.instance.CharacterDeath(data.charName));
        deathEvent.AddListener(() => transform.root.gameObject.SetActive(false));
        deathEvent.AddListener(() => GameManager.instance.StartText(data.charName + " muere"));

        recieveDamageButton.SetActive(false);
        selectCardButton.SetActive(false);
    }
   
    public void SetPlayerName(string _name)
    {
        playerNameTxt.text = _name;
    }
    public void RecieveDamage(ref int damage, ElementType elementAttack)
    {
        if (elementAttack == data.debility && elementAttack != data.affinity)
        {
            currentHealth -= 20 + damage;
            GameManager.instance.StartText(data.charName + " recibe "+ damage+" + 20 de daño extra por debilidad");
        }
        else
        {
            currentHealth -= damage;
            GameManager.instance.StartText(data.charName + " recibe " + damage + " de daño");
        }
        if (currentHealth > data.health) currentHealth = data.health;
        characterHealthTxt.text = currentHealth.ToString() + "ps";
        GameManager.instance.DamageDeal(data.charName);
        if (currentHealth <= 0)
        {
            deathEvent.Invoke();
        }
    }
    public void AddEnergy(int _energy, ElementType element)
    {
        energyElements[element] += _energy;

        int count = 0;
        GameManager.instance.StartText(data.charName +" recive energia de " + element.ToString());
        foreach (ElementType e in Enum.GetValues(typeof(ElementType)))
        {
            if (energyElements[e] > 0)
            { 
                energyImages[count].SetActive(true);
                energyTxt[count].SetActive(true);
                energyTxt[count].GetComponent<TMP_Text>().text = energyElements[e].ToString();
                energyImages[count].GetComponent<Image>().sprite = GameManager.instance.elementSprites[e];
                count++;
            }
        }
    }
    public void ColorCardEffect(bool _Active)
    {
        effectAnimator.SetBool("Effect", _Active);
    }
    public void ActiveDamage(bool _active)
    {
        if (_active == recieveDamageButton.activeSelf) return;
        recieveDamageButton.SetActive(_active);
        ColorCardEffect(_active);
    }
    public void ActiveCard(bool _active)
    {
        grid.SetActive(_active);
    }
    public void LoadActions(CharacterData data)
    {
        currentHealth = data.health;
        characterName.text = "";//data.charName;
        characterHealthTxt.text = currentHealth.ToString() +"ps";
        foreach (ActionData action in data.actions)
        {
            GameObject actionUI = Instantiate(actionUIPrefab, grid.transform);
            ActionUIManager actionUIManager = actionUI.GetComponent<ActionUIManager>();
            actionUIManager.SetActionData(action);
            SetCardColor(background, data.affinity);
            actionUI.GetComponentInChildren<Button>().onClick.AddListener(() => ActionSelected(action));
        }
        grid.SetActive(false);
    }

    void ActionSelected(ActionData actionData)
    {
        if (energyElements[actionData.elementType]>= actionData.energyCost)
        {
            GameManager.instance.ActiveAction(actionData.actionValue, data.charName, actionData.elementType );
            grid.SetActive(false);
            return;
        }
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
