using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public int maxHealth = 10;
    int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
