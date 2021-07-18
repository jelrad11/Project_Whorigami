using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public int gameStage;
    
    public List<GameObject> buttonPins;
    public List<GameObject> sideButtonPins;
    public GameObject pauseMenu;
    public int maxButtons;
    public int currentButton = 0;
    public int maxSideButtons;
    public int sideButton = 0;
    public float bSC = 0.5f;
    private float buttonSwitchCooldown = 0.1f;

    public float sliderValueChange = 0.1f;
    public float slCD = 0.2f;
    private float sliderCooldown = 0.2f;
    private bool pauseMenuActive = true;

    
    private void Update()
    {
        buttonSwitchCooldown -= Time.deltaTime;
        sliderCooldown -= Time.deltaTime;

        changeButtonHover();
        sliderInteraction();
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start") || (Input.GetButtonDown("Cancel") && pauseMenuActive))
            pauseMenuTrigger();

        if(Input.GetButtonDown("Submit") && pauseMenuActive){
            switch(currentButton){
                case 0:
                pauseMenuTrigger();
                break;
                case 1:
                loadLatest();
                break;
                case 2:
                restartLevel();
                break;
                case 3:
                changeToMenu();
                break;
                default:
                break;
            }
        }
    }

    private void applySettings(){

    }
    
    public void changeToMenu(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // public void saveGame(){
    //     SaveSystem.SavePlayer(gameStage, )
    // }

    public void restartLevel(){
        switch(gameStage){
            case 0:
            Data.LoadSave = false;
            SceneManager.LoadScene("JellyScene");
            break;
        }
    }
    public void loadLatest(){
        switch(gameStage){
            case 0:
            Data.LoadSave = true;
            SceneManager.LoadScene("JellyScene");
            break;
        }
    }

    private void sliderInteraction(){
        if(sideButton != 0){
            Slider slider = sideButtonPins[2 - sideButton].GetComponent<Slider>();
            if((Input.GetAxis("DPadVertical") > 0.25f || Input.GetAxis("Vertical") > 0.25f) && sliderCooldown <= 0f){
                slider.value = slider.value + sliderValueChange;
                sliderCooldown = slCD;
            }
            if((Input.GetAxis("DPadVertical") < -0.25f || Input.GetAxis("Vertical") < -0.25f) && sliderCooldown <= 0f){
                slider.value = slider.value - sliderValueChange;
                sliderCooldown = slCD;
            }        
        }
    }

    public void pauseMenuTrigger(){
        pauseMenuActive = !pauseMenuActive;
        pauseMenu.SetActive(pauseMenuActive);
    }
    public void hoverButton(int button){ //0 = Save; 1 = Resume; 2 = Restart Level; 3 = Exit; 4 = Subtitles; 5 = Music
        for(int i = 0; i < maxButtons; i++){
            if(i == button)
                buttonPins[i].SetActive(true);
            else
                buttonPins[i].SetActive(false);
        }
    }
    private void switchButton(int currentButton, int sideButton){
        if(sideButton == 0){
            hoverButton(currentButton);
        }
        else{
            hoverButton(maxButtons - sideButton);
        }
    }
    private void changeButtonHover(){
        if((Input.GetAxis("DPadVertical") > 0.25f || Input.GetAxis("Vertical") > 0.25f) && buttonSwitchCooldown <= 0f){
            currentButton--;
            if(currentButton >= maxButtons - maxSideButtons)
                currentButton = 0;
            else if(currentButton < 0)
                currentButton = maxButtons - 1 - maxSideButtons;

            switchButton(currentButton, sideButton);

            buttonSwitchCooldown = bSC;
        }
        if((Input.GetAxis("DPadVertical") < -0.25f || Input.GetAxis("Vertical") < -0.25f) && buttonSwitchCooldown <= 0f){
            currentButton++;
            if(currentButton >= maxButtons - maxSideButtons)
                currentButton = 0;
            else if(currentButton < 0)
                currentButton = maxButtons - 1 - maxSideButtons;

            switchButton(currentButton, sideButton);

            buttonSwitchCooldown = bSC;
        }
        if((Input.GetAxis("DPadHorizontal") < -0.25f || Input.GetAxis("Horizontal") < -0.25f) && buttonSwitchCooldown <= 0f){
            sideButton++;
            if(sideButton > maxSideButtons)
                sideButton = 0;
            else if(sideButton < 0)
                sideButton = maxSideButtons;

            switchButton(currentButton, sideButton);
            buttonSwitchCooldown = bSC;
        }
        if((Input.GetAxis("DPadHorizontal") > 0.25f || Input.GetAxis("Horizontal") > 0.25f) && buttonSwitchCooldown <= 0f){
            sideButton--;
            if(sideButton > maxSideButtons)
                sideButton = 0;
            else if(sideButton < 0)
                sideButton = maxSideButtons;

            switchButton(currentButton, sideButton);
            buttonSwitchCooldown = bSC;
        }
    }
}
