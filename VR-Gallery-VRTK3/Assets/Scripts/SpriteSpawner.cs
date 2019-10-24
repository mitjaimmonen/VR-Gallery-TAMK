using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawner : Spawner {

	[SerializeField] bool useCustomAmount = false;
	[SerializeField] Sprite[] sprites;

    public override void Start()
    {
        objects = new Transform[sprites.Length];
        SpawnObjects();
    }

    public override void ExtendComponents(GameObject o, int i){
		Sprite sprite = sprites [i];
		o.GetComponent<SpriteRenderer> ().sprite = sprite;
	}
}
