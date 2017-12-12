using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class BoundaryControll : MonoBehaviour {

	public RectTransform piecePanel;
	public RectTransform scrollPanel;
	ScrollRect scrollRect;
	Vector2 anchoredPos;

	// Use this for initialization
	void Start () {
		scrollRect = piecePanel.GetComponent<ScrollRect> ();
		float scrollPanelLength = PiecesScrollRect.getListLength_x ();
		scrollPanel.sizeDelta = new Vector2 (scrollPanelLength, scrollPanel.sizeDelta.y);
		anchoredPos = scrollPanel.anchoredPosition;
	}


	public void ResizeBoundary () {
		scrollRect = piecePanel.GetComponent<ScrollRect> ();
		float scrollPanelLength = PiecesScrollRect.getListLength_x ();
		scrollPanel.sizeDelta = new Vector2 (scrollPanelLength, scrollPanel.sizeDelta.y);

		if (scrollPanel.anchoredPosition.x > scrollPanel.sizeDelta.x /2)
		{
			scrollPanel.anchoredPosition = new Vector2 (scrollPanel.sizeDelta.x / 2, scrollPanel.anchoredPosition.y);
		}

		float rightBoundaryLimitX = -(scrollPanel.sizeDelta.x - piecePanel.sizeDelta.x) / 2;

		if (scrollPanel.localPosition.x < rightBoundaryLimitX) //(2000 - 800)/2
		{
			scrollPanel.localPosition = new Vector2 (rightBoundaryLimitX,scrollPanel.localPosition.y);
		}

		if (PiecesScrollRect.pieceList.Count == 1)
		{
			scrollPanel.localPosition = Vector2.zero;
		}


	}


}
