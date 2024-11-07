Thank you for aqcuiring this tool, I hope you will get use of it!
If there are any issues or if you have a feature request don't hesitate to contact me at molkiar@gmail.com or at the forums https://forum.unity.com/threads/thumbnail-generator-editortool.1010716/.

*More Isses / Troubleshooting scroll to the bottom

*** COMMON ALPHA ISSUES ***
If alpha transparency does not work, it is usually a postProcessProfile that is removing the alpha. Try disabling the setting which changes alpha or disable the postProcessing temporarely.



<Changelog>

----- Thumbnail Generator Editor Tool v1.3.5 - 2023-03-04 -----
Tool is now allowed in prefabStage InContext instead of going to the standard sceneView. Instead a warning is shown with a button to open source prefab.

Tool can now always be opened from contextmenu instead of being grayed out if no prefab is selected.
Fixed a null error when opening a selected prefab via the scene hierarchy using the Asset dropdown.


----- Thumbnail Generator Editor Tool v1.3.4 - 2021-06-06 -----

Quick generate updates:
 - Added option for near/far clip planes.
 - Fixed empty animationclips causing a crash.

Fixed unitys sceneViewLightning not always being applied if normal sceneLightning is turned off.

Updated icons from 18x19 to 16x16 to better match Unity2020 and above.


----- Thumbnail Generator Editor Tool v1.3.3 - 2021-02-11 -----

Added seperate option to overwrite numbering suffix for quick generate. (replaces the old image if it exist)


----- Thumbnail Generator Editor Tool v1.3.2 - 2021-01-11 -----

Quick generate updates:
 - Added option to choose between skybox / solid color.

 - Fixed HDRP always using skybox, transparency now correctly works when using a color with alpha. (HDRP enable alpha output https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@10.2/manual/Alpha-Output.html)


----- Thumbnail Generator Editor Tool v1.3.1 - 2020-12-30 -----

Added setting to override default savepath.

Added setting to override suffix position. (if suffix is used)
	- {filename} - filename
	- {x} - suffixnumber

Added setting to override default filename.
	- {assetname} - original prefab/scenename

Renamed numberingSuffix dropdown FileExists -> Auto


----- Thumbnail Generator Editor Tool v1.3 - 2020-12-16 -----

Now uses more internal Unity api calls to try and emulate SceneView lightning more correctly when SceneView lightning setting is turned off.

Added support to change displayed camera in GameView and having multiple cameras render to same display.

Fixed backgrounsScene in prefabStage not automatically closing in Unity 2019 (bug introduced in v1.2.1)

Fixed some cases where gizmos could be uninentionally drawn in Unity 2020 and up.


----- Thumbnail Generator Editor Tool v1.2.3 - 2020-12-10 -----

Fixed UI scaling not being applied to the tool preview.


----- Thumbnail Generator Editor Tool v1.2.2 - 2020-12-04 -----

Added a canvas setting in thumbnailSettings to control if canvases should be modified to world space.
 - Disabled as default.

Moved assemblydefinition to include all assets in the tool. (shader+readme) 


 ----- Thumbnail Generator Editor Tool v1.2.1 - 2020-12-03 -----
 
Fixed backgroundscene in prefabstage not working in Unity2020

Fixed issues when switching stagecontext with thumbnail generator window open in Unity2020

Fixed extra hidden light null error.


 ----- Thumbnail Generator Editor Tool v1.2.0 - 2020-11-29 -----

Now properly supports creating screenshots/thumbnails in playMode. 
  - Preview window of GameView will be painted on the latest opened SceneView.
  - Some features are disabled in playMode to ensure stability.

Fixed issues with the previous "hidden feature" since playMode is now properly supported.

Update to newer Unity api to prevent auto script update.



 ----- Thumbnail Generator Editor Tool v1.1.2 - 2020-11-20  -----
 
Fixed guiskin loading issue if Thumbnail window is opened while Editor is starting


 ----- Thumbnail Generator Editor Tool v1.1.1 - 2020-11-14  -----
 
Fixed postProcessStackV2 obsolete error in Unity 2020 + 2021

Added option to disable force use of backgroundScene lightning

Improved example CircleFrame quality

Added an example CircleFrame with inner noise
 

 ----- Thumbnail Generator Editor Tool v1.1.0 - 2020-10-07  -----
  
  New feature!
	BackgroundScenes - drag&drop a scene into the tool and it will automatically load and unload when closing the tool.
	 - in normal sceneViewMode the scene will be loaded and unloaded dynamically as multiscene and using the backgroundScene as the active scene (backgroundScene will become main lightning source, can be changed manually in hierarchy).
	 - prefabStage uses Unity's built in environmentalScene which override any postProcessProfile (I use the environmentalScene duo some constraints related to the disparity between prefabStage and sceneloading) 

  New feature!
	Live preview - shows the result live in the sceneView (limit to sceneView resolution)

Important info!
Tool is now a global tool instead of a local gameobject tool. It can be found in the toolbar where move/rotate/translate tools are or under dropdown menu.
 * A lot of core tool functionality has thus been rewritten.


You can now have multiple scenes open and switch between them.

