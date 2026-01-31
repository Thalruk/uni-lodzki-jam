using System;
using UnityEngine;

public class ItemBaseClass : MonoBehaviour, Interactable
{
    [SerializeField] protected float cooldown;
    public Sprite maskImage;
    protected Transform player, trashPoint;
    protected bool isCollected = false, isHasDurationTime;
    public static event Action<ItemBaseClass> OnItemCollected;
    public bool isPassive, isEquipped;
    protected float useDuration;
    float timer;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerGraphics").transform;
        trashPoint = GameObject.FindWithTag("TrashPoint").transform;
    }
    protected virtual void Use()
    {

    }

    protected virtual void Collect()
    {
        isCollected = true;
        OnItemCollected?.Invoke(this);
        gameObject.transform.parent = player;
        gameObject.transform.position = trashPoint.position;
        if (gameObject.TryGetComponent(out Collider2D collider))
        {
            collider.enabled = false;
        }

    }
    void OnCollected()
    {
        if (PlayerMovement.isItemsFull) return;
        Collect();
    }
    public void Interact()
    {
        if (isEquipped && isCollected && Time.time > timer)
        {
            Use();
            timer = Time.time + cooldown;
            if (isHasDurationTime)
            {
                Invoker.InvokeDelayed(Unequip, useDuration);
            }
        }
    }
    protected virtual void Unequip()
    {
        gameObject.transform.position = trashPoint.position;
        isEquipped = false;
    }
    public bool TryEquip()
    {
        if (Time.time > timer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnItemChange(bool isThisItem)
    {
        if (isThisItem)
        {
            gameObject.transform.localPosition = player.localPosition;
            gameObject.transform.rotation = player.rotation;
            isEquipped = true;
        }
        else
        {
            gameObject.transform.position = trashPoint.position;
            isEquipped = false;
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
