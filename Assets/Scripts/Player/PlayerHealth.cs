using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    [Header("Health")]
    public float maxHealth = 100f;
    private float health;
    private PlayerUI _playerUI;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;

    [Header("Sounds")] 
    public AudioSource damageSound;
    public AudioSource healSound;
    public AudioSource healNoSound;

    private float durationTimer;


    private void Start() {
        health = maxHealth;
        _playerUI = GetComponent<PlayerUI>();
        SetOverlayOpacity(0);
    }

    private void Update() {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0) {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration) {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                SetOverlayOpacity(tempAlpha);
            }
        }
    }

    public void UpdateHealthUI() {
        _playerUI.UpdateHealth(health);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        durationTimer = 0;
        SetOverlayOpacity(0.4f);
        damageSound.Play();
    }

    public void RestoreHealth(float healAmount) {
        if (health < maxHealth) {
            healSound.Play();
            health += healAmount;
        } else {
            healNoSound.Play();
        }
    }

    private void SetOverlayOpacity(float alpha) {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, alpha);
    }
}
