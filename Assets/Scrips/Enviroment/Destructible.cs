using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour // scrips kiểm tra khi quái hoặc người đánh trúng mấy cái bụi và hộp thì hiển thị hiệu ứng bị hủy và drop ra item
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
        {
            GetComponent<PickUpSpawner>().DropItems(); // drop item
            Instantiate(destroyVFX, transform.position, Quaternion.identity); // tạo vfx destroy
            Destroy(gameObject); // hủy vfx
        }
    }
}
