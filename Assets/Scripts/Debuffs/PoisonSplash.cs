
using UnityEngine;

public class PoisonSplash : MonoBehaviour
{
    public int Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Virus")
        {
            other.GetComponent<Virus>().TakeDamage(Damage, Element.POISON);
            Destroy(gameObject);
        }
    }
}
