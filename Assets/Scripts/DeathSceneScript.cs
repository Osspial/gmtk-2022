using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathSceneScript : MonoBehaviour
{
	public Score finalscore;
	public TextMeshProUGUI scoreInfo;
    public bool introDeath = false;
    // Start is called before the first frame update
    void Start()
    {
       scoreInfo.text = "Score: "+finalscore.score;
    }

	 public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        if (introDeath) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        else UnityEngine.SceneManagement.SceneManager.LoadScene("DiceTest");
		finalscore.score = 0;
    }
	public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
