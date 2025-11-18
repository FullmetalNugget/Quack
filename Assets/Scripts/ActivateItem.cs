using UnityEngine;

public class ActivateItem : MonoBehaviour
{
    public GameObject itemToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemToActivate.SetActive(true);
        }
    }
}