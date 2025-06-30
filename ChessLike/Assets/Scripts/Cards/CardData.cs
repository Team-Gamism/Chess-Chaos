using System.Collections.Generic;
using ChessEngine;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card Data", menuName = "Card", order = 1)]
public class CardData : ScriptableObject
{
	public string Title;
	public string Description;
	public bool isOwn;
	public SkillType skillType;
	
	public List<ChessPieceType> pieces;

	public bool isZone;
	public int RequireZoneCnt;
	public int MaxZoneCnt;
	public int cantMoveCnt;
	public bool isPiece;
	public int RequirePieceCnt;
	public int MaxPieceCount;
	public CardTier cardTier;

	public Sprite cardImage;
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

public enum CardTier
{
	Common = 0,
	Uncommon,
	Rare,
	Mythic,
	Legendary
}
