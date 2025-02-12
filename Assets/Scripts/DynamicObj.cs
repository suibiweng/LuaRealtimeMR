using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.Networking;
using TMPro;

public class DynamicObj : MonoBehaviour
{
    public bool isSelected = false;

    public VoiceLabel voiceLabel;
    public TMP_Text promptText;
    public GenObject genObject;
    public LuaMonoBehaviour luaMonoBehaviour;
    public string DownloadURL;
    public string ServerURL;
    public string ID;
    //public Dictionary<string, string> Log = null;
    public List<string> Log = new List<string>();

    public string prompt;

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
        

     prompt= voiceLabel.Label.text ;
    }


    public void SendCommand(string command){


        StartCoroutine(SendtheCommand(ServerURL,command,ID,prompt));


    }

        public void setVoiceInput(){

        prompt= voiceLabel.Label.text;

        
    

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
