using UnityEngine;

public class WallFader : MonoBehaviour
{
    public GameObject[] wallsToFade;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[WallFader] Player has entered trigger!");
            foreach (var wall in wallsToFade)
            {
                var wallMesh = wall.GetComponent<MeshRenderer>();
                if (wallMesh.enabled)
                    wallMesh.enabled = !wallMesh.enabled;
            }
        }
    }
}