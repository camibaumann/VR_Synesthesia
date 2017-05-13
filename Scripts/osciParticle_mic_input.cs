using UnityEngine;
using System.Collections;
//adapted from Ray Gong's tutorial code https://www.youtube.com/watch?v=GY7INfwXp2o
// modifications made by Ranjini Narayan and Camille Baumann-Jaeger

public class osciParticle_mic_input : MonoBehaviour {
    
	//mic
	public float duration = 1.0f;
	public bool IsWorking = true;
	private bool _lastValueOfIsWorking;

	public bool realTimeOutput = true;
	private bool _lastValueOfRealtimeOutput;

	AudioSource _audioSource;
	private float _lastVolume = 0;

	//particles
    [Range(0, 16384)]//this returns an error when either one is larger than 2^14, and I dont know why..please tell me asap if you figuered this out
	public int resolution = 16384;
    [Range(0, 16384)]
	public int particleCount = 16384;
    public int particlesCounted; //for debug, see the last line of code

	private float[] samplesX;
	private float[] samplesY;

	public ParticleSystem.Particle[] points;

	void Start () 
	{
		//mic 
		_audioSource = GetComponent<AudioSource>();
		if (IsWorking)
		{
			WorkStart();
		}

		//particles
		samplesX = new float[particleCount];
		samplesY = new float[particleCount];
		//Draw ();
	}
	
	private void Draw(){
		//AudioSource audio = GetComponent<AudioSource> ();

		points = new ParticleSystem.Particle[resolution];

		AudioListener.GetOutputData (samplesX, 0);
		AudioListener.GetOutputData (samplesY, 1);
		//print (Mathf.Max (samplesY));

		float increment = 1f/ ( resolution - 1);

		for (int i = 0; i < resolution; i++) {
			float x = i * increment;
			points[i].position = new Vector3((int)(resolution * (samplesX[i] + 1f) / 2) * increment / 10, (int)(resolution * (samplesY[i] + 1f) / 2) * increment * 20, (int)((resolution * i) / resolution) * increment * 20);//"3D", Z axis is time
            //points [i].position = new Vector3((int)(resolution * (samplesX [i] + 1f) / 2) * increment , (int)(resolution * (samplesY [i] + 1f) / 2) * increment, 0) * 20;//Vectorscope/2D,
		 	//points [i].position = new Vector3((int)((resolution * i) / resolution) * increment, (int)(resolution * (samplesX [i] + 1f) / 2)* increment, 0);//normal oscilloscope, takes only 1 input, x is left, y is right channel
			//points [i].startColor =new Color (1, x, 0);//RGB color, x is how old the particle is, older it is, lower th value
			points [i].startColor =new Color (x * points[i].position.x, x * points[i].position.y, x * points[i].position.z);
			points [i].startSize = .01f; //Particle size
		}
	}

	void Update () 
	{
		//mic
		CheckIfIsWorkingChanged();
		CheckIfRealtimeOutputChanged();

		//float t = Mathf.PingPong(Time.time, duration)/duration;
		
		//if (IsWorking)
		//{
		ParticleSystem particlesys = GetComponent<ParticleSystem> ();
		Draw ();
		particlesys.SetParticles (points, particleCount);//renders the particles
        particlesCounted = particlesys.particleCount; //counting how many particles is on screen, for debug purpose.
		//}
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
		if (_lastValueOfRealtimeOutput != realTimeOutput)
		{
			DisableSound(realTimeOutput);
		}
		_lastValueOfRealtimeOutput = realTimeOutput;
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
		while (!(Microphone.GetPosition(null)>0))
		{
			_audioSource.Play();
		}
	}

	private void WorkStop()
	{
		Microphone.End(null);
	}
}
