using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField] public Transform prefab;
	[SerializeField] public int shapesAmount = 0;
	[SerializeField] public bool useParentScale = false;
	[SerializeField] public float minScale = 0.3f;
	[SerializeField] public float maxScale = 2f;
	[SerializeField] public bool useParentPosition = true;
	[SerializeField] public bool assignRandomRotation = false;
	[SerializeField] public float minRadius = 0f;
	[SerializeField] public float maxRadius = 5f;
	[SerializeField] public bool spawnOnCircle = false;
	[SerializeField] public float scaleMultiplier = 1.0f;

	public Transform[] objects;

	// Use this for initialization
	public virtual void Start () {
        objects = new Transform[shapesAmount];
		SpawnObjects();
	}

	public void SpawnObjects(){
        Debug.Log(objects.Length);
		for (int i = 0; i < objects.Length; i++)
		{
			Transform t = Instantiate(prefab);
			t.gameObject.SetActive(false);
			SetScale(t, i);
			SetPosition(t, i);
			ExtendPosition(t, i);
			SetRotation(t, i);
			ExtendRotation(t, i);
			//SetComponents();
			ExtendComponents(t.gameObject, i);
			t.gameObject.SetActive(true);
			//Active();
			ExtendActive(t.gameObject, i);
		}
	}

	void SetScale(Transform t, int i){
		Vector3 scale = useParentScale ? transform.localScale : Vector3.one;
		scale *= Random.Range(minScale, maxScale);
		t.localScale = scale;
	}

	void SetPosition(Transform t, int i){
		Vector3 pos = useParentPosition ? transform.position : Vector3.zero;
		Vector3 modifier;
		if (spawnOnCircle)
		{
			Vector2 circle2 = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
			Vector3 circle3 = new Vector3(circle2.x, 0, circle2.y);
			modifier = circle3;
		} else {
			Vector3 sphere = Random.onUnitSphere * Random.Range(minRadius, maxRadius);
			modifier = sphere;
		}
		pos += modifier;
		t.position = pos;
	}

	void SetRotation(Transform t, int i){
		t.rotation = assignRandomRotation ? Random.rotation : Quaternion.identity;
	}

	/*void Active(){

	}*/

	public virtual Transform[] ExtendStart(Transform[] tempObjects){
        return tempObjects;
    }
	public virtual void ExtendPosition(Transform t, int i){}
	public virtual void ExtendRotation(Transform t, int i){}
	public virtual void ExtendComponents(GameObject o, int i){}
	public virtual void ExtendActive(GameObject o, int i){}
}
