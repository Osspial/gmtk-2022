using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	public GameObject MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	 public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("DiceTest");
    }
	public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
    }
	public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
