using UnityEngine;

public static class Utils
{
	// https://discussions.unity.com/t/143857
	public static void SetGlobalScale(this Transform transform, Vector2 globalScale)
	{
		var lossy = transform.lossyScale;

		transform.localScale = Vector2.one;
		transform.localScale = new Vector2 (
			globalScale.x / lossy.x,
			globalScale.y / lossy.y
		);
	}
}