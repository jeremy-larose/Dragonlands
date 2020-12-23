using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthBar;
    // Start is called before the first frame update
    private void Start()
    {
        healthBar.localScale = new Vector3(1f, 1f, 1f);
    }

    public void SetSize(float sizeNormalized)
    {
        healthBar.localScale = new Vector3(sizeNormalized, 1f, 1f);
    }
}
