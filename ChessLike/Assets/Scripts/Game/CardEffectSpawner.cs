using UnityEngine;

public class CardEffectSpawner : MonoBehaviour
{
	[SerializeField] private GameObject effector;
	public void SpawnEffector(CardData data)
	{
		var obj = Instantiate(effector, transform);
		obj.GetComponent<CardEffector>().CardAppear(data);
	}
}
