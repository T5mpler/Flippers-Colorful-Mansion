using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200004F RID: 79
public class WizardScript : MonoBehaviour
{
	// Token: 0x0600018A RID: 394 RVA: 0x0000F5EC File Offset: 0x0000D7EC
	private void Start()
	{
		this.spellCast = false;
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.Wander();
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = Mathf.RoundToInt(UnityEngine.Random.Range(15f, 20f)) * am.TotalMultipler;
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000F668 File Offset: 0x0000D868
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
			this.agent.SetDestination(this.gc.partyPositions[3].position);
		}
		if (this.wandCooldown > 0f)
		{
			this.wandCooldown -= Time.deltaTime;
		}
		else if (this.brokeWand)
		{
			this.wizardSprite.sprite = this.idle;
			this.agent.SetDestination(this.player.position);
			this.brokeWand = false;
		}
		if (this.spellTime <= 0f && this.spellCooldown > 0f)
		{
			this.spellCooldown -= Time.deltaTime;
		}
		if (this.spellTime > 0f)
		{
			this.spellTime -= Time.deltaTime;
		}
		else if (this.inSpell)
		{
			this.inSpell = false;
			this.playerScript.StopSpell();
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (!this.audioDevice.isPlaying && this.audioInQueue > 0)
		{
			this.PlayQueue();
		}
		if (guilt > 0f)
		{
			guilt -= Time.deltaTime;
		}
		if (principalDisappearTime > 0f)
		{
			principalDisappearTime -= Time.deltaTime;
		}
		else if (principalDisappeared)
		{
			principalDisappeared = false;
			principal.gameObject.SetActive(true);
			principal.quietAudioDevice.PlayOneShot(principal.audNoDisappear);
			principal.TargetWizard();
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" & !this.brokeWand)
		{
			this.db = true;
			this.TargetPlayer();
			return;
		}
		this.db = false;
		if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
		{
			this.Wander();
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
	public void Wander()
	{
		this.wanders++;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
		if (this.wanders == 5)
		{
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
			this.audioDevice.PlayOneShot(this.aud_Finds[num]);
			this.wanders = 0;
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000F938 File Offset: 0x0000DB38
	public void TargetPlayer()
	{
		if (this.spellCooldown <= 0f)
		{
			if (this.gc.stunnedCharacter.transform.name != base.transform.name)
			{
				this.agent.speed = 25f * am.TotalMultipler;
			}
			if (!targetingPlayer)
			{
				targetingPlayer = true;
				QueueAudio(aud_NewSubject);
			}
			this.agent.SetDestination(this.player.position);
			this.coolDown = 0.2f;
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000F9AB File Offset: 0x0000DBAB
	public void QueueAudio(AudioClip sound)
	{
		this.audioQueue[this.audioInQueue] = sound;
		this.audioInQueue++;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000F9C9 File Offset: 0x0000DBC9
	private void PlayQueue()
	{
		this.audioDevice.PlayOneShot(this.audioQueue[0]);
		this.UnqueueAudio();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
	private void UnqueueAudio()
	{
		for (int i = 1; i < this.audioInQueue; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue--;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000FA24 File Offset: 0x0000DC24
	public void ClearAudioQueue()
	{
		for (int i = 1; i < this.audioQueue.Length; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue = 0;
	}
	public IEnumerator DoSpell()
	{
		this.spells++;
		this.spellCast = true;
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 0f;
		}
		this.wizardAnimator.SetTrigger("spellCasting");
		RemoveAllinQueue();
		this.QueueAudio(this.aud_MagicSpells);
		this.num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 0f));
		if (!ClipInQueue(this.aud_Spells[this.num])) this.QueueAudio(this.aud_Spells[this.num]);
		this.coolDown = 1f;
		this.spellCooldown = (float)Mathf.RoundToInt(UnityEngine.Random.Range(25f, 40f));
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = (float)Mathf.RoundToInt(UnityEngine.Random.Range(10f, 15f)) * am.TotalMultipler;
		}
		StartCoroutine(WaitForAllAudio(OnSpellAudioPlayed));
		guilt = 25f;
		this.spellCast = false;
		targetingPlayer = false;
		this.wizardSprite.sprite = this.idle;
		yield break;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
	public void BreakWand(bool playAudio)
	{
		this.spells = 0;
		if (playAudio) this.audioDevice.PlayOneShot(this.aud_NoWand);
		this.wizardSprite.sprite = this.noWand;
		this.wandCooldown = UnityEngine.Random.Range(30f, 50f);
		this.brokeWand = true;
		this.Wander();
	}
	public IEnumerator WaitForAllAudio(Action OnAudioQueueComplete = null)
	{
		agent.isStopped = true;
		float totalTimeToWait = 0f;
		foreach (AudioClip audioClip in audioQueue)
		{
			if (audioClip != null)
			{
				totalTimeToWait += audioClip.length;
				yield return null;
			}
		}
		yield return new WaitForSeconds(totalTimeToWait);
		agent.isStopped = false;
		OnAudioQueueComplete?.Invoke();
		yield break;
	}
	void OnSpellAudioPlayed()
	{
		this.playerScript.CastSpell();
		this.ClearAudioQueue();
		RemoveAllinQueue();
	}
	bool ClipInQueue(AudioClip audioClip)
	{
		for (int i = 0; i < audioQueue.Length; i++)
		{
			if (audioQueue[i] == audioClip)
			{
				return true;
			}
		}
		return false;
	}
	public void RemoveAllinQueue()
	{
		for (int i = 0; i < audioQueue.Length; i++)
		{
			audioQueue[i] = null;
		}
	}
	public void PrincipalFate()
	{
		int chance = UnityEngine.Random.Range(0, 1);
		switch (chance)
		{
			case 0:
				principal.wizardSeen = false;
				principal.gameObject.SetActive(false);
				principalDisappeared = true;
				principalDisappearTime = UnityEngine.Random.Range(15f, 25f);
				audioDevice.PlayOneShot(aud_Shutup);
				break;
		}
	}
	// Token: 0x04000324 RID: 804
	public NavMeshAgent agent;

	// Token: 0x04000325 RID: 805
	public AudioSource audioDevice;

	// Token: 0x04000326 RID: 806
	public bool db;

	// Token: 0x04000327 RID: 807
	public Transform player;

	// Token: 0x04000328 RID: 808
	public Transform wanderTarget;

	// Token: 0x04000329 RID: 809
	public AILocationSelectorScript wanderer;

	// Token: 0x0400032A RID: 810

	// Token: 0x0400032B RID: 811
	public int wanders;

	// Token: 0x0400032C RID: 812
	public AudioClip[] aud_Finds = new AudioClip[2];

	// Token: 0x0400032D RID: 813
	public Animator wizardAnimator;

	// Token: 0x0400032E RID: 814
	public Transform wizardTransform;

	// Token: 0x0400032F RID: 815
	public AudioClip aud_NewSubject;

	// Token: 0x04000330 RID: 816
	public AudioClip[] aud_Spells = new AudioClip[3];

	// Token: 0x04000331 RID: 817
	public AudioClip aud_MagicSpells;

	// Token: 0x04000332 RID: 818
	private int audioInQueue;

	// Token: 0x04000333 RID: 819
	private AudioClip[] audioQueue = new AudioClip[20];

	// Token: 0x04000334 RID: 820
	public PlayerScript playerScript;

	// Token: 0x04000335 RID: 821
	public int num;

	// Token: 0x0400033A RID: 826
	public bool spellCast;

	// Token: 0x0400033B RID: 827
	public bool spellCasted;

	// Token: 0x0400033D RID: 829
	public int spells;

	// Token: 0x0400033E RID: 830

	// Token: 0x0400033F RID: 831
	public AudioClip aud_NoWand;

	public AudioClip aud_KnowItAll;

	public AudioClip aud_Shutup;

	public AudioClip aud_Gah;
	public AudioClip aud_DoToWand;
	public AudioClip aud_Pay;

	// Token: 0x04000340 RID: 832
	public bool brokeWand;

	// Token: 0x04000341 RID: 833
	public GameControllerScript gc;

	// Token: 0x04000342 RID: 834
	public SpriteRenderer wizardSprite;

	// Token: 0x04000343 RID: 835
	public Sprite noWand;

	// Token: 0x04000344 RID: 836
	public Sprite idle;

	// Token: 0x04000345 RID: 837
	public bool inSpell;

	public bool targetingPlayer;
	private float coolDown;
	public float spellTime;
	public float spellCooldown;
	public float wandCooldown;
	public float spellGuilt;
	public GameObject blurEffect;
	public float guilt;
	public PrincipalScript principal;
	float principalDisappearTime;
	bool principalDisappeared;
}