// Author: Tobias Löf Melker
// Created: 2020 for usage with Unity Engine

using UnityEngine;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
using NumberingSuffix = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.NumberingSuffix;
using Projection = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.Projection;
using AssetPathInfo = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGenerator.AssetPathInfo;
using UnityEditor.SceneManagement;
using System;
using System.Reflection;
#if UNITY_POSTPROCESS
using UnityEngine.Rendering.PostProcessing;
#elif UNITY_SRP_POSTPROCESS
using UnityEngine.Rendering;
#endif
#if !UNITY_2020_2_OR_NEWER
using ToolManager = UnityEditor.EditorTools.EditorTools;
#endif

// window, used by tool, in editor sceneView, prefabStage sceneView or gameView
namespace UnityEditor.EditorTools.TLM.ThumbnailGenerator
{
    public class ThumbnailGeneratorWindow : EditorWindow
    {
        [SerializeField]
        public GUISkin guiSkin = null;
        [SerializeField]
        public Texture2D placementTexture = null;

        private GUIStyle guiStyle;

        public static ThumbnailGeneratorWindow instance { get; private set; }
        public ThumbnailGeneratorTool tool { get; set; }

        private ThumbnailGenerator.ThumbnailResult _thumbnailResult = null;
        public ThumbnailGenerator.ThumbnailResult thumbnailResult
        {
            get => _thumbnailResult;
            set
            {
                if (_thumbnailResult != null)
                    _thumbnailResult.Dispose();
                _thumbnailResult = value;
            }
        }

