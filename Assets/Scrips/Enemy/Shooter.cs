using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate; 

    private bool isShooting = false;

    private void OnValidate()
    {
        if(oscillate) { stagger = true; }
        if(!oscillate) { stagger = false; }
        if(projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if(burstCount < 1) { burstCount = 1; }
        if(timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if(restTime < 0.1f ) { restTime = 0.1f; }
        if(startingDistance < 0.1f) { startingDistance = 0.1f; }
        if(angleSpread == 0) { angleSpread = 1; }
        if(bulletMoveSpeed <= 0) { bulletMoveSpeed = 1; }
    }
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

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
        }

        for (int i = 0; i < burstCount; i++) 
        {
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle); 
            }

            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for(int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos , Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); } 
        }
        
        yield return new WaitForSeconds(restTime);
        isShooting = false;        
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if(angleSpread != 0f)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
