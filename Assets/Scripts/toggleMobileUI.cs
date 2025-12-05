using UnityEngine;

public class toggleMobileUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      #if UNITY_ANDROID
      // Show on Android
      transform.Find("MobileControls").gameObject.SetActive(true);
      #else
      // Hide on PC
      transform.Find("MobileControls").gameObject.SetActive(false);
      #endif
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
