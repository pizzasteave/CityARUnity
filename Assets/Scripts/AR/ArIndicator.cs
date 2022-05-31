using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;

public class ArIndicator : Singleton<ArIndicator>
{


    public GameObject placementIndicator;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid;
    public bool notPlaced;


    private ARAnchor anchor;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        notPlaced = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (placementIndicator == null)
            return;

        if (notPlaced)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
    }


    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }

    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, -cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(cameraBearing).normalized;
        }
    }

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public void Place()
    {
        var instantiatedObject = Instantiate(placementIndicator, placementPose.position, placementPose.rotation);

        // Make sure the new GameObject has an ARAnchor component
        var anchor = instantiatedObject.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = instantiatedObject.AddComponent<ARAnchor>();
        }
        Debug.Log($"Created regular anchor (id: {anchor.nativePtr}).");
  
    }

    public void Pick()
    {
        Ray screenCenter = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hitObject; 

        if(Physics.Raycast(screenCenter,out hitObject))
        {
            Destroy(placementIndicator);

            var modelInfo =  hitObject.transform.GetComponent<ModelInfo>();
            print(modelInfo.originPrefab);
            ModelBehaviour.Instance.prefab =  modelInfo.GetPrefab();
            ModelBehaviour.Instance.OnIcon();
        }

    }


    void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
