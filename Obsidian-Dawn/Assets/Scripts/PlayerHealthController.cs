using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController PlayerHealthManager;

    [HideInInspector]
    public int CurrentHealth;
    public int MaxHealth;

    private void Awake()
    {
        PlayerHealthManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
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
    }

}
