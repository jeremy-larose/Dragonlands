using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueWindow : MonoBehaviour
{
    private const string KAlphaCode = "<color=#00000000>";
    private const float KMaxTextTime = 0.1f;
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
            displayedText = Text.text.Insert(alphaIndex, KAlphaCode);
            Text.text = displayedText;
 
            yield return new WaitForSecondsRealtime(KMaxTextTime / typeSpeed);
        }
 
        State = DialogueState.Waiting;
 /*
        if (CursorObject != null)
            CursorObject?.SetActive(true); */
 
        yield return null;
    }
}