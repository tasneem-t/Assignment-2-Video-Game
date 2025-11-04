using UnityEngine;
using TMPro;

public class ShardUIManager : MonoBehaviour
{
    public TMP_Text shardsText;
    public TMP_Text memoriesText;

    public static ShardUIManager instance;

    private int totalShards = 0;
    private int totalMemories = 0;

    [Header("Guiding Orb (next-level)")]
    public GameObject guidingOrbPrefab;
    public Transform guidingSpawnPoint;          // optional; leave null to spawn at player
    public Vector3 guidingTargetPosition = Vector3.zero; // optional override
    public float guidingSpeed = 3f;

    void Awake()
    {
        instance = this;
    }

    public void AddShard(int amount, int shardsNeeded)
    {
        totalShards += amount;
        if (shardsText != null) shardsText.text = "Shards Collected: " + totalShards;

        if (totalShards >= shardsNeeded)
        {
            totalMemories++;
            totalShards = 0;
            if (memoriesText != null) memoriesText.text = "Memories Unlocked: " + totalMemories;
            if (shardsText != null) shardsText.text = "Shards Collected: " + totalShards;

            Debug.Log($"ShardUIManager: Memory unlocked. totalMemories={totalMemories}");

            // spawn guiding orb immediately when first memory unlocked
            if (totalMemories == 1)
                SpawnGuidingOrb();
        }
    }

    void SpawnGuidingOrb()
    {
        if (guidingOrbPrefab == null)
        {
            Debug.LogError("ShardUIManager.SpawnGuidingOrb: guidingOrbPrefab is NOT assigned in the Inspector.");
            return;
        }

        // Determine spawn position: use assigned spawn point, otherwise player's position, otherwise world origin
        Vector3 spawnPos = Vector3.zero;
        if (guidingSpawnPoint != null) spawnPos = guidingSpawnPoint.position;
        else
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null) spawnPos = player.transform.position + Vector3.up * 1f; // spawn slightly above player
        }

        GameObject go = Instantiate(guidingOrbPrefab, spawnPos, Quaternion.identity);
        if (go == null)
        {
            Debug.LogError("ShardUIManager.SpawnGuidingOrb: Instantiate returned null.");
            return;
        }

        MoveOrb mover = go.GetComponent<MoveOrb>();
        if (mover != null)
        {
            // If a target override is given in inspector, use it; otherwise keep prefab's value
            if (guidingTargetPosition != Vector3.zero) mover.targetPosition = guidingTargetPosition;
            mover.speed = guidingSpeed;
        }
        else
        {
            Debug.LogWarning("ShardUIManager.SpawnGuidingOrb: spawned prefab has no MoveOrb component. It will not move.");
        }

        Debug.Log("ShardUIManager: Guiding orb spawned.");
    }
}
