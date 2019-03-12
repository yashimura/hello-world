////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// (c) Cosmo Machia Inc. [http://www.cosmomachia.co.jp]
// 2018
////
using UnityEngine;
using System.Collections.Generic;
using Mix2App.Lib.Events;

#if UNITY_EDITOR
using System;
using System.Reflection;
#endif

namespace Mix2App.UI {
    /// <summary>
    /// Place this to root canvas
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [UnityEngine.AddComponentMenu("UI/_!UIManager")]
    public class UIManager: MonoBehaviour {
        /// <summary>
        /// Singletone instance point.
        /// Used only by system.
        /// If you want to access - use <see cref="Instance"/>
        /// </summary>
        private static UIManager __Instance;

        [SerializeField] private GameObject BGDummy = null;

        private void Awake() {
            // Get self root canvas
            RootCanvas = this.GetComponent<Canvas>();

            // Setup Singletone access point
            if (__Instance != null) {
                Debug.LogWarning("Scene have more than one UIManager!", this);
            } else {
                __Instance = this;
                OpenedWindows = new Stack<UIWindow>();
                if (RootCanvas == null) {
                    Debug.LogError("RootCanvas not assigned!", this);
                }
            }
        }

        private void OnDestroy() {
            // prevent memory leak
            __Instance = null;
            //THINK: you can make this object undestroyable and move to next scenes.
            OpenedWindows = null;
        }

        /// <summary>
        /// safe access to self singletone instance
        /// </summary>
        protected static UIManager Instance {
            get {
                if (__Instance != null)
                    return __Instance;
                throw new MissingComponentException("UIManager not found!");
            }
        }

        private Canvas RootCanvas;

        private static Stack<UIWindow> OpenedWindows;

        /// <summary>
        /// Show window from prefab.
        /// Also, hides current window(if have) untill new closed.
        /// </summary>
        /// <typeparam name="T">Window component on prefab</typeparam>
        /// <param name="prefab">Prefab with specified T component</param>
        /// <returns>T component of created window</returns>
        public static T ShowModal<T>(T prefab, bool hide_last = true) where T : UIWindow {
            if (prefab == null) {
                Debug.LogError("Prefab cannot be null!", Instance);
                throw new System.NullReferenceException();
            }

            

            GameObject go = GameObject.Instantiate(prefab.gameObject, Instance.RootCanvas.transform);
            T wnd = go.GetComponent<T>();
            if (wnd == null) {
                Debug.LogError("Prefab [" + prefab.name + "] not contain requested component (" + typeof(T).Name + ") on itself", prefab);
                throw new MissingComponentException();
            }

            wnd.Setup();

            if (hide_last) {
                if (OpenedWindows.Count > 0)
                    OpenedWindows.Peek().gameObject.SetActive(false);
            } else {
                GameObject dm = GameObject.Instantiate(Instance.BGDummy, Instance.RootCanvas.transform);
                dm.transform.SetAsLastSibling();
                wnd.AddCloseAction(()=> {
                    Destroy(dm);
                });
            }


            go.transform.SetAsLastSibling();

            wnd.AddCloseAction(() => {
                OpenedWindows.Pop();
                if (OpenedWindows.Count > 0)
                    OpenedWindows.Peek().gameObject.SetActive(true);

            });

            OpenedWindows.Push(wnd);
            wnd.gameObject.SetActive(true);
            return wnd;
        }

#if UNITY_EDITOR
        public bool DebugElements = true;
        public UIElement[] ElementsForDebug;

        private IEnumerable<FieldInfo> EnumerateFields(Type tp, BindingFlags flags) {
            foreach (var item in tp.GetFields(flags))
                yield return item;
            
            if (tp.BaseType != null)
                foreach (var item in EnumerateFields(tp.BaseType, flags & (~BindingFlags.Public)))
                    yield return item;
        }

        /// <summary>
        /// Find not assigned fild with SerializeField and Required attribute
        /// </summary>
        /// <param name="item">object to check</param>
        /// <returns>fields list</returns>
        private IEnumerable<string> CheckFields(object item) {
            foreach (var field in EnumerateFields(item.GetType(), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                if (field.GetCustomAttributes(typeof(Required), false).Length != 0) {
                    object val = field.GetValue(item);
                    if (val == null)
                        yield return field.Name;
                    else
                        foreach (var err in CheckFields(val))
                            yield return (field.Name + '.' + err);
                }
        }

        private void Start() {
            if (DebugElements) {
                foreach (var item in ElementsForDebug) {
                    if (item != null) {
                        List<string> RequiredFields = new List<string>(CheckFields(item));

                        if (RequiredFields.Count > 0) {
                            string error_message = string.Format("Object [{0}] has errors: ", item.name);
                            foreach (var error in RequiredFields)
                                error_message += "\n    " + string.Format("Required field [{0}] must be assigned!", error);
                            Debug.LogAssertion(error_message, item);
                        } else 
                            Debug.Log(string.Format("Object [{0}] has no errors!", item));
                    }
                }
            }
        }
#endif




        private static GameEventHandler GEHandler;
        private static GameObject[] proposeWindow;
        private static GameObject cameraWindow;
        private static bool proposeFlag;

        public static void GEHandlerSet(GameEventHandler _handle){
            GEHandler = _handle;
        }
        public static void proposeWindowSet(GameObject[] _obj){
			proposeWindow = _obj;
		}
        public static void cameraWindowSet(GameObject _obj){
            cameraWindow = _obj;
        }
        public static void proposeFlagSet(bool _flag){
            proposeFlag = _flag;
        }


        public static GameEventHandler GEHandlerGet(){
            return GEHandler;
        }
        public static GameObject[] proposeWindowGet(){
            return proposeWindow;
        }
        public static GameObject cameraWindowGet(){
			return cameraWindow;
		}
        public static bool proposeFlagGet(){
            return proposeFlag;
        }

    }
}
