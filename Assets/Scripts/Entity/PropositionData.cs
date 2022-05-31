using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropositionData 
{
    [System.Serializable]
    public class PropositionResponse
    {
        public List<Proposition> data;
    }

    [System.Serializable]
    public class Proposition
    {
        public string _id;
        public string name;
        public List<string> likes;
        public string valid;
        public string image;
    }

    [System.Serializable]
    public class responseCreateProp
    {
        public string name;
        public string description;
        public string image;
        public string likes;
        public string createdBy;
        public string _id;
    }
}
