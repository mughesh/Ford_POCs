using UnityEngine;

public class AddLastAudio : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] LastPlayedAudio playedAudio;
    private void OnEnable()
    {
        playedAudio.clip = clip;
    }
}
