// Author: Tobias Löf Melker
// Created: 2020 for usage with Unity Engine

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using NumberingSuffix = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.NumberingSuffix;
using ModifyCanvas = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.ModifyCanvas;
using Projection = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.Projection;
using CameraBackground = UnityEditor.EditorTools.TLM.ThumbnailGenerator.ThumbnailGeneratorSettings.CameraBackground;
#if UNITY_POSTPROCESS
using UnityEngine.Rendering.PostProcessing;
#elif UNITY_SRP_POSTPROCESS
using UnityEngine.Rendering;
#if UNITY_URP_POSTPROCESS
using UnityEngine.Rendering.Universal;
#elif UNITY_HDRP_POSTPROCESS
using UnityEngine.Rendering.HighDefinition;
#endif
#endif
#if !UNITY_2020_2_OR_NEWER
using ToolManager = UnityEditor.EditorTools.EditorTools;
#endif

// the core class for generating / saving the thumbnail indepentend from tool/quick generate.
namespace UnityEditor.EditorTools.TLM.ThumbnailGenerator
{
    public static class ThumbnailGenerator
    {
        public static ThumbnailGeneratorSettings Settings;
        static ThumbnailGenerator()
        {
            // hack :: a bit of hack to get a referenced field. It would be better if we could read the default fields for .meta files. 
            // It's not the best way of doing it, but I do not want hard coded paths to files in case a user wants to relocate the tool.
            var tempSettingsReference = ScriptableObject.CreateInstance<ThumbnailGeneratorSettings>();
            if (tempSettingsReference.serializedSettingsReference == null)
            {
                Debug.LogError("Default ThumbnailGeneratorSettings missing! Please try reimporting the plugin or create a new settingsObject and set is as default in the scriptableObject");
                Settings = tempSettingsReference;
            }
            else Settings = tempSettingsReference.serializedSettingsReference;
        }
        [MenuItem("Assets/Create/Thumbnail/Open Thumbnail Generator", isValidateFunction: false, priority: 211)]
        static void ThumbnailWindow()
        {
            ToolManager.SetActiveTool<ThumbnailGeneratorTool>();
        }

        [MenuItem("Assets/Create/Thumbnail/Open With Selected Prefab", isValidateFunction: false, priority: 212)]
        static void ThumbnailWindow_SelectPrefab()
        {
            if (Selection.activeObject is GameObject prefab)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(prefab))
                {
                    prefab = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                }

                AssetDatabase.OpenAsset(prefab);
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                Selection.activeObject = prefabStage.prefabContentsRoot;
                ToolManager.SetActiveTool<ThumbnailGeneratorTool>();
            }
        }

        [MenuItem("Assets/Create/Thumbnail/Open With Selected Prefab", isValidateFunction: true, priority: 212)]
        static bool AssetIsSingleSelectedPrefabValidation()
        {
            if (Selection.objects.Length == 1 && Selection.activeObject is GameObject prefab)
            {
                return PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.Variant;
            }
            else if (Selection.objects.Length == 0)
            {
                return true;
            }

            return false;
        }


        [MenuItem("Assets/Create/Thumbnail/Quick Generate Thumbnail", isValidateFunction: true, priority: 213)]
        static bool AssetIsPrefabValidation()
        {
            // Could possibly add other objects that are not prefabs for quick generate. But rather keep the version stable for now.
            if (Selection.activeObject is GameObject prefab)
            {
                return PrefabUtility.GetPrefabAssetType(prefab) != PrefabAssetType.NotAPrefab;
            }
            return false;
        }
        [MenuItem("Assets/Create/Thumbnail/Thumbnail Generator Settings ", isValidateFunction: false, priority = 215)]
        static void GotoThumbnailGeneratorSettings()
        {
            Selection.activeObject = ThumbnailGenerator.Settings;
        }

        [MenuItem("Assets/Create/Thumbnail/Extract Unity Thumbnail", isValidateFunction: true, priority: 214)]
        static bool AssetHasPreview()
        {
            return AssetPreview.GetAssetPreview(Selection.activeObject) != null;
        }

        [MenuItem("Assets/Create/Thumbnail/Quick Generate Thumbnail", isValidateFunction: false, priority: 213)]
        static void QuickGeneratePrefabThumbnail()
        {
            var renderUtil = new PreviewRenderUtility();
            var selectedObjects = Selection.objects;

            if (Settings.backgroundScene != null)
            {
                var backgroundScene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(Settings.backgroundScene), OpenSceneMode.Additive);
                foreach (var rootObject in backgroundScene.GetRootGameObjects())
                {
                    EditorSceneManager.MoveGameObjectToScene(rootObject, renderUtil.camera.scene);
                }
                if (EditorSceneManager.GetActiveScene() == backgroundScene)
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(Settings.backgroundScene), OpenSceneMode.Single);
                }
                else
                {
                    EditorSceneManager.CloseScene(backgroundScene, true);
                }
            }

            if (Settings.cameraBackgroundType == CameraBackground.SolidColor)
            {
                renderUtil.camera.clearFlags = CameraClearFlags.SolidColor;
            }
            else
            {
                renderUtil.camera.clearFlags = CameraClearFlags.Skybox;
            }
            renderUtil.camera.orthographic = Settings.projection == Projection.Orthographic;
            renderUtil.camera.orthographicSize = Settings.orthopgrahicSize;
            renderUtil.camera.fieldOfView = Settings.fieldOfView;
            renderUtil.camera.transform.localPosition = Settings.cameraPosition;
            renderUtil.camera.transform.localEulerAngles = Settings.cameraRotation;
            renderUtil.camera.nearClipPlane = Settings.nearClipPlane;
            renderUtil.camera.farClipPlane = Settings.farClipPlane;

            renderUtil.lights[0].transform.position = renderUtil.camera.transform.position;
            renderUtil.lights[1].transform.position = renderUtil.camera.transform.position;
            renderUtil.lights[0].transform.rotation = renderUtil.camera.transform.rotation;
            renderUtil.lights[1].transform.rotation = renderUtil.camera.transform.rotation * Quaternion.Euler(340f, 218f, 177f);

