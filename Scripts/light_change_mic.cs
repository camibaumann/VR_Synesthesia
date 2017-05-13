using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
//adapted from benjaminoutram's tutorial on answers.unity3d.com
//modifications made by Camille Baumann-Jaeger

[RequireComponent(typeof(AudioSource))]
public class light_change_mic : MonoBehaviour
{	
	//light
	public float duration = 1.0F;
    public Color color0 = Color.red;
    public Color color1 = Color.blue;
    public Light lt;

    //mic
    public bool IsWorking = true;
  	private bool _lastValueOfIsWorking;
 
  	public bool RaltimeOutput = true;
  	private bool _lastValueOfRaltimeOutput;
 
  	AudioSource _audioSource;
  	private float _lastVolume = 0;

  	//timer
  	float timeLeft = 300.0f;

    void Start()
    {
        lt = GetComponent<Light>();
        _audioSource = GetComponent<AudioSource>();
        if (IsWorking)
    	{
      		WorkStart();
    	}
    }
    void Update() 
    {	
    	//timer
    	timeLeft -= Time.deltaTime;
    	if (timeLeft == 200)
    	{
    		WorkStop();
    	}
    	if (timeLeft == 100)
    	{
    		WorkStart();
    	}

    	//mic
    	CheckIfIsWorkingChanged();
    	CheckIfRealtimeOutputChanged();

        float t = Mathf.PingPong(Time.time, duration) / duration;
        if (IsWorking)
        {
        	lt.color = Color.Lerp(color0, color1, t);
        }

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