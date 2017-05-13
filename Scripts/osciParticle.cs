using UnityEngine;
using System.Collections;
//adapted from Ray Gong's tutorial code https://www.youtube.com/watch?v=GY7INfwXp2o
// modifications made by Ranjini Narayan and Camille Baumann-Jaeger

public class osciParticle : MonoBehaviour {
    [Range(0, 16384)]//this returns an error when either one is larger than 2^14, and I dont know why..please tell me asap if you figuered this out
	public int resolution = 2048;
    [Range(0, 16384)]
	public int particleCount = 2048;
    public int particlesCounted; //for debug, see the last line of code

	private float[] samplesX;
	private float[] samplesY;

	private ParticleSystem.Particle[] points;

	void Start () {
		samplesX = new float[particleCount];
		samplesY = new float[particleCount];
		Draw ();
	}
	

	private void Draw(){
		AudioSource audio = GetComponent<AudioSource> ();

		points = new ParticleSystem.Particle[resolution];

		audio.GetOutputData (samplesX, 0);
		audio.GetOutputData (samplesY, 1);

		float increment = 1f/ ( resolution - 1);

		for (int i = 0; i < resolution; i++) {
			float x = i * increment;
			points[i].position = new Vector3((int)(resolution * (samplesX[i] + 1f) / 2) * increment, (int)(resolution * (samplesY[i] + 1f) / 2) * increment, (int)((resolution * i) / resolution) * increment) * 10;//"3D", Z axis is time
			//points[i].position = new Vector3((int)(resolution * (samplesX[i] + 1f) / 2) * increment, (int)(resolution * (samplesY[i] + 1f) / 2) * increment, (int)((resolution * i) / resolution) * increment) * 5;
            //points [i].position = new Vector3((int)(resolution * (samplesX [i] + 1f) / 2) * increment , (int)(resolution * (samplesY [i] + 1f) / 2) * increment, 0);//Vectorscope/2D,
		 	//points [i].position = new Vector3((int)((resolution * i) / resolution) * increment, (int)(resolution * (samplesX [i] + 1f) / 2)* increment, 0);//normal oscilloscope, takes only 1 input, x is left, y is right channel
			//points [i].startColor =new Color (1, x, 0);//RGB color, x is how old the particle is, older it is, lower th value
			points [i].startColor =new Color (x * points[i].position.x, x * points[i].position.y, x * points[i].position.z);
			points [i].startSize = .02f; //Particle size
		}

	
	}

	void Update () {
		
		ParticleSystem particlesys = GetComponent<ParticleSystem> ();
		Draw ();
		particlesys.SetParticles (points, particleCount);//renders the particles
        particlesCounted = particlesys.particleCount; //counting how many particles is on screen, for debug purpose.
	}
}
