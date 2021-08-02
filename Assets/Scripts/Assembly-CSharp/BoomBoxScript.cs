using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BoomBoxScript : MonoBehaviour
{
    public Rigidbody rb;
    float bounceDelay;
    float timeToDestroy;
    public AudioSource audioDevice;
    public Sprite[] boomBoxSprites = new Sprite[4];
    public AudioClip[] music = new AudioClip[5];
    ParticleSystem bounce;
    public SpriteRenderer spriteRendeer;
    private void Awake()
    {
        timeToDestroy = UnityEngine.Random.Range(35f, 80f);
        audioDevice.volume = UnityEngine.Random.Range(0.35f, 0.5f);
        UpdateSprite();
        bounce = GameObject.Find("BoomBoxBounce").GetComponent<ParticleSystem>();
        bounce.Stop();
    }
    private void Update()
    {
        if (Time.time > bounceDelay)
        {
            bounceDelay += UnityEngine.Random.Range(3f, 5f);
            Bounce();
        }
        if (timeToDestroy > 0f)
        {
            timeToDestroy -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        if (!audioDevice.isPlaying)
        {
            audioDevice.PlayOneShot(music[UnityEngine.Random.Range(0, music.Length)]);
        }
    }
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 35f * audioDevice.volume);
        List<Collider> colliderList = colliders.ToList();
        foreach (Collider collider in colliderList)
        {
            NavMeshAgent navMeshAgent = collider.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null && !navMeshAgent.GetComponent<SweepScript>())
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }
    }
    public void AddVolume(float amount)
    {
        audioDevice.volume += amount;
        UpdateSprite();
    }
    void UpdateSprite()
    {
        if (GameControllerScript.i.IsFloatBetweenValue(audioDevice.volume, 0.4f, 0.6f))
        {
            spriteRendeer.sprite = boomBoxSprites[0];
        }
        else if (GameControllerScript.i.IsFloatBetweenValue(audioDevice.volume, 0.61f, 0.8f))
        {
            spriteRendeer.sprite = boomBoxSprites[1];
        }
        else if (GameControllerScript.i.IsFloatBetweenValue(audioDevice.volume, 0.81f, 0.99f))
        {
            spriteRendeer.sprite = boomBoxSprites[2];
        }
        else if (audioDevice.volume == 1f)
        {
            spriteRendeer.sprite = boomBoxSprites[3];
        }
    }
    void Bounce()
    {
        bounce.transform.position = transform.position;
        bounce.Play();
        rb.velocity = Vector3.up * 10f;
    }
    bool HasParticleSystemFinished()
    {
        return !bounce.GetComponent<ParticleSystem>().isPlaying;
    }
}