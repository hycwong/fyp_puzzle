using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using SlotCreate;
using System.Collections.Generic;

public class JigsawPuzzle : MonoBehaviour {

	// The result image
	public Image baseImage;
	public Image[] pieceImage;

	public GameObject errorBox;

	public GameObject sharedObject;
	public SharedVariables v;

	public GameObject slotPrefab;
	public float base_w;
	public float base_h;
	public float piece_w;
	public float piece_h;
	public Sprite[] newSpriteArray;
	public int gameLevel;
	public GameObject levelGameObject;

	Sprite newSprite;
	Sprite newSprite0,newSprite1,newSprite2,newSprite3;
	Sprite newSprite4,newSprite5,newSprite6,newSprite7,newSprite8; 
	Sprite newSprite9,newSprite10,newSprite11,newSprite12,newSprite13,newSprite14,newSprite15;


	void Awake () {
		sharedObject = GameObject.FindGameObjectsWithTag ("SharedObject")[0] as GameObject;
		v = sharedObject.GetComponent<SharedVariables> ();
		byte[] data = v.imageData;
		int w = v.width;
		int h = v.height;
		gameLevel = v.level;
		loadImage(data,w,h);
	}


	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D(w, h, TextureFormat.BGRA32, false);
		t.LoadImage(data);


