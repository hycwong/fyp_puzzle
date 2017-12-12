using UnityEngine;
using System.Collections;
using com.fyp.project.androidplugin;
using SimpleJSON;
using UnityEngine.UI;
using Prototype.NetworkLobby;


public class PhotoController : MonoBehaviour {

	public LobbyMainMenu MainPanel;

	private int imageFrom = 999;
	const int CAMERA = 0;
	const int GALLERY = 1;
	const int NONE = 2;

	public bool saveImage;

	public Button CameraButton;
	public Button GalleryButton;

	public Button CreateRoomButton;
	public Button ConfirmImageButton;

	public Image previewImage;
	public GameObject previewMask;

	public GameObject previewImage_Button;
	public GameObject provideImage_Text;

	public GameObject SharedObject2;
	public SharedVariable2 v;

	public byte[] currentData = null;
	public int currentWidth = 0;
	public int currentHeight = 0;


	void Start () {
		SharedObject2 = GameObject.Find ("SharedObject2");
		v = SharedObject2.GetComponent<SharedVariable2> ();

		CreateRoomButton.interactable = false;

		PhotoPlugin.getInstance().delegatedImage = imageHandle;
	}


	void Update () {
		if (SharedObject2 == null) {
			SharedObject2 = GameObject.FindGameObjectsWithTag ("SharedObject2") [0] as GameObject;
			v = SharedObject2.GetComponent<SharedVariable2> ();
		}

		if (currentData != null && currentWidth != 0 && currentHeight != 0) {
			if (saveImage == true) {
				if (v.imageData == null || v.width == 0 || v.height == 0) {
					v.imageData = currentData;
					v.width = currentWidth;
					v.height = currentHeight;
					v.imageSize = currentData.Length;
				} else if (currentData != v.imageData) {
					v.imageData = currentData;
					v.width = currentWidth;
					v.height = currentHeight;
					v.imageSize = currentData.Length;
				}
			}
		}

		if (v.imageData == null || v.width == 0 || v.height == 0)
			CreateRoomButton.interactable = false;
		else
			CreateRoomButton.interactable = true;
	}


	public void openCamera() {
		PhotoPlugin.getInstance().openCamera();
		imageFrom = CAMERA;
		PhotoPlugin.getInstance().delegatedImage = imageHandle;
	}

	public void openGallery() {
		PhotoPlugin.getInstance().openGallery();
		imageFrom = GALLERY;
		PhotoPlugin.getInstance().delegatedImage = imageHandle;
		saveImage = false;
	}


	void imageHandle(string message, byte[] data) {

		JSONArray json_array = (JSONArray)JSON.Parse(message);
		JSONNode json_node = json_array[0];
		int w = json_node["width"].AsInt;
		int h = json_node["height"].AsInt;

		currentData = data;
		currentWidth = w;
		currentHeight = h;

		if (data.Length != 0 && w != 0 && h != 0) {
			loadImage (data, w, h);

			if (imageFrom == CAMERA) {
				changePhotoButtonColor (CAMERA);
			} else if(imageFrom == GALLERY) {
				changePhotoButtonColor (GALLERY);
			}

			ConfirmImageButton.interactable = true;
		}

	}


	void loadImage(byte[] data, int w, int h) {

		Texture2D t = new Texture2D (w, h, TextureFormat.BGRA32, false);
		t.LoadImage (data);

		Sprite newSprite = Sprite.Create (t as Texture2D, new Rect (0f, 0f, t.width, t.height), Vector2.zero);

		int base_w = w;
		int base_h = h;

		if (w > h) {
			base_w = 580;
			base_h = base_w * h / w;
		} else if (w < h) {
			base_h = 580;
			base_w = base_h * w / h;
		} else {
			base_w = 580;
			base_h = base_w;
		}

		previewImage.rectTransform.sizeDelta = new Vector2 (base_w, base_h);
		previewImage.sprite = newSprite;
		previewMask.SetActive (false);


		if (w > h) {
			base_w = 200;
			base_h = base_w * h / w;
		} else if (w < h) {
			base_h = 200;
			base_w = base_h * w / h;
		} else {
			base_w = 200;
			base_h = base_w;
		}

		previewImage_Button.SetActive (true);
		provideImage_Text.SetActive (false);
		previewImage_Button.GetComponent<Image>().rectTransform.sizeDelta = new Vector2 (base_w, base_h);
		previewImage_Button.GetComponent<Image>().sprite = newSprite;

	}


	void changePhotoButtonColor(int choice) {

		Color32 Select = new Color32 (240, 200, 255, 255);
		Color32 NotSelect = new Color32 (255, 255, 255, 255);

		if (choice == CAMERA) {
			CameraButton.GetComponent<Image> ().color = Select;
			GalleryButton.GetComponent<Image> ().color = NotSelect;
		} else if (choice == GALLERY) {
			CameraButton.GetComponent<Image> ().color = NotSelect;
			GalleryButton.GetComponent<Image> ().color = Select;
		} else if (choice == NONE) {
			CameraButton.GetComponent<Image> ().color = NotSelect;
			GalleryButton.GetComponent<Image> ().color = NotSelect;
		}

	}


	public void ConfirmImage(GameObject panel) {
		if (currentData != null && currentWidth != 0 && currentHeight != 0) {
			v.imageData = currentData;
			v.width = currentWidth;
			v.height = currentHeight;
			v.imageSize = currentData.Length;
		}

		changePhotoButtonColor (NONE);
		panel.SetActive (false);

		if (v.imageData != null && v.width != 0 && v.height != 0)
			CreateRoomButton.interactable = true;

		saveImage = true;
	}


	public void CancelImage(GameObject panel) {
		changePhotoButtonColor (NONE);
		panel.SetActive (false);

		currentData = v.imageData;
		currentWidth = v.width;
		currentHeight = v.height;

		if (v.imageData == null || v.width == 0 || v.height == 0) {
			previewImage.rectTransform.sizeDelta = new Vector2 (580, 580);
			previewImage.sprite = null;
			previewMask.SetActive (true);

			previewImage_Button.SetActive (false);
			provideImage_Text.SetActive (true);
			previewImage_Button.GetComponent<Image> ().rectTransform.sizeDelta = new Vector2 (200, 200);
			previewImage_Button.GetComponent<Image> ().sprite = null;

			changePhotoButtonColor (NONE);
			ConfirmImageButton.interactable = false;

		} else if (v.imageData != null && v.width != 0 && v.height != 0) {
			loadImage (v.imageData, v.width, v.height);
		}
	}


	public void openPanel(GameObject panel) {
		panel.SetActive (true);
	}

	public void closePanel(GameObject panel) {
		panel.SetActive (false);
	}



}
