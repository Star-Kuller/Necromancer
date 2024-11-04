using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class Dj : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips;
        [SerializeField] private float fadeDuration = 2f;
        [SerializeField] private AudioMixerGroup audioMixerGroup;
        
        public float Progress
        {
            get => CurrentSource.time / CurrentClip.length;
            set => CurrentSource.time = Mathf.Clamp(value * CurrentClip.length, 0, CurrentClip.length);
        }

        public string CurrentSongName => CurrentClip.name;
        public float CurrentSongTime => CurrentSource.time;
        public float CurrentSongLength => CurrentClip.length;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if(value)
                    StopSong();
                else
                    ResumeSong();
            }
        }

        private AudioSource[] _audioSources;
        private int _currentIndex;
        private AudioSource NextSource => _audioSources[(_currentIndex + 1) % 2];
        private AudioClip NextClip => audioClips[(_currentIndex + 1) % audioClips.Count];
        private AudioSource CurrentSource => _audioSources[_currentIndex % 2];
        private AudioClip CurrentClip => audioClips[_currentIndex % audioClips.Count];
        
        private bool _isPaused;
        private bool _isSongStopped;

        public void Next()
        {
            if(CurrentSource.time > CurrentClip.length - fadeDuration)
                CurrentSource.Stop();
            else
                CurrentSource.time = CurrentClip.length - fadeDuration;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            StopSong();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(!_isPaused) ResumeSong();
        }
        
        private void Awake()
        {
            _audioSources = new AudioSource[2];
            for (var i = 0; i < 2; i++)
            {
                _audioSources[i] = gameObject.AddComponent<AudioSource>();
                _audioSources[i].loop = false;
                _audioSources[i].outputAudioMixerGroup = audioMixerGroup;
            }

            if (audioClips.Count == 0) return;
            
            CurrentSource.clip = CurrentClip;
            NextSource.clip = NextClip;
            CurrentSource.Play();
            StartCoroutine(PlayAudioSequence());
        }

        private IEnumerator PlayAudioSequence()
        {
            while (true)
            {
                if (CurrentSource.time > CurrentClip.length - fadeDuration)
                {
                    var fadeTime = CurrentSource.time - (CurrentClip.length - fadeDuration);
                    NextSource.time = fadeTime;
                    var fadeProgress = fadeTime / fadeDuration;
                    CurrentSource.volume = 1 - fadeProgress;
                    NextSource.volume = fadeProgress;
                    if (!NextSource.isPlaying && !_isPaused)
                    {
                        NextSource.clip = NextClip;
                        NextSource.Play();
                    }
                }
                else
                {
                    CurrentSource.volume = 1;
                    NextSource.volume = 0;
                    NextSource.time = 0;
                    NextSource.Stop();
                }
                if (!CurrentSource.isPlaying && !_isSongStopped)
                {
                    _currentIndex++;
                    if (!CurrentSource.isPlaying)
                    {
                        CurrentSource.clip = CurrentClip;
                        CurrentSource.Play();
                    }
                }
                yield return null;
            }
        }

        private void StopSong()
        {
            _isSongStopped = true;
            CurrentSource.Pause();
            NextSource.Pause();
        }
        
        private void ResumeSong()
        {
            _isSongStopped = false;
            CurrentSource.UnPause();    
            NextSource.UnPause();
        }
    }
}
