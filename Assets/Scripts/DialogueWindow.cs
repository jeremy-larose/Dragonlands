using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//Created by Allen Devs 2020, free to use in your game, don't sweat it, enjoy!
 
using System.Collections;
using UnityEngine;
using TMPro;
 
public class DialogueWindow : MonoBehaviour
{
    const string kAlphaCode = "<color=#00000000>";
    const float kMaxTextTime = 0.1f;
    public int typeSpeed = 2;
 
    public TMP_Text Text;
    private string CurrentText;
 
    Animator _animator;
 
    //A prompt that appears when the dialogue is done writing (usually in the bottom right).
    public GameObject CursorObject;
 
    //A state that can be used to block or allow player input.
    public enum DialogueState
    {
        None,
        Writing,
        Waiting
    }
    public DialogueState State;
    private static readonly int Open = Animator.StringToHash("open");

    void Start()
    {
        _animator = GetComponent<Animator>();
        
        if (_animator == null)
        {
            Debug.LogError("No Animator Controller on DialogueWindow: " + gameObject.name);
        }
    }
 
    public void Show(string text, int textSpeed )
    {
        _animator.SetBool(Open, true);
        CurrentText = text;
        typeSpeed = textSpeed;

/*        if(CursorObject != null)
            CursorObject?.SetActive(false); */
    }
 
    public void Close()
    {        
        _animator.SetBool(Open, false);
    }
 
    public void OnDialogueOpened()
    {
        StartCoroutine(DisplayText());
    }
 
    public void OnDialogueClosed()
    {
        StopAllCoroutines();
        Text.text = "";
 
        State = DialogueState.None;
 /*
        if (CursorObject != null)
            CursorObject?.SetActive(false); */
    }
 
    private IEnumerator DisplayText()
    {
        if (Text == null)
        {
            Debug.LogError("Text is not linked in DialogueWindow: " + gameObject.name);
            yield return null;
        }
 
        State = DialogueState.Writing;
 
        Text.text = "";
 
        string originalText = CurrentText;
        string displayedText = "";
        int alphaIndex = 0;
 
        foreach(char c in CurrentText.ToCharArray())
        {
            alphaIndex++;
            Text.text = originalText;
            displayedText = Text.text.Insert(alphaIndex, kAlphaCode);
            Text.text = displayedText;
 
            yield return new WaitForSecondsRealtime(kMaxTextTime / typeSpeed);
        }
 
        State = DialogueState.Waiting;
 /*
        if (CursorObject != null)
            CursorObject?.SetActive(true); */
 
        yield return null;
    }
}