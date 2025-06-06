using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JetBrains.Annotations;

public class PieceData : MonoBehaviour
{
	public Sprite[] sprites;
	public PieceType PieceType;    //기물 종류
	public bool IsPlayerPiece;     //플레이어 기물 여부 확인

	public bool IsStatic;          //고정 상태 확인
	[HideInInspector]
	public int StaticTurn;         //얼마나 오래 고정되어 있을 것인지 확인

	public Vector2Int coordinate;    //현재 테이블에서의 좌표

	private TableManager tableManager;
	private RectTransform rectTransform;
	[HideInInspector]
	public TableData curTable;

	public int moveCount;

	public TableManager GetTableManager() { return tableManager; }

	public void SpawnAnimation(int idx)	//스폰 시 나타나는 애니메이션
	{
		moveCount = 0;
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

			GetComponent<PieceHandler>().isEnable = false;
		}
		GetComponent<Image>().sprite = sprites[n];
		
		UpdateField();

		//애니메이션 코드
		Vector3 endPos = GetTablePosition(idx);
		Vector3 startPos = new Vector3(endPos.x, endPos.y+2000, 0f);

		Sequence animation = DOTween.Sequence();

		animation.Append(rectTransform.DOAnchorPos(endPos, 1f).SetEase(Ease.OutCirc));
		animation.Join(rectTransform.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), 1f));

		rectTransform.anchoredPosition = startPos;
		rectTransform.GetComponent<Image>().color = new Color(1f,1f,1f,0f);

		animation.Play().OnComplete(() =>
		{
			GetComponent<PieceHandler>().isDragable = true;

			//현재 인덱스 값으로 좌표 저장
			coordinate = tableManager.TableList[idx].Coordinate;

			//테이블 가져오기
			curTable = tableManager.GetTableByCoordinate(coordinate);
			curTable.piece = this;
			curTable.IsPiece = true;

			GetComponent<PieceHandler>().SetTablelist();

		});
	}

	public void UpdateField()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
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

		//앙파상 발동 코드
		Vector2Int coord = new Vector2Int(coordinate.x, coordinate.y + (IsPlayerPiece ? 1 : -1));

		TableData t = tableManager.GetTableByCoordinate(coord);

		Debug.Log($"{coord}, {t == null}");

		if (t != null && t.IsPiece)
		{
			Debug.Log("ㅇㄴㅁㄹ1");
			if (t.piece.PieceType == PieceType.Pawn && !t.piece.IsPlayerPiece){
				Debug.Log("ㅇㄴㅁㄹ");
				EnPassantHandler enPassantHandler = t.piece.GetComponent<EnPassantHandler>();
				if(enPassantHandler.isEnPassant) 
					Destroy(enPassantHandler.gameObject);
			}
		}

		if (IsPlayerPiece)
		{
			if (curTable.piece != null && !curTable.piece.IsPlayerPiece) Destroy(curTable.piece.gameObject);
			
			else curTable.piece = this;
		}
		else
		{
			if (curTable.piece && curTable.piece.IsPlayerPiece) Destroy(curTable.piece.gameObject);

			else curTable.piece = this;
		}
	}

}

