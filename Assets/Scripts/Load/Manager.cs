using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Manager : Singleton<Manager>
{
    public static bool json = false; 
    public static string dataFileName = "data.json";
    public static string dataPath = "http://127.0.0.1:8080/api/proposition/all";

    public static string alldatapath;

    #region LoadMenuFromCash
    public void LoadDataFromCash (UnityAction<PropositionData.PropositionResponse> callback)
    {
       StartCoroutine(GetJsonFromCash(callback));
    
    }

    IEnumerator GetJsonFromCash(UnityAction<PropositionData.PropositionResponse> callback)
    {
        string filePath = Path.Combine(Application.temporaryCachePath, dataFileName);


        PropositionData.PropositionResponse data = new PropositionData.PropositionResponse();
        if (File.Exists(filePath))
        {
            string datajson = File.ReadAllText(filePath);

            data = JsonUtility.FromJson<PropositionData.PropositionResponse>(datajson);
        }
        else
        {
            print("cant load data ");
        }
        print("logged from cash");
 
        callback.Invoke(data);
   
        yield return json = true ;  
    }
    #endregion

    #region LoadMenuFromServer
    public void LoadDataFromServer(UnityAction<PropositionData.PropositionResponse> callback)
    {
        StartCoroutine(GetJsonFromServer(callback));

    }

    IEnumerator GetJsonFromServer(UnityAction<PropositionData.PropositionResponse> callback)
    {

        PropositionData.PropositionResponse data = new PropositionData.PropositionResponse();

        using (UnityWebRequest request = UnityWebRequest.Get(dataPath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                print(request.result.ToString());
            }
            else
            {
                string datajson = request.downloadHandler.text;

                //delet old cashed version AND
                DeleteCache();
                print("old cash deleted...");
                data = JsonUtility.FromJson<PropositionData.PropositionResponse>(datajson);
                //SAVE the new one 
                SaveIntoCash(datajson);
            }
        }

        Debug.Log("logged from server");
        callback.Invoke(data);
        yield return json = true; 
    }

     void DeleteCache()
    {
        string path = Application.temporaryCachePath;

        DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    void SaveIntoCash(string datajson)
    {
        File.WriteAllText(Path.Combine(Application.temporaryCachePath, dataFileName), datajson);
        print($"New Cash Saved under {Application.temporaryCachePath}");
    }

    #endregion


    #region LoadMenuFromServer
    public void LoadAllDataFromServer(UnityAction<PropositionData.PropositionResponse> callback)
    {
        StartCoroutine(GetAllJsonFromServer(callback));

    }

    IEnumerator GetAllJsonFromServer(UnityAction<PropositionData.PropositionResponse> callback)
    {

        PropositionData.PropositionResponse data = new PropositionData.PropositionResponse();

        using (UnityWebRequest request = UnityWebRequest.Get(alldatapath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
              print(request.result.ToString());
            }
            else
            {
                string datajson = request.downloadHandler.text;

                //delet old cashed version AND
                DeleteCache();
                print("old cash deleted...");

                data = JsonUtility.FromJson<PropositionData.PropositionResponse>(datajson);
                //SAVE the new one 
                SaveIntoCash(datajson);
            }
        }

        Debug.Log("logged from server");
        callback.Invoke(data);
        yield return json = true;
    }


    #endregion

}
