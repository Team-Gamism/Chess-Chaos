using System.Collections.Generic;
using System.Linq;
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
	public void EnableImage(params PieceType[] pieces)
	{
		GameManager.instance.isSelectorEnable = true;
		image.enabled = true;
		tweenMover.SetActive(true);
		transform.SetSiblingIndex(FindAndGetFirstPiece(pieces).gameObject.transform.GetSiblingIndex() - 1);
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

		EnablePieceMaterial(pieces);

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

		Debug.Log("호출됨" + pieces.Count);
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].SetOutline(1);
		}
	}
}
