using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SinuousProductions;

public class CardDisplay : MonoBehaviour
{

    public Card cardData;

    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image[] typeImages;

    public Image damageImage;

    private Color[] cardColors = {
        new Color(0.44f, 0f, 0f), //Fire
        new Color(0.42f, 0.25f, 0.08f), //Earth
        new Color(0.1f, 0.2f, 0.35f), //Water
        new Color(0.23f, 0.06f, 0.21f),  //Dark
        new Color(0.54f, 0.55f, 0.39f), //Light
        new Color(0.38f, 0.51f, 0.55f) //Air
    };

    private Color[] typeColors = {
        Color.red, //Fire
        new Color(0.8f, 0.52f, 0.24f), //Earth
        Color.blue, //Water
        Color.black,  //Dark
        Color.yellow, //Light
        Color.white //Air
    };

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        //Update the main card image color based on the first card type
        cardImage.color = cardColors[(int)cardData.cardType[0]];

        damageImage.color = typeColors[(int)cardData.damageType[0]];
        
        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        damageText.text = $"{cardData.damageMin} - {cardData.damageMax}";

        //Update type images
        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count){
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else{
                typeImages[i].gameObject.SetActive(false);
            }
        }
    }
}
