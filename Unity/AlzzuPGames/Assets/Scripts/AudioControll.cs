using UnityEngine;
using System.Collections;

public class AudioControll : SingletonBehaviour<AudioControll> {

	public AudioSource source;

	public AudioClip startGameSound,backSound,selectLevelSound,infoSound,chooseOptionSound,chooseOptionSound1,countDownSound,endGame_yay,sound_correct,sound_incorrect,sound_bounce;

	public static GameObject ac;

	private float volume = 0.8f;


	void Awake() {
		DontDestroyOnLoad(this);
		source = GetComponent<AudioSource> ();
	}

	public void playBackSound(){
		source.PlayOneShot (backSound, volume);
	}
		
	public void playStartGameSound() {
		source.PlayOneShot (startGameSound, volume);
	}

	public void playSelectLevelSound() {
		source.PlayOneShot (selectLevelSound, volume);
	}

	public void playInfoSound() {
		source.PlayOneShot (infoSound, volume);
	}

	public void playOptionSound() {
		source.PlayOneShot (chooseOptionSound, volume);
	}

	public void playOptionSound1() {
		source.PlayOneShot (chooseOptionSound1, volume);
	}

	public void playCountDown() {
		source.PlayOneShot (countDownSound, volume);
	}

	public void playSoundYay() {
		source.PlayOneShot (endGame_yay, volume * 0.5f);
	}

	public void playAnsResult(bool b){
		if (b) {
			source.PlayOneShot (sound_correct, volume * 0.7f);
		} else {
			source.PlayOneShot (sound_incorrect, volume * 0.5f);
		}
	}

	public void playBounce(){
		source.PlayOneShot (sound_bounce, volume * 0.5f);
	}


}
