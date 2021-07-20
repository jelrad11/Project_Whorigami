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

    public Movement_Paper movement_Paper;
    public Movement_Bird_G movement_Bird_G;
    public StoryController storyController;
    public List<AudioSource> audioSources;

    private Audio_mix audio_mix_script;

    private void Awake()
    {
        audio_mix_script = GameObject.FindGameObjectWithTag("AudioSources").GetComponent<Audio_mix>();
    }

    private void Start()
    {
        applySettings();

        // example

    }
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
        if(gameStage == 0){
            if(Data.LoadSave){
                PlayerData data = SaveSystem.LoadPlayer();
                movement_Paper.canFly = data.canFly;
                movement_Paper.canTransformLong = data.canTransformLong;
                movement_Paper.canTransformShort = data.canTransformShort;
                movement_Paper.canTurn = data.canTurn;

                movement_Paper.applySettings();
            }
        }
        storyController.applySettings();
        OptionData optData = SaveSystem.LoadOptions();

        
        
        audio_mix_script.SetVolume(-80f + (optData.audio * 80f)); // -80f = silence, 0 = full volume

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
            case 1:
            Data.LoadSave = false;
            SceneManager.LoadScene("Bird Scene-Final");
            break;
        }
    }
    public void loadLatest(){
        switch(gameStage){
            case 0:
            Data.LoadSave = true;
            SceneManager.LoadScene("JellyScene");
            break;
            case 1:
            Data.LoadSave = true;
            SceneManager.LoadScene("Bird Scene-Final");
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
        if(pauseMenuActive) {
            SaveSystem.SaveOptions(sideButtonPins[0].GetComponent<Slider>().value, sideButtonPins[1].GetComponent<Slider>().value);
            applySettings();
        }
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
