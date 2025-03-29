using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class CharacterUIManager : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] GameObject actionUIPrefab;
    [SerializeField] Image background;
    [SerializeField] TMP_Text characterName;
    [SerializeField] List<GameObject> energyImages;

    public void LoadEnergyImages(int _energy)
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
            }
            energyImages[0].GetComponentInChildren<GameObject>().SetActive(false);
        }
        else
        {
            energyImages[0].SetActive(true);
            energyImages[0].GetComponentInChildren<GameObject>().SetActive(true);
            energyImages[0].GetComponentInChildren<TMP_Text>().text = _energy.ToString();
        }
    }
    public void LoadActions(CharacterData data)
    {
        foreach (ActionData action in data.actions)
        {
            GameObject actionUI = Instantiate(actionUIPrefab, grid.transform);
            actionUI.GetComponent<ActionUIManager>().SetActionData(action);
            SetCardColor(background, data.affinity);
            characterName.text = data.charName;
            actionUI.GetComponentInChildren<Button>().onClick.AddListener(() => Debug.Log(action.actionName));
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
            case ElementType.Rayo:
                _img.color = Color.yellow;
                break;
            case ElementType.Aire:
                _img.color = Color.white;
                break;
            case ElementType.Planta:
                _img.color = Color.green;
                break;

        }
    }
}
