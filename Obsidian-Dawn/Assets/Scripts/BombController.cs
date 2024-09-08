using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject Explosion;
    [SerializeField]
    private float timeToExplode = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
