using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityEditor;
public class DynamicObjManager : MonoBehaviour
{

    public Transform Head,LeftHand,RightHand;

    public string ServerURL;
    public string uploadPort,downloadPort;

    public GameObject GenObjPrefab;

    public Dictionary<string,GameObject> GenObjsDic;

    // Start is called before the first frame update
    void Start()
    {
        GenObjsDic=new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){

            createObj();

        }
    }

    public void createObj()
    {
        GameObject newObj = Instantiate(GenObjPrefab, LeftHand.position, Quaternion.identity);

        DynamicObj dynamicObj =newObj.GetComponent<DynamicObj>();

        dynamicObj.ID= IDGenerator.GenerateID();
        GenObjsDic.Add(dynamicObj.ID,newObj);

        dynamicObj.DownloadURL=ServerURL+":"+downloadPort+"/";
        dynamicObj.ServerURL=ServerURL+":"+uploadPort;


        dynamicObj.Initialize();

        //genScript.luaMonoBehaviour.StartFetchingCode( genScript.genObject.DownloadURL,genScript.ID);
     
     
    }



}
