using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
//original by benjaminoutram on answers.unity3d.com
  
  [RequireComponent(typeof(AudioSource))]
  public class mic_listener : MonoBehaviour
  {
      public bool IsWorking = true;
      private bool _lastValueOfIsWorking;
 
      public bool RaltimeOutput = true;
      private bool _lastValueOfRaltimeOutput;
 
      AudioSource _audioSource;
      private float _lastVolume = 0;
      
      void Start()
      {
          _audioSource = GetComponent<AudioSource>();
 
          if (IsWorking)
          {
              WorkStart();
          }
 
      }
 
 
      void Update()
      {
          CheckIfIsWorkingChanged();
          CheckIfRealtimeOutputChanged();
      }
 
      public void CheckIfIsWorkingChanged()
      {
          if (_lastValueOfIsWorking != IsWorking)
          {
              if (IsWorking)
              {
                  WorkStart();
              }
              else
              {
                  WorkStop();
              }
          }
 
          _lastValueOfIsWorking = IsWorking;
      }
 
      public void CheckIfRealtimeOutputChanged()
      {
          if (_lastValueOfRaltimeOutput != RaltimeOutput)
          {
              DisableSound(RaltimeOutput);
          }
 
          _lastValueOfRaltimeOutput = RaltimeOutput;
      }
 
      public void DisableSound(bool SoundOn)
      {
          if (SoundOn)
          {
              if (_lastVolume > 0)
              {
                  _audioSource.volume = _lastVolume;
              }
              else
              {
                  _audioSource.volume = 1f;
              }
          }
          else
          {
              _lastVolume = _audioSource.volume;
              _audioSource.volume = 0f;
          }
      }
 
      private void WorkStart()
      {
          _audioSource.clip = Microphone.Start(null, true, 10, 44100);
          _audioSource.loop = true;
          while (!(Microphone.GetPosition(null) > 0))
          {
              _audioSource.Play();
          }
      }
 
      private void WorkStop()
      {
          Microphone.End(null);
      }
  }
