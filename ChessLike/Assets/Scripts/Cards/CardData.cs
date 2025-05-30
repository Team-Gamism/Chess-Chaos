using UnityEngine;

[CreateAssetMenu(fileName ="New Card Data", menuName = "Card", order = 1)]
public class CardData : ScriptableObject
{
	public string Title;
	public string Description;
	public bool isOwn;
	
	public PieceType pieces;
}

public enum PieceType
{
	Pawn = 0,
	Bishop,
	Rook,
	Knight,
	Queen,
	King
}
