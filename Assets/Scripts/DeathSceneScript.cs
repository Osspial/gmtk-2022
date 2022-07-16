using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathSceneScript : MonoBehaviour
{
	public Score finalscore;
	public GameObject DeathInfo;
	public TextMeshProUGUI scoreInfo;
    // Start is called before the first frame update
    void Start()
    {
		DeathInfo.SetActive(true);
        scoreInfo.text = "Score: "+finalscore.score;
    }

	 public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("DiceTest");
		finalscore.score = 0;
    }
	public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
