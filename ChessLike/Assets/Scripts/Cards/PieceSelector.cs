using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelector : MonoBehaviour
{
	private Canvas cameraSize;

	private Image image;

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
		image.enabled = true;
		transform.SetSiblingIndex(FindFirstPiece(piece).transform.GetSiblingIndex() - 1);
	}

	private GameObject FindFirstPiece(PieceType piece)
	{
		PieceData[] pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToArray();

		PieceData result = new PieceData();

		foreach(PieceData n in pieces)
		{
			if(n.PieceType == piece && n.coordinate.x < result.coordinate.x)
			{
				result = n;
			}
		}
		return result.gameObject;
	}

}
