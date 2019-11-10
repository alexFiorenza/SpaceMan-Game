﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    manaPotion,
    coin
}
public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.coin;
    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;
    GameObject player;

    bool hasBeenCollected = false;

    public int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        player= GameObject.Find("Player");
    }
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.coin:
                GameManager.sharedInstance.CollectObject(this);
                break;
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            Collect();
        }
    }
}