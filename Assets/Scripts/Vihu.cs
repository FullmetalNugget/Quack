using UnityEngine;

public class Vihu : MonoBehaviour
{
    public enum VihuTyyppi
    {
        Rekka,
        Varis,
        Ansa
    }

    public VihuTyyppi type;

    [Header("Varis")]
    public float HP_varis = 1f;
    public float dmg_varis = 1f;

    [Header("Rekka")]
    public float speed = 15f;
    public float HP_rekka = 5f;
    public float dmg_rekka = 5f;

    [Header("Ansa")]
    public float damage_ansa = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch (type)
        {
            case VihuTyyppi.Rekka:
                //HandleRekka();
                break;

            case VihuTyyppi.Varis:
                //HandleVaris();
                break;

            case VihuTyyppi.Ansa:
                //HandleAnsa();
                break;
        }
    }
}
