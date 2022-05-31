using Mopsicus.InfiniteScroll;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadProposition : Singleton<LoadProposition> {

	[SerializeField]
	private InfiniteScroll Scroll;

	
	public int Count = 100;

	List<PropositionData.Proposition> itemList;
	
	const string imageFolder = "http://127.0.0.1:8080/";

	private bool mainDone = true;
	int mainthread;

	void Start () {
		
		Manager.Instance.LoadDataFromServer(SplitPlat);

	}

	void FillItems(List<PropositionData.Proposition> itemListtemp)
    {
		itemList = itemListtemp;
		
		Scroll.OnFill += OnFillItem;
		Scroll.OnHeight += OnHeightItem;

		Scroll.InitData(Count);
	}

	//void OnFillItem (int index, GameObject item) {
	//	var button = item.GetComponentInChildren<BehaviourTest>();
	//	button.Init(itemList[index].titre,itemList[index].image);
     
	//		foreach(var tex in DownloadImageController.Instance.tex2D)
 //           {
	//			if(tex.name == itemList[index].image)
	//			{
	//			button.image.texture = tex; 
	//			}     
 //           }
        
 //      // var url = Path.Combine(imageFolder, itemList[index].image);

 //      // DownloadImageController.Instance.LoadImage(button.image,url ,itemList[index].image);
 //   }

	
	void OnFillItem(int index, GameObject item)
	{
		var propElement = item.GetComponentInChildren<PropositionElement>();
		propElement.Init(itemList[index].name, itemList[index].image);

		foreach (var tex in LoadImage.Instance.tex)
        {
            if (tex.name == itemList[index].image)
            {
                propElement.photo.texture = tex;
            }
        }

        //Load every shit 

        //if((index % 6f) == 0 && index != 0 && mainDone)
        //      {
        //	AsyncLoad();
        //      }


        if (index == 9f && mainDone)
        {
			test();
		}

	}

	int OnHeightItem (int index) {
		return 300;
	}

	public void SceneLoad (int index) {
		SceneManager.LoadScene (index);
	}


	public void SplitPlat(PropositionData.PropositionResponse allProp)
	{
	

		if (allProp == null)
		{
			SceneManager.LoadScene(0);
		}
		
		Count = allProp.data.Count();
		//clear list on every call to prevent stacking of plats 
		API.Instance.itemList.Clear();

		if (allProp.data.Count() > 9)
		{
			mainthread = 9; 
		
		} else
			mainthread = allProp.data.Count();


		foreach(var prop in allProp.data)
        {
			API.Instance.itemList.Add(prop);
		}

		for (int i = 0; i < mainthread; i++)
		{
			var prop = allProp.data[i];

			API.Instance.itemList.Add(prop);

			var url = prop.image;

			LoadImage.Instance.currentPath = Application.persistentDataPath + Path.GetFileName(url);
			StartCoroutine(LoadImage.Instance.OnLoadImageFromWebButtonClick(url,default,prop.image));
		}


		API.Instance.GetItemList(FillItems);
		
	}

	void test ()
    {
		StartCoroutine(AsyncLoad());
    }

	IEnumerator  AsyncLoad()
    {

		yield return new WaitForSeconds(2f);

		//DownloadImageController.Instance.DestroyAllTextures();

		for (int i = mainthread; i < itemList.Count; i++)
		{
			var plat = itemList[i];

			var url = Path.Combine(imageFolder, plat.image);
			DownloadImageController.Instance.LoadImage(url, plat.image);
		}
		
		mainDone = false; 
	}


	void Update()
    {
		
	}
}