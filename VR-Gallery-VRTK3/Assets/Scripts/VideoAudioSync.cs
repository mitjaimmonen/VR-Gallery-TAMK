using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoAudioSync : MonoBehaviour {

	AudioSource ads;
	VideoPlayer vdp;
	void Awake () {
		ads = GetComponent<AudioSource>();
		vdp = GetComponent<VideoPlayer>();
		vdp.prepareCompleted += PlayVideo;
	}
	void PlayVideo(VideoPlayer source){
		ads.Play();
		vdp.Play();
	}
}
