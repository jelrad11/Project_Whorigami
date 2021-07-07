using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    
    public List<GameObject> buttonPins;
    private int currentButton = -1;
    public void hoverButton(int button){ //0 = Save; 1 = Resume; 2 = Restart Level; 3 = Exit; 4 = Subtitles; 5 = Music
        for(int i = 0; i <= 5; i++){
            if(i == button)
                buttonPins[i].SetActive(true);
            else
                buttonPins[i].SetActive(false);
        }
    }
}
