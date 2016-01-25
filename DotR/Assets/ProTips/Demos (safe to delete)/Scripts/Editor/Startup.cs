using System;
using UnityEditor;
using UnityEngine;

namespace ModelShark
{
    [InitializeOnLoad]
    public class Startup
    {
        /// <summary>
        /// Append the two ProTips demo scenes to the build settings, so the customer doesn't get an error when running the demo scene and
        /// clicking the buttons that switch between scenes.
        /// </summary>
        static Startup()
        {
            bool proTipsSceneFound = false;
            bool workshopSceneFound = false;
            int newSceneCount = 0;

            EditorBuildSettingsScene[] originalScenes = EditorBuildSettings.scenes;
            for (int i = 0; i < originalScenes.Length; i++)
            {
                if (originalScenes[i].path.Contains("ProTips Demo"))
                    proTipsSceneFound = true;
                if (originalScenes[i].path.Contains("Tooltip Workshop"))
                    workshopSceneFound = true;
            }
            if (!proTipsSceneFound)
                newSceneCount++;
            if (!workshopSceneFound)
                newSceneCount++;
            if (newSceneCount == 0) return;

            var newSettings = new EditorBuildSettingsScene[originalScenes.Length + newSceneCount];
            Array.Copy(originalScenes, newSettings, originalScenes.Length);

            if (!proTipsSceneFound)
            {
                var sceneToAdd = new EditorBuildSettingsScene("Assets/ProTips/Demos (safe to delete)/ProTips Demo.unity", true);
                newSettings[newSettings.Length - newSceneCount] = sceneToAdd;
                newSceneCount--;
            }
            if (!workshopSceneFound)
            {
                var sceneToAdd = new EditorBuildSettingsScene("Assets/ProTips/Demos (safe to delete)/Tooltip Workshop.unity", true);
                newSettings[newSettings.Length - newSceneCount] = sceneToAdd;
                newSceneCount--;
            }

            EditorBuildSettings.scenes = newSettings;
            Debug.Log("Added ProTips demo scenes to BuildSettings."); 
        }
    }
}
