using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002F RID: 47
public class JoeGameScript : MonoBehaviour
{
	// Token: 0x060000E0 RID: 224 RVA: 0x00008874 File Offset: 0x00006A74
	private void Start()
	{
		this.joe.beatUps = 0;
		this.joe.beatingUp = false;
		this.joe.wrongAnswers = 0;
		this.problem = 0;
		this.playerAnswer.characterValidation = InputField.CharacterValidation.Alphanumeric;
		this.playerAnswer.ActivateInputField();
		int num = Mathf.RoundToInt(Random.Range(0f, 2f));
		this.tipText.text = this.tipTexts[num];
		this.NewProblem();
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000088F4 File Offset: 0x00006AF4
	public void Update()
	{
		if (this.inQuestion)
		{
			this.gc.UnlockMouse();
		}
		else
		{
			this.gc.LockMouse();
		}
		if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
		{
			this.CheckAnswer();
		}
		if (this.problem > 3)
		{
			this.endDelay -= 1f * Time.unscaledDeltaTime;
			if (this.endDelay <= 0f)
			{
				this.ExitGame();
			}
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00008974 File Offset: 0x00006B74
	public void NewProblem()
	{
		if (this.problem <= 3)
		{
			this.problem++;
			this.sign = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.playerAnswer.ActivateInputField();
			if (this.sign == 0)
			{
				this.solution = "B";
				this.questionText.fontSize = 18;
				this.questionText.text = string.Concat(new object[]
				{
					"Problem: ",
					this.problem,
					": \n",
					"When did the meme start?",
					"                                      A) July 2018          B) Feburary 2019",
					"                                 C) September 2019       D) May 2017"
				});
			}
			if (this.sign == 1)
			{
				this.solution = "A";
				this.questionText.fontSize = 18;
				this.questionText.text = string.Concat(new object[]
				{
					"Problem: ",
					this.problem,
					": \n",
					"What meme is this part of?",
					"                                      A) Ligma         B) Ninja",
					"                                 C) Clout Chaser        D)  Twitter"
				});
			}
			if (this.sign == 2)
			{
				this.solution = "A";
				this.questionText.fontSize = 18;
				this.questionText.text = string.Concat(new object[]
				{
					"Problem: ",
					this.problem,
					": \n",
					"What is the joke?",
					"                                      A) Joe Mama           B) Yo Mama ",
					"                                 C) Sus' mama       D) Tu Mama"
				});
			}
			if (this.sign == 3)
			{
				this.solution = "A";
				this.questionText.fontSize = 18;
				this.questionText.text = string.Concat(new object[]
				{
					"Problem: ",
					this.problem,
					": \n",
					"Who is joe?",
					"                                      A) Don't ask who Joe is          B) Joe Mama",
					"                                 C) Your mom's person         D) Someone "
				});
			}
			if (this.sign == 4)
			{
				this.solution = "D";
				this.questionText.fontSize = 18;
				this.questionText.text = string.Concat(new object[]
				{
					"Problem: ",
					this.problem,
					": \n",
					"What social media app made this meme popular?",
					"                                      A) Youtube          B) Facebook",
					"                                 C) Twitter       D) Instagram"
				});
			}
			this.inQuestion = true;
			return;
		}
		this.inQuestion = false;
		this.endDelay = 2f;
		if (this.wrongAnswers == 0)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 3f));
			this.questionText.text = this.correctHintTexts[num];
			return;
		}
		int num2 = Mathf.RoundToInt(Random.Range(0f, 3f));
		this.questionText.text = this.incorrectHintTexts[num2];
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00008C54 File Offset: 0x00006E54
	public void CheckAnswer()
	{
		if (this.playerAnswer.text == this.solution)
		{
			this.results[this.problem - 1].texture = this.correctAnswer;
		}
		else
		{
			this.joe.wrongAnswers++;
			this.results[this.problem - 1].texture = this.incorrectAnswer;
		}
		this.NewProblem();
		this.playerAnswer.text = string.Empty;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00008CD8 File Offset: 0x00006ED8
	public void ExitGame()
	{
		this.joe.DecideEmotion();
		this.gc.DeactivateJoeGame(base.gameObject);
		this.joe.agent.speed = (float)Mathf.RoundToInt(Random.Range(15f, 20f));
	}

	// Token: 0x040001B6 RID: 438
	public AudioSource audioDevice;

	// Token: 0x040001B7 RID: 439
	public int sign;

	// Token: 0x040001B8 RID: 440
	public InputField playerAnswer;

	// Token: 0x040001B9 RID: 441
	public Text questionText;

	// Token: 0x040001BA RID: 442
	public string[] tipTexts = new string[]
	{
		"Be good with memes",
		"Know your meme",
		"Know what meme i'm talking about"
	};

	// Token: 0x040001BB RID: 443
	public Text tipText;

	// Token: 0x040001BC RID: 444
	public Texture correctAnswer;

	// Token: 0x040001BD RID: 445
	public Texture incorrectAnswer;

	// Token: 0x040001BE RID: 446
	public RawImage[] results = new RawImage[3];

	// Token: 0x040001BF RID: 447
	public string solution;

	// Token: 0x040001C0 RID: 448
	public int problem;

	// Token: 0x040001C1 RID: 449
	private float endDelay;

	// Token: 0x040001C2 RID: 450
	public GameControllerScript gc;

	// Token: 0x040001C3 RID: 451
	public string[] correctHintTexts = new string[]
	{
		"Thank you so much 4 your help! It means a lot",
		"You'll make a lot of friends if you're like this!",
		"Man how did I not know that!!",
		"I think I have a new best friend now!"
	};

	// Token: 0x040001C4 RID: 452
	public string[] incorrectHintTexts = new string[]
	{
		"WHYYY You suck! You told me incorrect information",
		"Why I hate you, I thought you were nice but I guess i was wrong...",
		"Grrrrrrrrrr........... YOU'RE SO STUPID, LIKE WTAF IS WRONG WITH YOU",
		"I HATE YOU FOREVER!!!! I KNOW WHERE YOU LIVE ESTUPIDO"
	};

	// Token: 0x040001C5 RID: 453
	public int wrongAnswers;

	// Token: 0x040001C6 RID: 454
	private bool inQuestion;

	// Token: 0x040001C7 RID: 455
	public JoeWondererScript joe;
}
