using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
	public GameObject Pawn, Knight, Rook, Bishop, Queen, King;

	public void SpawnPieces()
	{
		StartCoroutine(SpawnCoroutine());
	}
	IEnumerator SpawnCoroutine()
	{
		List<(int id, string name)> pieces = new List<(int id, string name)>()
		{
			(0, "Rook"),
			(1, "Knight"),
			(2, "Bishop"),
			(3, "Queen"),
			(4, "King"),
			(5, "Bishop"),
			(6, "Knight"),
			(7, "Rook"),

			(8, "Pawn"),
			(9, "Pawn"),
			(10, "Pawn"),
			(11, "Pawn"),
			(12, "Pawn"),
			(13, "Pawn"),
			(14, "Pawn"),
			(15, "Pawn"),

			(48, "Pawn"),
			(49, "Pawn"),
			(50, "Pawn"),
			(51, "Pawn"),
			(52, "Pawn"),
			(53, "Pawn"),
			(54, "Pawn"),
			(55, "Pawn"),

			(56, "Rook"),
			(57, "Knight"),
			(58, "Bishop"),
			(59, "Queen"),
			(60, "King"),
			(61, "Bishop"),
			(62, "Knight"),
			(63, "Rook"),
		};
		for (int i = 0; i < pieces.Count; i++)
		{
			var name = pieces[i].name;
			var id = pieces[i].id;
			var isWhite = id > 32 ? true : false;

			switch (name)
			{
				case "Pawn": Spawn(Pawn, id, isWhite); break;
				case "Knight": Spawn(Knight, id, isWhite); break;
				case "Rook": Spawn(Rook, id, isWhite); break;
				case "Bishop": Spawn(Bishop, id, isWhite); break;
				case "Queen": Spawn(Queen, id, isWhite); break;
				case "King": Spawn(King, id, isWhite); break;
			}

			yield return new WaitForSeconds(0.05f);
		}
	}
	private void Spawn(GameObject piece, int id, bool isWhite)
	{
		var n = Instantiate(piece);
		n.transform.SetParent(this.transform, false);
		n.GetComponent<PieceData>().IsPlayerPiece = isWhite;
		n.GetComponent<PieceData>().SpawnAnimation(id);
	}
}
