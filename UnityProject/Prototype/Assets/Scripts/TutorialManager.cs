using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialText;
    private int step = 0;

    private inventoryManager inventoryManagerInstance;

    // Start is called before the first frame update
    void Start()
    {
        ShowNextStep();
    }

    public void ShowNextStep()
    {
        switch(step)
        {
            case 0:
                tutorialText.text = "Welcome to Dream State! Use WASD to move around.";
                ResetPotionIndex();
                break;

            case 1:   
            tutorialText.text = "Press space to jump.";
                break;

            case 2:
                tutorialText.text = "Watch out for enemies! Use right click to attack and left to block.";
                break;

            case 3:
                tutorialText.text = "You must be hurt! Pick up a red potion and press 'E' to use it.";
                break;

            case 4:
                tutorialText.text = "Be careful of the spider webs and thorns!!!";
                break;

            default:
                tutorialText.text = "Tutorial Complete!! Head over to the portal to continue";
                ResetPotionIndex();
                break;
        }

        step++;
    }

    private void ResetPotionIndex()
    {
        if (inventoryManagerInstance != null)
        {
            inventoryManagerInstance.healingItemIndex = 0;
            inventoryManagerInstance.UpdateCount();
        }
    }
}
