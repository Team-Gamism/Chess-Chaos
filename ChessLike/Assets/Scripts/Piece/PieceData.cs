using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PieceData : MonoBehaviour
{
	public Sprite[] sprites;
	public PieceType PieceType;    //기물 종류
	public bool IsPlayerPiece;	   //플레이어 기물 여부 확인

	public bool IsStatic;		   //고정 상태 확인
	[HideInInspector]
	public int StaticTurn;         //얼마나 오래 고정되어 있을 것인지 확인

	public Vector2Int coordinate;    //현재 테이블에서의 좌표

	private TableManager tableManager;
	private RectTransform rectTransform;
	[HideInInspector]
	public TableData curTable;



	public void SpawnAnimation(int idx)	//스폰 시 나타나는 애니메이션
	{
		int n = 0;
		if (IsPlayerPiece)
		{
			if (GameManager.instance.PlayerColor)
			{
				n += 2;
			}
		}
		else
		{
			if (!GameManager.instance.PlayerColor) n += 3;
			else n += 1;

			GetComponent<PieceHandler>().enabled = false;
		}
		GetComponent<Image>().sprite = sprites[n];
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();

		Vector3 endPos = GetTablePosition(idx);
		Vector3 startPos = new Vector3(endPos.x, endPos.y+2000, 0f);

		Sequence animation = DOTween.Sequence();

		animation.Append(rectTransform.DOAnchorPos(endPos, 1f).SetEase(Ease.OutCirc));
		animation.Join(rectTransform.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), 1f));

		rectTransform.anchoredPosition = startPos;
		rectTransform.GetComponent<Image>().color = new Color(1f,1f,1f,0f);

		animation.Play().OnComplete(() => {
			GetComponent<PieceHandler>().isDragable = true; });

		coordinate = tableManager.TableList[idx].Coordinate;

		curTable = tableManager.GetTableByCoordinate(coordinate);
		curTable.piece = this;
		curTable.IsPiece = true;
	}
	
	public Vector2 GetTablePosition(int idx)
	{
		Vector3 tableTrans = tableManager.TableList[idx].position;
		tableTrans = Camera.main.WorldToScreenPoint(tableTrans);

		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), tableTrans, Camera.main, out canvasPos);

		return canvasPos;
	}

	public void UpdateTableCoordinate()
	{
		curTable = tableManager.GetTableByCoordinate(coordinate);
		if (curTable.piece != null && !curTable.piece.IsPlayerPiece) Destroy(curTable.piece.gameObject);
		else
		{
			curTable.piece = this;
		}
	}
}

