using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth> 
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 15f;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack  knockBack;
    private Flash flash;

    protected override void Awake()
    {
        base.Awake();
        knockBack  = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    private void HealPlayer()
    {
        currentHealth += 1;
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if(!canTakeDamage) {return; }

        ScreenShake.Instance.ShakeScreen();
        knockBack.GetKnockBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());

        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());

    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
