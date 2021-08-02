using UnityEngine;
public class Letter : MonoBehaviour
{
    public TextMesh text;
    [HideInInspector] public Transform player;
    public MoveModifier moveMod;
    public float speed = 10f;
    string letter;
    InverjaScript inverja;
    public void Initalize(string letter, float timer, InverjaScript inverja)
    {
        moveMod.timer = timer;
        moveMod.tick = false;
        moveMod.OnMoveModRemoved += MoveMod_OnMoveModRemoved;
        text.text = letter;
        this.letter = letter;
        this.inverja = inverja;
    }

    private void MoveMod_OnMoveModRemoved(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * Time.deltaTime * speed;
        if (Input.inputString == letter.ToLower())
        {
            foreach (ActivityModifierScript activity in FindObjectsOfType<ActivityModifierScript>())
            {
                activity.movementModList.Remove(moveMod);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC") && other.name != "Inverja" && other.name != "Its a Bully")
        {
            ActivityModifierScript activityModifier = other.GetComponent<ActivityModifierScript>();
            activityModifier.movementModList.Add(moveMod);
            OnHit();
            if (other.CompareTag("Player"))
            {
                inverja.totalHits++;
                PlayerScript.instance.stamina -= 20f;
            }
        }
    }
    public void OnHit()
    {
        moveMod.tick = true;
        GetComponentInChildren<SpriteRenderer>().sprite = null;
        GetComponent<Collider>().enabled = false;
    }
}
