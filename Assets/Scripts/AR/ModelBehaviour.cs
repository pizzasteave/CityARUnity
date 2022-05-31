using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehaviour : Singleton<ModelBehaviour>
{
    public GameObject prefab;

    public void OnIcon()
    {
        Destroy(ArIndicator.Instance.placementIndicator);

        var indicator = Instantiate(prefab, Vector3.one, Quaternion.identity);
        indicator.SetActive(false);

        ArIndicator.Instance.placementIndicator = indicator;

    }
}
