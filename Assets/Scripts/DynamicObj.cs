using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.Networking;

public class DynamicObj : MonoBehaviour
{
    public GenObject genObject;
    public LuaMonoBehaviour luaMonoBehaviour;
    public string DownloadURL;
    public string ServerURL;
    public string ID;
    //public Dictionary<string, string> Log = null;
    public List<string> Log = new List<string>();


    public void Initialize(){

       
        genObject.ID = ID;
        luaMonoBehaviour.ID = ID;
        genObject.DownloadURL = DownloadURL;
        genObject.ServerURL = ServerURL;
        luaMonoBehaviour.StartFetchingCode(DownloadURL, ID);
        


        

    }
    // Start is called before the first frame update
    void Start()
    {


        Initialize();

        
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }




        public IEnumerator SendtheCommand( string url,string command ,string urlid,string Prompt)
{
    
    if (command != "")
    {
        WWWForm form = new WWWForm();

        form.AddField("Command", command);
        form.AddField("URLID",urlid);
        form.AddField("Prompt",Prompt);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Command sent: " + command);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
}
