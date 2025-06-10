using System.Linq;
using Unity.VisualScripting;
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
		GameManager.instance.isSelectorEnable = true;
		image.enabled = true;
		transform.SetSiblingIndex(FindFirstPiece(piece).gameObject.transform.GetSiblingIndex() - 1);
	}

	public void DisableImage()
	{
		GameManager.instance.isSelectorEnable = false;
		image.enabled = false;
		transform.SetSiblingIndex(FindObjectsOfType<PieceData>().Length - 1);
	}


	private PieceData FindFirstPiece(PieceType piece)
	{
		PieceData[] pieces = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece).ToArray();

		PieceData result = null;

		foreach(PieceData n in pieces)
		{
			if(n.PieceType == piece)
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
			}
		}
		return result;
	}
}
