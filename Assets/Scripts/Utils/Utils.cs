using UnityEngine;

public static class Utils
{
	// https://discussions.unity.com/t/143857
	public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
	{
		var lossy = transform.lossyScale;

		transform.localScale = Vector3.one;
		transform.localScale = new Vector3 (
			globalScale.x / lossy.x,
			globalScale.y / lossy.y,
			globalScale.z / lossy.z
		);
	}
}