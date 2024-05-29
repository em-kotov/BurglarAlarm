using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SecurityAlarm : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private float _maxVolume = 1;
    private float _minVolume = 0;
    private float _volumeStep = 0.1f;
    private float _delay = 0.1f;

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Key>(out Key key) == false)
            StartAlarm();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Key>(out Key key) == false)
            StopAlarm();
    }

    private void StartAlarm()
    {
        if (_audioSource.isPlaying == false)
        {
            _audioSource.Play();
            StartCoroutine(IncreaseVolumeSmoothly());
        }
    }

    private void StopAlarm()
    {
        StartCoroutine(DecreaseVolumeSmoothlyUntilStop());
    }

    private IEnumerator IncreaseVolumeSmoothly()
    {
        var wait = new WaitForSeconds(_delay);
        _audioSource.volume = _minVolume;

        while (_audioSource.volume != _maxVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, _volumeStep);
            yield return wait;
        }
    }

    private IEnumerator DecreaseVolumeSmoothlyUntilStop()
    {
        var wait = new WaitForSeconds(_delay);
        _audioSource.volume = _maxVolume;

        while (_audioSource.volume != _minVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _minVolume, _volumeStep);
            yield return wait;
        }

        _audioSource.Stop();
    }
}
