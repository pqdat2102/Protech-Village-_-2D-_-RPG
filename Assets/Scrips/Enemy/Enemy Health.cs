using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("")]
    [SerializeField] private int startHealth = 3; // Máu khởi đầu của Quái
    [SerializeField] private GameObject deathVFXPrefabs; // prefab vfx của quái lúc chết
    [SerializeField] private float knockBackThrust = 20f; // khoảng các bị đẩy lùi


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
        currentHealth -= damage; // trừ máu
        knockBack.GetKnockBack(PlayerController.Instance.transform, knockBackThrust); // bị đẩy lùi
        StartCoroutine(flash.FlashRoutine()); // bị trắng
        StartCoroutine(CheckDetectDeathRoutine()); // kiểm tra xem quái chết chưa
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime()); // Chết cũng phải bị giật về sau rồi mới cho chết
        DetectDeath();
    }

    public void DetectDeath() // Khi hp quái <= 0, quái chết
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefabs, transform.position, Quaternion.identity); // Bật VFX chết
            GetComponent<PickUpSpawner>().DropItems(); // getcomponet dropitem để rơi item khi quái chết
            Destroy(gameObject); // hủy game object
        }
    }
}
