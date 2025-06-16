using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MultiPieceSelector : MonoBehaviour
{
	private Canvas cameraSize;
	public PieceSetter pieceSetter;

	[SerializeField]
	private GameObject log;

	[SerializeField]
	private Button DoneBtn;

	private Image image;

	public bool Donable = false;

	[HideInInspector]
	public CardData cardData;
	//private PieceType pieceType;


	private void OnEnable()
	{
		image = GetComponent<Image>();
		cameraSize = FindObjectsOfType<Canvas>().Where(p => p.CompareTag("ScreenUI")).FirstOrDefault();

		Vector2 scale = cameraSize.GetComponent<RectTransform>().sizeDelta;

		GetComponent<RectTransform>().sizeDelta = new Vector2(scale.x + 500, scale.y + 500);

		GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);

		log.SetActive(true);
		DoneBtn.gameObject.SetActive(true);
		DoneBtn.onClick.AddListener(() => ExecutePieceMove());
	}

	private void Update()
	{
		if (!Donable)
		{
			DoneBtn.interactable = false;
		}
		else
		{
			DoneBtn.interactable = true;
		}
	}

	// public void EnableImage(PieceType piece)
	// {
	// 	GameManager.instance.isSelectorEnable = true;
	// 	image.enabled = true;
	// 	pieceType = piece;
	// 	log.SetActive(true);
	// 	transform.SetSiblingIndex(FindFirstPiece(piece).gameObject.transform.GetSiblingIndex() - 1);
	// }
	public void EnableImage(params PieceType[] pieces)
	{
		GameManager.instance.isSelectorEnable = true;
		transform.SetSiblingIndex(FindAndGetFirstPiece(pieces).gameObject.transform.GetSiblingIndex() - 1);
	}
	private void DisableImage()
	{
		GameManager.instance.isSelectorEnable = false;
		log.SetActive(false);
		DoneBtn.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
	// public void EnableImage(bool isPlayerPiece)
	// {
	// 	GameManager.instance.isSelectorEnable = true;
	// 	image.enabled = true;
	// 	transform.SetSiblingIndex(FindPiece(isPlayerPiece).gameObject.transform.GetSiblingIndex() - 1);
	// }

	// public void DisableImage()
	// {
	// 	GameManager.instance.isSelectorEnable = false;
	// 	image.enabled = false;
	// 	DisablePieceMaterial(pieceType);
	// 	GameManager.instance.SortPieceSibling();
	// }

	private PieceData FindAndGetFirstPiece(PieceType[] pieces)
	{
		List<PieceData> pieceList = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToList();

		// pieces 배열에 존재하지 않는 형태를 가진 기물을 발견할 시 기물을 리스트에서 제외
		for (int i = 0; i < pieceList.Count; i++)
		{
			if (!IsInclude(pieceList[i], pieces))
			{
				pieceList.RemoveAt(i);
			}
		}

		pieceList.Sort((a, b) =>
		{
			if (a.coordinate.x == b.coordinate.x)
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});

		PieceData result = null;

		foreach (PieceData n in pieceList)
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
	// private PieceData FindPiece(bool isPlayerPiece)
	// {
	// 	List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece == isPlayerPiece).ToList();


	// 	pieces.Sort((a, b) =>
	// 	{
	// 		if (a.coordinate.x == b.coordinate.x)
	// 			return a.coordinate.y.CompareTo(b.coordinate.y);
	// 		else
	// 			return a.coordinate.x.CompareTo(b.coordinate.x);
	// 	});

	// 	PieceData result = null;

	// 	foreach (PieceData n in pieces)
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

	// 	return result;
	// }

	// private PieceData FindFirstPiece(PieceType piece)
	// {
	// 	List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();


	// 	pieces.Sort((a, b) =>
	// 	{
	// 		if (a.coordinate.x == b.coordinate.x)
	// 			return a.coordinate.y.CompareTo(b.coordinate.y);
	// 		else
	// 			return a.coordinate.x.CompareTo(b.coordinate.x);
	// 	});

	// 	PieceData result = null;

	// 	foreach (PieceData n in pieces)
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

	// 	EnablePieceMaterial(pieceType);

	// 	return result;
	// }

	// private void DisablePieceMaterial(PieceType piece)
	// {
	// 	List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();
	// 	for (int i = 0; i < pieces.Count; i++)
	// 	{
	// 		pieces[i].SetOutline(0);
	// 	}
	// }

	// private void EnablePieceMaterial(PieceType piece)
	// {
	// 	List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == piece).ToList();

	// 	Debug.Log("호출됨" + pieces.Count);
	// 	for (int i = 0; i < pieces.Count; i++)
	// 	{
	// 		pieces[i].SetOutline(1);
	// 	}
	// }

	// private void EnablePieceMaterial(PieceType[] piece)
	// {
	// 	List<PieceData> pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToList();

	// 	for (int i = 0; i < pieces.Count; i++)
	// 	{
	// 		if (!IsInclude(pieces[i], piece))
	// 		{
	// 			pieces.RemoveAt(i);
	// 		}
	// 	}

	// 	Debug.Log("호출됨" + pieces.Count);
	// 	for (int i = 0; i < pieces.Count; i++)
	// 	{
	// 		pieces[i].SetOutline(1);
	// 	}
	// }

	private void ExecutePieceMove()
	{
		List<PieceData> pieces = pieceSetter.pieceSelected.ToList();
		for (int i = 0; i < pieces.Count; i++)
		{
			DisableMaterial(pieces[i]);
		}

		//추후 조건 더 추가하기
		if (GameManager.instance.WeirdCasting || 
			GameManager.instance.GodsOne)
		{
			FindObjectOfType<SkillLoader>().ExecuteSkill();
		}
		DisableImage();
	}

	/// <summary>
	/// 기묘한 캐슬링에 사용되는 함수.
	/// </summary>
	/// <returns>큐에 존재하는 하나의 요소를 반환한다.</returns>
	public PieceData ReturnPiece()
	{
		return pieceSetter.pieceSelected.First();
	}

	/// <summary>
	/// 큐에 존재하는 요소를 리스트로 반환한다.
	/// </summary>
	public void ReturnPieces()
	{

	}
	public void QueueManage(PieceData piece)
	{
		Queue<PieceData> queue = pieceSetter.pieceSelected;
		if (queue.Count > 0)
		{
			if (queue.Contains(piece))
			{
				RemoveFromQueue(piece);
				queue = pieceSetter.pieceSelected;
				Debug.Log("동일한 요소가 발견되어 해당 요소를 삭제함" + $" {queue.Count}");
			}
			else if (queue.Count >= cardData.MaxPieceCount)
			{
				DisableMaterial(queue.First());
				queue.Dequeue();
				EnableMateiral(piece);
				queue.Enqueue(piece);
				Debug.Log("큐에 요소를 지우고 새 요소 삽입함" + $" {queue.Count}");
			}
			else
			{
				EnableMateiral(piece);
				queue.Enqueue(piece);
				Debug.Log("큐에 새 요소 삽입함" + $" {queue.Count}");
			}
		}
		else
		{
			EnableMateiral(piece);
			queue.Enqueue(piece);
			Debug.Log("큐에 새 요소 삽입함" + $" {queue.Count}");
		}

		pieceSetter.pieceSelected = queue;
	}

	private void EnableMateiral(PieceData piece)
	{
		piece.GetComponent<Image>().material.SetFloat("_OutlineThick", 1f);
	}
	private void DisableMaterial(PieceData piece)
	{
		piece.GetComponent<Image>().material.SetFloat("_OutlineThick", 0f);
	}

	public void RemoveFromQueue(PieceData target)
	{
		Queue<PieceData> newQueue = new Queue<PieceData>();
		foreach (PieceData item in pieceSetter.pieceSelected)
		{
			if (!item.Equals(target))
			{
				newQueue.Enqueue(item);
				Debug.Log($"요소 삽입함!{item.gameObject.name}");
			}
			else
			{
				DisableMaterial(item);
				Debug.Log($"요소 제거함!{item.gameObject.name}");
			}
		}
		pieceSetter.pieceSelected = newQueue;
	}

}
