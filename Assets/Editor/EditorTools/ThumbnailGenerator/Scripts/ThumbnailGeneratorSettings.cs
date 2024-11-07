// Author: Tobias Löf Melker
// Created: 2020 for usage with Unity Engine

using UnityEditor;
using UnityEngine;
#if UNITY_POSTPROCESS
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;
#elif UNITY_SRP_POSTPROCESS
using UnityEngine.Rendering;
#endif

// settings object for tool default settings and dropdown quick generate settings.
namespace UnityEditor.EditorTools.TLM.ThumbnailGenerator
{
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RecreateQuickThumbnailSettings", order = 1)]
    public class ThumbnailGeneratorSettings : ScriptableObject
    {
        public enum NumberingSuffix
        {
            None = 0, // If file exists. Overwrite file.
            Auto = 1, // If file exists. Add a suffix to filename (or increase suffix by 1).
            Always = 2 // Add a suffix to filename (or increase suffix by 1)</param>
        }

        public enum ModifyCanvas
        {
            Disabled = 0,
            EnabledOnCapture = 1,
            EnabledOnCaptureAndPreview = 2
        }

        public enum Projection
        {
            Perspective = 0,
            Orthographic = 1
        }
        /* */ [Header("Default settings")] /* */
        [Tooltip("Should previously used settings be remembered, until Unity is closed, when closing the Thumbnail Generator.")]
        public bool rememberSessionSettings = true;
        public int thumbnailWidth = 512;
        public int thumbnailHeight = 512;
        [Tooltip("Background Color for transparent parts.")]
        public Color backgroundColor = new Color(0.333f, 0.333f, 0.333f, 0f);

        public bool useBackgroundSceneLightning = true;
        public SceneAsset backgroundScene = null;
        public Material postProcessMaterial = null;
#if UNITY_POSTPROCESS
        public PostProcessProfile postProcessProfile = null;
#elif UNITY_SRP_POSTPROCESS
        public VolumeProfile volumeProfile = null;
#endif
        [Tooltip("Adds a number to the file if it already exist. If set to None, the file will be overwritten!")]
        public NumberingSuffix numberingSuffix = NumberingSuffix.Auto;

        /* */ [Header("Camera Position Settings")] /* */

        public Vector3 cameraPosition = new Vector3(-1.4f, 1.2f, -4);
        public Vector3 cameraRotation = new Vector3(10, 20, 0);
        public Projection projection = Projection.Perspective;
        public float fieldOfView = 60;
        public float orthopgrahicSize = 5f;
        [Header("Settings only affecting quick generation")]
        public bool playParticles = true;
        public float particlePlayBackTime = 1;
        public bool playAnimationClips = false;
        public float animationPlayBackTime = 0;
        public float nearClipPlane = 0.01f;
        public float farClipPlane = 1000f;
        [Tooltip("Setting extractUsingStaticPreview to false will use the exact copy of the asset thumbnail exactly as it. Meaning fixed to a low resolution.")]
        public bool extractUsingStaticPreview = true;

        public enum CameraBackground
        {
            SolidColor = 0,
            SkyBox = 1
        }
        public CameraBackground cameraBackgroundType = CameraBackground.SolidColor;

        [Tooltip("Adds a number to the file if it already exist. If set to None, the file will be overwritten!")]
        public NumberingSuffix quickGenerateSuffix = NumberingSuffix.Auto;
        

        /* */ [Header("Canvas")] /* */
        [Tooltip("Should canvas be modified by the tool? Normal is to be disabled. If enabled overlay canvas and camera canvas change temporarely change mode to worldcamera. (experimental)")]
        public ModifyCanvas setCanvasWorldSpace = ModifyCanvas.Disabled;

        [Header("Livepreview")]
        [Tooltip("Livepreview of the how the thumbnail will look with postProcessing. Turn off if you experience framerate drops.")]
        public bool livePreview = true;
        [Tooltip("Livepreview of the how the thumbnail will look with postProcessing. Turn off if you experience framerate drops.")]
        public Color previewBackgroundColor = new Color(0.14f, 0.24f, 0.33f);

        /* */ [Header("Set how Suffix will be added to filename")] /* */
        [NotEmpty("{filename}_{x}")]
        public string suffixFormat = "{filename}_{x}";

        /* */ [Header("Set Default FileNames for SceneView/PrefabView")] /* */
        [NotEmpty("{assetname}")]
        public string sceneFileName = "GeneratedThumbnail";
        [NotEmpty("{assetname}")]
        public string prefabFileName = "{assetname}_thumbnail";

        /* */ [Header("Override Save FolderPaths for SceneView/PrefabView")] /* */
        [AssetPath]
        public string scenePath = string.Empty;
        [AssetPath]
        public string prefabPath = string.Empty;

        /* Note :: in case settings need to be recreated, uncomment the createAssetMenu comment and recreate. Then remove hideininspector on the "serialized field and drag it as "default" setting in the inspector.
        The better question is, do I really need to do this hacky way to easily reference settings? Could add a Resource map and load via path. */
        [HideInInspector]
        public ThumbnailGeneratorSettings serializedSettingsReference = null;
    }


    internal class NotEmptyAttribute : PropertyAttribute
    {
        public string defaultValue;
        public NotEmptyAttribute(string defaultValue)
        {
            this.defaultValue = defaultValue;
        }
        private NotEmptyAttribute() { }
    }
    [CustomPropertyDrawer(typeof(NotEmptyAttribute))]
    internal class NotEmptyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            NotEmptyAttribute notEmptyAttribute = attribute as NotEmptyAttribute;

            EditorGUI.PropertyField(position, property, label, true);

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = notEmptyAttribute.defaultValue;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }

    internal class AssetPathAttribute : PropertyAttribute { }


    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    internal class AssetPathDrawer : PropertyDrawer
    {
        private GUIContent guiContentBrowseFilepath;
        
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //AssetPathAttribute assetPath = attribute as AssetPathAttribute;

            if (guiContentBrowseFilepath == null)
            {
                guiContentBrowseFilepath = EditorGUIUtility.IconContent("Folder Icon");
                guiContentBrowseFilepath.text = "";
            }

            Rect newPos = position;
            newPos.width -= 30;

            GUI.enabled = false;
            EditorGUI.PropertyField(newPos, property, label, true);
            GUI.enabled = true;

            if (GUI.Button(new Rect(newPos.x+newPos.width+2, newPos.y-2, 30-4, newPos.height+4), guiContentBrowseFilepath))
            {
                string filePanelStartFolder = property.stringValue;
                if (string.IsNullOrEmpty(filePanelStartFolder))
                    filePanelStartFolder = "Assets/";
                string relativeAssetPath = EditorUtility.SaveFilePanelInProject($"Select {property.displayName}", "examplename", "png", "Selected folder will override default behaviour. Cancel will restore to default behaviour.", filePanelStartFolder);
                var mapsInAssetPath = relativeAssetPath.Split('/');
                relativeAssetPath = string.Empty;
                for (int i = 0; i < mapsInAssetPath.Length - 1; i++)
                {
                    relativeAssetPath += mapsInAssetPath[i] + '/';
                }

                if (string.IsNullOrEmpty(relativeAssetPath) == false)
                {
                    property.stringValue = relativeAssetPath;
                    property.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    property.stringValue = null;
                    property.serializedObject.ApplyModifiedProperties();

                }
            }
        }
    }
}