        // sends necesary data to thumbnailGenerator
        void GenerateThumbnail()
        {
            var assetPathInfo = new AssetPathInfo(relativeAssetPath, fileName, numberingSuffix, AssetPathInfo.AssetType.scene, userOverrideRelativeAssetPath, userOverrideFileName);

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
                assetPathInfo.assetType = AssetPathInfo.AssetType.prefab;

            if (useGameCamera && gameCameras != null && gameCameras.Length != 0)
            {
                assetPathInfo.assetType = AssetPathInfo.AssetType.scene;
                thumbnailResult = ThumbnailGenerator.GenerateThumbnail(gameCameras, thumbnailWidth, thumbnailHeight, assetPathInfo, backgroundColor, postProcessMaterial);
            }
            else if (toolCamera != null)
            {
                thumbnailResult = ThumbnailGenerator.GenerateThumbnail(new[] { toolCamera }, thumbnailWidth, thumbnailHeight, assetPathInfo, backgroundColor, postProcessMaterial);
            }
            else if (Camera.main != null)
            {
                thumbnailResult = ThumbnailGenerator.GenerateThumbnail(new[] { Camera.main }, thumbnailWidth, thumbnailHeight, assetPathInfo, backgroundColor, postProcessMaterial);
            }
            else if (SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
            {
                thumbnailResult = ThumbnailGenerator.GenerateThumbnail(new[] { SceneView.lastActiveSceneView.camera }, thumbnailWidth, thumbnailHeight, assetPathInfo, backgroundColor, postProcessMaterial);
            }
            else
                Debug.LogWarning("No camera in focus avaliable to render thumbnail.");
        }

        private static GUIContent guiContentGotoSettings;
        private static GUIContent guiContentAnimationWindow;
        private static GUIContent guiContentSaveCameraPosition;
        private static GUIContent guiContentLoadCameraPosition;
        private static GUIContent guiContentBrowseFilepath;
        private static GUIContent guiContentGenerateThumbnail;
        private static GUIContent guiContentClose;
        private static GUIContent guiContentClose_small;
        private static GUIContent guiContentPrefab;
        private static bool guiContentLoaded = false;
        public static void LoadThumbnailGeneratorWindowIcons()
        {
            if (guiContentLoaded) return;

            // note :: reference from https://unitylist.com/p/5c3/Unity-editor-icons icon paths might get updated
            guiContentGotoSettings = EditorGUIUtility.IconContent("_Popup");
            guiContentGotoSettings.text = " Goto Global Settings   ";

            guiContentAnimationWindow = EditorGUIUtility.IconContent("UnityEditor.AnimationWindow");
            guiContentAnimationWindow.text = " Open Animation Window";

            guiContentSaveCameraPosition = EditorGUIUtility.IconContent("CollabPush");
            guiContentSaveCameraPosition.text = " Save Camera Position";

            guiContentLoadCameraPosition = EditorGUIUtility.IconContent("CollabPull");
            guiContentLoadCameraPosition.text = " Load Camera Position     ";

            guiContentBrowseFilepath = EditorGUIUtility.IconContent("Folder Icon");
            guiContentBrowseFilepath.text = " Browse Custom Filepath";

            guiContentGenerateThumbnail = EditorGUIUtility.IconContent("SaveActive");
            guiContentGenerateThumbnail.text = " Generate Thumbnail";

            guiContentClose = EditorGUIUtility.IconContent("winbtn_mac_close_h");
            guiContentClose.text = " Close";

            guiContentClose_small = EditorGUIUtility.IconContent("winbtn_mac_close_h");
            guiContentClose_small.text = string.Empty;

            guiContentPrefab = EditorGUIUtility.IconContent("Prefab Icon");
            guiContentPrefab.text = string.Empty;
        }

        private static ToolWindow_SettingsSnapshot applySettingsOnRestart;

        public class ToolWindow_SettingsSnapshot
        {
            public bool limitTextureSize = true;
            public bool playbackParticleSystems = false;
            public float particlePlaybackTime = 0;
            public int thumbnailWidth;
            public int thumbnailHeight;
            public string relativeAssetPath;
            public string fileName = null;
            public NumberingSuffix numberingSuffix = NumberingSuffix.Auto;
            public Color backgroundColor;

            private SceneAsset currentBackgroundSceneAsset = null;
            public Material postProcessMaterial = null;
#if UNITY_POSTPROCESS
            private PostProcessProfile currentPostProcessProfile = null;
#elif UNITY_SRP_POSTPROCESS
            private VolumeProfile currentVolumeProfile = null;
#endif
            public void ApplySettings(ThumbnailGeneratorWindow window)
            {
                window.limitTextureSize = limitTextureSize;
                window.playbackParticleSystems = playbackParticleSystems;
                window.particlePlaybackTime = particlePlaybackTime;
                window.thumbnailWidth = thumbnailWidth;
                window.thumbnailHeight = thumbnailHeight;
                window.relativeAssetPath = relativeAssetPath;
                window.numberingSuffix = numberingSuffix;
                window.backgroundColor = backgroundColor;

                window.currentBackgroundSceneGUIProperty = currentBackgroundSceneAsset;
                window.postProcessMaterial = postProcessMaterial;
#if UNITY_POSTPROCESS
                window.currentPostProcessProfile = currentPostProcessProfile;
#elif UNITY_SRP_POSTPROCESS
                window.currentVolumeProfile = currentVolumeProfile;
#endif
            }

            public ToolWindow_SettingsSnapshot(ThumbnailGeneratorWindow window)
            {
                limitTextureSize = window.limitTextureSize;
                playbackParticleSystems = window.playbackParticleSystems;
                particlePlaybackTime = window.particlePlaybackTime;
                thumbnailWidth = window.thumbnailWidth;
                thumbnailHeight = window.thumbnailHeight;
                relativeAssetPath = window.relativeAssetPath;
                numberingSuffix = window.numberingSuffix;
                backgroundColor = window.backgroundColor;

                currentBackgroundSceneAsset = window.currentBackgroundSceneGUIProperty;
                postProcessMaterial = window.postProcessMaterial;
#if UNITY_POSTPROCESS
                currentPostProcessProfile = window.currentPostProcessProfile;
#elif UNITY_SRP_POSTPROCESS
                currentVolumeProfile = window.currentVolumeProfile;
#endif
            }
        }

        // user GUI settings
        bool limitTextureSize = true;
        bool playbackParticleSystems = false;
        float particlePlaybackTime = -1;
        public int thumbnailWidth { get; private set; }
        public int thumbnailHeight { get; private set; }
        string relativeAssetPath = null;
        string userOverrideRelativeAssetPath = null;
        string fileName = null;
        string userOverrideFileName = null;
        NumberingSuffix numberingSuffix = NumberingSuffix.Auto;
        public Color backgroundColor { get; private set; }

        private SceneAsset currentBackgroundSceneGUIProperty = null;
        private SceneAsset currentBackgroundSceneAsset = null;
        public Scene? currentBackgroundScene { get; private set; } = null;
        public Material postProcessMaterial { get; private set; } = null;
#if UNITY_POSTPROCESS
        private PostProcessProfile currentPostProcessProfile = null;
#elif UNITY_SRP_POSTPROCESS
        private VolumeProfile currentVolumeProfile = null;
#endif
        // end user GUI settings

        public Camera toolCamera { get; set; } = null;
        public EditorWindow sceneWindow { get; set; } = null;
        [NonSerialized]
        public Camera[] gameCameras = null;
        [NonSerialized]
        public bool useGameCamera = false;

        private void OnFocus()
        {
            if (ToolManager.IsActiveTool(tool) == false)
            {
                ToolManager.SetActiveTool(tool);
            }
        }

        private void OnEnable()
        {
            // note :: we init in OnEnable instead of Awake to make sure our ThumbnailGenerator has extracted the settings correctly.            
#if UNITY_2020_1_OR_NEWER
            // note: Unity has some weird issues with InContext prefabStage and no api. So we force open current scene to move out of InContext prefabStage and keep current camera transform.
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.mode == PrefabStage.Mode.InContext)
            {
                openedInPrefabStageInContext = true;
                //EditorSceneManager.OpenScene(prefabStage.openedFromInstanceRoot.scene.path, OpenSceneMode.Additive);
            }
#endif

            LoadThumbnailGeneratorWindowIcons();
            EditorApplication.quitting += Close;

            minSize = new Vector2(Mathf.Min(450, Screen.currentResolution.width), Mathf.Min(750, Screen.currentResolution.height));
            minSize = EditorGUIUtility.PixelsToPoints(minSize);

            Focus();
            instance = this;

            if (applySettingsOnRestart != null)
            {
                applySettingsOnRestart.ApplySettings(this);
                applySettingsOnRestart = null;
            }
            else
            {
                backgroundColor = ThumbnailGenerator.Settings.backgroundColor;
                thumbnailWidth = ThumbnailGenerator.Settings.thumbnailWidth;
                thumbnailHeight = ThumbnailGenerator.Settings.thumbnailHeight;
                currentBackgroundSceneGUIProperty = ThumbnailGenerator.Settings.backgroundScene;
                postProcessMaterial = ThumbnailGenerator.Settings.postProcessMaterial;
#if UNITY_POSTPROCESS
                currentPostProcessProfile = ThumbnailGenerator.Settings.postProcessProfile;
#elif UNITY_SRP_POSTPROCESS
                currentVolumeProfile = ThumbnailGenerator.Settings.volumeProfile;
#endif
                numberingSuffix = ThumbnailGenerator.Settings.numberingSuffix;

            }
            ExtractAssetPath(ref fileName, ref relativeAssetPath);

            // note : We don't really "need" this style. It's only a hacky way to change background color behind the rendered previewImage. If you want to do a custom guiSkin you don't need it.
            guiStyle = guiSkin.GetStyle("previewThumbnailStyle");

            PrefabStage.prefabStageOpened += PrefabStageOpened;
            PrefabStage.prefabStageClosing += PrefabStageClosed;
            EditorApplication.playModeStateChanged += PlayModeStateChanged;

            InitSceneObjects();
        }

