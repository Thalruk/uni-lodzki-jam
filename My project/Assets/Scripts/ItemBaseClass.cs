using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseClass : MonoBehaviour, Interactable
{
    [SerializeField] Transform trashPoint;
    protected bool isCollected = false;
    protected float cooldown;
    public static event Action<ItemBaseClass> OnItemCollected;
    public bool isPassive;
    float timer;
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
        gameObject.transform.position = trashPoint.position;
    }
    public void Interact()
    {
        if(isCollected && !isPassive && Time.time > timer)
        {
            Use();
            timer = Time.time + cooldown;
        }
        if (isPassive)
        {
            Use();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollected();
        }
    }
}
