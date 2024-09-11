using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundBehavior : MonoBehaviour {
	
	public int musicIndex = 0;
	public bool pause = false;
	public Camera cam;
	List<AudioClip> musics;
	Object[] musicsArray;
	
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	void Start()
	{ 	
		musics = new List<AudioClip>();
//		foreach(AudioClip music in musicsArray)
//			musics.Add(music);
		
		musics.Add(Resources.Load("Musicas/Music_Alan") as AudioClip);
		musics.Add(Resources.Load("Musicas/Musica_Alan_2") as AudioClip);
		musics.Add(Resources.Load("Musicas/music1") as AudioClip);
		musics.Add(Resources.Load("Musicas/music2") as AudioClip);
		musics.Add(Resources.Load("Musicas/music3") as AudioClip);
		
		this.GetComponent<AudioSource>().clip = musics[0];
		this.GetComponent<AudioSource>().Play();

		//cam = GameObject.FindObjectOfType<Camera>();			
		this.transform.parent = cam.transform;
		this.transform.localPosition = Vector3.zero;
	}
	
	void Update () {
		if (Input.GetKeyDown ("right"))
			next ();
		if (Input.GetKeyDown ("left"))
			back ();
		
		if (Input.GetKeyDown ("p"))
			pause = !pause;
		if(pause && this.GetComponent<AudioSource>().isPlaying)
			this.GetComponent<AudioSource>().Pause();
		if(!pause && !this.GetComponent<AudioSource>().isPlaying)
			this.GetComponent<AudioSource>().Play();

	}
	
	void changeMusic(int index)
	{
		this.GetComponent<AudioSource>().clip = musics[index];
		this.GetComponent<AudioSource>().Play();
	}
	
	public void next ()
	{
		musicIndex ++;
		if(musicIndex >= musics.Count)
			musicIndex = 0;
		this.GetComponent<AudioSource>().clip = musics[musicIndex];
		this.GetComponent<AudioSource>().Play();
	}
	
	public void back()
	{
		musicIndex --;
		if(musicIndex < 0)
			musicIndex = musics.Count - 1;
		this.GetComponent<AudioSource>().clip = musics[musicIndex];
		this.GetComponent<AudioSource>().Play();
	}
}
