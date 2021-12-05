using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private static MusicManager musicManagerInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (musicManagerInstance == null)
        {
            musicManagerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
