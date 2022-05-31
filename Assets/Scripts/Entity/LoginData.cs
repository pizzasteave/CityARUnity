using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginData 
{
    [System.Serializable]
    public class LoginResponse
    {
        public int code;
        public string msg;
        public Account data; 
    }

    [System.Serializable]
    public class Account
    {
        public string  _id;
        public string email;
        public string firstname;
        public string phone;
        public string accessToken;
        public string role;
        public string gouv;
        public string mun;
        
    }
}
