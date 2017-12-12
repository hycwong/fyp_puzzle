using UnityEngine;
using System.Collections;
using com.fyp.project.androidplugin;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Include Facebook namespace
using Facebook.Unity;


public class MenuController : MonoBehaviour {

	public string GameName;

	private bool photoIsSelected = false;
	private bool levelIsSelected = false;
	AudioControll audioControll;
	public GameObject sharedObject;
	public SharedVariables v;

	private int imageFrom = 999;
	const int CAMERA = 0;
	const int GALLERY = 1;

	private int level = 0;
	const int EASY = 2;
	const int NORMAL = 3;
	const int HARD = 4;

	public bool withChoice;

	public Toggle tipsToggle; 


	// FB SDK
    // Awake function from Unity's MonoBehaviour
    void Awake() {
		
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();

        if (!FB.IsInitialized) {
            // Initialise the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialised, signal an app activation App Event
            FB.ActivateApp();
            Debug.Log("The Facebook SDK Initialised");
        }
    }
		
    private void InitCallback() {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            Debug.Log("The Facebook SDK Initialised");
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialise the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    //FB SDK END


	void Start () {
		// Handle Image data returned from Plugin
		PhotoPlugin.getInstance().delegatedImage = imageHandle;

		if (withChoice == true) {

			photoIsSelected = true;
			levelIsSelected = true;

			sharedObject = GameObject.FindGameObjectsWithTag ("SharedObject")[0] as GameObject;
			v = sharedObject.GetComponent<SharedVariables> ();

			byte[] data = v.imageData;
			int w = v.width;
			int h = v.height;

			if (data.Length == 0 || w == 0 || h == 0) {
				SceneManager.LoadSceneAsync ("GP_Menu", LoadSceneMode.Single);
			} else {
				loadImage (data, w, h);
			}


			Button easyButton = GameObject.Find("Easy Button").GetComponent<Button> ();
			ColorBlock easyColors = easyButton.colors;

			Button normalButton = GameObject.Find("Normal Button").GetComponent<Button> ();
			ColorBlock normalColors = normalButton.colors;

			Button hardButton = GameObject.Find("Hard Button").GetComponent<Button> ();
			ColorBlock hardColors = hardButton.colors;

			Color32 levelSelect;
			if (GameName == "GP") {
				levelSelect = new Color32 (171, 210, 255, 255);
			} else if (GameName == "JP") {
				levelSelect = new Color32 (223, 255, 193, 255);
			} else {
				levelSelect = new Color32 (255, 255, 255, 255);
			}

			if (v.level == EASY) {
				easyColors.normalColor = levelSelect;
				easyButton.colors = easyColors;
				level = EASY;

			} else if (v.level == NORMAL) {
				normalColors.normalColor = levelSelect;
				normalButton.colors = normalColors;
				level = NORMAL;

			} else if (v.level == HARD) {
				hardColors.normalColor = levelSelect;
				hardButton.colors = hardColors;
				level = HARD;
			}


			Button camera = GameObject.Find("Camera Button").GetComponent<Button> ();
			Button gallery = GameObject.Find("Gallery Button").GetComponent<Button> ();

			Color32 Select;
			if (GameName == "GP") {
				Select = new Color32 (223, 255, 193, 255);
			} else if (GameName == "JP") {
				Select = new Color32 (255, 220, 180, 255);
			} else {
				Select = new Color32 (255, 255, 255, 255);
			}

			if (v.imageFrom.Equals("camera")) {
				imageFrom = CAMERA;
				camera.GetComponent<Image> ().color = Select;
			} else if (v.imageFrom.Equals("gallery")) {
				imageFrom = GALLERY;
				gallery.GetComponent<Image> ().color = Select;
			}

			if (GameName == "GP") {
				tipsToggle.isOn = v.withTips;
			}
		}


	}

	public void openCamera() {
		audioControll.playOptionSound1 ();
		PhotoPlugin.getInstance().openCamera();
		imageFrom = CAMERA;
		PhotoPlugin.getInstance().delegatedImage = imageHandle;
	}

	public void openGallery() {
		audioControll.playOptionSound1 ();
		PhotoPlugin.getInstance().openGallery();
		imageFrom = GALLERY;
		PhotoPlugin.getInstance().delegatedImage = imageHandle;
	}


	/* Change Photo Buttons Color */
	public void changePhotoButtonColor(int choice) {
		
		Button camera = GameObject.Find("Camera Button").GetComponent<Button> ();
		Button gallery = GameObject.Find("Gallery Button").GetComponent<Button> ();

		Color32 Select;
		if (GameName == "GP") {
			Select = new Color32 (223, 255, 193, 255);
		} else if (GameName == "JP") {
			Select = new Color32 (255, 220, 180, 255);
		} else {
			Select = new Color32 (255, 255, 255, 255);
		}
		Color32 NotSelect = new Color32 (255, 255, 255, 255);

		if (choice == 1) {			// Click + Choose from Camera
			camera.GetComponent<Image> ().color = Select;
			gallery.GetComponent<Image> ().color = NotSelect;

		} else if (choice == 2) {	// Click + Choose from Gallery
			camera.GetComponent<Image> ().color = NotSelect;
			gallery.GetComponent<Image> ().color = Select;

		} 

	}


	/* Create Image with data from Plugin */
	void imageHandle(string message, byte[] data) {
		
		JSONArray json_array = (JSONArray)JSON.Parse(message);
		JSONNode json_node = json_array[0];
		int w = json_node["width"].AsInt;
		int h = json_node["height"].AsInt;

		v.imageData = data;
		v.width = w;
		v.height = h;

		if (data.Length != 0 && w != 0 && h != 0) {
			photoIsSelected = true;
			loadImage (data, w, h);

			if (imageFrom == CAMERA) {
				changePhotoButtonColor (1);
				v.imageFrom = "camera";
			} else if(imageFrom == GALLERY) {
				changePhotoButtonColor (2);
				v.imageFrom = "gallery";
			}
		}

	}


	public Image previewImage;
	public RectTransform previewSpace;
	public GameObject Mask;

	/* Load image into previewImage */
	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D (w, h, TextureFormat.BGRA32, false);
		t.LoadImage (data);

		Sprite newSprite = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width, t.height), Vector2.zero);

		int base_w = w;
		int base_h = h;

		if (w > h) {
			base_w = (int)(previewSpace.rect.width * 0.55);
			base_h = base_w * h / w;
		} else if (w < h) {
			base_h = (int)(previewSpace.rect.height * 0.7);
			base_w = base_h * w / h;
		} else {
			base_w = (int)(previewSpace.rect.width * 0.55);
			base_h = base_w;
		}

		previewImage.rectTransform.sizeDelta = new Vector2 (base_w, base_h);
		previewImage.sprite = newSprite;
		Mask.SetActive (false);

	}


	/* Select Level */
	public void selectLevel(int lv) {

		audioControll.playSelectLevelSound();
		levelIsSelected = true;
		level = lv;
		v.level = lv;

		// Change Level Buttons Color
		Button easyButton = GameObject.Find("Easy Button").GetComponent<Button> ();
		ColorBlock easyColors = easyButton.colors;

		Button normalButton = GameObject.Find("Normal Button").GetComponent<Button> ();
		ColorBlock normalColors = normalButton.colors;

		Button hardButton = GameObject.Find("Hard Button").GetComponent<Button> ();
		ColorBlock hardColors = hardButton.colors;

		Color32 Select;
		if (GameName == "GP") {
			Select = new Color32 (171, 210, 255, 255);
		} else if (GameName == "JP") {
			Select = new Color32 (223, 255, 193, 255);
		} else {
			Select = new Color32 (255, 255, 255, 255);
		}
		Color32 NotSelect = new Color32 (255, 255, 255, 255);

		if (lv == EASY) {
			easyColors.normalColor = Select;
			easyButton.colors = easyColors;

			normalColors.normalColor = NotSelect;
			normalButton.colors = normalColors;

			hardColors.normalColor = NotSelect;
			hardButton.colors = hardColors;

		} else if (lv == NORMAL) {
			easyColors.normalColor = NotSelect;
			easyButton.colors = easyColors;

			normalColors.normalColor = Select;
			normalButton.colors = normalColors;

			hardColors.normalColor = NotSelect;
			hardButton.colors = hardColors;

		} else if (lv == HARD) {
			easyColors.normalColor = NotSelect;
			easyButton.colors = easyColors;

			normalColors.normalColor = NotSelect;
			normalButton.colors = normalColors;

			hardColors.normalColor = Select;
			hardButton.colors = hardColors;
		}

	}


	public GameObject PhotoNotSelect;
	public GameObject LevelNotSelect;
	public GameObject NothingSelect;

	/* After pressing "Start" button */
	public void LoadScene() {
		if (photoIsSelected == true && levelIsSelected == true) {
			audioControll.playStartGameSound();
			SceneManager.LoadSceneAsync(GameName, LoadSceneMode.Single);

		} else if (photoIsSelected == false && levelIsSelected == false) {
			openPanel(NothingSelect);

		} else if (photoIsSelected == false) {
			openPanel(PhotoNotSelect);

		} else if (levelIsSelected == false) {
			openPanel(LevelNotSelect);
		}

	}


	// Close Error Panel & Instruction Panel
	public void closePanel(GameObject panel) {
		audioControll.playBackSound ();
		panel.SetActive (false);
	}
	// Open Error Panel & Instruction Panel
	public void openPanel(GameObject panel) {
		audioControll.playInfoSound ();
		panel.SetActive (true);
	}


	// Refer to "back_click" image
	public Texture2D back_click;	

	// "Back" Button Click -> Change image and text color
	public void backButtonDown_Image(RawImage buttonImage) {
		buttonImage.texture = back_click;
	}
	public void backButtonDown_Text(Text buttonText) {
		buttonText.color = new Color32 (125, 220, 220, 255);
	}


	// WithTips Toggle
	public void WithTipsToggle(bool withTips) {
		v.withTips = withTips;
	}


}
