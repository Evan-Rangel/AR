using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActionUIManager:MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] List<GameObject> energyCostImages;
    [SerializeField] GameObject energyCostTexts;
    [field:SerializeField] public int energyCost { get; private set; }  
    [field:SerializeField] public ElementType elementType { get; private set; }  

    public void SetActionData(ActionData actionData)
    {
        nameText.text = actionData.actionName;
        energyCost = actionData.energyCost;
        elementType = actionData.elementType;
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
            energyCostTexts.SetActive(false);
            energyCostTexts.SetActive(false);
        }
        else
        {
            energyCostImages[0].SetActive(true);
            SetImageColor(energyCostImages[0].GetComponent<Image>(), actionData.elementType);

            energyCostTexts.SetActive(true);
            energyCostTexts.SetActive(true);
            energyCostTexts.GetComponent<TMP_Text>().text = actionData.energyCost.ToString();
            //energyCostTexts[0].GetComponent<TMP_Text>().text = "X";
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
            case ElementType.Planta:
                _img.color = Color.green;
                break;
            case ElementType.Normal:
                _img.color = Color.white;
                break;
        }
    }
}
