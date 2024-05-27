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
    private float _delay = 0.25f;

    private void Start()
    {
        _audioSource.clip = _audioClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Key>() == false)
            StartAlarm();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Key>() == false)
            StopAlarm();
    }

    private void StartAlarm()
    {
        if (_audioSource.isPlaying == false)
        {
            _audioSource.Play();
            StartCoroutine(IncreaseVolumeSmoothly(_minVolume, _maxVolume));
        }
    }

    private void StopAlarm()
    {
        StartCoroutine(IncreaseVolumeSmoothly(_maxVolume, _minVolume));

        if (_audioSource.volume == _minVolume)
            _audioSource.Stop();
    }

    private IEnumerator IncreaseVolumeSmoothly(float startVolume, float targetVolume)
    {
        var wait = new WaitForSeconds(_delay);
        float currentVolume;

        _audioSource.volume = startVolume;

        while (_audioSource.volume != targetVolume)
        {
            currentVolume = _audioSource.volume;
            _audioSource.volume = Mathf.MoveTowards(currentVolume, targetVolume, _volumeStep);

            yield return wait;
        }

        _audioSource.volume = targetVolume;
    }
}
