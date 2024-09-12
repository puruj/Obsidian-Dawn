using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController Instance;

    public Slider BossHealthSlider;

    public int CurrentHealth = 30;

    [SerializeField]
    private BossBattle boss;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BossHealthSlider.maxValue = CurrentHealth;
        BossHealthSlider.value = CurrentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= Mathf.Abs(damageAmount);

        if (CurrentHealth <= 0 )
        {
            CurrentHealth = 0;
            boss.EndBattle();
        }

        BossHealthSlider.value = CurrentHealth;
    }
}
