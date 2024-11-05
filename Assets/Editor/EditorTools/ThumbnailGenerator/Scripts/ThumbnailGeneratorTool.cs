// Author: Tobias Löf Melker
// Created: 2020 for usage with Unity Engine

using UnityEngine;
#if !UNITY_2020_2_OR_NEWER
using ToolManager = UnityEditor.EditorTools.EditorTools;
#endif

namespace UnityEditor.EditorTools.TLM.ThumbnailGenerator
{
    [EditorTool("Thumbnail Generator")]
    public class ThumbnailGeneratorTool : EditorTool
    {
        [HideInInspector]
        [SerializeField]
        Texture2D toolIcon = null;
        [HideInInspector]
        [SerializeField]
        Texture2D toolIcon_darkmode = null;
        [HideInInspector]
        [SerializeField]
        Texture2D toolIconSelected = null;

        GUIContent guiContent;
        GUIContent guiContent_selected;

        public static ThumbnailGeneratorWindow window { get; set; }
        public Camera camera { get; set; }

        public override GUIContent toolbarIcon
        {
            get
            {
                if (ToolManager.activeToolType == typeof(ThumbnailGeneratorTool))
                    return guiContent_selected;
                else
                    return guiContent;
            }
        }

        public override bool IsAvailable()
        {
            return true;
        }

        void ActiveToolChanged()
        {
            if (ToolManager.IsActiveTool(this) == false)
            {
                return;
            }

            if (ThumbnailGeneratorWindow.instance == null)
            {
                window = CreateInstance<ThumbnailGeneratorWindow>();
                window.tool = this;
                window.titleContent = guiContent;
                window.Show();
            }
            else
            {
                window = ThumbnailGeneratorWindow.instance;
                window.tool = this;
                window.InitSceneObjects();
            }
        }

        void OnEnable()
        {
            bool darkMode = EditorGUIUtility.isProSkin;

            if (guiContent == null)
            {
                if (darkMode == false && toolIcon != null)
                    guiContent = new GUIContent("Thumbnail Generator", toolIcon, "Create thumbnails directly in the scene.");
                else if (darkMode == true && toolIcon_darkmode)
                    guiContent = new GUIContent("Thumbnail Generator", toolIcon_darkmode, "Create thumbnails directly in the scene.");
                else
                {
                    // Usually you'll want to use an icon (19x18 px to match Unity's icons)
                    string iconAssetPath = "Assets/Editor/EditorTools/ThumbnailGenerator/icon_thumbnailGenerator.png";
                    if (darkMode)
                    {
                        iconAssetPath = "Assets/Editor/EditorTools/ThumbnailGenerator/icon_thumbnailGenerator_darkmode.png";
                    }

                    var icon16x16 = AssetDatabase.LoadAssetAtPath<Texture2D>(iconAssetPath);
                    if (icon16x16 != null)
                        guiContent = new GUIContent("Thumbnail Generator", icon16x16, "Create thumbnails directly in the scene.");
                    else
                        guiContent = new GUIContent("Thumbnail Generator", "Create thumbnails directly in the scene.");
                }
            }
            if (guiContent_selected == null)
            {
                if (toolIconSelected != null)
                    guiContent_selected = new GUIContent("Thumbnail Generator", toolIconSelected, "Create thumbnails directly in the scene.");
                else
                {
                    // Usually you'll want to use an icon (16x16 px to match Unity's icons - 19x18 was older unity 2019)
                    var icon16x16 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/EditorTools/ThumbnailGenerator/icon_thumbnailGenerator_selected.png");
                    if (icon16x16 != null)
                        guiContent_selected = new GUIContent("Thumbnail Generator", icon16x16, "Create thumbnails directly in the scene.");
                    else
                        guiContent_selected = new GUIContent("Thumbnail Generator", "Create thumbnails directly in the scene.");
                }
            }
            ToolManager.activeToolChanged += ActiveToolChanged;
        }

        private void OnDisable()
        {
            ToolManager.activeToolChanged -= ActiveToolChanged;
        }

        private Vector2 CalculateAspectRatio(int width, int height)
        {
            var AR = new Vector2(width, height);
            if (AR.x < AR.y)
            {
                AR /= AR.x;
            }
            else
            {
                AR /= AR.y;
            }
            return AR;
        }

        private Vector2 CalculateViewBox(int cameraWidth, int cameraHeight, Vector2 aspectRatio)
        {
            float xSize = cameraWidth / aspectRatio.x;
            float ySize = cameraHeight / aspectRatio.y;

            if (xSize < ySize)
            {
                xSize = cameraWidth;
                ySize = cameraWidth / aspectRatio.x * aspectRatio.y;
            }
            else if (xSize > ySize)
            {
                ySize = cameraHeight;
                xSize = cameraHeight / aspectRatio.y * aspectRatio.x;
            }

            return new Vector2(xSize, ySize);
        }

