using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    [HideInInspector]
    public int CurrentHealth;
    public int MaxHealth;

    private void Awake()
    {
        Instance = this;        
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
        
    }

    public void DamagePlayer(int damageAmount)
    {
        CurrentHealth -= Mathf.Abs(damageAmount);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            gameObject.SetActive(false);
        }

        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);

    }

}