		if (gameLevel == 2) {	

			newSprite = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width, t.height), Vector2.zero);

			newSprite0 = Sprite.Create (t as Texture2D, new Rect (0f, t.height / 2, t.width / 2, t.height / 2), Vector2.zero);
			newSprite1 = Sprite.Create (t as Texture2D, new Rect (t.width / 2, t.height / 2, t.width / 2, t.height / 2), Vector2.zero);
			newSprite2 = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width / 2, t.height / 2), Vector2.zero);
			newSprite3 = Sprite.Create (t as Texture2D, new Rect (t.width / 2, 0f, t.width / 2, t.height / 2), Vector2.zero);

			List<Sprite> spriteList = new List<Sprite> ();
			spriteList.Add (newSprite0);
			spriteList.Add (newSprite1);
			spriteList.Add (newSprite2);
			spriteList.Add (newSprite3);

			newSpriteArray = spriteList.ToArray ();
		}

		if (gameLevel == 3) 
		{
			 newSprite = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width, t.height), Vector2.zero);

			 newSprite0 = Sprite.Create(t as Texture2D, new Rect(0f, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);
			 newSprite1 = Sprite.Create(t as Texture2D, new Rect(t.width/3, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);
			 newSprite2 = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, 2*t.height/3, t.width/3, t.height/3), Vector2.zero);

			 newSprite3 = Sprite.Create(t as Texture2D, new Rect(0f, t.height/3, t.width/3, t.height/3), Vector2.zero);
			 newSprite4 = Sprite.Create(t as Texture2D, new Rect(t.width/3, t.height/3, t.width/3, t.height/3), Vector2.zero);
			 newSprite5 = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, t.height/3, t.width/3, t.height/3), Vector2.zero);

			 newSprite6 = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width/3, t.height/3), Vector2.zero);
			 newSprite7 = Sprite.Create(t as Texture2D, new Rect(t.width/3, 0f, t.width/3, t.height/3), Vector2.zero);
			 newSprite8 = Sprite.Create(t as Texture2D, new Rect(2*t.width/3, 0f, t.width/3, t.height/3), Vector2.zero);

			List<Sprite> spriteList = new List<Sprite> ();

			spriteList.Add (newSprite0);
			spriteList.Add (newSprite1);
			spriteList.Add (newSprite2);
			spriteList.Add (newSprite3);
			spriteList.Add (newSprite4);
			spriteList.Add (newSprite5);
			spriteList.Add (newSprite6);
			spriteList.Add (newSprite7);
			spriteList.Add (newSprite8);

			newSpriteArray = spriteList.ToArray ();
		}

		if (gameLevel == 4) 
		{
			
			newSprite = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width, t.height), Vector2.zero);

			newSprite0 = Sprite.Create(t as Texture2D, new Rect(0f, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite1 = Sprite.Create(t as Texture2D, new Rect(t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite2 = Sprite.Create(t as Texture2D, new Rect(t.width/2, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite3 = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 3*t.height/4, t.width/4, t.height/4), Vector2.zero);

			newSprite4 = Sprite.Create(t as Texture2D, new Rect(0f, t.height/2, t.width/4, t.height/4), Vector2.zero);
			newSprite5 = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);
			newSprite6 = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/2, t.width/4, t.height/4), Vector2.zero);
			newSprite7 = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/2, t.width/4, t.height/4), Vector2.zero);

			newSprite8 = Sprite.Create(t as Texture2D, new Rect(0f, t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite9 = Sprite.Create(t as Texture2D, new Rect(t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite10 = Sprite.Create(t as Texture2D, new Rect(t.width/2, t.height/4, t.width/4, t.height/4), Vector2.zero);
			newSprite11 = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, t.height/4, t.width/4, t.height/4), Vector2.zero);

			newSprite12 = Sprite.Create(t as Texture2D, new Rect(0f, 0f, t.width/4, t.height/4), Vector2.zero);
			newSprite13 = Sprite.Create(t as Texture2D, new Rect(t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);
			newSprite14 = Sprite.Create(t as Texture2D, new Rect(t.width/2, 0f, t.width/4, t.height/4), Vector2.zero);
			newSprite15 = Sprite.Create(t as Texture2D, new Rect(3*t.width/4, 0f, t.width/4, t.height/4), Vector2.zero);

			List<Sprite> spriteList = new List<Sprite> ();
			spriteList.Add (newSprite0);
			spriteList.Add (newSprite1);
			spriteList.Add (newSprite2);
			spriteList.Add (newSprite3);
			spriteList.Add (newSprite4);
			spriteList.Add (newSprite5);
			spriteList.Add (newSprite6);
			spriteList.Add (newSprite7);
			spriteList.Add (newSprite8);
			spriteList.Add (newSprite9);
			spriteList.Add (newSprite10);
			spriteList.Add (newSprite11);
			spriteList.Add (newSprite12);
			spriteList.Add (newSprite13);
			spriteList.Add (newSprite14);
			spriteList.Add (newSprite15);

			newSpriteArray = spriteList.ToArray ();
		}


		base_w = w;
		base_h = h;
		piece_w = w/gameLevel;
		piece_h = h/gameLevel;

		if (w > h) {
			base_w = 600;
			base_h = 600 * h / w;
			piece_w = 600/gameLevel;
			piece_h = 600/gameLevel * h / w;
		} else if (w < h) {
			base_w = 600 * w / h;
			base_h = 600;
			piece_w =  600/gameLevel * w / h;
			piece_h =  600/gameLevel;
		} else {
			base_w = 600;
			base_h = 600;
			piece_w =  600/gameLevel;
			piece_h =  600/gameLevel;
		}


		baseImage.rectTransform.sizeDelta = new Vector2 (base_w, base_h);

		Vector2 base_location = baseImage.rectTransform.localPosition;


		baseImage.sprite = newSprite;

		Color c = baseImage.color;
		c.a = 0.5f;
		baseImage.color = c;


		CreateSlots (base_w, base_h, piece_w, piece_h);
	}


	void CreateSlots(float base_w,float base_h,float piece_w,float piece_h){

		RectTransform containerRectTransform = baseImage.GetComponent<RectTransform>();
		float scrollHeight = piece_h * gameLevel;
		int sliceAmount = gameLevel * gameLevel;

		GameObject[] slot;
		slot = new GameObject[sliceAmount];


		int j = 0;	
		for (int i = 0; i < sliceAmount; i++) {
			
			if (i % gameLevel == 0)
				j++;
			slot[i] = (GameObject)Instantiate (slotPrefab, baseImage.transform, false);
			slot[i].name = "slot" + i;
			RectTransform transform = slot[i].GetComponent<RectTransform> ();

			Vector3 temp = new Vector3 (0, 0, -0.01f);

			float x = -containerRectTransform.rect.width / 2 + piece_w * (i % gameLevel);
			float y = containerRectTransform.rect.height / 2 - piece_h * j;

			print ("x = " + x);
			print ("y = " + y);	

			transform.offsetMin = new Vector2(x, y);

			x = transform.offsetMin.x + piece_w;
			y = transform.offsetMin.y + piece_h;
			transform.offsetMax = new Vector2(x, y);

		}


	}
		

	/* Quit Message Confirmation */
	public void openQuitMsgBox(GameObject quitMsgBox) {
		quitMsgBox.SetActive (true);
	}

	public void Ans_No(GameObject quitMsgBox) {
		quitMsgBox.SetActive (false);
	}


}
