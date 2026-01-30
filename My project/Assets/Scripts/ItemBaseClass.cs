using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseClass : MonoBehaviour, Interactable
{
    protected bool isCollected = false;
    protected float cooldown;
    public static event Action<ItemBaseClass> OnItemCollected;
    protected virtual void Use()
    {

    }
    void OnCollected()
    {
        if(PlayerMovement.isItemsFull) return;
        isCollected = true;
        OnItemCollected?.Invoke(this);
        Destroy(gameObject);
    }
    public void Interact()
    {
        if(isCollected)
        Use();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollected();
        }
    }
}
