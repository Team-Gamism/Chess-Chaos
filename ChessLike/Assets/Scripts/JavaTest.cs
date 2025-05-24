using UnityEngine;

public class StockfishAndroid : MonoBehaviour
{
	private AndroidJavaObject stockfish;

	void Start()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                stockfish = new AndroidJavaObject("com.example.stockfishlibrary", activity);
            }

            stockfish.Call("initEngine");
            stockfish.Call("sendCommand", "uci");
            string result = stockfish.Call<string>("readResponse");
            Debug.Log("Stockfish: " + result);

        }
        catch (System.Exception e)
        {
            Debug.LogError("Stockfish 초기화 실패: " + e.Message);
        }
#endif
	}
}