        public void InitSceneObjects()
        {
            //CreateExtraSceneLight();
#if UNITY_POSTPROCESS || UNITY_SRP_POSTPROCESS
            CreatePostProcessVolumeAndLayer();
#endif

        }

        // Calculates a path to the generated thumbnail, using either the prefabname or the scenename.
        private void ExtractAssetPath(ref string fileName, ref string relativeAssetPath)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                relativeAssetPath = string.Empty;
#if UNITY_2019
                var mapsInAssetPath = prefabStage.prefabAssetPath.Split('/');
#else
                var mapsInAssetPath = prefabStage.assetPath.Split('/');
#endif
                for (int i = 0; i < mapsInAssetPath.Length - 1; i++)
                {
                    relativeAssetPath += mapsInAssetPath[i] + '/';
                }
                var prefabFileNameSplit = mapsInAssetPath.Last().Split('.');
                var prefabName = string.Join(string.Empty, prefabFileNameSplit.Take(prefabFileNameSplit.Length - 1));
                fileName = prefabName;
            }
            else
            {
                relativeAssetPath = string.Empty;
                var scene = SceneManager.GetActiveScene();
                var mapsInAssetPath = scene.path.Split('/');
                for (int i = 0; i < mapsInAssetPath.Length - 1; i++)
                {
                    relativeAssetPath += mapsInAssetPath[i] + '/';
                }
                fileName = scene.name;
            }
        }

        // note : now tries to use Unitys own SceneViewLight. Code kept for possible future use.

        //private Light Light0
        //{
        //    get;
        //    set;
        //}

        //private Light Light1
        //{
        //    get;
        //    set;
        //}
        //private bool useSceneLight = true;
        //public void CheckNeedExtraLight(bool state)
        //{
        //    if (Light0 == null)
        //    {
        //        CreateExtraSceneLight();
        //    }
        //    else if (useSceneLight == state)
        //    {
        //        return;
        //    }

        //    useSceneLight = state;

        //    // note : disabled messing with the lightning, even if I were to keep track of individual lightningsettings users could add lightning while using the tool.
        //    //foreach (var light in FindObjectsOfType<Light>())
        //    //{
        //    //    light.enabled = state;
        //    //}
        //    Light[] lights;
        //    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        //    if (prefabStage != null && prefabStage.prefabContentsRoot.activeInHierarchy)
        //    {
        //        lights = prefabStage.prefabContentsRoot.GetComponentsInChildren<Light>();
        //    }
        //    else
        //    {
        //        lights = FindObjectsOfType<Light>(); // note : this might be ultra slow for large scenes. Need some caching+culling. Keeping as is for now since it's only done on state switch.
        //    }

        //    // if no natural light and sceneView lightning is on. We use custom light instead.
        //    if (lights.Where(x => x.hideFlags == HideFlags.None && x.isActiveAndEnabled).Count() == 0)
        //    {
        //        Light0.enabled = !state;
        //        Light1.enabled = !state;
        //    }
        //}

        //// if unity is set to have sceneView light instead of normal scene light we create extra light sources.
        //public void CreateExtraSceneLight()
        //{
        //    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        //    // Same light as from PreviewRenderUtility
        //    Light0 = CreateLight(prefabStage);
        //    Light1 = CreateLight(prefabStage);
        //    Light0.color = new Color(0.769f, 0.769f, 0.769f, 1f); ;
        //    Light1.transform.rotation = Quaternion.Euler(340f, 218f, 177f);
        //    Light1.color = new Color(0.4f, 0.4f, 0.45f, 0f) * 0.7f;

        //    if (SceneView.lastActiveSceneView != null)
        //    {
        //        CheckNeedExtraLight(SceneView.lastActiveSceneView.sceneLighting);
        //    }
        //}

        //private Light CreateLight(PrefabStage prefabStage)
        //{
        //    var tempGO = new GameObject("tempLight");
        //    tempGO.hideFlags = HideFlags.HideAndDontSave;

        //    if (prefabStage != null)
        //    {
        //        tempGO.transform.parent = prefabStage.prefabContentsRoot.transform;
        //    }
        //    Light component = tempGO.AddComponent<Light>();
        //    component.type = LightType.Directional;
        //    component.intensity = 1f;
        //    component.enabled = false;
        //    return component;
        //}

