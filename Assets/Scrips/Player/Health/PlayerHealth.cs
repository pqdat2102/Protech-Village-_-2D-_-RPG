using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth> 
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 15f;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    private Slider healhSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack  knockBack;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";

    protected override void Awake()
    {
        base.Awake();
        knockBack  = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }  
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
        UpdateHealthSlider();
        CheckIfPlayerDeath();

    }


    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Death");
        }
    }

    private void UpdateHealthSlider()
    {
        if(healhSlider == null) 
        {
            healhSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healhSlider.maxValue = maxHealth;
        healhSlider.value = currentHealth;
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
