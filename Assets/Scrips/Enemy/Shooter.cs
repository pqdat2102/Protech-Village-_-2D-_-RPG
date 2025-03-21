using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab; // Prefab của viên đạn
    [SerializeField] private float bulletMoveSpeed; // Tốc độ di chuyển của đạn
    [SerializeField] private int burstCount; // Số lần bắn loạt
    [SerializeField] private int projectilesPerBurst; // Số đạn trong mỗi loạt (ví dụ: 5 viên)
    [SerializeField][Range(0, 359)] private float angleSpread; // Góc phân bố đạn (độ)
    [SerializeField] private float startingDistance = 0.1f; // Khoảng cách từ enemy đến điểm spawn đạn
    [SerializeField] private float timeBetweenBursts; // Thời gian nghỉ giữa các loạt
    [SerializeField] private float restTime = 1f; // Thời gian nghỉ sau khi bắn xong


    private bool isShooting = false;
    public void Attack()
    {
        if (!isShooting) 
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, curentAngle, angleStep;

        TargetConeOfInfluence(out startAngle, out curentAngle, out angleStep);

        for (int i = 0; i < burstCount; i++) 
        { 
            for(int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(curentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos , Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }
                curentAngle += angleStep;
            }

            curentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBursts);
            TargetConeOfInfluence(out startAngle, out curentAngle, out angleStep);
        }
        
        yield return new WaitForSeconds(restTime);
        isShooting = false;        
    }

    private void TargetConeOfInfluence(out float startAngle, out float curentAngle, out float angleStep)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        float endAngle = targetAngle;
        curentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if(angleSpread != 0f)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            curentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float curentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(curentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(curentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