#if UNITY_POSTPROCESS
        PostProcessVolume tempPostProcessVolume = null;
        PostProcessLayer tempPostProcessLayer = null;
        LayerMask? previousPostProcessLayer = null;
        Transform previousPostProcessTrigger = null;

        public void CreatePostProcessVolumeAndLayer()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            var tempGO = new GameObject("tempProstProcessVolume");
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            if (prefabStage != null)
            {
                tempGO.transform.parent = prefabStage.prefabContentsRoot.transform;
            }
            tempPostProcessVolume = tempGO.AddComponent<PostProcessVolume>();
            tempPostProcessVolume.isGlobal = true;

            tempPostProcessVolume.profile = currentPostProcessProfile;

            var mainCam = Camera.main;
            if (mainCam == null)
            {
                mainCam = FindObjectOfType<Camera>(); ;
                if (mainCam == null)
                {
                    mainCam = tempGO.AddComponent<Camera>();
                }
            }
            var postProcessLayer = mainCam.gameObject.GetComponent<PostProcessLayer>();
            if (postProcessLayer == null)
            {
                tempPostProcessLayer = mainCam.gameObject.AddComponent<PostProcessLayer>();
                tempPostProcessLayer.volumeLayer = -1; // set mask to everything, not sure why LayerMask.GetMask("Everything") set it to 0.
                tempPostProcessLayer.volumeTrigger = mainCam.transform;
            }
            else
            {
                previousPostProcessLayer = postProcessLayer.volumeLayer;
                previousPostProcessTrigger = postProcessLayer.volumeTrigger;
                postProcessLayer.volumeLayer = -1;
                postProcessLayer.volumeTrigger = mainCam.transform;
            }
        }

        public void CheckPostProcessProfileChanged(PostProcessProfile newPostProcessProfile)
        {
            if (newPostProcessProfile != currentPostProcessProfile)
            {
                currentPostProcessProfile = newPostProcessProfile;

                if (tempPostProcessVolume == null && currentPostProcessProfile != null)
                {
                    var tempGO = new GameObject("tempProstProcessVolume");
                    tempGO.hideFlags = HideFlags.HideAndDontSave;

                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (prefabStage != null)
                    {
                        tempGO.transform.parent = prefabStage.prefabContentsRoot.transform;
                    }
                    tempPostProcessVolume = tempGO.AddComponent<PostProcessVolume>();
                    tempPostProcessVolume.isGlobal = true;
                }

                tempPostProcessVolume.profile = currentPostProcessProfile;
                if (SceneView.lastActiveSceneView != null)
                    SceneView.lastActiveSceneView.Repaint();
            }
        }
#elif UNITY_SRP_POSTPROCESS
        Volume tempPostProcessVolume = null;
        public void CreatePostProcessVolumeAndLayer()
        {
            if (currentVolumeProfile == null) return; 

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            var tempGO = new GameObject("tempProstProcessVolume");
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            if (prefabStage != null)
            {
                tempGO.transform.parent = prefabStage.prefabContentsRoot.transform;
            }
            tempPostProcessVolume = tempGO.AddComponent<Volume>();
            tempPostProcessVolume.isGlobal = true;
            tempPostProcessVolume.profile = currentVolumeProfile;
            tempPostProcessVolume.priority = 1;
        }

        public void CheckPostProcessProfileChanged(VolumeProfile newVolumeProfile)
        {
            if (newVolumeProfile != currentVolumeProfile)
            {
                currentVolumeProfile = newVolumeProfile;

                if (tempPostProcessVolume == null && currentVolumeProfile != null)
                {
                    var tempGO = new GameObject("tempProstProcessVolume");
                    tempGO.hideFlags = HideFlags.HideAndDontSave;

                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (prefabStage != null)
                    {
                        tempGO.transform.parent = prefabStage.prefabContentsRoot.transform;
                    }
                    tempPostProcessVolume = tempGO.AddComponent<Volume>();
                    tempPostProcessVolume.isGlobal = true;
                    tempPostProcessVolume.priority = 1;
                }

                tempPostProcessVolume.profile = currentVolumeProfile;
                if (SceneView.lastActiveSceneView != null)
                    SceneView.lastActiveSceneView.Repaint();
            }
        }
#endif
        private bool prefabEnvironmentWasChanged = false;
        private bool prefabEnvironmentChanging = false;
#if UNITY_2020_1_OR_NEWER
        private bool prefabEnvironmentLoadFakePrefab = false;
#endif
        private void ApplyBackgroundSceneToPrefabStage(PrefabStage prefabStage)
        {
            prefabEnvironmentWasChanged = true;
            prefabEnvironmentChanging = true;
            // note :: unity has no open api way of adding scenes to prefabStage, or objects from scenes in a stable way, so we instead utilize unitys internal api for prefab environment.
            var previousPrefabStageEnvironment = EditorSettings.prefabRegularEnvironment;
            var previousPrefabStageUIEnvironment = EditorSettings.prefabUIEnvironment;
            EditorSettings.prefabRegularEnvironment = currentBackgroundSceneGUIProperty;
            EditorSettings.prefabUIEnvironment = currentBackgroundSceneGUIProperty;

            ReloadPrefabStage(prefabStage);

            EditorSettings.prefabRegularEnvironment = previousPrefabStageEnvironment;
            EditorSettings.prefabUIEnvironment = previousPrefabStageUIEnvironment;

            prefabEnvironmentChanging = false;

            //CheckNeedExtraLight(SceneView.lastActiveSceneView.sceneLighting);
        }

        private void ReloadPrefabStage(PrefabStage prefabStage)
        {

#if UNITY_2020_1_OR_NEWER
            // note :: Unity no longer reloads prefabs even if prefabEnvironment has changed. And provide no API to do so either resulting in this is a hacky workarround to force reload.
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.assetPath);
            var tempPrefabAssetPath = relativeAssetPath + fileName + "_TEMP_UNITY2020_GO.prefab";
            var tempGO = new GameObject("tempGO");
            var tempPrefab = PrefabUtility.SaveAsPrefabAsset(tempGO, tempPrefabAssetPath);
            prefabEnvironmentLoadFakePrefab = true;
            AssetDatabase.OpenAsset(tempPrefab);
            prefabEnvironmentLoadFakePrefab = false;
            AssetDatabase.DeleteAsset(tempPrefabAssetPath);
