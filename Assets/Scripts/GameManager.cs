using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    private Slider bossHealth;
    private Slider playerHealth;
    private GameObject pauseMenu;
    private GameObject activeUI;
    private GameObject endgameUI;
    private Scene currentScene;
    public TextMeshProUGUI endgameText;
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
            endgameUI = GameObject.Find("Endgame UI");
            endgameUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !endgameUI.activeInHierarchy)
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

        if (bossHealth.value < 1)
        {
            BossController boss = GameObject.Find("fishBoss").GetComponent<BossController>();
            boss.BossDeath();
            StartCoroutine(EndgameDelay("player"));
        }
    }

    public void WoundPlayer()
    {
        playerHealth.value--;

        if (playerHealth.value < 1)
        {
            PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.PlayerDeath();
            StartCoroutine(EndgameDelay("boss"));
        }
    }

    IEnumerator EndgameDelay(string winner)
    {
        yield return new WaitForSeconds(2);
        DisplayEndgame(winner);
        
    }

    private void DisplayEndgame(string winner)
    {
        endgameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        switch (winner)
        {
            case "player":
                {
                    endgameText.text = "You Win!";
                    break;
                }
            case "boss":
                {
                    endgameText.text = "Game Over";
                    break;
                }
        }
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
