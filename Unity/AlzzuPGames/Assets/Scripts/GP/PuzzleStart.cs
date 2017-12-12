using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PuzzleStart : MonoBehaviour {	/* When the game starts */

	public RectTransform GameSpace;		// Space for playing game

	public GameObject GameSpace_2x2;	// Space for placing 2x2 puzzle pieces
	public GameObject GameSpace_3x3;	// Space for placing 3x3 puzzle pieces
	public GameObject GameSpace_4x4;	// Space for placing 4x4 puzzle pieces

	public GameObject baseImage;		// Original Image

	public List<Puzzle> puzzleList_2x2;	// List of 2x2 puzzle
	public List<Puzzle> puzzleList_3x3;	// List of 3x3 puzzle
	public List<Puzzle> puzzleList_4x4;	// List of 4x4 puzzle
	public Puzzle questionPiece;		// Question Piece

	public GameObject errorBox;			// Error Page

	public GameObject lines_2x2;		// Lines that separate 2x2 puzzle
	public GameObject lines_3x3;		// Lines that separate 3x3 puzzle
	public GameObject lines_4x4;		// Lines that separate 4x4 puzzle

	public GameObject RememberTime;		// GameObject to show the time of remembering

	public GameObject CountDownTimer;	// Count Down Timer for remembering
	public GameObject SkipButton;		// Skip button for remembering time
	private bool endTimer = false;		// To test when the timer end or not

	public GameObject sharedObject;
	public SharedVariables v;

	public static int puzzleSize;
	private int gameLevel;

	private int base_w;
	private int base_h;
	private int piece_w;
	private int piece_h;

	public static bool withTips;

	AudioControll audioControll;


	// When the script starts, get image data from "sharedObject"
	void Awake () {
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();
		sharedObject = GameObject.FindGameObjectsWithTag ("SharedObject")[0] as GameObject;
		v = sharedObject.GetComponent<SharedVariables> ();

		byte[] data = v.imageData;
		int w = v.width;
		int h = v.height;

		gameLevel = v.level;
		puzzleSize = gameLevel * gameLevel;

		withTips = v.withTips;

		if (data.Length == 0 || w == 0 || h == 0) {
			errorBox.SetActive(true);
		} else {
			loadImage (data, w, h);
		}

		shuffle (puzzleSize);

		if (gameLevel == 2) {
			GameSpace_2x2.SetActive (true);
		} else if (gameLevel == 3) {
			GameSpace_3x3.SetActive (true);
		} else if (gameLevel == 4) {
			GameSpace_4x4.SetActive (true);
		}

		StartCoroutine (StartToRemember());

	}


	// Load the image into each puzzle piece
	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D(w, h, TextureFormat.BGRA32, false);
		t.LoadImage(data);

		setPuzzlePieceSize (w, h);

		Sprite baseSprite = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width, t.height), Vector2.zero);
		baseImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (base_w, base_h);
		baseImage.GetComponent<Image>().sprite = baseSprite;

		questionPiece.puzzleImage.rectTransform.sizeDelta = new Vector2 (piece_w, piece_h);
		lines_2x2.GetComponent<RectTransform> ().sizeDelta = new Vector2 (base_w, base_h);
		lines_3x3.GetComponent<RectTransform> ().sizeDelta = new Vector2 (base_w, base_h);
		lines_4x4.GetComponent<RectTransform> ().sizeDelta = new Vector2 (base_w, base_h);


		Vector2 base_location = baseImage.GetComponent<RectTransform>().localPosition;

		if (gameLevel == 2) {
			
			puzzleList_2x2 [0].spriteImage = Sprite.Create (t as Texture2D, new Rect (0f, t.height / 2, t.width / 2, t.height / 2), Vector2.zero) as Sprite;
			puzzleList_2x2 [1].spriteImage = Sprite.Create (t as Texture2D, new Rect (t.width / 2, t.height / 2, t.width / 2, t.height / 2), Vector2.zero) as Sprite;
			puzzleList_2x2 [2].spriteImage = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width / 2, t.height / 2), Vector2.zero) as Sprite;
			puzzleList_2x2 [3].spriteImage = Sprite.Create (t as Texture2D, new Rect (t.width / 2, 0f, t.width / 2, t.height / 2), Vector2.zero) as Sprite;

			for (int i = 0; i < puzzleList_2x2.Count; i++) {
				puzzleList_2x2 [i].puzzleImage.rectTransform.sizeDelta = new Vector2 (piece_w, piece_h);
				puzzleList_2x2 [i].puzzleImage.sprite = puzzleList_2x2 [i].spriteImage;
			}

			puzzleList_2x2[0].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y+piece_h/2);
			puzzleList_2x2[1].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y+piece_h/2);
			puzzleList_2x2[2].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y-piece_h/2);
			puzzleList_2x2[3].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y-piece_h/2);

		} else if (gameLevel == 3) {
			
			puzzleList_3x3[0].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[1].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/3, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[2].spriteImage = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);

			puzzleList_3x3[3].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, t.height/3, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[4].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/3, t.height/3, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[5].spriteImage = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, t.height/3, t.width/3, t.height/3), Vector2.zero);

			puzzleList_3x3[6].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[7].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/3, 0f, t.width/3, t.height/3), Vector2.zero);
			puzzleList_3x3[8].spriteImage = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, 0f, t.width/3, t.height/3), Vector2.zero);
		
			for (int i = 0; i < puzzleList_3x3.Count; i++) {
				puzzleList_3x3 [i].puzzleImage.rectTransform.sizeDelta = new Vector2 (piece_w, piece_h);
				puzzleList_3x3 [i].puzzleImage.sprite = puzzleList_3x3 [i].spriteImage;
			}

			puzzleList_3x3[0].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w, base_location.y+piece_h);
			puzzleList_3x3[1].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x, base_location.y+piece_h);
			puzzleList_3x3[2].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w, base_location.y+piece_h);

			puzzleList_3x3[3].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w, base_location.y);
			puzzleList_3x3[4].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x, base_location.y);
			puzzleList_3x3[5].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w, base_location.y);

			puzzleList_3x3[6].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w, base_location.y-piece_h);
			puzzleList_3x3[7].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x, base_location.y-piece_h);
			puzzleList_3x3[8].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w, base_location.y-piece_h);

		} else if (gameLevel == 4) {

			puzzleList_4x4[0].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[1].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[2].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[3].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);

			puzzleList_4x4[4].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, t.height/2, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[5].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[6].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/2, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[7].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);

			puzzleList_4x4[8].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[9].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[10].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/4, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[11].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);

			puzzleList_4x4[12].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[13].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[14].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, 0f, t.width/4, t.height/4), Vector2.zero);
			puzzleList_4x4[15].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);

			for (int i = 0; i < puzzleList_4x4.Count; i++) {
				puzzleList_4x4 [i].puzzleImage.rectTransform.sizeDelta = new Vector2 (piece_w, piece_h);
				puzzleList_4x4 [i].puzzleImage.sprite = puzzleList_4x4 [i].spriteImage;
			}

			puzzleList_4x4[0].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y+(piece_h+piece_h/2));
			puzzleList_4x4[1].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y+(piece_h+piece_h/2));
			puzzleList_4x4[2].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y+(piece_h+piece_h/2));
			puzzleList_4x4[3].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y+(piece_h+piece_h/2));

			puzzleList_4x4[4].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y+piece_h/2);
			puzzleList_4x4[5].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y+piece_h/2);
			puzzleList_4x4[6].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y+piece_h/2);
			puzzleList_4x4[7].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y+piece_h/2);

			puzzleList_4x4[8].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y-piece_h/2);
			puzzleList_4x4[9].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y-piece_h/2);
			puzzleList_4x4[10].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y-piece_h/2);
			puzzleList_4x4[11].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y-piece_h/2);

			puzzleList_4x4[12].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y-(piece_h+piece_h/2));
			puzzleList_4x4[13].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y-(piece_h+piece_h/2));
			puzzleList_4x4[14].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y-(piece_h+piece_h/2));
			puzzleList_4x4[15].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y-(piece_h+piece_h/2));

		}

	}


	void setPuzzlePieceSize(int w, int h) {
		if (w > h) {
			base_w = (int) GameSpace.rect.width - 50;
			base_h = base_w * h / w;
			piece_w = base_w / gameLevel;
			piece_h = piece_w * h / w;
		} else if (w < h) {
			base_h = (int) GameSpace.rect.height - 50;
			base_w = base_h * w / h;
			piece_h = base_h / gameLevel;
			piece_w = piece_h * w / h;
		} else {
			base_w = (int) GameSpace.rect.width - 50;
			base_h = base_w;
			piece_w = base_w / gameLevel;
			piece_h = piece_w;
		}
	}


	// Determine order of puzzle for guessing
	public List<int> puzzleOrderList;
	public static List<int> list;

	void shuffle(int puzzleSize) {

		for (int i = 0; i < puzzleSize; i++) {
			puzzleOrderList.Add(i);
		}

		for (int i = 0; i < puzzleOrderList.Count; i++) {
			int temp = puzzleOrderList [i];
			int randomIndex = Random.Range(i, puzzleOrderList.Count);
			puzzleOrderList[i] = puzzleOrderList[randomIndex];
			puzzleOrderList[randomIndex] = temp;
		}

		if (withTips == true) {
			int tipsCount = (int)(puzzleSize / 4);
			for (int i = 0; i < tipsCount; i++) {
				if (puzzleSize == 4) {
					puzzleList_2x2 [puzzleOrderList [0]].puzzleButton.interactable = false;
					puzzleList_2x2 [puzzleOrderList [0]].guessMask.SetActive (false);
				} else if (puzzleSize == 9) {
					puzzleList_3x3 [puzzleOrderList [0]].puzzleButton.interactable = false;
					puzzleList_3x3 [puzzleOrderList [0]].guessMask.SetActive (false);
				} else if (puzzleSize == 16) {
					puzzleList_4x4 [puzzleOrderList [0]].puzzleButton.interactable = false;
					puzzleList_4x4 [puzzleOrderList [0]].guessMask.SetActive (false);
				}
				puzzleOrderList.RemoveAt (0);
			}
		}

		if (puzzleSize == 4) {
			questionPiece.puzzleImage.sprite = puzzleList_2x2 [puzzleOrderList [0]].spriteImage;
		} else if (puzzleSize == 9) {
			questionPiece.puzzleImage.sprite = puzzleList_3x3 [puzzleOrderList [0]].spriteImage;
		} else if (puzzleSize == 16) {
			questionPiece.puzzleImage.sprite = puzzleList_4x4 [puzzleOrderList [0]].spriteImage;
		}
		
		questionPiece.ID = puzzleOrderList[0];
		puzzleOrderList.RemoveAt (0);

		list = puzzleOrderList;
	}


	// At the beginning of each game, provide some time for player to remember the puzzle
	IEnumerator StartToRemember() {
		RememberTime.SetActive (true);

		baseImage.SetActive (true);

		yield return new WaitForSeconds(0.3f);
		CountDownTimer.SetActive (true);
		SkipButton.SetActive (true);

		for (int i = 5; i > 0; i--) {
			audioControll.playCountDown ();

			if (endTimer == true) {
				break;
			}
			CountDownTimer.GetComponent<Text>().text = i.ToString();
			yield return new WaitForSeconds(0.5f);
			if (endTimer == true) {
				break;
			}
			yield return new WaitForSeconds(0.5f);
			if (endTimer == true) {
				break;
			}
		}

		baseImage.SetActive (false);

		RememberTime.SetActive (false);

		if (gameLevel == 2) {
			lines_2x2.SetActive (true);
		} else if (gameLevel == 3) {
			lines_3x3.SetActive (true);
		} else if (gameLevel == 4) {
			lines_4x4.SetActive (true);
		}
	}


	public void skipTimer() {
		endTimer = true;
	}


}
