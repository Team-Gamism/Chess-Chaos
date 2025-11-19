using UnityEngine;

public class CardEffectSpawner : MonoBehaviour
{
	[SerializeField] private CardEffector effector;

	public void SpawnEffector(CardData data, SpawnType spawnType = SpawnType.UseCard)
	{
		effector.CardAppear(data, spawnType);
	}
}

public enum SpawnType
{
	UseCard,
	BreakCard
}