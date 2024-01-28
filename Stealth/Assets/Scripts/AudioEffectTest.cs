using UnityEngine;
using System.Collections;
using Exocortex.DSP;

public class AudioEffectTest : MonoBehaviour {
	
	public AudioClip clip;
	
	void OnGUI(){
		if(GUI.Button(new Rect(0, 0, 100, 100), "Play!")){
			float[] clipSamples = new float[clip.samples];
			clip.GetData(clipSamples, 0); //Get all sample data
			for(int i = 0; i < clipSamples.Length / 512; i++){
				//Apply FFT to chunks
				int offset = i*512;
				float[] fftInput = new float[1024];
				for(int j = 0; j < 512; j++){
					fftInput[j*2] = clipSamples[j+offset];					
				}
				Fourier.FFT(fftInput, 512, FourierDirection.Forward);
				//"Reverse" the spectrum
				for(int j = 0; j < 256; j+=2){
					float tempR = fftInput[j];
					float tempI = fftInput[j+1];
					fftInput[j] = fftInput[510-j];
					fftInput[j+1] = fftInput[511-j];
					fftInput[510-j] = tempR;
					fftInput[511-j] = tempI;
				}
				for(int j = 512; j < 768; j+=2){
					float tempR = fftInput[j];
					float tempI = fftInput[j+1];
					fftInput[j] = fftInput[1022-j+512];
					fftInput[j+1] = fftInput[1023-j+512];
					fftInput[1022-j+512] = tempR;
					fftInput[1023-j+512] = tempI;
				}
				//Inverse FFT
				Fourier.FFT(fftInput, 512, FourierDirection.Backward);
				//Convert to real
				float[] realData = new float[512];
				for(int j = 0; j < 512; j++){
					realData[j] = fftInput[j*2];
				}
				//clip.SetData(realData, offset);
			}
			//audio.PlayOneShot(clip);
		}
	}
}
