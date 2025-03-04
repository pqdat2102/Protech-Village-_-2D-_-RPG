using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("")]
    [SerializeField] private int startHealth = 3; // Máu khởi đầu của Quái
    [SerializeField] private GameObject deathVFXPrefabs;
    [SerializeField] private float knockBackThrust = 15f;


    private int currentHealth; // Máu hiện tại của quái 
    private KnockBack knockBack; // Đẩy lùi khi bị tấn công
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>(); // Gọi Component Flash
        knockBack = GetComponent<KnockBack>(); // gọi Component KnockBack
    }
    private void Start()
    {
        currentHealth = startHealth; // Bắt đầu, máu hiện tại bằng máu khởi đầu
    }

    public void TakeDamage(int damage) // Hàm tính dame quái nhận vào và đẩy lùi quái đi 15f
    {
        currentHealth -= damage;
        knockBack.GetKnockBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath() // Khi hp quái <= 0 thì hủy quái
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefabs, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
