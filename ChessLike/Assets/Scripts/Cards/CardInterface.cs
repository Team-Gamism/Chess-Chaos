using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInterface : MonoBehaviour
{

}
public interface ISwapHandler
{
	void OnSwapped(int dir, GameObject swapped);
}
