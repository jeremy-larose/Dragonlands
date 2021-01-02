using System.Collections;
using TMPro;
using UnityEngine;

public class DoorTrigger : Interactable
{
    private static readonly int Open = Animator.StringToHash("open");
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private bool shouldDisplayText;
    [SerializeField] private string textToDisplay;
    [SerializeField] private AudioClip clipToPlay;
    private Animator animatorDisplayText;

    private Animator animatorDoorLeft;
    private Animator animatorDoorRight;

    private void Awake()
    {
        animatorDoorLeft = doorLeft.GetComponent<Animator>();
        animatorDoorRight = doorRight.GetComponent<Animator>();
        animatorDisplayText = displayText.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (shouldDisplayText)
        {
            StartCoroutine(DisplayAreaName());
        }

        if (animatorDoorLeft.GetBool(Open) || animatorDoorRight.GetBool(Open))
        {
            CloseDoors();
            base.Interact();
        }
        else
        {
            OpenDoors();
            AudioManager.instance.PlayMusic(clipToPlay);
            base.Interact();
        }
    }

    private void OpenDoors()
    {
        animatorDoorLeft.SetBool(Open, true);
        animatorDoorRight.SetBool(Open, true);
    }

    private void CloseDoors()
    {
        animatorDoorLeft.SetBool(Open, false);
        animatorDoorRight.SetBool(Open, false);
    }

    private IEnumerator DisplayAreaName()
    {
        displayText.gameObject.SetActive(true);
        animatorDisplayText.SetBool(Open, true);

        displayText.text = textToDisplay;
        yield return new WaitForSeconds(4f);
        animatorDisplayText.SetBool(Open, false);
        yield return new WaitForSeconds(4f);
        displayText.gameObject.SetActive(false);
    }
}