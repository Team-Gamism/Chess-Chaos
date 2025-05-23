using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class StockfishRunner : MonoBehaviour
{
	private string stockfishPath;

	void Start()
	{
		StartCoroutine(RunStockfish());
	}

	IEnumerator RunStockfish()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
        string persistentPath = Application.persistentDataPath + "/stockfish";
        if (!File.Exists(persistentPath))
        {
            TextAsset bin = Resources.Load<TextAsset>("stockfish"); // Resources 폴더에 넣어야 함
            File.WriteAllBytes(persistentPath, bin.bytes);

            // 권한 부여
            using (AndroidJavaClass jc = new AndroidJavaClass("java.lang.Runtime"))
            {
                AndroidJavaObject runtime = jc.CallStatic<AndroidJavaObject>("getRuntime");
                runtime.Call<AndroidJavaObject>("exec", new string[] { "chmod", "755", persistentPath });
            }
        }
        stockfishPath = persistentPath;
#else
		stockfishPath = Application.dataPath + "/stockfish"; // 에디터용 경로 (예: Assets/stockfish)
#endif

		var process = new Process();
		process.StartInfo.FileName = stockfishPath;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.CreateNoWindow = true;
		process.Start();

		StreamWriter sw = process.StandardInput;
		StreamReader sr = process.StandardOutput;

		sw.WriteLine("uci");
		sw.WriteLine("isready");
		sw.WriteLine("ucinewgame");
		sw.WriteLine("position startpos");
		sw.WriteLine("go depth 5");
		sw.Flush();

		// 5초 타임아웃
		float timeout = Time.realtimeSinceStartup + 5f;

		while (Time.realtimeSinceStartup < timeout && !process.HasExited)
		{
			if (!sr.EndOfStream)
			{
				string line = sr.ReadLine();
				UnityEngine.Debug.Log("Stockfish: " + line);

				if (line.StartsWith("bestmove"))
					break;
			}
			yield return null;
		}

		sw.WriteLine("quit");
		sw.Close();
		process.WaitForExit();
		process.Close();
	}
}
