using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth> 
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 15f;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    private Slider healhSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack  knockBack;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Map1";
    readonly int DEAD_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        knockBack  = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        isDead = false;
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
        if(currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);

            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEAD_HASH);
            StartCoroutine(DeadLoadSceneRoutine());
            
        }
    }

    private IEnumerator DeadLoadSceneRoutine()
    {
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
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
