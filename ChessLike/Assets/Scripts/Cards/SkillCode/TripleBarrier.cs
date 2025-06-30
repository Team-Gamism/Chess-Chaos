using System.Collections.Generic;
using System.Linq;
using ChessEngine.Game;
using UnityEngine;

public class TripleBarrier : MonoBehaviour, ITableSkill
{
	[SerializeField]
	private TableSelector TableSelectPanel;

	[SerializeField]
	private CardData cardData;
	private NCard ncard;
    private void Start()
    {
		ncard = GetComponent<NCard>();
    }

    public void LoadSelector()
    {
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
		TableSelectPanel.gameObject.SetActive(true);
    }

    public void Execute(List<VisualChessTableTile> tiles)
    {
		for (int i = 0; i < tiles.Count; i++)
		{
			tiles[i].isTileblock = true;
			tiles[i].UpdateTileBlock();

			tiles[i].GetComponent<TileState>().SetTileState(cardData.cantMoveCnt);
		}
		ncard.DOEndAnimation();
    }
}
