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
                    SceneManager.LoadScene("My Game");
                    break;
                }
        }
    }


    public void WoundBoss()
    {
        bossHealth.value--;
    }
}
