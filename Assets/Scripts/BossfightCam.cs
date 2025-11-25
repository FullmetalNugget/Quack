using UnityEngine;

public class BossfightCam : MonoBehaviour
{
    public GameObject itemToActivate;
    public GameObject itemToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemToActivate.SetActive(true);
            itemToDeactivate.SetActive(false);
        }
    }
}