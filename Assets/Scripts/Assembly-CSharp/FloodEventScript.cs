using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class FloodEventScript : MonoBehaviour
{
    public static FloodEventScript instance;
    public GameControllerScript gc;
    Vector3 origin;
    Vector2 floodVelocity;
    public GameObject whirlpoolPrefab;
    public Transform player;
    public Transform wanderTarget;
    public AILocationSelectorScript wanderer;
    public List<GameObject> whirlpoolList;
    public List<Vector3> whirlpoolSpawnPoints;
    public MoveModifier moveMod;
    public float moveSpeed = 125f;
    public Material floodMaterial;
    const float originY = -3.25f;
    float timeToMove;
    int minimumWhirlpools = 12;
    int maximumWhirlpools = 30;
    public System.Random controlledRandom;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        controlledRandom = new System.Random();
        whirlpoolList = new List<GameObject>();
        floodMaterial.mainTextureOffset = Vector2.zero;
        GetComponent<AudioSource>().volume = 1f;
        Vector3 vector = transform.position;
        vector.y = originY;
        transform.position = vector;
        Initalize();
        foreach (ActivityModifierScript activityModifier in FindObjectsOfType<ActivityModifierScript>())
        {
            if (!activityModifier.movementModList.Contains(moveMod))
            {
                activityModifier.movementModList.Add(moveMod);
            }
        }
        MoveFlood(moveSpeed);
        for (int i = 0; i < controlledRandom.Next(minimumWhirlpools, maximumWhirlpools); i++) SpawnWhirlpool();
    }
    void Initalize()
    {
        AudioSource audioDevice = GetComponent<AudioSource>();
        audioDevice.Play();
        origin = transform.position + Vector3.up * 0.4f;
        transform.position = origin;
        RefreshSpawnPoints();
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, origin) >= 30f)
        {
            ResetPosition();
        }
        if (Time.time >= timeToMove)
        {
            MoveFlood(moveSpeed);
            timeToMove += UnityEngine.Random.Range(10f, 20f);
        }
        if (floodMaterial.mainTextureOffset.y > 500f)
        {
            floodMaterial.mainTextureOffset = Vector2.zero;
        }
        OpenDoors();
        floodMaterial.mainTextureOffset += floodVelocity * Time.deltaTime / moveSpeed;
    }
    void MoveFlood(float speed)
    {
        floodVelocity = GetRandomDirection() * speed;
    }
    Vector2 GetRandomDirection()
    {
        float x, y;
        x = UnityEngine.Random.Range(0f, 1f);
        y = UnityEngine.Random.Range(0f, 1f);
        return new Vector2(x, y);
    }
    void OpenDoors()
    {
        DoorScript[] doorScripts = GameObject.FindObjectsOfType<DoorScript>();
        for (int i = 0; i < doorScripts.Length; i++)
        {
            doorScripts[i].OpenDoor();
        }
    }
    void ResetPosition()
    {
        transform.position = origin;
        MoveFlood(moveSpeed);
    }
    public WhirlpoolScript SpawnWhirlpool()
    {
        int chance = controlledRandom.Next(whirlpoolSpawnPoints.Count);
        Vector3 spawnPosition = whirlpoolSpawnPoints[chance];
        whirlpoolSpawnPoints.RemoveAt(chance);
        if (whirlpoolSpawnPoints.Count <= 0)
        {
            RefreshSpawnPoints();
        }
        GameObject whirlpool = Instantiate(whirlpoolPrefab, spawnPosition + Vector3.down * 2f, Quaternion.identity);
        whirlpoolList.Add(whirlpool);
        return whirlpool.GetComponent<WhirlpoolScript>();
    }
    void RefreshSpawnPoints()
    {
        List<Transform> transforms = wanderer.newLocation.ToList();
        List<Vector3> vector3s = new List<Vector3>();
        foreach (Transform transform in transforms)
        {
            vector3s.Add(transform.position);
        }
        whirlpoolSpawnPoints = vector3s;
    }
    void DespawnAllWhirlpools()
    {
        foreach (GameObject whirlpool in whirlpoolList)
        {
            WhirlpoolScript whirlpoolScript = whirlpool.GetComponent<WhirlpoolScript>();
            whirlpoolScript.StopAllCoroutines();
            StartCoroutine(whirlpoolScript.Deform(4f));
        }
    }
    public IEnumerator StopEvent()
    {
        AudioSource audioDevice = GetComponent<AudioSource>();
        foreach (ActivityModifierScript activityModifier in FindObjectsOfType<ActivityModifierScript>())
        {
            if (activityModifier.movementModList.Contains(moveMod))
            {
                activityModifier.movementModList.Remove(moveMod);
            }
        }
        while (transform.position.y > -10f)
        {
            transform.position -= Vector3.up * 5f * Time.deltaTime;
            float newVolume = Mathf.InverseLerp(-10f, originY, transform.position.y);
            audioDevice.volume = newVolume;
            yield return null;
        }
        DespawnAllWhirlpools();
        yield return new WaitUntil(() => whirlpoolList.Count == 0);
        gameObject.SetActive(false);
        yield break;
    }
}
