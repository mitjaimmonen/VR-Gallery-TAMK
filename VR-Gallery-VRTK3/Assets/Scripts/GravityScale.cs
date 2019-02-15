using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScale : MonoBehaviour {


	[SerializeField]private bool affectThis = true;
	[SerializeField]private bool affectChildren = true;

    [Range(-2f,2f), Tooltip("Change gravity strength. 1 = Default gravity.")]
    [SerializeField]private float gravityScale = 1f;
    private Rigidbody rb;
	private List<Rigidbody> rbChildren = new List<Rigidbody>();


	bool checkAffectChildren;
	bool checkAffectThis;
	bool gravityScaleUpdating = false;
	float tempGravity;

    private void Start()
    {
		Init();
    }

	private void Init()
	{
		checkAffectChildren = affectChildren;
		checkAffectThis = affectThis;
		
		if (rbChildren != null && rbChildren.Count > 0)
		{
			for(int i = 0; i < rbChildren.Count; i++)
			{
				if (!affectThis && rbChildren[i].gameObject == this.gameObject)
					continue;

				rbChildren[i].useGravity = true;
			}
			rbChildren.Clear();
		}

		if (affectChildren)
		{
			foreach (var temp in GetComponentsInChildren<Rigidbody>(true))
			{
				if (!affectThis && temp.gameObject == this.gameObject)
					continue;

				rbChildren.Add(temp);
				temp.useGravity = false;
			}
		}
		else if (affectThis)
		{
			rb = GetComponent<Rigidbody>();
			rb.useGravity = false;
		}
	}
    private void FixedUpdate()
    {
		if (checkAffectChildren != affectChildren || checkAffectThis != affectThis) 
		{
			//Has to initialize again if bool changes
			Init();
		}
		else
		{
			if (affectChildren)
			{
				for (int i = 0; i < rbChildren.Count; i++)
				{
					if (!rbChildren[i].isKinematic && rbChildren[i].gameObject.activeSelf)
						rbChildren[i].AddForce(Physics.gravity * rbChildren[i].mass * gravityScale);
				}
			}
			else if (affectThis)
			{
				rb.AddForce(Physics.gravity * rb.mass * gravityScale);
			}
		}
    }

	public void SetGravityOverTime(float gravity, float lerpTime)
	{
		tempGravity = gravity;
		if (!gravityScaleUpdating)
			StartCoroutine(UpdateGravityScale(lerpTime));
	}

	private IEnumerator UpdateGravityScale(float lerpTime)
	{
		gravityScaleUpdating = true;
		float startGravity = gravityScale;
		float endGravity = tempGravity;
		float t = 0;
		while (gravityScale != endGravity || t < lerpTime)
		{
			gravityScale = Mathf.Lerp(startGravity, endGravity, Easing.Ease(t/lerpTime, Curve.SmoothStep));
			if (endGravity != tempGravity)
			{
				//Changed while coroutine still running. Update to new values without overlapping coroutines.
				t = 0;
				startGravity = gravityScale;
				endGravity = tempGravity;
			}
			t += Time.deltaTime;
			yield return null;
		}
		
		gravityScaleUpdating = false;
		
	}
}