Tool now remembers your session settings between settings between uses, can be toggled on/off in options.

Dynamic lights created now follow the camera position/rotation.

Added dynamic PostProcessVolumes to Scriptable Render Piplines (URP&HDRP)

Fixed a memory leak related to prefabStage

Fixed cases where window was destroyed twice.

fixed case where PostProcessLayer was not added correctly if camera existed in scene, but without MainCamera tag.

* "hidden feature" if no open scene exist, it will attempt to draw the game camera instead.




--- changelog v1.0.1 - 2020-08-24 ---

Gizmos are now automatically hidden when opening the tool

Added icon for dark mode

Fixed obsolete message in Unity 2020

Refined some example frames/masks


 ----- Thumbnail Generator Editor Tool v1.0.0 - 2020-06-06 -----

For use with Unity 2019.1 and above.
Optimized for usage with 2019.3 and above (for example tool color scheme is different in 2019.1)

Supports Unity PostProcessStack V2
Materials from Unity SRP is supported.
SRP PostProcessVolume from URP and HDRP is only partly supported, there seems to be many Unity native SRP issues. Therefore Custom Volume for quick generation is disabled and some lightning differences might be noticable.


Includes 3 distinct features.

 --- Thumbnail Generator Tool --- 
Avaliable by
 - Clicking on custom tool camera icon in sceneView and the prefabStage.
 - Right clicking on a prefab and selecting "Create > Thumbnail > Prefab Thumbnail Generator".
 
Opens a tool window to create a customized thumbnail following the user camera.
 
 
 --- Quick Generate Thumbnail --- 
Avaliable by
 -  right clicking on a prefab and selecting "Create > Thumbnail > Quick Generate Thumbnail".
 
Uses Unity's previewUtility to quickly create a scene in the background and render all selected images.
 
 
 --- Extract Unity Thumbnail ---
Avaliable by
 -  right clicking on a prefab and selecting "Create > Thumbnail > Extract Unity Thumbnail".

Extracts the same image that Unity uses for it's thumbnails using the specified default resolution.





<Troubleshooting>
----- ***** ----- Troubleshooting ----- ***** -----

* Background is not Transparent, and no postprocess effects are writing to alpha channel (like fog)
When using HDRP you must enable alpha channel transparency.
from https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@10.2/manual/Alpha-Output.html
"To configure HDRP to output an alpha channel, open your HDRP Asset (menu: Edit > Project Settings > Graphics > Scriptable Render Pipeline Asset)), go to the Rendering section, and set the Color Buffer Format to R16G16B16A16."


* I can't find the tool
Look at the global toolbar (same as move,rotate etc.) and click on the custom tool icon.
You can also right click a prefab Create -> Thumbnail to show all avaliable options.


* Images are foggy / have a skybox when using the tool window.
Disable skybox and fog in the sceneview. The tool tries to mimic the view settings.


* Tool doesn't start
A scriptableObject with default settings and settings for quick generate + extract unity thumbnail must be linked. 
If settings object does not exist it is recommended to reimport the package.


* Backgroundscenes have weird lightning
Check if all the opened scenes have the same lightning settings and have built updated lightning.
Note: By design the lightning will be used from the backgroundScene. If this is not desired, set useBackgroundSceneLightning = false in ThumbnailSettingsData.Asset


* Quick generate thumbnail is white/pink when using a SRP (URP or HDRP)
Unity has an issue in Scriptable Render Pipelines where PostProcessProfiles are sometimes inherited from MainScene. Closing the scene and opening an empty scene should restore values to normal.
It could also be related to exposure settings - see below * SRP prefabs are white


* Some settings not avaliable in playMode
This is by design to ensure any play scripts don't tamber with the Tool. They can be manually added back in the code if you are sure your scripts will not tamper with them really need those features, but it is not officially supported.


* Quick generate thumbnail URP volume profile is not applied
There seems to be an issue where Unity does not apply volume profiles for their previewScenes. Which quick generate uses. 
(if this feature is very important to you, please contact me for more information)


* In prefabStage, postProcessProfile doesn't work
 - PostProcessStack V2
	When using a backgroundScene in prefabStage any dynamically added PostProcessProfile will be overidden by that backgroundScene.
	Workarround is to set the postProcessProfile in the source of backgroundScene, then it will be applied.
 -  SRP PostProcess
	Check if the latest open scene had correct camera with SRP (URP or HDRP) components. Try adding the volume manually and see if you get the same look.


* I can not use SRP shader graph material in PostProcessMaterial
Shader graph uses the commandbuffer, so instead create a PostProcessProfile that uses your material and then reference that profile instead.
It seems to sometimes be working-isch, could be different between SRP / Shader Graph versions.
(if this feature is very important to you, please contact me for more information)
https://forum.unity.com/threads/shader-graph-image-effects-for-the-postprocessing-stack.567115/
https://github.com/iBicha/ImageEffectGraph


* HDRP prefabs are white
This is  related to exposure in the SRP settings profile. Try set it to fixed and tweak according to your needs.
https://forum.unity.com/threads/hdrp-lit-materials-only-completely-white-with-newest-update.819609/


Support and suggestions molkiar@gmail.com