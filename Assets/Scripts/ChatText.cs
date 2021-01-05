using TMPro;
using UnityEngine;

public class ChatText : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 1f;
    [SerializeField] private TextMeshProUGUI textToWrite;
    [SerializeField] private TextMeshProUGUI nameMesh;
    private float disappearTimer;
    private float groundPosition;
    private Vector3 moveVector;
    private Color textColor;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = textToWrite;
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 4f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }


    public static ChatText Create(Vector3 position, string textToWrite, string nameToWrite, Color color)
    {
        Transform chatPopupTransform =
            Instantiate(GameAssets.i.pfChatText, position, Quaternion.identity);
        ChatText chatPopup = chatPopupTransform.GetComponent<ChatText>();
        chatPopup.Setup(textToWrite, nameToWrite, color);
        return chatPopup;
    }

    public void Setup(string textToWrite, string nameToWrite, Color color)
    {
        textMesh.SetText(textToWrite);
        nameMesh.SetText(nameToWrite);
        textMesh.color = Color.cyan;

        textMesh.fontSize = 14f;
        textMesh.color = color;

        disappearTimer = DISAPPEAR_TIMER_MAX;
    }
}