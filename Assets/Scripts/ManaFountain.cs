using System.Collections.Generic;
using UnityEngine;

public class ManaFountain : MonoBehaviour
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
            FindFirstObjectByType<Healer>().mana = FindFirstObjectByType<Healer>().maxMana;
            // Ally[] allies = FindObjectsByType<Ally>(FindObjectsSortMode.None);
            // foreach (Ally ally in allies){
            //     ally.health = ally.maxHealth;
            // }
            flashAnimator.SetTrigger("FlashBlue");
        }
    }

    void playActivateSound(){
        audioSource.PlayOneShot(audioSource.clip);
    }
}
