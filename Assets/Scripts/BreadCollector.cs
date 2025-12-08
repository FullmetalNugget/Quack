using UnityEngine;

public class BreadCollector : MonoBehaviour
{
    public HealthManager hm;    // Assign the object with HealthManager
    public bool goldenBread;    // Set true for golden bread

    public void CollectBread()
    {
        ushort amount = goldenBread ? (ushort)10 : (ushort)1;

        // ✨ DO NOT TOUCH GameStats DIRECTLY
        // USE YOUR ADDPOINTS FUNCTION
        hm.AddPoints(amount);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            CollectBread();
    }
}

