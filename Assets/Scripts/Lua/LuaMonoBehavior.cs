using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.UI;

namespace LuaIntegration
{
    public class LuaMonoBehaviour : MonoBehaviour
    {
        private Script luaScript;
        private DynValue startFunction;
        private DynValue updateFunction;
        private DynValue fixedUpdateFunction;
        private DynValue lateUpdateFunction;
        private DynValue onTriggerEnterFunction;
        private DynValue onTriggerExitFunction;
        private DynValue onCollisionEnterFunction;
        private DynValue onCollisionExitFunction;
        private DynValue onButtonClickFunction;
        
        public Text uiText;
        public Button uiButton;
        public string luaScriptText = @"
            function start()
                print('Lua Start() called!')
            end

            function update(deltaTime)
                print('Lua Update() called! DeltaTime:', deltaTime)
            end

            function fixedUpdate()
                print('Lua FixedUpdate() for physics calculations')
            end

            function lateUpdate()
                print('Lua LateUpdate() called')
            end

            function onTriggerEnter(other)
                print('Lua OnTriggerEnter() called with:', other.name)
            end

            function onTriggerExit(other)
                print('Lua OnTriggerExit() called with:', other.name)
            end

            function onCollisionEnter(other)
                print('Lua OnCollisionEnter() with:', other.name)
            end

            function onCollisionExit(other)
                print('Lua OnCollisionExit() with:', other.name)
            end

            function onButtonClick()
                print('Lua: Button clicked!')
            end

            function updateUIText(textObject, newText)
                textObject.text = newText
            end
        ";

        void Start()
        {
            // Register Unity types
            UserData.RegisterType<Transform>();
            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<Quaternion>();
            UserData.RegisterType<Rigidbody>();
            UserData.RegisterType<Collider>();
            UserData.RegisterType<Material>();
            UserData.RegisterType<Renderer>();
            UserData.RegisterType<Camera>();
            UserData.RegisterType<Light>();
            UserData.RegisterType<AudioSource>();
            UserData.RegisterType<ParticleSystem>();
            UserData.RegisterType<ParticleSystemProxy>();
            UserData.RegisterType<Animator>();
            UserData.RegisterType<Button>();
            UserData.RegisterType<Text>();
            UserData.RegisterType<LuaMonoBehaviour>();

            // Initialize Lua
            luaScript = new Script();
            luaScript.DoString(luaScriptText);

            // Get Lua functions
            startFunction = luaScript.Globals.Get("start");
            updateFunction = luaScript.Globals.Get("update");
            fixedUpdateFunction = luaScript.Globals.Get("fixedUpdate");
            lateUpdateFunction = luaScript.Globals.Get("lateUpdate");
            onTriggerEnterFunction = luaScript.Globals.Get("onTriggerEnter");
            onTriggerExitFunction = luaScript.Globals.Get("onTriggerExit");
            onCollisionEnterFunction = luaScript.Globals.Get("onCollisionEnter");
            onCollisionExitFunction = luaScript.Globals.Get("onCollisionExit");
            onButtonClickFunction = luaScript.Globals.Get("onButtonClick");

            // Call Start() if it exists
            if (startFunction != null && startFunction.Type == DataType.Function)
            {
                luaScript.Call(startFunction);
            }

            // Setup UI Button Click Event
            if (uiButton != null && onButtonClickFunction != null)
            {
                uiButton.onClick.AddListener(() => luaScript.Call(onButtonClickFunction));
            }
        }

        void Update()
        {
            if (updateFunction != null && updateFunction.Type == DataType.Function)
            {
                luaScript.Call(updateFunction, Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            if (fixedUpdateFunction != null && fixedUpdateFunction.Type == DataType.Function)
            {
                luaScript.Call(fixedUpdateFunction);
            }
        }

        void LateUpdate()
        {
            if (lateUpdateFunction != null && lateUpdateFunction.Type == DataType.Function)
            {
                luaScript.Call(lateUpdateFunction);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (onTriggerEnterFunction != null && onTriggerEnterFunction.Type == DataType.Function)
            {
                luaScript.Call(onTriggerEnterFunction, other.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (onTriggerExitFunction != null && onTriggerExitFunction.Type == DataType.Function)
            {
                luaScript.Call(onTriggerExitFunction, other.gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnterFunction != null && onCollisionEnterFunction.Type == DataType.Function)
            {
                luaScript.Call(onCollisionEnterFunction, collision.gameObject);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (onCollisionExitFunction != null && onCollisionExitFunction.Type == DataType.Function)
            {
                luaScript.Call(onCollisionExitFunction, collision.gameObject);
            }
        }
    }
}
