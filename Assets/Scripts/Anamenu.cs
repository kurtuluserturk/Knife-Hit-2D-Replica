using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Anamenu : MonoBehaviour
{ 
    public void PlayGame()
    {
        SceneManager.LoadScene("oyun");
    }
    public void AboutPanel()
    {
        SceneManager.LoadScene("about");
    }
    public void QuitGame()
    {               
        Application.Quit();
    }    
}