#else
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.prefabAssetPath);
#endif
            AssetDatabase.OpenAsset(prefab);
        }

        public void CheckBackGroundSceneChanged(SceneAsset newBackgroundScene)
        {
            if (newBackgroundScene != currentBackgroundSceneAsset)
            {
                if (currentBackgroundScene.HasValue)
                {
                    SceneManager.UnloadSceneAsync(currentBackgroundScene.Value, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    currentBackgroundScene = null;
                }

                currentBackgroundSceneGUIProperty = newBackgroundScene;
                currentBackgroundSceneAsset = newBackgroundScene;

                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    ApplyBackgroundSceneToPrefabStage(prefabStage);
                }
                else
                {
                    if (newBackgroundScene != null)
                    {
                        var newScenePath = AssetDatabase.GetAssetPath(currentBackgroundSceneGUIProperty);

                        // prevent opened scenes from being added as backgroundScenes
                        var openedSceneCount = EditorSceneManager.sceneCount;
                        for (int i = 0; i < openedSceneCount; i++)
                        {
                            var openedScene = EditorSceneManager.GetSceneAt(i);
                            if (openedScene.path == newScenePath)
                            {
                                return;
                            }
                        }

                        currentBackgroundScene = EditorSceneManager.OpenScene(newScenePath, OpenSceneMode.Additive);
                        if (ThumbnailGenerator.Settings.useBackgroundSceneLightning)
                            EditorSceneManager.SetActiveScene(currentBackgroundScene.Value);

                        foreach (var rootGameObjects in currentBackgroundScene.Value.GetRootGameObjects())
                        {
                            rootGameObjects.hideFlags = HideFlags.DontSave; // unitybug :: if hiding from inspector, it is not properly removed by unity when unloading scene.
                        }
                    }
                    if (SceneView.lastActiveSceneView != null)
                        SceneView.lastActiveSceneView.Repaint();
                }
            }
        }

        #region particlePlayback
        bool previousParticlePlaybackEnabled = false;
        float previousParticlePlaybackTime = 1;
        /* note : I intended for the use of Unity's built in play particle effect in the scene. 
           But there is a bug when you simulate all layers and stop, only selected layer will stop and all other will continue to play.
           So to rememdy that you can set at least set a specific playbackTime for particles.
           There is also a bug where a particlesystem needs to be selected... */
        void SetParticlePlayback(bool enabled, float time)
        {
            if (enabled == false)
            {
                if (previousParticlePlaybackEnabled)
                {
                    previousParticlePlaybackEnabled = false;
                    previousParticlePlaybackTime = -1;

                    foreach (var ps in GetParticleSystems())
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                }
                return;
            }

            // if no settings were changed we don't want to do anything.
            if (enabled == previousParticlePlaybackEnabled)
            {
                if (previousParticlePlaybackTime == time)
                {
                    return;
                }
            }

            previousParticlePlaybackEnabled = enabled;
            previousParticlePlaybackTime = time;

            var pSystems = GetParticleSystems();
            if (0 < pSystems.Length)
                Selection.objects = pSystems.Select(ps => ps.gameObject).ToArray();
            foreach (var ps in pSystems)
            {
                if (0 <= time)
                {
                    ps.Simulate(time, true, true);
                }
                else
                {
                    ps.Play(true);
                }
            }
        }
        ParticleSystem[] GetParticleSystems()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            ParticleSystem[] res;
            if (prefabStage != null && prefabStage.prefabContentsRoot.activeInHierarchy)
            {
                res = prefabStage.prefabContentsRoot.GetComponentsInChildren<ParticleSystem>();
            }
            else
            {
                res = FindObjectsOfType<ParticleSystem>(); // todo : this will be ultra slow for large scenes. Maybe need some caching/culling?
            }
            return res;
        }
        #endregion

#if UNITY_HDRP_POSTPROCESS
        private static bool showHDRPPostProcessWarning = true;
#endif
#if UNITY_2020_1_OR_NEWER
        private bool openedInPrefabStageInContext = false;
        private static bool showHPrefabStageInContextWarning = true;
#endif

        void OnGUI()
        {
            bool repaintSceneView = false;
            if (focusedWindow != null)
            {
                var windowType = focusedWindow.GetType();
                if (windowType == typeof(SceneView))
                {
                    useGameCamera = false;
                }
                else if ((windowType.FullName == "UnityEditor.GameView" || windowType.BaseType.FullName == "UnityEditor.PlayModeView")) // note: GameView type is internal :(
                {
                    useGameCamera = true;
                    repaintSceneView = true;
                    var targetDisplayField = windowType.GetProperty("targetDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
                    int targetDisplay = targetDisplayField != null ? (int)targetDisplayField.GetValue(focusedWindow) : 0;
                    gameCameras = Camera.allCameras.Where(cam => cam.targetDisplay == targetDisplay).ToArray();
                }
            }


            // note : now tries to use Unitys own SceneViewLight. Code kept for possible future use.
            // scenelights 
            //if (Application.isPlaying == false)
            //{
            //    if (SceneView.lastActiveSceneView != null)
            //        CheckNeedExtraLight(SceneView.lastActiveSceneView.sceneLighting);
            //}

            //if (tool != null && tool.camera != null && Light0 != null)
            //{
            //    Light0.transform.position = tool.camera.transform.position;
            //    Light1.transform.position = tool.camera.transform.position;
            //    Light0.transform.rotation = tool.camera.transform.rotation;
            //    Light1.transform.rotation = tool.camera.transform.rotation * Quaternion.Euler(340f, 218f, 177f);
            //}

            int showWarningSpace = 0;
#if UNITY_HDRP_POSTPROCESS
            if (showHDRPPostProcessWarning)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    showWarningSpace = 50;
                    EditorGUILayout.HelpBox("HDRP have very specific limits. Add dynamic resources with caution. ColorBuffer affects alpha output. See readme for more information how to enable alpha output.", MessageType.Warning);
                    if (GUILayout.Button(guiContentClose_small, new[] { GUILayout.Width(30), GUILayout.Height(20) }))
                    {                        
                        showHDRPPostProcessWarning = false;
                    }
                }
            }
