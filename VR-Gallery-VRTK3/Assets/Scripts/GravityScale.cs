using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityScale : MonoBehaviour {

    [Range(-2f,2f), Tooltip("Change gravity strength. 1 = Default gravity.")]
    [SerializeField]private float gravityScale = 1f;
    private Rigidbody rb;

	bool gravityScaleUpdating = false;
	float tempGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravityScale);
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
