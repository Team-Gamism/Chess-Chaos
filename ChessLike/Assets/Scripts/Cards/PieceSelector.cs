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
		GetComponent<RectTransform>().sizeDelta = cameraSize.GetComponent<RectTransform>().sizeDelta;

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

		return pieces[0];
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

		foreach(PieceData n in pieces)
		{
			if(result == null)
			{
				result = n;
			}
			else
			{
				if(n.coordinate.x < result.coordinate.x)
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
		for(int i = 0; i < pieces.Count; i++)
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
}
