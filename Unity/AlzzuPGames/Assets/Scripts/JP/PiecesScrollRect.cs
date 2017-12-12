using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// This script is used to controll the piece panel for scrolling 
public class PiecesScrollRect : MonoBehaviour {

	public RectTransform panel;
	public Image[] pieces;
	public RectTransform centre;
	public GameObject piecePrefab;
	public JigsawPuzzle puz_2;

	private float[] distances;
	public static float pieceDistance;
	private bool isDragging = false;
	private int minPieceNum;
	private static int pieceListStartPosX;
	public static List<Piece> pieceList;
	public static BoundaryControll bc;
	int num_pieces;
	public static  int piece_width;
	public static int piece_width_with_gap;


	public class Piece 
	{	
		private string name;
		private int pieceID;
		public GameObject pieceObj;
		public RectTransform rectTransform;
		public Image pieceImage;

		public Piece(int id,string name,Sprite sprite,GameObject obj)
		{
			pieceID = id;

			this.name = name;
			this.pieceObj = obj;
			this.pieceObj.name = name;
			pieceImage = pieceObj.GetComponent<Image>();
			rectTransform = pieceImage.GetComponent<RectTransform>();
			pieceImage.sprite = sprite;
			pieceObj.GetComponent<PieceHandle>().setPiece(this);
		}

		public int getPieceID()
		{
			return this.pieceID;
		}

		public string getPieceName()
		{
			return this.name;
		}

		public void setLocalPos(Vector3 vec)
		{
			rectTransform.localPosition = vec;
		}

		public void setPieceSize(float w,float h)
		{
			rectTransform.sizeDelta = new Vector2 (w, h);
		}

	}


	public static float getListLength_x()
	{
		return piece_width_with_gap * pieceList.Count;
	}
		

	void Awake() {

		GameObject scriptObj = GameObject.Find ("GameController");
		puz_2 = scriptObj.GetComponent<JigsawPuzzle>();
		num_pieces = puz_2.gameLevel * puz_2.gameLevel;
		piece_width = (int)puz_2.piece_w;
		piece_width_with_gap = piece_width + 50;
		pieces = new Image[num_pieces]; 
		int lengthx = (piece_width_with_gap * (puz_2.gameLevel * puz_2.gameLevel));
		pieceListStartPosX= -lengthx/2 + piece_width_with_gap/2;
		float posX = pieceListStartPosX;

		pieceList = new List<Piece> ();

		int[] randomIntArray = new int[num_pieces];
		for (int a = 0; a < randomIntArray.Length; a++) {
			randomIntArray [a] = a;
		}

		for (int b = 0; b < randomIntArray.Length; b++) {
			int temp = randomIntArray [b];
			int random = Random.Range (b, randomIntArray.Length);
			randomIntArray [b] = randomIntArray[random]; 
			randomIntArray [random] = temp;
		}

		for (int i = 0; i < randomIntArray.Length; i++) {
			print (randomIntArray [i]);
		}

		for (int i = 0; i < num_pieces; i++) {

			int randomIndex = randomIntArray [i];

			string name = "piece" + randomIndex;
			if (puz_2.newSpriteArray == null) {
				print ("null");
			}
			Piece p = new Piece (randomIndex, name, puz_2.newSpriteArray [randomIndex],(GameObject)Instantiate (piecePrefab, panel, false));
			p.setLocalPos (new Vector3(posX, 0, 0));
			posX += piece_width_with_gap;
			p.setPieceSize (puz_2.piece_w, puz_2.piece_h);
			pieceList.Add (p);
		}

		pieceDistance = pieceList [1].rectTransform.anchoredPosition.x - pieceList [0].rectTransform.anchoredPosition.x;

		foreach (Piece p in pieceList) {
			print (p.getPieceID ()+p.getPieceName());

		}

	}


	public static void Reposition()
	{	
		pieceListStartPosX += piece_width_with_gap/2;
		int posX = pieceListStartPosX;
		foreach (Piece b in pieceList) {
			b.setLocalPos (new Vector3 (posX, 0, 0));
			posX += piece_width_with_gap;

		}

		bc = GameObject.Find ("BoundaryControll").GetComponent<BoundaryControll> ();
		bc.ResizeBoundary ();
	}


	void Lerp(float position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 10f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);
		panel.anchoredPosition = newPosition;
	}


	public void StartDrag()
	{
		isDragging = true;
	}


	public void EndDrag()
	{
		isDragging = false;
	}


	public static void RemoveFromList(int itemID)
	{	
		foreach (Piece p in pieceList) 
		{
			if (p.getPieceID() == itemID) 
			{
				pieceList.Remove (p);
				Reposition ();
				return;
			}
		}

		Debug.LogError ("Cant find the pieces!!!!!!");
	}


}
