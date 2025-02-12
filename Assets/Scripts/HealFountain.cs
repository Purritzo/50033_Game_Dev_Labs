using System.Collections.Generic;
using UnityEngine;

public class HealFountain : MonoBehaviour
{
    public bool available;
    public Sprite pressedSprite;
    private SpriteRenderer spriteRenderer; 
    public Animator flashAnimator;

    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        available = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (available == false){
            spriteRenderer.sprite = pressedSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            available = false;
            playActivateSound();
            // JANK WAY FOR NOW
            FindFirstObjectByType<Healer>().health = FindFirstObjectByType<Healer>().maxHealth;
            Ally[] allies = FindObjectsByType<Ally>(FindObjectsSortMode.None);
            foreach (Ally ally in allies){
                ally.health = ally.maxHealth;
                ally.hpBar.SetHealth(ally.maxHealth); // NOTE: I really should not need to manual sync this in future
            }
            flashAnimator.SetTrigger("FlashGreen");
        }
    }

    void playActivateSound(){
        audioSource.PlayOneShot(audioSource.clip);
    }
}
