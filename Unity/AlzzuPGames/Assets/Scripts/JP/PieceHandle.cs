using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// This script is used to give the pieces ID for checking if the pieces and slots match 
public class PieceHandle : MonoBehaviour {

	public PiecesScrollRect.Piece piece;

	public string name;
	public int id;
	List<PiecesScrollRect.Piece> pieceList; 


	public void setPiece(PiecesScrollRect.Piece p)
	{
		piece = p;
		name = piece.getPieceName();
		id = piece.getPieceID();
	}

}
