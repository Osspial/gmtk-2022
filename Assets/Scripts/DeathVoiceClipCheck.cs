using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVoiceClipCheck : MonoBehaviour
{
	private AudioSource source;
	public Score finalscore;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
		PlayFunnyLine();
    }

    // Update is called once per frame
    void Update()
    {	 
    }
	void PlayFunnyLine() {
		if (finalscore.score > 0) {
			source.Play();
		}
	}
}