        public override void OnToolGUI(EditorWindow sceneWindow)
        {
            if (/*sceneWindow != SceneView.lastActiveSceneView ||*/ window == null) return;

            if (window.toolCamera != Camera.current) // Camera.current might reference different sceneView if we have multiple sceneViews open at the same time.
            {
                camera = Camera.current;
                window.toolCamera = camera;
                window.sceneWindow = sceneWindow;
            }

            var cameraWidth = (int)(camera.pixelRect.width + camera.pixelRect.x);
            var cameraHeight = (int)(camera.pixelRect.height + camera.pixelRect.y);

            var rtAR = CalculateAspectRatio(window.thumbnailWidth, window.thumbnailHeight);
            var viewBoxSize = CalculateViewBox(cameraWidth, cameraHeight, rtAR);

            float xOffset = Mathf.Ceil((float)System.Math.Round(System.Math.Round((camera.pixelRect.width - viewBoxSize.x), System.MidpointRounding.AwayFromZero)) / 2f);
            float yOffset = Mathf.Ceil((float)System.Math.Round(System.Math.Round((camera.pixelRect.height - viewBoxSize.y), System.MidpointRounding.AwayFromZero)) / 2f);

            // note: Scale preview viewports with UI saling pixelPerPoint.
            xOffset *= 1f / EditorGUIUtility.pixelsPerPoint;
            yOffset *= 1f / EditorGUIUtility.pixelsPerPoint;
            viewBoxSize = EditorGUIUtility.PixelsToPoints(viewBoxSize);
            var saledCamPixelRect = EditorGUIUtility.PixelsToPoints(camera.pixelRect);

            var thumbnailRect = new Rect(xOffset + saledCamPixelRect.x, yOffset + saledCamPixelRect.y, viewBoxSize.x, viewBoxSize.y);
            var thumbnailBorderRect = new Rect(xOffset + saledCamPixelRect.x - 2, yOffset + saledCamPixelRect.y - 2, viewBoxSize.x + 4, viewBoxSize.y + 4);

            var cameraOutsideThumbnailLeft = new Rect(saledCamPixelRect.x, saledCamPixelRect.y, xOffset, viewBoxSize.y);
            var cameraOutsideThumbnailRight = new Rect(saledCamPixelRect.x + xOffset + viewBoxSize.x, saledCamPixelRect.y, xOffset, viewBoxSize.y);
            var cameraOutsideThumbnailTop = new Rect(saledCamPixelRect.x, saledCamPixelRect.y + yOffset + viewBoxSize.y, viewBoxSize.x, yOffset);
            var cameraOutsideThumbnailBottom = new Rect(saledCamPixelRect.x, saledCamPixelRect.y, viewBoxSize.x, yOffset);

            Color fadeColor = new Color(0f, 0f, 0f, 0.7f);
            Color borderColor = new Color(0.5f, 0.5f, 0.5f, 1);

            using (new Handles.DrawingScope(Color.white))
            {
                if (ThumbnailGenerator.Settings.livePreview)
                {
                    Texture2D liveThumbnail;
                    if (window.useGameCamera && window.gameCameras != null && window.gameCameras.Length != 0)
                    {
                        liveThumbnail = ThumbnailGenerator.GenerateThumbnailTexture(window.gameCameras, (int)viewBoxSize.x, (int)viewBoxSize.y, window.backgroundColor, window.postProcessMaterial, isPreview: true);
                    }
                    else
                    {
                        liveThumbnail = ThumbnailGenerator.GenerateThumbnailTexture(new[] { camera }, (int)viewBoxSize.x, (int)viewBoxSize.y, window.backgroundColor, window.postProcessMaterial, isPreview: true);
                    }

                    Handles.BeginGUI();
                    EditorGUI.DrawRect(cameraOutsideThumbnailLeft, fadeColor);
                    EditorGUI.DrawRect(cameraOutsideThumbnailRight, fadeColor);
                    EditorGUI.DrawRect(cameraOutsideThumbnailTop, fadeColor);
                    EditorGUI.DrawRect(cameraOutsideThumbnailBottom, fadeColor);
                    EditorGUI.DrawRect(thumbnailBorderRect, borderColor);
                    EditorGUI.DrawRect(thumbnailRect, ThumbnailGenerator.Settings.previewBackgroundColor);
                    GUI.DrawTexture(thumbnailRect, liveThumbnail);
                    Handles.EndGUI();
                    DestroyImmediate(liveThumbnail);
                }


                // uggly viewfinder
                //Handles.color = fadeColor;
                //Handles.DrawWireDisc(camera.transform.position + camera.transform.forward, camera.transform.forward, 0.2f);
                //Handles.DrawLine(camera.transform.position + camera.transform.forward - camera.transform.right * 0.1f + camera.transform.up * 0.05f, camera.transform.position + camera.transform.forward + camera.transform.right * 0.1f + camera.transform.up * 0.05f);
                //Handles.DrawLine(camera.transform.position + camera.transform.forward - camera.transform.right * 0.1f - camera.transform.up * 0.05f, camera.transform.position + camera.transform.forward + camera.transform.right * 0.1f - camera.transform.up * 0.05f);
                //Handles.DrawSolidDisc(camera.transform.position + camera.transform.forward, camera.transform.forward, 0.005f);
            }
        }
    }
}