using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

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
}
