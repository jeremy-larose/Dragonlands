using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("open");
    public string dialogueText;
    public string nameText;
    public DialogueWindow dialogue;
    [SerializeField] private int textSpeed;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = dialogue.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(Open, true);

            if (nameText == "")
                nameText = gameObject.name;

            dialogue.Show(dialogueText, nameText, textSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(Open, false);
            dialogue.Close();
        }
    }
}