using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
	public Score finalscore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void PlayButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("DiceTest");
		finalscore.score = 0;
    }
	public void ChosenDeath()
    {
        // Quit Game
		finalscore.score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroDeath");
    }
	
	
}
