using System.Collections.Generic;
using ChessEngine.Game;
using UnityEngine;

public class AvoidLanding : MonoBehaviour, ITableSkill

{
	[SerializeField]
	private TableSelector TableSelectPanel;

	private CardData cardData;

	public void Execute(List<VisualChessTableTile> tiles)
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			tiles[i].isTileblock = true;
			tiles[i].UpdateTileBlock();

			tiles[i].GetComponent<TileState>().SetTileState(cardData.cantMoveCnt);
		}

		GetComponent<NCard>().DOEndAnimation();
	}

    public void LoadSelector()
    {
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
		TableSelectPanel.gameObject.SetActive(true);
    }
}
