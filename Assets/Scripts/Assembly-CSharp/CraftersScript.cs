using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000017 RID: 23
public class CraftersScript : MonoBehaviour
{
	// Token: 0x0600005E RID: 94 RVA: 0x000041ED File Offset: 0x000023ED
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.sprite.SetActive(false);
	}

	public ActivityModifierScript am
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	private void Update()
	{
		if (am.TotalAdder.magnitude > 0f) agent.velocity += am.TotalAdder;
		if (this.gc.officeParty)
		{
			this.agent.SetDestination(this.gc.partyPositions[1].position);
		}
		if (this.forceShowTime > 0f)
		{
			this.forceShowTime -= Time.deltaTime;
		}
		if (this.gettingAngry)
		{
			this.anger += Time.deltaTime;
			if (this.anger >= 1f & !this.angry)
			{
				this.angry = true;
				this.audioDevice.PlayOneShot(this.aud_Intro);
				this.spriteImage.sprite = this.angrySprite;
			}
		}
		else if (this.anger > 0f)
		{
			this.anger -= Time.deltaTime;
		}
		if (this.angry)
		{
			if (this.gc.stunnedCharacter.transform.name != base.transform.name)
			{
				this.agent.speed += 60f * am.TotalMultipler * Time.deltaTime;
			}
			this.TargetPlayer();
			if (!this.audioDevice.isPlaying)
			{
				this.audioDevice.PlayOneShot(this.aud_Loop);
			}
			return;
		}
		if (((base.transform.position - this.agent.destination).magnitude <= 20f & (base.transform.position - this.player.position).magnitude >= 60f) || this.forceShowTime > 0f)
		{
			this.sprite.SetActive(true);
			return;
		}
		this.sprite.SetActive(false);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000043E8 File Offset: 0x000025E8
	private void FixedUpdate()
	{
		if (this.gc.notebooks >= 7)
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" & this.craftersRenderer.isVisible & this.sprite.activeSelf)
			{
				this.gettingAngry = true;
				return;
			}
			this.gettingAngry = false;
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004490 File Offset: 0x00002690
	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!this.angry)
		{
			this.agent.SetDestination(location);
			if (flee)
			{
				this.forceShowTime = 3f;
			}
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000044B5 File Offset: 0x000026B5
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000044D0 File Offset: 0x000026D0
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" & this.angry)
		{
			this.player.position = new Vector3(5f, this.player.position.y, 80f);
			this.baldiAgent.Warp(new Vector3(5f, this.baldi.position.y, 125f));
			this.player.LookAt(new Vector3(this.baldi.position.x, this.player.position.y, this.baldi.position.z));
			this.gc.DespawnCrafters();
		}
	}

	// Token: 0x04000086 RID: 134
	public bool db;

	// Token: 0x04000087 RID: 135
	public bool angry;

	// Token: 0x04000088 RID: 136
	public bool gettingAngry;

	// Token: 0x04000089 RID: 137
	public float anger;

	// Token: 0x0400008A RID: 138
	private float forceShowTime;

	// Token: 0x0400008B RID: 139
	public Transform player;

	// Token: 0x0400008C RID: 140
	public Transform playerCamera;

	// Token: 0x0400008D RID: 141
	public Transform baldi;

	// Token: 0x0400008E RID: 142
	public NavMeshAgent baldiAgent;

	// Token: 0x0400008F RID: 143
	public GameObject sprite;

	// Token: 0x04000090 RID: 144
	public GameControllerScript gc;

	// Token: 0x04000091 RID: 145
	private NavMeshAgent agent;

	// Token: 0x04000092 RID: 146
	public Renderer craftersRenderer;

	// Token: 0x04000093 RID: 147
	public SpriteRenderer spriteImage;

	// Token: 0x04000094 RID: 148
	public Sprite angrySprite;

	// Token: 0x04000095 RID: 149
	private AudioSource audioDevice;

	// Token: 0x04000096 RID: 150
	public AudioClip aud_Intro;

	// Token: 0x04000097 RID: 151
	public AudioClip aud_Loop;
}
