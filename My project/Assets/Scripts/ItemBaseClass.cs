using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseClass : MonoBehaviour, Interactable
{
    protected bool isCollected = false;
    protected float cooldown;
    public static event Action<ItemBaseClass> OnItemCollected;
    public static bool isPassive;
    protected virtual void Use()
    {

    }
    protected virtual void Collect()
    {
        isCollected = true;
        OnItemCollected?.Invoke(this);
    }
    void OnCollected()
    {
        if(PlayerMovement.isItemsFull) return;
        Collect();
        Destroy(gameObject);
    }
    public void Interact()
    {
        if(isCollected && !isPassive)
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
