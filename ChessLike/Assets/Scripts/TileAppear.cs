using UnityEngine;
using DG.Tweening;

public class TileAppear : MonoBehaviour
{
	public PieceSpawner pieceSpawner;
	private void Start()
	{
		Transform tile;
		for(int i = 0; i < transform.childCount; i++)
		{
			Sequence appear = DOTween.Sequence();
			Vector3 pos = transform.GetChild(i).position;

			tile = transform.GetChild(i);

			tile.position = new Vector3(tile.position.x, tile.position.y-5f, 0);
			tile.rotation = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));

			SpriteRenderer tileSR = tile.GetComponent<SpriteRenderer>();
			tileSR.color = new Color(1, 1, 1, 0f);

			appear.Append(tile.DOMoveY(pos.y, 0.75f).SetEase(Ease.OutBack));
			appear.Join(tile.DORotate(new Vector3(0,0,0), Random.Range(0.5f, 1f)).SetEase(Ease.OutCirc));
			appear.Join(tileSR.DOColor(new Color(1, 1, 1, 1), 1.2f));
			appear.PrependInterval(i * 0.02f);
			if(i == transform.childCount - 1)
			{
				appear.OnComplete(() =>
				{
					pieceSpawner.SpawnPieces();
				});
			}
		}
	}
}