#endif
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
#if UNITY_2020_1_OR_NEWER
            if (openedInPrefabStageInContext)
            {
                if (showHPrefabStageInContextWarning)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        showWarningSpace = 50;
                        EditorGUILayout.HelpBox("To prevent issues with prefab InContext mode. You can goto the source prefab.", MessageType.Warning);
                        using (new EditorGUILayout.VerticalScope())
                        {
                            if (GUILayout.Button(guiContentClose_small, new[] { GUILayout.Width(30), GUILayout.Height(20) }))
                            {
                                showHPrefabStageInContextWarning = false;
                            }
                            if (GUILayout.Button(guiContentPrefab, new[] { GUILayout.Width(30), GUILayout.Height(20) }))
                            {
                                ReloadPrefabStage(prefabStage);
                                ToolManager.SetActiveTool<ThumbnailGeneratorTool>();
                            }
                        }
                    }
                }
            }
#endif

            // texture size
            GUILayout.Space(5);
            var prevWidth = thumbnailWidth;
            var prevHeight = thumbnailHeight;
            if (limitTextureSize)
            {
                thumbnailWidth = EditorGUILayout.IntSlider("Texture Width", thumbnailWidth, 16, 2048);
                thumbnailHeight = EditorGUILayout.IntSlider("Texture Height", thumbnailHeight, 16, 2048);
            }
            else
            {
                thumbnailWidth = EditorGUILayout.IntSlider("Texture Width", thumbnailWidth, 1, 16384);
                thumbnailHeight = EditorGUILayout.IntSlider("Texture Height", thumbnailHeight, 1, 16384);
            }
            if (thumbnailWidth == 0)
                thumbnailWidth = 1;
            if (thumbnailHeight == 0)
                thumbnailHeight = 1;
            if (prevWidth != thumbnailWidth || prevHeight != thumbnailHeight)
            {
                repaintSceneView = true;
            }
            limitTextureSize = EditorGUILayout.Toggle("Limit texturesize", limitTextureSize);

            // particle systems
            if (Application.isPlaying)
            {
                GUI.enabled = false;
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                playbackParticleSystems = EditorGUILayout.Toggle("Play Particles", playbackParticleSystems);
                GUI.enabled = playbackParticleSystems;
                particlePlaybackTime = EditorGUILayout.Slider("Particle Playback Time", particlePlaybackTime, -1, 20);
                GUI.enabled = true;
            }
            if (Application.isPlaying)
            {
                GUI.enabled = true;
            }
            else
            {
                SetParticlePlayback(playbackParticleSystems, particlePlaybackTime);
            }



            // buttons
            SceneView sceneView = SceneView.lastActiveSceneView;
            Camera sceneCam = null;
            SceneView.CameraSettings sceneCamSettings = null;
            if (sceneView != null)
            {
                sceneCam = SceneView.lastActiveSceneView.camera;
                sceneCamSettings = SceneView.lastActiveSceneView.cameraSettings;
            }
            Action settingsAndAnimationButtons = () =>
            {
                GUILayout.FlexibleSpace();
                // note :: reference from https://unitylist.com/p/5c3/Unity-editor-icons icon paths might get updated
                if (GUILayout.Button(guiContentGotoSettings, new[] { GUILayout.MaxWidth(180), GUILayout.Height(20) }))
                {
                    Selection.activeObject = ThumbnailGenerator.Settings;
                }
                GUILayout.FlexibleSpace();


                if (GUILayout.Button(guiContentAnimationWindow, new[] { GUILayout.MaxWidth(180), GUILayout.Height(20) }))
                {
                    EditorApplication.ExecuteMenuItem("Window/Animation/Animation");
                }

            };
            Action saveLoadCameraSettings = () =>
            {
                GUILayout.FlexibleSpace();
                if (sceneCam == null)
                    GUI.enabled = false;

                if (GUILayout.Button(guiContentSaveCameraPosition, new[] { GUILayout.MaxWidth(180), GUILayout.Height(20) }))
                {
                    ThumbnailGenerator.Settings.cameraPosition = sceneCam.transform.position;
                    ThumbnailGenerator.Settings.cameraRotation = sceneCam.transform.rotation.eulerAngles;
                    ThumbnailGenerator.Settings.projection = sceneCam.orthographic ? Projection.Orthographic : Projection.Perspective;
                    ThumbnailGenerator.Settings.fieldOfView = sceneCam.fieldOfView;
                    ThumbnailGenerator.Settings.orthopgrahicSize = sceneCam.orthographicSize;
                }
                GUILayout.FlexibleSpace();


                if (GUILayout.Button(guiContentLoadCameraPosition, new[] { GUILayout.MaxWidth(180), GUILayout.Height(20) }))
                {
                    sceneCam.transform.position = ThumbnailGenerator.Settings.cameraPosition;
                    sceneCam.transform.rotation = Quaternion.Euler(ThumbnailGenerator.Settings.cameraRotation);
                    sceneView.AlignViewToObject(sceneCam.transform);
                    sceneCamSettings.fieldOfView = ThumbnailGenerator.Settings.fieldOfView;
                    sceneView.orthographic = ThumbnailGenerator.Settings.projection == Projection.Orthographic;
                }
                GUI.enabled = true;
            };
            GUILayout.Space(4);

            if (EditorGUIUtility.currentViewWidth < 180 * 4)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    settingsAndAnimationButtons();
                    GUILayout.Space(10);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    saveLoadCameraSettings();
                    GUILayout.Space(10);
                }
            }
            else
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    settingsAndAnimationButtons();
                    saveLoadCameraSettings();
                }
            }
            GUILayout.Space(4);


            //SetAnimationPlayback(playbackAnimations, animationPlaybackTime);

            using (new EditorGUILayout.HorizontalScope())
            {
                backgroundColor = EditorGUILayout.ColorField("BackgroundColor", backgroundColor);
            }

            if (Application.isPlaying)
            {
                GUI.enabled = false;
            }

            GUILayout.Space(10);
            SceneAsset backgroundScene = null;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Use BackgroundScene: ");
                backgroundScene = EditorGUILayout.ObjectField(currentBackgroundSceneGUIProperty, typeof(SceneAsset), false) as SceneAsset;
                if (Application.isPlaying == false)
                {
                    CheckBackGroundSceneChanged(backgroundScene);
                }
                GUILayout.Space(10);
            }

            if (Application.isPlaying)
            {
                GUI.enabled = true;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Apply PostProcessMaterial: ");
                postProcessMaterial = EditorGUILayout.ObjectField(postProcessMaterial, typeof(Material), false) as Material;
                GUILayout.Space(10);
            }

            if (Application.isPlaying)
            {
                GUI.enabled = false;
            }

            // postprocess profiles
