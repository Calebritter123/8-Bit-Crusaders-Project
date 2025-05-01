using UnityEngine;

public class UIPersistence : MonoBehaviour
{
    private static UIPersistence instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates if another UI loads
        }
    }
}
