using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
  
    float horizontal;
    float vertical;
    [SerializeField] Vector2 movementDirection;
    Rigidbody2D rb;
    List<ItemBaseClass> items = new List<ItemBaseClass>();
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    private float _baseDashCooldown, _baseSpeed;
    public static bool isItemsFull = false;
    int equippedItemIndex;
    public void AddItemToInventory(ItemBaseClass item)
    {
        if (items.Count < 3)
        {
            items.Add(item);
        }
        else
        {
            isItemsFull = true;
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _baseDashCooldown = dashCooldown;
        _baseSpeed = speed;
        FoxMask.OnFoxMaskUsed += FoxMaskUsed;
        ItemBaseClass.OnItemCollected += AddItemToInventory;
        FoxMask.OnFoxMaskEndEffect += ResetMovementToBasic;
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        if (Input.GetMouseButtonDown(1) && canDash && movementDirection.sqrMagnitude > 0.1f)
        {
            StartCoroutine(Dash());
        }
        HandelKeyInventoryDown();

    }
    void UseEquippedItem()
    {
        items[equippedItemIndex].Interact();
    }
    void FoxMaskUsed()
    {
        SetDashCooldown(dashCooldown / 2, speed * 1.2f);
    }
    void HandelKeyInventoryDown()
    {
        if (items.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && items.Count>0 && items[0].TryEquip())
        {
            EquipItem(0);
            equippedItemIndex = 0;
            if (items[equippedItemIndex].isPassive)
            {
                items[equippedItemIndex].Interact();
            }


        }else if (Input.GetKeyDown(KeyCode.Alpha2) && items.Count > 1 && items[1].TryEquip())
        {
            EquipItem(1);
            equippedItemIndex = 1;
            if (items[equippedItemIndex].isPassive)
            {
                items[equippedItemIndex].Interact();
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && items.Count > 2 && items[2].TryEquip()) 
        {
            EquipItem(2);
            equippedItemIndex = 2;
            if (items[equippedItemIndex].isPassive)
            {
                items[equippedItemIndex].Interact();
            }

        }
        if (!items[equippedItemIndex].isPassive && Input.GetMouseButtonDown(0))
        {
            UseEquippedItem();
        }
    }
    void EquipItem(int index)
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].OnItemChange(i == index);
        }
    }
    public void ResetMovementToBasic()
    {
        dashCooldown = _baseDashCooldown;
        speed = _baseSpeed;
    }
    public void SetDashCooldown(float dashCooldownValue, float speedValue)
    {
        dashCooldown = dashCooldownValue;
        speed = speedValue;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = movementDirection * speed;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = movementDirection.normalized * dashPower;
        //tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        //tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void OnDisable()
    {
        FoxMask.OnFoxMaskUsed -= FoxMaskUsed;
        ItemBaseClass.OnItemCollected -= AddItemToInventory;
        FoxMask.OnFoxMaskEndEffect -= ResetMovementToBasic;
    }
    private void OnDestroy()
    {
        FoxMask.OnFoxMaskUsed -= FoxMaskUsed;
        ItemBaseClass.OnItemCollected -= AddItemToInventory;
        FoxMask.OnFoxMaskEndEffect -= ResetMovementToBasic;
    }
}
