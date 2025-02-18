using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

// Proxy class for Lua to access GameObject Transform

public class LuaScriptDownloader : MonoBehaviour
{
    private Script luaScript;
    private string luaFilePath;
    public string luaURL = "http://localhost:8000/rotation.lua"; // Change to your server URL
    public float rotationSpeed = 50.0f;

    void Start()
    {
        luaFilePath = Path.Combine(Application.persistentDataPath, "rotation.lua");
        StartCoroutine(DownloadAndLoadLuaScript());
    }

    void Update()
    {
        if (luaScript != null)
        {
            // Get the Lua function
            DynValue rotateFunction = luaScript.Globals.Get("rotate_object");
            if (!rotateFunction.IsNil())
            {
                // 🔹 Pass GameObjectProxy instead of `this`
                // GameObjectProxy proxy = new GameObjectProxy(transform);
                // luaScript.Call(rotateFunction, proxy, rotationSpeed, Time.deltaTime);
            }
        }

        // Press Space to reload the script
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DownloadAndLoadLuaScript());
        }
    }

    IEnumerator DownloadAndLoadLuaScript()
    {
        Debug.Log("📥 Downloading Lua script...");

        using (UnityWebRequest request = UnityWebRequest.Get(luaURL))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Failed to download Lua script: {request.error}");
                yield break;
            }

            File.WriteAllText(luaFilePath, request.downloadHandler.text);
            Debug.Log("✅ Lua script downloaded and saved!");

            LoadLuaScript();
        }
    }

    void LoadLuaScript()
    {
        if (File.Exists(luaFilePath))
        {
            string luaCode = File.ReadAllText(luaFilePath);
            luaScript = new Script();

            // 🔹 Register GameObjectProxy so Lua can recognize it
            // UserData.RegisterType<GameObjectProxy>();

            luaScript.DoString(luaCode);
            Debug.Log("✅ Lua script loaded successfully.");
        }
        else
        {
            Debug.LogError("❌ Lua script file not found!");
        }
    }
}
