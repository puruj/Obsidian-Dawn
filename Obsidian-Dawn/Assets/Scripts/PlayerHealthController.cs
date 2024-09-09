using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    [HideInInspector]
    public int CurrentHealth;
    public int MaxHealth;

    public float InvincibilityLength;

    public float FlashLength;

    public SpriteRenderer[] PlayerSprites;

    private float invincibilityCounter;

    private float flashCounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;

        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                foreach(SpriteRenderer spriteRenderer in PlayerSprites)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                }
                flashCounter = FlashLength;
            }

            if (invincibilityCounter <= 0)
            {
                foreach (SpriteRenderer spriteRenderer in PlayerSprites)
                {
                    spriteRenderer.enabled = true;
                }
                flashCounter = 0f;
            }
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if (invincibilityCounter <= 0)
        {
            CurrentHealth -= Mathf.Abs(damageAmount);

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;                
                RespawnController.Instance.Respawn();
            }
            else
            {
                invincibilityCounter = InvincibilityLength;
            }

            UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
        }
    }

    public void FillHealth()
    {
        CurrentHealth = MaxHealth;
        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        CurrentHealth += Mathf.Abs(healAmount);

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
    }


}
