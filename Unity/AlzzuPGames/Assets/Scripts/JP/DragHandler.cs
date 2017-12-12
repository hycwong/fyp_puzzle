using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This script is used to control the piece when user dragging the piece before release
public class DragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler	{

	public static GameObject itemBeingDragged;
	Vector2 startPosition;
	Vector2 releasePosition;
	Transform startParent;
	public ScrollRect piecePanel;


	// Use this for initialization
	public void Awake()
	{
		piecePanel = gameObject.GetComponentInParent<ScrollRect> ();
		if (piecePanel == null) {
			return;
		}
	}


	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.localPosition;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}


	public void OnDrag (PointerEventData eventData)
	{
		Vector3 screenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 100);
		transform.position = Camera.main.ScreenToWorldPoint (screenPoint);
	}


	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		releasePosition = transform.localPosition;

		if (transform.parent == startParent) {
				transform.localPosition = startPosition;
		}

	}

 	
}
