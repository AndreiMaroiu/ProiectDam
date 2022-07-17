using UnityEngine;
using UnityEngine.Audio;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager musicManagerInstance;
        [SerializeField] private AudioMixer _volumes;

        private const float MaxVolume = 5.00f;

        private void Awake()
        {
            _volumes.SetFloat("Master", PlayerPrefs.GetFloat("Master", MaxVolume));
            _volumes.SetFloat("Music", PlayerPrefs.GetFloat("Music", MaxVolume));
            _volumes.SetFloat("Sound Effects", PlayerPrefs.GetFloat("Sound Effects", MaxVolume));

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
}
