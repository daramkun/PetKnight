using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClips;

    void Awake()
    {
        if (!_audioSource)
            _audioSource = GetComponent<AudioSource>();
    }

    public void Play(int index)
    {
        _audioSource.PlayOneShot(_audioClips[index], 0.5f);
    }
}