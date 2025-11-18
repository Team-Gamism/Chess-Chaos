using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class NNUEFileManager : MonoBehaviour
{

    public void AddNNUEFile(string nnueFileName)
    {
        StartCoroutine(CopyNNUEFileToPersistentPath(nnueFileName));
    }

    IEnumerator CopyNNUEFileToPersistentPath(string fileName)
    {
        string persistentPath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(persistentPath))
        {
            Debug.Log("NNUE 파일이 이미 존재합니다: " + persistentPath);
            yield break;
        }

        string streamingPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string sourcePath = streamingPath;

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get(sourcePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("파일 복사 실패: " + www.error);
            yield break;
        }

        byte[] fileData = www.downloadHandler.data;
        File.WriteAllBytes(persistentPath, fileData);
#else
        File.Copy(sourcePath, persistentPath, true);
#endif

        Debug.Log("NNUE 파일 복사 완료: " + persistentPath);
    }
}