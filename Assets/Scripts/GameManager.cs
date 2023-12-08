using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private Slider bossHealth;
    private Slider playerHealth;
    private GameObject pauseMenu;
    private GameObject activeUI;
    private Scene currentScene;

    public bool gameActive;
    // Start is called before the first frame update
    void Start()
    {
       
      
       currentScene = SceneManager.GetActiveScene();

        // Initialises UI components in script and locks cursor in centre of screen to avoid issues with mouse navigation
        if (currentScene.name == "My Game")
        {
            Cursor.lockState = CursorLockMode.Locked;
            bossHealth = GameObject.Find("Boss Health Bar").GetComponent<Slider>();
            playerHealth = GameObject.Find("Player Health Bar").GetComponent<Slider>();
            pauseMenu = GameObject.Find("Ingame Menu UI");
            pauseMenu.SetActive(false);
            activeUI = GameObject.Find("Active UI");
            gameActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentScene.name == "My Game")
            {
                UpdateMenu();
            }
        }

    }

    public void OnButtonPress(string buttonText)
    {
        switch (buttonText)
        {
            case "Start":
                {
                    // loads main game scene
                    SceneManager.LoadScene("My Game");
                    break;
                }
            case "Quit":
                {
                    // loads main menu scene
                    SceneManager.LoadScene("Main Menu");
                    break;
                }
        }
    }


    public void WoundBoss()
    {
        // reduces boss health slider by 1 when called
        bossHealth.value--;
    }

    public void WoundPlayer()
    {
        playerHealth.value--;
    }

    private void UpdateMenu()
    {
        if(gameActive)
        {
            gameActive = false; 
            pauseMenu.SetActive(true);
            activeUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else 
        {
            gameActive = true;
            pauseMenu.SetActive(false);
            activeUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
