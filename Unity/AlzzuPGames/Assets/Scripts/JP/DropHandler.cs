using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// This script is used to control the piece after the user release the piece 
public class DropHandler : MonoBehaviour,IDropHandler {

	GameObject gameController;
	PieceHandle ph ;
	ScoreControll sc ;
	AudioControll audioControll;


	void Awake(){
		gameController = GameObject.Find ("GameController");
		sc = gameController.GetComponent<ScoreControll> ();
		audioControll = GameObject.Find ("AudioController").GetComponent<AudioControll> ();
	} 


	public GameObject item{
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			
			}
			return null;
		}
	}


	IEnumerator Delay(float time){
		yield return new WaitForSeconds (time);
	}


	public void OnDrop (PointerEventData eventData){

		if (DragHandler.itemBeingDragged != null) {
			 ph = DragHandler.itemBeingDragged.GetComponent<PieceHandle> ();
		}

		if (ph == null) {
			return;
		}
		string itemName = ph.name;
		int itemID =  ph.id;
		string slotName = gameObject.name;

		bool isMatch = false;

		if (itemName [itemName.Length - 1] == slotName [slotName.Length - 1]) {
			audioControll.playAnsResult (true);

			isMatch = true;
		} else {
			audioControll.playBounce();

			sc.WrongGuess ();
			return;
		}

		if (!item && isMatch) {
			PiecesScrollRect.RemoveFromList(itemID);

			DragHandler.itemBeingDragged.transform.SetParent (transform);
			DragHandler.itemBeingDragged.transform.localPosition = new Vector3 (0, 0, 0);
			Delay (0.1f);
			sc.CorrectGuess ();
			sc.CheckGameEnd ();
		}

	}


}
