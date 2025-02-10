using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using UnityEngine;

public class DLLDownloader : MonoBehaviour
{
    public string dllURL = "http://localhost:8000/MyDLL.dll"; // Change this to your server URL

    public GameObject prefab;

    void Start()
    {
      //  StartCoroutine(DownloadAndLoadDLL());
    }

    void  Update()  {

        if(Input.GetKeyDown(KeyCode.Space)){


             StartCoroutine(DownloadAndLoadDLL());   




        }        



    }

    public void StartDownloadDll(){

        StartCoroutine(DownloadAndLoadDLL());   



    }

    IEnumerator DownloadAndLoadDLL()
    {
        string dllPath = Path.Combine(Application.persistentDataPath, "DynamicScript.dll");

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(dllURL, dllPath);
        }

        yield return new WaitForSeconds(1); // Ensure file is downloaded

        // Load the DLL from persistentDataPath (since StreamingAssets is read-only)
        LoadDLL(dllPath);
    }


  void LoadDLL(string dllPath)
    {
        if (!File.Exists(dllPath))
        {
            Debug.LogError($"❌ DLL not found at: {dllPath}");
            return;
        }

        try
        {
            Debug.Log($"📥 Loading DLL from: {dllPath}");

            // 🔹 Read the DLL into memory
            byte[] dllBytes = File.ReadAllBytes(dllPath);
            Assembly loadedAssembly = Assembly.Load(dllBytes);

            Debug.Log($"✅ Loaded Assembly: {loadedAssembly.FullName}");

            // 🔹 Search for the assembly manually (since FirstOrDefault is not available)
            Assembly assembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == "DynamicScript") // Change "DynamicScript" to your actual DLL name
                {
                    assembly = asm;
                    break;
                }
            }

            // If the assembly was not found, use the loaded assembly
            if (assembly == null)
            {
                assembly = loadedAssembly;
            }

            if (assembly == null)
            {
                Debug.LogError($"❌ Failed to load assembly: DynamicScript");
                return;
            }

            Debug.Log($"🔹 Assembly Loaded Successfully: {assembly.FullName}");

            // 🔹 Get the Type of the script in the DLL
            Type scriptType = assembly.GetType("DynamicScript");
            if (scriptType == null)
            {
                Debug.LogError($"❌ Script 'DynamicScript' not found in DLL.");
                return;
            }

            Debug.Log($"✅ Found Script: {scriptType.Name}");

            // 🔹 Instantiate Prefab and Attach Script
            GameObject G = Instantiate(prefab);
            if (G == null)
            {
                Debug.LogError($"❌ Failed to instantiate prefab.");
                return;
            }

            Debug.Log($"🎮 Instantiated Prefab: {G.name}");

            // 🔹 Add the script dynamically
            G.AddComponent(scriptType);

            Debug.Log($"✅ Successfully added {scriptType.Name} to {G.name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ DLL Load Error: {ex.Message}\n{ex.StackTrace}");
        }
    }


    // void LoadDLL(string dllPath)
    // {
    //     if (!File.Exists(dllPath))
    //     {
    //         Debug.LogError($"DLL not found at {dllPath}");
    //         return;
    //     }


    //     byte[] dllBytes = File.ReadAllBytes(dllPath);
    //     Assembly loadedAssembly = Assembly.Load(dllBytes);


    //    // Assembly loadedAssembly = Assembly.LoadFile(dllPath);
    //     Debug.Log($"Loaded DLL: {loadedAssembly.FullName}");

    //     // Example: Call a method using Reflection
    //     // Type myType = loadedAssembly.GetType("MyNamespace.MyClass");
    //     // MethodInfo method = myType?.GetMethod("MyMethod");
    //     // method?.Invoke(null, null);



    //         Assembly assembly = Assembly.Load("DynamicScript");
    //         if (assembly == null)
    //         {
    //             Debug.LogError($"Failed to load assembly: {"DynamicScript"}");
    //             return;
    //         }

    //         // Get the script Type from the DLL
    //         Type scriptType = assembly.GetType("DynamicScript");
    //         if (scriptType == null)
    //         {
    //             Debug.LogError($"Script '{"DynamicScript"}' not found in {"DynamicScript"}");
    //             return;
    //         }



    //     GameObject G = Instantiate(prefab);

    //     G.AddComponent(scriptType);
    //     // G.GetComponent<DynamicObj>().AddScript();


    // }
}
