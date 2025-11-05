using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField] LastPlayedAudio last;
    [SerializeField] AudioSource audioSource;

    public void PlayLast()
    {
        audioSource.clip = last.clip;
        audioSource.Play();
    }
}