#if UNITY_POSTPROCESS
            PostProcessProfile fieldPostProcessProfile = null;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Apply PostProcessProfile: ");
                fieldPostProcessProfile = EditorGUILayout.ObjectField(currentPostProcessProfile, typeof(PostProcessProfile), false) as PostProcessProfile;
                CheckPostProcessProfileChanged(fieldPostProcessProfile);
                GUILayout.Space(10);
            }
            GUILayout.Space(12);
#elif UNITY_SRP_POSTPROCESS
            // error :: The volume is not applied dynamically. So we force scene editing volymes to prevent issues.
            VolumeProfile fieldPostProcessProfile = null;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Apply PostProcessProfile: ");
                fieldPostProcessProfile = EditorGUILayout.ObjectField(currentVolumeProfile, typeof(VolumeProfile), false) as VolumeProfile;
                CheckPostProcessProfileChanged(fieldPostProcessProfile);
                GUILayout.Space(10);
            }
            GUILayout.Space(12);
#else
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Apply PostProcessProfile: ");
                GUI.enabled = false;
                var userEnteredProfileNoAssembly = EditorGUILayout.ObjectField(null, typeof(UnityEngine.Object), false);
                GUI.enabled = true;
                GUILayout.Space(10);
            }
            GUILayout.Space(12);            
