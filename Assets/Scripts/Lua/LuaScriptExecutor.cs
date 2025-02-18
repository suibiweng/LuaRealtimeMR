using UnityEngine;
using MoonSharp.Interpreter;
using System.Collections;
using LuaProxies;
public class LuaScriptExecutor : MonoBehaviour
{
    private Script luaScript;
    public float rotationSpeed = 50.0f;
    public string luaScriptUrl = "https://yourserver.com/path/to/rotate_object.lua"; // Replace with your Lua script URL

    void Start()
    {
        // Register Unity types with MoonSharp
        UserData.RegisterType<Transform>();
        UserData.RegisterType<Quaternion>();
        UserData.RegisterType<GameObject>();
        UserData.RegisterType<Vector3>();
        UserData.RegisterType<TransformProxy>();

        // Initialize the Lua interpreter
        luaScript = new Script();

        // Start coroutine to download and execute Lua script
       // StartCoroutine(DownloadAndExecuteLuaScript());
    }
   public void fireTheScript(){

StartCoroutine(DownloadAndExecuteLuaScript());


    }

    IEnumerator DownloadAndExecuteLuaScript()
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(luaScriptUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string luaCode = www.downloadHandler.text;
                luaScript.DoString(luaCode);
            }
            else
            {
                Debug.LogError("Failed to download Lua script: " + www.error);
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){

            fireTheScript();

        }
        // Get the Lua function
        DynValue rotateFunction = luaScript.Globals.Get("rotate_object");
        if (rotateFunction != null && rotateFunction.Type == DataType.Function)
        {
            // Create a TransformProxy for this GameObject's transform
            TransformProxy proxy = new TransformProxy(transform);

            // Call the Lua function
            luaScript.Call(rotateFunction, UserData.Create(proxy), rotationSpeed, Time.deltaTime);
        }
    }





}
