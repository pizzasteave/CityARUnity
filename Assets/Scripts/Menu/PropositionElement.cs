using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropositionElement : MonoBehaviour
{

    public TextMeshProUGUI title;
    public RawImage photo;
    public void Init(string title,string image = default ,Texture2D photo = default)
    {
        this.title.text = title;

        if (photo)
            this.photo.texture = photo;
    }

}