#endif

            if (Application.isPlaying)
            {
                GUI.enabled = true;
            }

            // numberingSuffix and browse custom assetPath
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5);
                numberingSuffix = (NumberingSuffix)EditorGUILayout.EnumPopup("AddNumberingSuffix", numberingSuffix);
                GUILayout.Space(10);


                if (GUILayout.Button(guiContentBrowseFilepath, new[] { GUILayout.MaxWidth(200), GUILayout.Height(20) }))
                {
                    string relativeAssetPath = EditorUtility.SaveFilePanelInProject("Select save path", $"{userOverrideFileName ?? fileName}.png" ?? "GeneratedThumbnail.png", "png", "Name your thumbnail");

                    if (string.IsNullOrEmpty(relativeAssetPath) == false)
                    {
                        var mapsInAssetPath = relativeAssetPath.Split('/');
                        userOverrideRelativeAssetPath = string.Empty;
                        for (int i = 0; i < mapsInAssetPath.Length - 1; i++)
                        {
                            userOverrideRelativeAssetPath += mapsInAssetPath[i] + '/';
                        }
                        userOverrideFileName = mapsInAssetPath.Last();
                        userOverrideFileName = userOverrideFileName.Remove(userOverrideFileName.Length - ".png".Length);
                    }
                }
                GUILayout.Space(10);
            }
            // display assetPath format
            var assetPathInfo = new AssetPathInfo(relativeAssetPath, fileName, numberingSuffix, AssetPathInfo.AssetType.scene, userOverrideRelativeAssetPath, userOverrideFileName);
            if (prefabStage != null)
            {
                assetPathInfo.assetType = AssetPathInfo.AssetType.prefab;
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (numberingSuffix == NumberingSuffix.Always)
                {
                    string suffixFormat = ThumbnailGenerator.Settings.suffixFormat;
                    if (string.IsNullOrEmpty(suffixFormat))
                    {
                        suffixFormat = "{filename}_{x}";
                    }
                    string fileName = suffixFormat.Replace("{filename}", assetPathInfo.GetFileName());
                    GUILayout.Label(assetPathInfo.GetAssetFolderPath() + fileName + ".png");
                }
                else
                {
                    GUILayout.Label(assetPathInfo.GetAssetFolderPath() + assetPathInfo.GetFileName() + ".png");
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.FlexibleSpace();

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = ToolManager.IsActiveTool(tool);
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(guiContentGenerateThumbnail, new[] { GUILayout.MaxWidth(220), GUILayout.Height(30) }))
                {
                    GenerateThumbnail();
                }
                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
                GUI.enabled = true;
            }
            // display the actual created assetPath
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                GUILayout.Label(thumbnailResult?.assetPath ?? "Generate thumbnail to show preview");
                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
            }
            // display the generated thumbnail
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();

                // note :: width/height calculation is not perfect. We just guess the height of the other content.
                var tempCol = GUI.color;
                GUI.color = Color.white;
                GUILayout.Box(thumbnailResult?.tex ?? placementTexture, guiStyle, new[] { GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false), GUILayout.Width(position.width - 10), GUILayout.Height(position.height - 370 - showWarningSpace) });
                GUI.color = tempCol;
                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
            }

            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(guiContentClose, new[] { GUILayout.MaxWidth(150), GUILayout.Height(20) }))
                {
                    Close();
                }
                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
            }
            GUILayout.Space(10);

            if (repaintSceneView && SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.Repaint();
            }
        }


        private void OnDestroy()
        {
            EditorApplication.quitting -= Close;

            if (ThumbnailGenerator.Settings.rememberSessionSettings)
            {
                applySettingsOnRestart = new ToolWindow_SettingsSnapshot(this);
            }
            CleanupScene();
            CleanupWindow();
            CleanupPrefabStage();
        }

        public void CleanupPrefabStage()
        {
            // cleaned prefabStage
            if (closingDuoToOtherPrefabOpening == false && prefabEnvironmentWasChanged && prefabEnvironmentChanging == false)
            {
                prefabEnvironmentWasChanged = false;
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    ReloadPrefabStage(prefabStage);
                }
            }
            closingDuoToOtherPrefabOpening = false;
        }

        private bool hasBackgroundSceneCleanupResponsability = true;
        private bool sceneMarkedForDelete = false;
        private bool closingDuoToOtherPrefabOpening = false;

        private void CleanupWindow()
        {
            if (prefabEnvironmentChanging == false)
                instance = null;

            PrefabStage.prefabStageOpened -= PrefabStageOpened;
            PrefabStage.prefabStageClosing -= PrefabStageClosed;
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;

            ThumbnailGeneratorTool.window = null;
            if (ToolManager.IsActiveTool(tool))
                ToolManager.RestorePreviousTool();
        }

        void PrefabStageClosed(PrefabStage stage)
        {
#if UNITY_2020_1_OR_NEWER
            if (prefabEnvironmentLoadFakePrefab) return;
#elif UNITY_2019
            // note: in older unity prefabStage has not ben nulled yet. So if prefabStage is same as closing one, we don't try to restart it.
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabEnvironmentChanging == false && prefabStage == stage)
                prefabEnvironmentWasChanged = false;
#endif

            CleanupScene();
            if (prefabEnvironmentChanging == false)
            {
                Close();
            }
        }

        // to prevent bugs, we close the window when changing between focused scenes
        void PrefabStageOpened(PrefabStage stage)
        {
#if UNITY_2020_1_OR_NEWER
            if (prefabEnvironmentLoadFakePrefab) return;
#endif
            if (sceneMarkedForDelete == false)
            {
                hasBackgroundSceneCleanupResponsability = false;
                if (currentBackgroundScene.HasValue)
                {
                    EditorSceneManager.CloseScene(currentBackgroundScene.Value, true);
                }
            }

            if (prefabEnvironmentChanging == false)
            {
                closingDuoToOtherPrefabOpening = true;
                Close();
            }
        }

        private void CleanupScene()
        {
            sceneMarkedForDelete = true;

            if (hasBackgroundSceneCleanupResponsability && currentBackgroundScene.HasValue)
            {
                EditorSceneManager.CloseScene(currentBackgroundScene.Value, true);
            }

#if UNITY_POSTPROCESS
            if (tempPostProcessVolume != null && tempPostProcessVolume.gameObject != null)
            {
                DestroyImmediate(tempPostProcessVolume.gameObject);
            }
            if (tempPostProcessLayer != null)
            {
                DestroyImmediate(tempPostProcessLayer);
            }
            else // reset mainCamera to previous settings
            {
                var mainCam = Camera.main;
                if (mainCam != null)
                {
                    var postProcessLayer = mainCam.gameObject.GetComponent<PostProcessLayer>();
                    if (postProcessLayer != null && previousPostProcessLayer != null)
                    {
                        postProcessLayer.volumeLayer = previousPostProcessLayer.Value;
                    }
                    if (postProcessLayer != null && previousPostProcessTrigger != null)
                    {
                        postProcessLayer.volumeTrigger = previousPostProcessTrigger;
                    }
                }
            }
#elif UNITY_SRP_POSTPROCESS
            if(tempPostProcessVolume != null && tempPostProcessVolume.gameObject != null)
            {
                DestroyImmediate(tempPostProcessVolume.gameObject);
            }
#endif
            //if (Light0 != null && Light0.gameObject != null)
            //{
            //    DestroyImmediate(Light0.gameObject);
            //}
            //if (Light1 != null && Light1.gameObject != null)
            //{
            //    DestroyImmediate(Light1.gameObject);
            //}
            if (Application.isPlaying == false)  // todo: check if isPlaying is true even when game is paused.
            {
                foreach (var ps in GetParticleSystems())
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }


        private void PlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                Close();
            }
        }


        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}