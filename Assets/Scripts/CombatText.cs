using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private static int sortingOrder;
    private float disappearTimer;
    private float groundPosition;
    private Vector3 moveVector;
    private Color textColor;
    private TextMeshPro textMesh;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        //transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * (3f * Time.deltaTime);

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .66f)
        {
            // first half of the popup
            //float increaseScaleAmount = .5f;
            transform.position += moveVector * Time.deltaTime;
            //transform.localScale += Vector3.one * (increaseScaleAmount * Time.deltaTime);
        }
        else
        {
            // second half of the popup
            //float decreaseScaleAmount = 2f;
            if (transform.position.y >= groundPosition)
            {
                transform.position -= moveVector * (9.8f * Time.deltaTime);
            }
            else
            {
                moveVector = Vector3.zero;
            }

            //transform.localScale -= Vector3.one * (decreaseScaleAmount * Time.deltaTime);
        }

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


    public static CombatText Create(Vector3 position, float damageAmount, bool isCriticalHit, Color color)
    {
        Transform damagePopupTransform =
            Instantiate(GameAssets.i.pfCombatText, position, Quaternion.identity);
        CombatText damagePopup = damagePopupTransform.GetComponent<CombatText>();
        damagePopup.Setup(damageAmount, isCriticalHit, color, position);

        return damagePopup;
    }

    public void Setup(float damageAmount, bool isCriticalHit, Color color, Vector3 position)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.color = Color.cyan;

        if (!isCriticalHit)
        {
            textMesh.fontSize = 5f;
            textMesh.color = color;
        }
        else
        {
            textMesh.fontSize = 5f;
            textMesh.color = Color.red;
        }

        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(0f, 2f, 0f);
        groundPosition = position.y - .1f;
    }
}