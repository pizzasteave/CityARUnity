using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropositionManager : Singleton<PropositionManager>
{

    public GameObject scrollContent;

    [Header("POST UI")]
    public GameObject CreateProp;
    public Canvas PropCanvas;
    public Canvas CreatePropCanvas;

    GameObject activityElement;

    public void AddPostToActivity(GameObject activityElement)
    {
        this.activityElement = activityElement;
        activityElement.transform.SetParent(scrollContent.transform);
        activityElement.GetComponent<RectTransform>().SetAsFirstSibling();
    }


    public void OnPostUI()
    {
        if(CreatePropCanvas.enabled)
        {
            CreatePropCanvas.enabled = false;
        }
        else
        {
            CreatePropCanvas.enabled = true;
        }
   

    }
}
