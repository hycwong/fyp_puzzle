using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PuzzleReady : MonoBehaviour {

	public GameObject baseImage;
	public GameObject GameSpace;
	public GameObject lines;

	public List<Puzzle> puzzleList;
	public RectTransform questionPiece;

	public static int puzzleSize;
	private int gameLevel;

	private int base_w;
	private int base_h;
	private int piece_w;
	private int piece_h;

	public GameObject SharedObject2;
	public SharedVariable2 v;


	void Awake () {
		gameLevel = 4;
		puzzleSize = 16;

		SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2")[0] as GameObject;
		v = SharedObject2.GetComponent<SharedVariable2> ();

		loadImage (v.imageData , v.width, v.height);

		shuffle (puzzleSize);
	}


	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D (w, h, TextureFormat.BGRA32, false);
		t.LoadImage (data);

		setPuzzlePieceSize (w, h);

		Sprite baseSprite = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width, t.height), Vector2.zero);
		baseImage.GetComponent<RectTransform>().sizeDelta = new Vector2 (base_w, base_h);
		baseImage.GetComponent<Image>().sprite = baseSprite;


		questionPiece.sizeDelta = new Vector2 (piece_w, piece_h);
		lines.GetComponent<RectTransform> ().sizeDelta = new Vector2 (base_w, base_h);


		Vector2 base_location = baseImage.GetComponent<RectTransform>().localPosition;

		puzzleList[0].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[1].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[2].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[3].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);

		puzzleList[4].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, t.height/2, t.width/4, t.height/4), Vector2.zero);
		puzzleList[5].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);
		puzzleList[6].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/2, t.width/4, t.height/4), Vector2.zero);
		puzzleList[7].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);

		puzzleList[8].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[9].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[10].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/4, t.width/4, t.height/4), Vector2.zero);
		puzzleList[11].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);

		puzzleList[12].spriteImage = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width/4, t.height/4), Vector2.zero);
		puzzleList[13].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);
		puzzleList[14].spriteImage = Sprite.Create(t as Texture2D, new Rect(t.width/2, 0f, t.width/4, t.height/4), Vector2.zero);
		puzzleList[15].spriteImage = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);

		for (int i = 0; i < puzzleList.Count; i++) {
			puzzleList [i].puzzleImage.rectTransform.sizeDelta = new Vector2 (piece_w, piece_h);
			puzzleList [i].puzzleImage.sprite = puzzleList [i].spriteImage;
		}

		puzzleList[0].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y+(piece_h+piece_h/2));
		puzzleList[1].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y+(piece_h+piece_h/2));
		puzzleList[2].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y+(piece_h+piece_h/2));
		puzzleList[3].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y+(piece_h+piece_h/2));

		puzzleList[4].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y+piece_h/2);
		puzzleList[5].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y+piece_h/2);
		puzzleList[6].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y+piece_h/2);
		puzzleList[7].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y+piece_h/2);

		puzzleList[8].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y-piece_h/2);
		puzzleList[9].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y-piece_h/2);
		puzzleList[10].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y-piece_h/2);
		puzzleList[11].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y-piece_h/2);

		puzzleList[12].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-(piece_w+piece_w/2), base_location.y-(piece_h+piece_h/2));
		puzzleList[13].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x-piece_w/2, base_location.y-(piece_h+piece_h/2));
		puzzleList[14].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+piece_w/2, base_location.y-(piece_h+piece_h/2));
		puzzleList[15].puzzleImage.rectTransform.localPosition = new Vector2 (base_location.x+(piece_w+piece_w/2), base_location.y-(piece_h+piece_h/2));


	}

	void setPuzzlePieceSize(int w, int h) {
		if (w > h) {
			base_w = (int) GameSpace.GetComponent<RectTransform>().rect.width - 50;
			base_h = base_w * h / w;
			piece_w = base_w / gameLevel;
			piece_h = piece_w * h / w;
		} else if (w < h) {
			base_h = (int) GameSpace.GetComponent<RectTransform>().rect.height - 50;
			base_w = base_h * w / h;
			piece_h = base_h / gameLevel;
			piece_w = piece_h * w / h;
		} else {
			if (GameSpace.GetComponent<RectTransform> ().rect.width >= GameSpace.GetComponent<RectTransform> ().rect.height) {
				base_w = (int)GameSpace.GetComponent<RectTransform> ().rect.width - 70;
				base_h = base_w;
				piece_w = base_w / gameLevel;
				piece_h = piece_w;
			} else {
				base_h = (int) GameSpace.GetComponent<RectTransform>().rect.height - 70;
				base_w = base_h;
				piece_h = base_h / gameLevel;
				piece_w = piece_h;
			}
		}
	}


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

		list = puzzleOrderList;
	}


}
