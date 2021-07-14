using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;


    private bool inOptions = false;
    public List<GameObject> buttonPins;
    public int currentButton;
    public int maxButtons;


    public List<GameObject> sliderPins;
    public int currentSlider;
    public int maxSliders;
    public List<Slider> sliders;
    
    public float bSC = 0.5f;
    private float buttonSwitchCooldown = 0.1f;

    public float sliderValueChange = 0.1f;
    public float slCD = 0.2f;
    private float sliderCooldown = 0.2f;

    private void Update()
    {
        buttonSwitchCooldown -= Time.deltaTime;
        sliderCooldown -= Time.deltaTime;

        if(!inOptions) changeButtonHover();
        else {
            changeSliderHover();
            sliderInteraction();
            if(Input.GetButtonDown("Cancel")) OptionTrigger();
        }
        if(Input.GetButtonDown("Submit") && !inOptions){
            switch(currentButton){
                case 0:
                Continue();
                break;
                case 1:
                NewGame();
                break;
                case 2:
                OptionTrigger();
                break;
                case 3:
                Quit();
                break;
                default:
                break;
            }
        }
    }
    public void Quit(){
        Application.Quit();
    }
    public void Continue(){
        
    }
    public void NewGame(){

    }
    public void OptionTrigger(){
        if(inOptions){
            inOptions = false;
            options.SetActive(false);
            mainMenu.SetActive(true);
        }
        else{
            inOptions = true;
            options.SetActive(true);
            mainMenu.SetActive(false);
        }
    }
    public void hoverButton(int button){ //0 = Save; 1 = Resume; 2 = Restart Level; 3 = Exit; 4 = Subtitles; 5 = Music
        for(int i = 0; i < maxButtons; i++){
            if(i == button)
                buttonPins[i].SetActive(true);
            else
                buttonPins[i].SetActive(false);
        }

    }
    public void hoverSlider(int slider){
        if(slider == 1){
            sliderPins[0].SetActive(true);
            sliderPins[1].SetActive(false);
        }
        else if(slider == 2){
            sliderPins[0].SetActive(false);
            sliderPins[1].SetActive(true);
        }
    }
    private void sliderInteraction(){
        if(currentSlider != 0){
            Slider slider = sliders[currentSlider - 1].GetComponent<Slider>();
            if((Input.GetAxis("DPadHorizontal") > 0.5f || Input.GetAxis("Horizontal") > 0.5f) && sliderCooldown <= 0f){
                slider.value = slider.value + sliderValueChange;
                sliderCooldown = slCD;
            }
            if((Input.GetAxis("DPadHorizontal") < -0.5f || Input.GetAxis("Horizontal") < -0.5f) && sliderCooldown <= 0f){
                slider.value = slider.value - sliderValueChange;
                sliderCooldown = slCD;
            }        
        }
    }

    private void changeSliderHover(){
        if((Input.GetAxis("DPadVertical") > 0.5f || Input.GetAxis("Vertical") > 0.5f) && sliderCooldown<= 0f){
            currentSlider++;
            if(currentSlider > maxSliders)
                currentSlider = 1;
            else if(currentSlider < 1)
                currentSlider = maxSliders;

            hoverSlider(currentSlider);

            sliderCooldown = slCD;
        }
        if((Input.GetAxis("DPadVertical") < -0.5f || Input.GetAxis("Vertical") < -0.5f) && sliderCooldown<= 0f){
            currentSlider--;
            if(currentSlider > maxSliders)
                currentSlider = 0;
            else if(currentSlider < 1)
                currentSlider = maxSliders;

            hoverSlider(currentSlider);

            sliderCooldown = slCD;
        }
    }
    private void changeButtonHover(){
        if((Input.GetAxis("DPadVertical") > 0.25f || Input.GetAxis("Vertical") > 0.25f) && buttonSwitchCooldown <= 0f){
            currentButton--;
            if(currentButton >= maxButtons)
                currentButton = 0;
            else if(currentButton < 0)
                currentButton = maxButtons - 1;

            hoverButton(currentButton);

            buttonSwitchCooldown = bSC;
        }
        if((Input.GetAxis("DPadVertical") < -0.25f || Input.GetAxis("Vertical") < -0.25f) && buttonSwitchCooldown <= 0f){
            currentButton++;
            if(currentButton >= maxButtons)
                currentButton = 0;
            else if(currentButton < 0)
                currentButton = maxButtons - 1;

            hoverButton(currentButton);

            buttonSwitchCooldown = bSC;
        }
    }
}
