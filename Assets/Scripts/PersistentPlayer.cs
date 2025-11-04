using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentPlayer : MonoBehaviour
{
    void Awake()
    {
        // Ensure only one persistent player exists
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called after a scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find a spawn point in the newly loaded scene
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;
        }
        else
        {
            // If no spawn found, do nothing (player stays where it is)
            Debug.Log("PersistentPlayer: no PlayerSpawn found in scene " + scene.name);
        }

        // If you use AudioListener on camera(s), ensure only one active (optional)
    }
}
