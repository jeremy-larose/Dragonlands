using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    public Light torch;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private UIInventory _uiInventory;
    private Vector3 _change = Vector3.zero;
    private Inventory _inventory;
    private Animator _myAnimator;
    private Character _myCharacter;
    private Rigidbody _myRigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _myAnimator = GetComponentInChildren<Animator>();
        _myCharacter = GetComponent<Character>();

        _inventory = new Inventory(UseItem);
        _uiInventory.SetPlayer(this);
        _uiInventory.SetInventory(_inventory);
    }

    // Update is called once per frame
    private void Update()
    {
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.z = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (torch.isActiveAndEnabled)
                torch.gameObject.SetActive(false);
            else
                torch.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0)) StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        AnimateAndMoveCharacter();
    }

    private void OnTriggerEnter(Collider other)
    {
        var itemGround = other.GetComponent<ItemGround>();

        if (itemGround != null)
        {
            // Touching Item
            _inventory.AddItem(itemGround.GetItem());
            itemGround.DestroySelf();
        }
    }

    private IEnumerator Attack()
    {
        _myAnimator.SetBool(Attacking, true);
        yield return new WaitForSeconds(.4f);
        _myAnimator.SetBool(Attacking, false);
    }

    private void AnimateAndMoveCharacter()
    {
        _change.Normalize();

        if (_change != Vector3.zero)
        {
            _myRigidbody.MovePosition(transform.position + _change * (moveSpeed * Time.deltaTime));
            _myAnimator.SetFloat(MoveX, _change.x);
            _myAnimator.SetFloat(MoveZ, _change.z);
            _myAnimator.SetBool(Moving, true);
        }
        else
        {
            _myAnimator.SetBool(Moving, false);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                Debug.Log("[Player] Using Health Potion!");
                _myCharacter.healthSystem.Heal(Dice.Roll(1, 8));
                _inventory.RemoveItem(new Item {itemType = Item.ItemType.HealthPotion, amount = 1});
                break;
            case Item.ItemType.Coin:
                Debug.Log("[Player] Using a coin!");
                _inventory.RemoveItem(new Item {itemType = Item.ItemType.Coin, amount = 1});
                break;
            default:
                Debug.Log("[Player] Using an item, but no action defined.");
                break;
        }
    }
}