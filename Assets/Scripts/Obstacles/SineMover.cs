/*
	Moves between two points in a sine motion.
	TODO: spawn line on runtime for indication
*/

using UnityEngine;

public class SineMover : MonoBehaviour
{
    [SerializeField] private float _moveDistance;
	[SerializeField] private float _lineDuration; // time taken from A -> B
	private Transform _wall;

	private void Awake()
	{
		_wall = transform.Find("Wall");
	}

	private void Update()
	{
		float interval = Mathf.Sin(Time.time * Mathf.PI / _lineDuration);
		_wall.transform.localPosition = new Vector2(
			0, _moveDistance/2 + _moveDistance/2 * interval);
	}
}
