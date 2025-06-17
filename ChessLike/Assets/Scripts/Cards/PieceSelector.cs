using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelector : MonoBehaviour
{

	[SerializeField]
	private GameObject tweenMover;
	private Canvas cameraSize;

	private Image image;

	private PieceType pieceType;

	private void Start()
	{
		image = GetComponent<Image>();
		cameraSize = FindObjectsOfType<Canvas>().Where(p => p.CompareTag("ScreenUI")).FirstOrDefault();

		Vector2 scale = cameraSize.GetComponent<RectTransform>().sizeDelta;

		GetComponent<RectTransform>().sizeDelta = new Vector2(scale.x + 500, scale.y + 500);

		GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);

		image.enabled = false;
	}

	public void EnableImage(PieceType piece)
	{
		GameManager.instance.isSelectorEnable = true;
		image.enabled = true;
		pieceType = piece;
		tweenMover.SetActive(true);
		transform.SetSiblingIndex(FindFirstPiece(piece).gameObject.transform.GetSiblingIndex() - 1);
	}

	public void EnableImageAndCheckCoord(PieceType piece)
	{
		if (FindFirstPieceAndCheckCoord(piece) == null)
		{
			FindObjectOfType<SkillLoader>().warningLog.log = "이동 가능한 기물이 없습니다!";
			FindObjectOfType<SkillLoader>().warningLog.gameObject.SetActive(true);
			if (GameManager.instance.TopChange) GameManager.instance.TopChange = false;
			else if (GameManager.instance.ChaosKnight) GameManager.instance.ChaosKnight = false;
		}
		else
		{
			transform.SetSiblingIndex(FindFirstPieceAndCheckCoord(piece).gameObject.transform.GetSiblingIndex() - 1);
			GameManager.instance.isSelectorEnable = true;
			image.enabled = true;
			pieceType = piece;
			tweenMover.SetActive(true);
		}
	}

	public void EnableImage(bool isPlayerPiece)
	{
		GameManager.instance.isSelectorEnable = true;
		image.enabled = true;
		tweenMover.SetActive(true);
		transform.SetSiblingIndex(FindPiece(isPlayerPiece).gameObject.transform.GetSiblingIndex() - 1);
	}

	public void DisableImage()
	{
		GameManager.instance.isSelectorEnable = false;
		image.enabled = false;
		tweenMover.SetActive(false);
		DisablePieceMaterial(pieceType);
		GameManager.instance.SortPieceSibling();
	}
	// {
	// 	List<PieceData> pieceList = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToList();

	// 	// pieces 배열에 존재하지 않는 형태를 가진 기물을 발견할 시 기물을 리스트에서 제외
	// 	for (int i = 0; i < pieceList.Count; i++)
	// 	{
	// 		if (!IsInclude(pieceList[i], pieces))
	// 		{
	// 			pieceList.RemoveAt(i);
	// 		}
	// 	}

	// 	pieceList.Sort((a, b) =>
	// 	{
	// 		if (a.coordinate.x == b.coordinate.x)
	// 			return a.coordinate.y.CompareTo(b.coordinate.y);
	// 		else
	// 			return a.coordinate.x.CompareTo(b.coordinate.x);
	// 	});

	// 	PieceData result = null;

	// 	foreach (PieceData n in pieceList)
	// 	{
	// 		if (result == null)
	// 		{
	// 			result = n;
	// 		}
	// 		else
	// 		{
	// 			if (n.coordinate.x < result.coordinate.x)
	// 				result = n;
	// 		}
	// 		n.transform.SetAsLastSibling();
	// 	}

	// 	EnablePieceMaterial(pieces);

	// 	return result;
	// }

	private bool IsInclude(PieceData piece, PieceType[] pieces)
	{
		for (int i = 0; i < pieces.Length; i++)
		{
			if (piece.PieceType == pieces[i])
			{
				return true;
			}
		}
		return false;
	}

	private PieceData FindPiece(bool isPlayerPiece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece == isPlayerPiece).ToList();


		pieces.Sort((a, b) =>
		{
			if (a.coordinate.x == b.coordinate.x)
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});

		PieceData result = null;

		foreach (PieceData n in pieces)
		{
			if (result == null)
			{
				result = n;
			}
			else
			{
				if (n.coordinate.x < result.coordinate.x)
					result = n;
			}
			n.transform.SetAsLastSibling();
		}

		return result;
	}

	/// <summary>
	/// 선택되는 요소를 메테리얼을 활성화한 상태로 반환합니다.
	/// </summary>
	/// <param name="piece"></param>
	/// <returns></returns>
	private PieceData FindFirstPiece(PieceType piece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();

		pieces.Sort((a, b) =>
		{
			if (a.coordinate.x == b.coordinate.x)
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});

		PieceData result = null;

		foreach (PieceData n in pieces)
		{
			if (result == null)
			{
				result = n;
			}
			else
			{
				if (n.coordinate.x < result.coordinate.x)
					result = n;
			}
			n.transform.SetAsLastSibling();
		}

		EnablePieceMaterial(pieceType);

		return result;
	}

	/// <summary>
	/// 선택되는 요소 + 조건 성립 시 메테리얼을 활성화한 상태로 반환
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="expectedCoord"></param>
	/// <returns></returns>
	private PieceData FindFirstPieceAndCheckCoord(PieceType piece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();

		pieces.Sort((a, b) =>
		{
			if (a.coordinate.x == b.coordinate.x)
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});

		PieceData result = null;

		foreach (PieceData n in pieces)
		{

			if (GameManager.instance.TopChange)
			{
				Vector2Int newCoord = new Vector2Int(7 - n.coordinate.x, 7 - n.coordinate.y);

				//갈 수 없는 좌표가 있으면 오브젝트 추가하지 않기
				TableData t = FindObjectOfType<TableManager>().GetTableByCoordinate(newCoord);

				if (t.piece && t.piece.IsPlayerPiece)
				{
					SetPieceMaterial(n, false);
					continue;
				}
			}

			else if (GameManager.instance.ChaosKnight)
			{
				//1. 현재 갈 수 있는 곳이 있는지 판별
				//2. 갈 수 있는 곳이 하나라도 있으면 아래 if문 실행하기
				List<Vector2Int> list = FindObjectOfType<ChaosKnight>().moveOffsetList;

				int count = list.Count;

				for (int i = 0; i < count; i++)
				{
					if (list.Count - 1 < i) break;
					Vector2Int nc = n.coordinate + list[i];
					Debug.Log($"{list[i]} + {n.coordinate} = {nc}");
					if (nc.x < 0 || nc.x > 7 || nc.y < 0 || nc.y > 7)
					{
						list.RemoveAt(i);
						continue;
					}

					TableData t = FindObjectOfType<TableManager>().GetTableByCoordinate(nc);
					Debug.Log(t ? 1 : 0);
					if (t.piece && t.piece.IsPlayerPiece)
					{
						list.RemoveAt(i);
					}
				}
				if (list.Count <= 0)
				{
					SetPieceMaterial(n, false);
					continue;
				}
			}


			if (result == null)
			{
				result = n;
			}
			else
			{
				if (n.coordinate.x < result.coordinate.x)
					result = n;
			}
			n.transform.SetAsLastSibling();
			SetPieceMaterial(n, true);
		}

		return result;
	}

	private void DisablePieceMaterial(PieceType piece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].SetOutline(0);
		}
	}

	private void EnablePieceMaterial(PieceType piece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();

		Debug.Log("호출됨" + pieces.Count);
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].SetOutline(1);
		}
	}

	private void SetPieceMaterial(PieceData piece, bool on)
	{
		piece.SetOutline(on ? 1 : 0);
	}

	private void EnablePieceMaterial(PieceType[] piece)
	{
		List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToList();

		for (int i = 0; i < pieces.Count; i++)
		{
			if (!IsInclude(pieces[i], piece))
			{
				pieces.RemoveAt(i);
			}
		}

		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].SetOutline(1);
		}
	}
}