#if UNITY_POSTPROCESS
            if (Settings.postProcessProfile != null)
            {
                // note :: We attach everything to camera so renderUtil cleanup will cleanup the postProcessComponents. And we don't have to worry about renderUtil.AddSingleGO being deprecated.
                var tempPostProcessVolume = renderUtil.camera.gameObject.AddComponent<PostProcessVolume>();
                tempPostProcessVolume.isGlobal = true;
                tempPostProcessVolume.profile = Settings.postProcessProfile;
                var tempPostProcessLayer = renderUtil.camera.gameObject.AddComponent<PostProcessLayer>();
                tempPostProcessLayer.volumeLayer = -1; // set mask to everything, not sure why LayerMask.GetMask("Everything") set it to 0.
                tempPostProcessLayer.volumeTrigger = renderUtil.camera.transform;
            }
#elif UNITY_URP_POSTPROCESS
            // error :: the volume is not applied.
            //var uac = renderUtil.camera.GetComponent<UniversalAdditionalCameraData>();
            //if (uac == null)
            //{
            //    uac = renderUtil.camera.gameObject.AddComponent<UniversalAdditionalCameraData>();
            //}
            //uac.renderPostProcessing = true;
            //uac.volumeLayerMask = LayerMask.GetMask("Default");
            //uac.volumeTrigger = renderUtil.camera.transform;
            //if (Settings.volumeProfile != null)
            //{
            //    // note :: We attach everything to camera so renderUtil cleanup will cleanup the postProcessComponents. And we don't have to worry about renderUtil.AddSingleGO being deprecated.
            //    var tempPostProcessVolume = renderUtil.camera.gameObject.AddComponent<Volume>();
            //    tempPostProcessVolume.isGlobal = true;
            //    tempPostProcessVolume.sharedProfile = Settings.volumeProfile;
            //    tempPostProcessVolume.weight = 1;
            //}
#elif UNITY_HDRP_POSTPROCESS
            //error :: The volume is not applied.
            //var uac = renderUtil.camera.GetComponent<HDAdditionalCameraData>();
            //if (uac == null)
            //{
            //    uac = renderUtil.camera.gameObject.AddComponent<HDAdditionalCameraData>();
            //}
            //uac.backgroundColorHDR = Settings.backgroundColor;
            //if (Settings.quickCreateCameraClear == ThumbnailGeneratorSettings.CameraClearOption.SolidColor)
            //{
            //    uac.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
            //}
            //else
            //{
            //    uac.clearColorMode = HDAdditionalCameraData.ClearColorMode.Sky;
            //}
            //uac.volumeLayerMask = LayerMask.GetMask("Default");
            //if (Settings.volumeProfile != null)
            //{
            //    // note :: We attach everything to camera so renderUtil cleanup will cleanup the postProcessComponents. And we don't have to worry about renderUtil.AddSingleGO being deprecated.
            //    var tempPostProcessVolume = renderUtil.camera.gameObject.AddComponent<Volume>();
            //    tempPostProcessVolume.isGlobal = true;
            //    tempPostProcessVolume.sharedProfile = Settings.volumeProfile;
            //    tempPostProcessVolume.weight = 1;
            //}
