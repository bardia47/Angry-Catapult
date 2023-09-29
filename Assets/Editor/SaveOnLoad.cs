using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]

public class SaveOnLoad
{

    static SaveOnLoad() {
        EditorApplication.playmodeStateChanged = () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode
                && !EditorApplication.isPlaying)
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    Debug.Log("Auto-Saved opened scenes before entering Play Mode");
                    AssetDatabase.SaveAssets();
                    EditorSceneManager.SaveOpenScenes();
                }
            }
        };
    }
}
