using UnityEngine;

public class NumberBallonScript : MonoBehaviour
{
	public int value;
	Rigidbody rb;
	public float timeToNextDirection;
	public float height;
	public Vector3 moveVelocity;
	public Transform ballonHolder;
	float delay;
	void Start()
	{
		height = transform.position.y;
		rb = GetComponent<Rigidbody>();
	}
	private void Update()
	{
		if (delay > 0f) delay -= Time.deltaTime;
		Vector3 vector3 = transform.position;
		vector3.y = height;
		transform.position = vector3;
		if (Time.time > timeToNextDirection)
		{
			ChangeDirection();
		}
		
	}
	private void OnCollisionEnter(Collision collision)
	{
		Reflect(collision.GetContact(0).normal);
	}
	void Reflect(Vector3 normal)
	{
		moveVelocity = Vector3.Reflect(moveVelocity, normal);
	}
	void ChangeDirection()
	{
		timeToNextDirection += UnityEngine.Random.Range(5f, 15f);
		System.Random random = new System.Random();
		if (random.NextDouble() <= 0.5f)
		{
			moveVelocity = GetRandomDirection() * 5f;
		}
		else
		{
			moveVelocity = InsideCircleDireection() * 5f;
		}
	}
	Vector3 GetRandomDirection()
	{
		return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
	}
	Vector3 InsideCircleDireection()
	{
		Vector3 vector = Random.insideUnitSphere;
		vector.y = 0f;
		return vector;
	}
	private void FixedUpdate()
	{
		rb.velocity = moveVelocity;
	}
}