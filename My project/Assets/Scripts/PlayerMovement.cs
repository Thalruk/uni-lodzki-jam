using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 movementDirection;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite vendigoSprite, playerSprite;
    [SerializeField] GameObject vendigoLeftHand, vendigoRigthHand, vendigoStateHands;
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing = false;
    [SerializeField] float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] List<Image> uiImagesOfMasks = new List<Image>();
    [SerializeField] List<Image> uiImagesOfMasksBG = new List<Image>();
    List<ItemBaseClass> items = new List<ItemBaseClass>();
    private float _baseDashCooldown, _baseSpeed;
    public static bool isItemsFull = false;
    int equippedItemIndex;
    bool VendigoState = false, vendigoLeftHandUsed;
    float horizontal;
    float vertical;
    Rigidbody2D rb;
    Color colorBase;
    public void AddItemToInventory(ItemBaseClass item)
    {
        if (items.Count < 3)
        {
            items.Add(item);
            uiImagesOfMasks[items.Count - 1].sprite = item.maskImage;
            uiImagesOfMasks[items.Count - 1].color = Color.white;
        }
        else
        {
            isItemsFull = true;
        }
    }
    private void Awake()
    {
        colorBase = uiImagesOfMasksBG[0].color;
        rb = GetComponent<Rigidbody2D>();
        _baseDashCooldown = dashCooldown;
        _baseSpeed = speed;
        playerSpriteRenderer.sprite = playerSprite;
        #region Subscriptions 
        FoxMask.OnFoxMaskUsed += FoxMaskUsed;
        ItemBaseClass.OnItemCollected += AddItemToInventory;
        FoxMask.OnFoxMaskEndEffect += ResetMovementToBasic;
        VendigoMask.OnVendigoMaskUsed += VendigoMaksUsed;
        VendigoMask.OnVendigoEndEffect += ResetVendigoState;
        #endregion
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        if (Input.GetMouseButtonDown(1) && canDash && movementDirection.sqrMagnitude > 0.1f && Time.timeScale != 0)
        {
            StartCoroutine(Dash());
        }
        HandelKeyInventoryDown();
        if (Time.timeScale == 0)
        {
            transform.position += (Vector3)movementDirection * speed * Time.unscaledDeltaTime;
        }
    }
    void UseEquippedItem()
    {
        items[equippedItemIndex].Interact();
    }

    void HandelKeyInventoryDown()
    {
        if (items.Count == 0) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (VendigoState)
            {
                VendigoAttack();
            }
            else if (!items[equippedItemIndex].isPassive)
            {
                UseEquippedItem();
            }
        }
        if (items.Any(x => x.isPassive && x.isEquipped)) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && items.Count > 0 && items[0].TryEquip())
        {
            EquipItem(0);
            equippedItemIndex = 0;
            
            if (items[equippedItemIndex].isPassive)
            {
                items[equippedItemIndex].Interact();
            }


        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && items.Count > 1 && items[1].TryEquip())
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetRidOfItem(equippedItemIndex);
        }

    }
    void UIUpdate()
    {
        for (int i = 0; i < uiImagesOfMasks.Count; i++)
        {
            uiImagesOfMasks[i].sprite = null;
            uiImagesOfMasks[i].color = new Color(0, 0, 0, 0);
            uiImagesOfMasksBG[i].color = colorBase;
        }
        for (int i = 0; i < items.Count; i++)
        {
            uiImagesOfMasks[i].sprite = items[i].maskImage;
            uiImagesOfMasks[i].color = Color.white;
        }
    }
    
    void GetRidOfItem(int index)
    {
        var item = items[index];
        item.transform.parent = null;
        item.gameObject.transform.position = transform.position+(transform.right*2f);
        item.GetComponent<Collider2D>().enabled = true;
        items.RemoveAt(index);
        UIUpdate();
    }
    void EquipItem(int index)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].OnItemChange(i == index);
            uiImagesOfMasksBG[i].color = i == index ? new Color32(255, 60, 50,100) : colorBase;

        }
    }
    #region FoxMaskLogic
    void FoxMaskUsed()
    {
        SetDashCooldown(dashCooldown / 2, speed * 1.2f);
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
    #endregion
    #region VendigoMaskLogic
    void VendigoMaksUsed()
    {
        playerSpriteRenderer.sprite = vendigoSprite;
        VendigoState = true;
        vendigoStateHands.SetActive(true);
    }
    void VendigoAttack()
    {
        vendigoLeftHandUsed = !vendigoLeftHandUsed;
        if (vendigoLeftHandUsed)
        {
            vendigoLeftHand.GetComponent<Animation>().Play();
        }
        else
        {
            vendigoRigthHand.GetComponent<Animation>().Play();
        }
    }
    void ResetVendigoState()
    {
        VendigoState = false;
        playerSpriteRenderer.sprite = playerSprite;
        vendigoStateHands.SetActive(false);
    }
    #endregion

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (Time.timeScale != 0)
        {
            rb.velocity = movementDirection * speed;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = new Vector2(transform.position.x - mousePos.x, transform.position.y - mousePos.y);
        transform.up = dir;
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
        VendigoMask.OnVendigoMaskUsed -= VendigoMaksUsed;
        VendigoMask.OnVendigoEndEffect -= ResetVendigoState;
    }
    private void OnDestroy()
    {
        FoxMask.OnFoxMaskUsed -= FoxMaskUsed;
        ItemBaseClass.OnItemCollected -= AddItemToInventory;
        FoxMask.OnFoxMaskEndEffect -= ResetMovementToBasic;
        VendigoMask.OnVendigoMaskUsed -= VendigoMaksUsed;
        VendigoMask.OnVendigoEndEffect -= ResetVendigoState;
    }
}
