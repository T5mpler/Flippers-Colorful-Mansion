using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    public Vector3 amount;
    void Update()
    {
        transform.Rotate(amount, speed * Time.deltaTime);
    }
}