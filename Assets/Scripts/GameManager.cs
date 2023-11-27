using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private Slider bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        // script is applied to an empty in both scenes, called component exists only in game scene. This therefore throws up error in main menu despite working overall, possible refactoring required
        bossHealth = GameObject.Find("Boss Health Bar").GetComponent<Slider>();
        Debug.Log(bossHealth.value);
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "My Game")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {

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
        }
    }


    public void WoundBoss()
    {
        // reduces boss health slider by 1 when called
        bossHealth.value--;
    }
}
