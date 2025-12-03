using UnityEngine;

public class Check : MonoBehaviour
{
    [Header("Check Settings")]
    public GameObject objectToCheck;
    public GameObject objectToActivate;

    void Update()
    {
        if (objectToCheck == null)
        {
            if (objectToActivate != null && !objectToActivate.activeSelf)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
