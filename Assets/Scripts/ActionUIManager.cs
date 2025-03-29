using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActionUIManager:MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] List<GameObject> energyCostImages;
    [SerializeField] List<GameObject> energyCostTexts;

    public void SetActionData(ActionData actionData)
    {
        nameText.text = actionData.actionName;
        for (int i = 0; i < energyCostImages.Count; i++)
        {
            energyCostImages[i].SetActive(false);
        }
        if (actionData.energyCost <= 3)
        {
            for (int i = 0; i < actionData.energyCost; i++)
            {
                energyCostImages[i].SetActive(true);
                SetImageColor(energyCostImages[i].GetComponent<Image>(), actionData.elementType);
            }
            energyCostTexts[0].SetActive(false);
            energyCostTexts[1].SetActive(false);
        }
        else
        {
            energyCostImages[0].SetActive(true);
            SetImageColor(energyCostImages[0].GetComponent<Image>(), actionData.elementType);

            energyCostTexts[0].SetActive(true);
            energyCostTexts[1].SetActive(true);
            energyCostTexts[1].GetComponent<TMP_Text>().text = actionData.energyCost.ToString();
            energyCostTexts[0].GetComponent<TMP_Text>().text = "X";
        }
    }
    void SetImageColor(Image _img, ElementType _type)
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
