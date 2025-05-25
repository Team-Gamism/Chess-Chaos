using UnityEngine;

public class StockfishBridge : MonoBehaviour
{
	private AndroidJavaObject runner;

	void Start()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        runner = new AndroidJavaObject("com.yourcompany.StockfishRunner");
        bool success = runner.Call<bool>("start", activity);
        Debug.Log("Stockfish started: " + success);
#endif
	}

	public void SendCommand(string command)
	{
		runner?.Call("sendCommand", command);
	}

	public string ReadOutput()
	{
		return runner?.Call<string>("readOutput");
	}

	void OnDestroy()
	{
		runner?.Call("stop");
	}
}
