using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    private Animator _animator;
    public string dialogueText;
    public DialogueWindow dialogue;
    private static readonly int Open = Animator.StringToHash("open");
    [SerializeField] private int textSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _animator = dialogue.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool( Open, true );
            dialogue.Show( dialogueText, textSpeed );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            _animator.SetBool( Open, false );
            dialogue.Close();
        }
    }
}
