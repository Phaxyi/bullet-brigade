/*
	Moves between >= 2 points, optionally easing between each point.
	Parent gameObject should not move, only its positions & Walls

	TODO: spawn line on runtime for indication
		https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Gizmos.DrawLine.html
*/

using System.Collections.Generic;
using UnityEngine;

namespace BulletBrigade {
	public class WallPath : MonoBehaviour
	{
		[SerializeField] private float _pathDuration = 1; 	// time taken for total path (looping), or from A -> B (not looping)
		[SerializeField] private bool _useSineEasing; 		// easing between each point
		[SerializeField] private bool _loop; 				// loop for >=3 point or simply reverse?

		private float _startTime; 
		private readonly List<Vector2> _points = new();
		private readonly List<float> _intervals = new();
		private readonly Dictionary<Transform, float> _wallPhaseMap = new();

		private void Start()
		{
			_startTime = Time.time;

			// get wallPhaseMap
			foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
			{
				_wallPhaseMap.Add(obstacle.transform, obstacle.phase);
			}

			// get points
			Transform posHolder = transform.Find("Positions");
			if (_loop)
			{
				foreach (Transform posObj in posHolder)
					_points.Add(posObj.position);

				_points.Add(posHolder.Find("1").position);
			}
			else
			{
				foreach (Transform posObj in posHolder)
					_points.Add(posObj.position);
				
				for (int i = posHolder.childCount - 2; i >= 0; i--)
				{
					Transform posObj = posHolder.GetChild(i);
					_points.Add(posObj.position);
				}
			}
			
			Destroy(posHolder.gameObject);

			// generate dict `_pointIntervalMap` of positions to lerp with based on given interval
			// based on distances between each other
			List<float> intermediate = new();
			Vector2 lastPoint = _points[0];
			float totalDist = 0;

			foreach (Vector2 point in _points)
			{
				totalDist += (point - lastPoint).magnitude;
				lastPoint = point;
				
				intermediate.Add(totalDist);
			}

			foreach (float interval in intermediate)
			{
				_intervals.Add(interval / totalDist);
			}
		}

		private void Update()
		{
			// get two points sandwiching current interval
			float interval = (Time.time - _startTime) % _pathDuration / _pathDuration;
			int p1Index = 0;
			
			for (int i = _intervals.Count - 1; i >= 0; i--)
			{
				if (interval > _intervals[i])
				{
					p1Index = i;
					break;
				}
			}

			Vector2 point1 = _points[p1Index];
			Vector2 point2 = _points[p1Index + 1];
			float t1 = _intervals[p1Index];
			float t2 = _intervals[p1Index + 1];
			float t = (interval - t1) / (t2 - t1);

			// https://easings.net/#easeInOutSine
			if (_useSineEasing)
				t = -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

			// move wall(s)
			foreach (var pair in _wallPhaseMap)
			{
				float phase = pair.Value;
				Vector2 lerpPos = Vector2.Lerp(point1, point2, (t + phase) % 1);

				pair.Key.position = lerpPos;
			}
		}
	}
}