#endif

            var width = Settings.thumbnailWidth;
            var height = Settings.thumbnailHeight;
            renderUtil.BeginPreview(new Rect(0, 0, width, height), null);
            renderUtil.camera.backgroundColor = Settings.backgroundColor;
            var previousRenderTexture = RenderTexture.active;
            RenderTexture temporaryRenderTexture = RenderTexture.GetTemporary(width, height);
            RenderTexture.active = temporaryRenderTexture;

            foreach (var obj in selectedObjects)
            {
                if (obj is GameObject prefabs)
                {
                    var instancedPrefabs = renderUtil.InstantiatePrefabInScene(prefabs);

                    if (Settings.playParticles)
                    {
                        foreach (var ps in instancedPrefabs.GetComponentsInChildren<ParticleSystem>())
                        {
                            ps.Simulate(Settings.particlePlayBackTime, true, true);
                        }
                    }

                    if (Settings.playAnimationClips)
                    {
                        foreach (var anim in instancedPrefabs.GetComponentsInChildren<Animation>())
                        {
                            if (anim.clip != null)
                            {
                                anim.clip.SampleAnimation(anim.gameObject, Settings.animationPlayBackTime);
                            }
                        }
                        foreach (var animator in instancedPrefabs.GetComponentsInChildren<Animator>())
                        {
                            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                            float normalizedTime;
                            if (stateInfo.length == 0)
                                normalizedTime = Settings.animationPlayBackTime;
                            else
                                normalizedTime = Settings.animationPlayBackTime / stateInfo.length;
                            animator.StartPlayback();
                            animator.Play(0, -1, normalizedTime);
                            animator.Update(0f);
                        }
                    }
#if UNITY_SRP_POSTPROCESS
                    renderUtil.Render(allowScriptableRenderPipeline: true, updatefov: false);
#else
                    renderUtil.Render(allowScriptableRenderPipeline: false, updatefov: false);
#endif
                    // note :: Wanted to use Unitys built in EndStaticPreview. But Unity doesn't blit the alpha to returned texture on StaticPreview... So we use a workarround and create our own tex2D.
                    var rendTexture = renderUtil.camera.targetTexture;

                    // note :: for some reason unity decides that 1024 is minimum, so we blit to downscale.
                    if (Settings.postProcessMaterial != null)
                    {
                        Graphics.Blit(rendTexture, temporaryRenderTexture, Settings.postProcessMaterial);
                    }
                    else
                    {
                        Graphics.Blit(rendTexture, temporaryRenderTexture);
                    }
                    Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                    newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
                    newTexture.Apply();

                    var assetPathInfo = new AssetPathInfo(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabs), prefabs.name, Settings.quickGenerateSuffix, AssetPathInfo.AssetType.prefab);
                    var generatedAssetPath = GenerateThumbnailAssetPaths(assetPathInfo);
                    SaveThumbnail(newTexture, generatedAssetPath);
                    UnityEngine.Object.DestroyImmediate(instancedPrefabs);
                }
            }
            renderUtil.EndPreview();
            renderUtil.Cleanup();
            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(temporaryRenderTexture);

        }

        [MenuItem("Assets/Create/Thumbnail/Extract Unity Thumbnail", isValidateFunction: false, priority: 214)]
        static void ExtractUnityThumbnail()
        {
            foreach (var obj in Selection.objects)
            {
                var assetPreview = AssetPreview.GetAssetPreview(obj);
                if (assetPreview == null)
                    continue;

                var numberingSuffix = Settings.numberingSuffix;
                if (numberingSuffix == NumberingSuffix.None)
                {
                    numberingSuffix = NumberingSuffix.Auto;
                }

                if (Settings.extractUsingStaticPreview)
                {
                    var edit = Editor.CreateEditor(obj);
                    var newTexture = edit.RenderStaticPreview(obj.name, new[] { obj }, Settings.thumbnailWidth, Settings.thumbnailHeight);
                    var assetPathInfo = new AssetPathInfo(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj), obj.name, numberingSuffix, AssetPathInfo.AssetType.prefab);
                    var generatedAssetPath = GenerateThumbnailAssetPaths(assetPathInfo);
                    SaveThumbnail(newTexture, generatedAssetPath);
                }
                else
                {
                    var assetPathInfo = new AssetPathInfo(AssetDatabase.GetAssetPath(obj), obj.name, numberingSuffix, AssetPathInfo.AssetType.prefab);
                    var generatedAssetPath = GenerateThumbnailAssetPaths(assetPathInfo);
                    SaveThumbnail(assetPreview, generatedAssetPath);
                }
            }
        }

        public struct AssetPathInfo
        {
            public enum AssetType
            {
                prefab,
                scene
            }
            public string relativeAssetPath;
            public string assetName;
            public NumberingSuffix numberingSuffix;
            public AssetType assetType;
            public string overriddenFilePath;
            public string overriddenFileName;

            public AssetPathInfo(string relativeAssetPath, string assetPath, NumberingSuffix numberingSuffix, AssetType assetType, string overriddenFilePath = null, string overriddenFileName = null)
            {
                this.relativeAssetPath = relativeAssetPath;
                this.assetName = assetPath;
                this.numberingSuffix = numberingSuffix;
                this.assetType = assetType;
                this.overriddenFilePath = overriddenFilePath;
                this.overriddenFileName = overriddenFileName;
            }

            public string GetAssetFolderPath()
            {
                if (string.IsNullOrEmpty(overriddenFilePath) == false)
                {
                    return overriddenFilePath;
                }
                else if (assetType == AssetType.prefab && string.IsNullOrEmpty(Settings.prefabPath) == false)
                {
                    return Settings.prefabPath;
                }
                else if (assetType == AssetType.scene && string.IsNullOrEmpty(Settings.scenePath) == false) 
                {
                    return Settings.scenePath;
                }
                else
                    return relativeAssetPath;
            }

            public string GetFileName()
            {
                if (string.IsNullOrEmpty(overriddenFileName) == false)
                {
                    return overriddenFileName;
                }
                else if (assetType == AssetType.prefab && string.IsNullOrEmpty(Settings.prefabFileName) == false)
                {
                    return Settings.prefabFileName.Replace("{assetname}", assetName);
                }
                else if (assetType == AssetType.scene && string.IsNullOrEmpty(Settings.sceneFileName) == false)
                {
                    return Settings.sceneFileName.Replace("{assetname}", assetName); ;
                }
                else
                {
                    return assetName;
                }
            }
        }

        public struct GeneratedThumbnailAssetPaths
        {
            public string relativeThumbnailPath;
            public string absoluteThumbnailPath;

            public GeneratedThumbnailAssetPaths(string relativeThumbnailPath, string absoluteThumbnailPath)
            {
                this.relativeThumbnailPath = relativeThumbnailPath;
                this.absoluteThumbnailPath = absoluteThumbnailPath;
            }
        }

        // Creates the actual filepath, checks if filepath already exist dependant on numberSuffix settings.
        // None: If file exists. Overwrite file.
        // FileExists: If file exists. Add a suffix to filename (or increase suffix by 1).
        // Always: Add a suffix to filename (or increase suffix by 1)
        public static GeneratedThumbnailAssetPaths GenerateThumbnailAssetPaths(AssetPathInfo assetPathInfo)
        {
            int suffixNumber = 1;

            var mapsInAssetPath = assetPathInfo.GetAssetFolderPath().Split('/');
            string dataPath = Application.dataPath + '/';
            string localAssetPath = string.Empty;
            for (int i = 0; i < mapsInAssetPath.Length - 1; i++)
            {
                localAssetPath += mapsInAssetPath[i] + '/';
            }
            for (int i = 1; i < mapsInAssetPath.Length - 1; i++)
            {
                dataPath += mapsInAssetPath[i] + '/';
            }

            string fileName = assetPathInfo.GetFileName() + ".png";

            if (assetPathInfo.numberingSuffix != NumberingSuffix.None)
            {
                string suffixFormat = Settings.suffixFormat;
                if (string.IsNullOrEmpty(suffixFormat)) // todo :: validate format?
                {
                    Debug.LogWarning("ThumbnailGeneratorSettings numberingSuffix was empty, value was automatically set to default {filename}_{x}");
                    Settings.suffixFormat = "{filename}_{x}";
                    suffixFormat = Settings.suffixFormat;
                }
                else if (suffixFormat.Contains("{filename}") == false || suffixFormat.Contains("{x}") == false)
                {
                    Debug.LogWarning("ThumbnailGeneratorSettings numberingSuffix did not contain both {filename} and {x}, value was automatically set to default {filename}_{x}");
                    Settings.suffixFormat = "{filename}_{x}";
                    suffixFormat = Settings.suffixFormat;
                }

                suffixFormat = suffixFormat.Replace("{filename}", assetPathInfo.GetFileName());

                if (assetPathInfo.numberingSuffix == NumberingSuffix.Always)
                    fileName = suffixFormat.Replace("{x}", suffixNumber.ToString()) + ".png";

                string tempFilePath = dataPath + fileName;
                while (System.IO.File.Exists(tempFilePath))
                {
                    suffixNumber++;
                    fileName = suffixFormat.Replace("{x}", suffixNumber.ToString()) + ".png";
                    tempFilePath = dataPath + fileName;
                }
            }

            localAssetPath += fileName;
            dataPath += fileName;

            return new GeneratedThumbnailAssetPaths(localAssetPath, dataPath);
        }

        public class ThumbnailResult : IDisposable
        {
            public Texture2D tex;
            public string assetPath;

            #region IDisposable Support
            private bool disposedValue = false;

            public void Dispose(bool disposing = true)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        if (Application.isPlaying)
                            UnityEngine.Object.Destroy(tex);
                        else
                            UnityEngine.Object.DestroyImmediate(tex);
                    }

                    tex = null;
                    assetPath = null;
                    disposedValue = true;
                }
            }

            void IDisposable.Dispose()
            {
                Dispose(true);
            }
            #endregion
        }

        // save the thumbnail to disc
        public static ThumbnailResult SaveThumbnail(Texture2D tex, GeneratedThumbnailAssetPaths assetPaths)
        {
            // encode and save Texture2D as .png
            byte[] bytes = tex.EncodeToPNG();
            System.IO.FileInfo file = new System.IO.FileInfo(assetPaths.absoluteThumbnailPath);
            file.Directory.Create();
            System.IO.File.WriteAllBytes(assetPaths.absoluteThumbnailPath, bytes);

            // set import settigs
            if (Application.isPlaying == false)
            {
                AssetDatabase.ImportAsset(assetPaths.relativeThumbnailPath, ImportAssetOptions.ForceSynchronousImport);
                SetTextureImportSettings(assetPaths.relativeThumbnailPath);
            }
            else
            {
                if (assetPathsAddedDuringPlayMode != null)
                {
                    assetPathsAddedDuringPlayMode.Add(assetPaths.relativeThumbnailPath);
                }
                else
                {
                    assetPathsAddedDuringPlayMode = new List<string>() { assetPaths.relativeThumbnailPath };
                    EditorApplication.playModeStateChanged += PlayModeExitSetImportSettings;
                }
            }

            return new ThumbnailResult
            {
                tex = tex,
                assetPath = assetPaths.relativeThumbnailPath
            };
        }

        /// <summary>
        /// Set import settings for created Texture. Texture should be imported before calling setSettings.
        /// </summary>
        /// <param name="relativePath"></param>
        private static void SetTextureImportSettings(string relativePath)
        {
            var textureImportSettings = (TextureImporter)AssetImporter.GetAtPath(relativePath);
            textureImportSettings.isReadable = true;
            textureImportSettings.textureType = TextureImporterType.Sprite;
            textureImportSettings.alphaIsTransparency = true;
            textureImportSettings.mipmapEnabled = false;
            textureImportSettings.isReadable = false;
            textureImportSettings.SaveAndReimport();
        }

        private static List<string> assetPathsAddedDuringPlayMode = null;
        private static void PlayModeExitSetImportSettings(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.playModeStateChanged -= PlayModeExitSetImportSettings;

                // note: Import settings must be done in unity main-thread, can not make it into parallel foreach :/
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                foreach (var asset in assetPathsAddedDuringPlayMode)
                {
                    SetTextureImportSettings(asset);
                }
                assetPathsAddedDuringPlayMode = null;
            }
        }

        //static MethodInfo sceneViewCustomLightMethod = typeof(SceneView).GetMethod("SetupCustomSceneLighting", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo sceneViewGetLights = typeof(SceneView).GetField("m_Light", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo sceneViewGetColor = typeof(SceneView).GetField("kSceneViewMidLight", BindingFlags.NonPublic | BindingFlags.Static);

        // note :: Uses lastActiveSceneView to modify camera. If you intend to call this function yourself, you might want to copy it and remove the extra code added to support various editor states.
        public static Texture2D GenerateThumbnailTexture(Camera[] cameras, int thumbnailWidth, int thumbnailHeight, Color backgroundColor, Material postProcessMaterial, bool isPreview = false)
        {
            Texture2D newThumbnailTexture = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.RGBA32, false);
            var canvases = ModifyCanvases(cameras[0], isPreview);

            var previousRenderTexture = RenderTexture.active;
            var temporaryRenderTexture = RenderTexture.GetTemporary(thumbnailWidth, thumbnailHeight);
            var temporaryRenderTextureWithMaterial = RenderTexture.GetTemporary(thumbnailWidth, thumbnailHeight);

            RenderTexture.active = temporaryRenderTexture;
            SceneView lastActiveSceneView = SceneView.lastActiveSceneView;
            
            SceneView.CameraSettings sceneCameraSettings = null;
            var previousGizmoSetting = false;
            if (lastActiveSceneView != null)
            {
                previousGizmoSetting = lastActiveSceneView.drawGizmos;
                lastActiveSceneView.drawGizmos = false;
                sceneCameraSettings = lastActiveSceneView.cameraSettings;

                if (lastActiveSceneView.sceneLighting == false && Application.isPlaying == false)
                {
                    //sceneViewCustomLightMethod.Invoke(lastActiveSceneView, null);
                    Light[] lights = (Light[])sceneViewGetLights.GetValue(lastActiveSceneView);
                    lights[0].transform.rotation = cameras[0].transform.rotation;
                    Color ambientColor = (Color)sceneViewGetColor.GetValue(lastActiveSceneView);
                    UnityEditorInternal.InternalEditorUtility.SetCustomLighting(lights, ambientColor);
                }
            }

            // note :: multi cameras is not really used by the tool. But if someone wants to reuse the render functionality with multiple cameras it is supported.
            foreach (var cam in cameras)
            {
                // save values from current state
                var previousForceRenderSetting = cam.forceIntoRenderTexture;
                var previousTargetTexture = cam.targetTexture;
                var previousBackgroundColor = cam.backgroundColor;
                var previousNearClipPlane = cam.nearClipPlane;
                var previousFarClipPlane = cam.farClipPlane;
                var previousHDRsetting = cam.allowHDR;
                cam.forceIntoRenderTexture = true;

                cam.targetTexture = temporaryRenderTexture;
                cam.backgroundColor = backgroundColor;

                if (Application.isPlaying == false && sceneCameraSettings != null) // camera settings is not always synced with sceneCameraSettings
                {
                    cam.nearClipPlane = sceneCameraSettings.nearClip;
                    cam.farClipPlane = sceneCameraSettings.farClip;
                    cam.fieldOfView = sceneCameraSettings.fieldOfView;
                }
                // note :: unity has some bug? where for some reason if you don't have hdr=true or PostProcessStack packet and you have disabled light (using sceneViewLight instead standard materials would not be rendered if allowHDR is false)
                cam.allowHDR = true;
                if (lastActiveSceneView != null && SceneView.lastActiveSceneView.cameraMode.drawMode == DrawCameraMode.Wireframe)
                {
                    GL.wireframe = true;
                    cam.Render();
                    GL.wireframe = false;
                }
                else
                {
                    cam.Render();
                }

                cam.forceIntoRenderTexture = previousForceRenderSetting;
                cam.backgroundColor = previousBackgroundColor;
                cam.targetTexture = previousTargetTexture;
                cam.nearClipPlane = previousNearClipPlane;
                cam.farClipPlane = previousFarClipPlane;
                cam.allowHDR = previousHDRsetting;

            }

            if (postProcessMaterial != null)
            {
                RenderTexture.active = temporaryRenderTextureWithMaterial;
                GL.Clear(true, true, Color.clear);
                Graphics.Blit(temporaryRenderTexture, temporaryRenderTextureWithMaterial, postProcessMaterial);
            }
            else
            {
                RenderTexture.active = temporaryRenderTexture;

            }
            // read from renderTexture into Texture2D
            if (isPreview)
            {
                Graphics.CopyTexture(RenderTexture.active, newThumbnailTexture);
            }
            else
            {
                newThumbnailTexture.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0, false);
                newThumbnailTexture.Apply();
            }

            // restore values to previous state
            if (Application.isPlaying == false && lastActiveSceneView != null)
            {
                lastActiveSceneView.drawGizmos = previousGizmoSetting;
                UnityEditorInternal.InternalEditorUtility.RemoveCustomLighting();
            }

            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(temporaryRenderTexture);
            RenderTexture.ReleaseTemporary(temporaryRenderTextureWithMaterial);

            RestoreCanvases(canvases);

            return newThumbnailTexture;
        }

        /// <summary>
        /// Renders thumbnail texture from Camera, applies postProcessmaterial and saves result to disc. Name is choosen dependant on numberSuffix setting.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="thumbnailWidth"></param>
        /// <param name="thumbnailHeight"></param>
        /// <param name="relativeAssetPath"></param>
        /// <param name="fileName"></param>
        /// <param name="numberingSuffix"> 
        ///  None: If file exists. Overwrite file.
        ///  FileExists: If file exists. Add a suffix to filename (or increase suffix by 1).
        ///  Always: Add a suffix to filename (or increase suffix by 1)</param>
        /// <param name="backgroundColor"></param>
        /// <param name="postProcessMaterial">Material to use with blit copy after rendering camera.</param>
        /// <returns></returns>
        public static ThumbnailResult GenerateThumbnail(Camera[] cameras, int thumbnailWidth, int thumbnailHeight, AssetPathInfo assetPathInfo, Color backgroundColor, Material postProcessMaterial)
        {
            if (cameras == null || cameras.Length != 0)
            {
                var generatedAssetPath = GenerateThumbnailAssetPaths(assetPathInfo);

                var newTexture = GenerateThumbnailTexture(cameras, thumbnailWidth, thumbnailHeight, backgroundColor, postProcessMaterial);

                var result = SaveThumbnail(newTexture, generatedAssetPath);

                return result;
            }
            else throw new System.ArgumentException("Can not generate thumbnail, Camera is null.");
        }

        /// <summary>
        /// Returns modified canvases that needs to be restored after creating thumbnail.
        /// </summary>
        /// <returns></returns>
        private static List<(Canvas, RenderMode, float, Camera)> ModifyCanvases(Camera cam, bool isPreview)
        {
            List<(Canvas canvas, RenderMode renderMode, float planeDistance, Camera worldCamera)> canvasSettings = new List<(Canvas, RenderMode, float, Camera)>();
            if ((!isPreview && Settings.setCanvasWorldSpace != ModifyCanvas.Disabled) || (isPreview && Settings.setCanvasWorldSpace == ModifyCanvas.EnabledOnCaptureAndPreview))
            {
                Canvas[] canvases;
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    canvases = prefabStage.prefabContentsRoot.GetComponentsInChildren<Canvas>();
                }
                else
                {
                    canvases = UnityEngine.Object.FindObjectsOfType<Canvas>();
                }

                foreach (var canvas in canvases)
                {
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        canvasSettings.Add((canvas, canvas.renderMode, canvas.planeDistance, canvas.worldCamera));

                        canvas.renderMode = RenderMode.WorldSpace;
                        canvas.planeDistance = cam.nearClipPlane + 0.01f;
                        canvas.worldCamera = cam;
                    }
                    else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                    {
                        canvasSettings.Add((canvas, canvas.renderMode, canvas.planeDistance, canvas.worldCamera));

                        canvas.renderMode = RenderMode.WorldSpace;
                        canvas.worldCamera = cam;
                    }
                }
            }
            return canvasSettings;
        }

        private static void RestoreCanvases(List<(Canvas canvas, RenderMode renderMode, float planeDistance, Camera worldCamera)> canvasSettings)
        {
            foreach (var canvasSetting in canvasSettings)
            {
                canvasSetting.canvas.worldCamera = canvasSetting.worldCamera;
                canvasSetting.canvas.planeDistance = canvasSetting.planeDistance;
                canvasSetting.canvas.renderMode = canvasSetting.renderMode;
            }
        }
    }
}
