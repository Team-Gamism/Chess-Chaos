using System.Collections.Generic;
using UnityEngine;

public class PieceSetter : MonoBehaviour
{
	public Queue<PieceData> pieceSelected = new Queue<PieceData>();

	[HideInInspector]
	public MultiPieceSelector mPicecSelector;

	private void OnEnable()
	{
		mPicecSelector = GetComponentInParent<MultiPieceSelector>();
		GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 230f, 0f);
	}

	private void Update()
	{
		if (pieceSelected.Count >= mPicecSelector.cardData.RequirePieceCnt && pieceSelected.Count < mPicecSelector.cardData.MaxPieceCount+1)
		{
			mPicecSelector.Donable = true;
		}
		else
		{
			mPicecSelector.Donable = false;
		}
	}
}
