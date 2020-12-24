using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _myRigidbody;
    private Animator _myAnimator;
    private Vector3 _change = Vector3.zero;
    public Light torch;

    [SerializeField] private float moveSpeed = 4f;

    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int Attacking = Animator.StringToHash("attacking");

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.z = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F1) )
        {
            if( torch.isActiveAndEnabled )
                torch.gameObject.SetActive( false );
            else
            {
                torch.gameObject.SetActive( true );
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {
        AnimateAndMoveCharacter();

    }

    private IEnumerator Attack()
    {
        _myAnimator.SetBool( Attacking, true );
        yield return new WaitForSeconds(.4f);
        _myAnimator.SetBool( Attacking, false);
    }

    void AnimateAndMoveCharacter()
    {
        _change.Normalize();

        if (_change != Vector3.zero)
        {
            _myRigidbody.MovePosition( transform.position + _change * (moveSpeed * Time.deltaTime ));
            _myAnimator.SetFloat(MoveX, _change.x);
            _myAnimator.SetFloat( MoveZ, _change.z );
            _myAnimator.SetBool( Moving, true );
        }
        else
        {
            _myAnimator.SetBool( Moving, false );
        }
    }
}
