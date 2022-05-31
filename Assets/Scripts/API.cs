using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class API : Singleton<API> {

    private string tempPath;
    public List<PropositionData.Proposition> itemList = new List<PropositionData.Proposition>();

    bool loaded = false;

    const string BundleFolder = "http://pizzasteavekun.me/AssetBundles/";
    const string ModelFolder = "http://pizzasteavekun.me/Models/";

    #region GetItemList
    public void GetItemList(UnityAction<List<PropositionData.Proposition>> callback) {
        StartCoroutine(GetItemListRoutine(callback));
    }

    IEnumerator GetItemListRoutine(UnityAction<List<PropositionData.Proposition>> callback) {

        yield return new WaitForSeconds(0.1f);
        callback.Invoke(itemList);
    }
    #endregion
 
}
