using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
//adapted from benjaminoutram's tutorial on answers.unity3d.com
//modifications made by Camille Baumann-Jaeger

[RequireComponent(typeof(AudioSource))]
public class light_change_update : MonoBehaviour
{	
	//light
	public float duration = 2.0F;
	public Light lt;
	//color 
	public int i = 0;
	float[] spectrum; 
	public float max = 0.0f;
	public float red= 0.0f;
	public float green = 0.0f;
	public float blue = 0.0f;
	public Color last_color = Color.blue;
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
			lt.color = last_color;
		}
	}
	void Update() 
	{	
		spectrum = new float[64];
		max = 0.0f;
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
		_audioSource.GetSpectrumData (spectrum, 0, FFTWindow.Rectangular); 
		for(var i = 0; i < 63; i++) {
			if (spectrum [i] > max)
				max = (spectrum[i] * 10000000);
		}
		float t = Mathf.PingPong(Time.time, duration) / duration;
		max = max % 255;
		red = Mathf.Abs(0.3f - max / 255);
		green = Mathf.Abs(1.0f - max / 255);
		blue = max / 255; 
		if (IsWorking & i > 100)
		{
			lt.color = Color.Lerp((new Color(red, green, blue)), last_color, t);
			last_color = new Color (red,green,blue);
			i = 0;
		}
		i++;

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