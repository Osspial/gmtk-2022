using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathSceneScript : MonoBehaviour
{
	static int score;
	public GameObject DeathInfo;
	public TextMeshProUGUI scoreInfo;
    // Start is called before the first frame update
    void Start()
    {
		score = 42;
        DeathSceneButton();
    }

    // Update is called once per frame
    void Update()
    {
        scoreInfo.text = "Score: "+score;
    }
	 public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("DiceTest");
    }
	public void DeathSceneButton()
    {
        // Show Main Menu
        DeathInfo.SetActive(true);
    }
	public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
