/*
	Moves between >= 2 points, optionally easing between each point.
	Parent gameObject should not move, only its positions & Walls
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletBrigade {
	public class WallPath : MonoBehaviour
	{
		[SerializeField] private float _pathDuration = 1; 	// time taken for total path to revist same point
		[SerializeField] private bool _useSineEasing; 		// easing between each point
		[SerializeField] private bool _loop; 				// loop for >2 points or simply reverse?

		private float _startTime; 
		private readonly List<Vector2> _points = new();
		private readonly List<float> _intervals = new();
		private readonly Dictionary<Transform, float> _wallPhaseMap = new();

		private void Awake()
		{
			_startTime = Time.time;

			// get wallPhaseMap
			foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
			{
				_wallPhaseMap.Add(obstacle.transform, obstacle.phase);
			}

			// get points
			Transform posHolder = transform.Find("Positions");
			_loop = posHolder.childCount < 3 ? false : _loop;

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

			// render lines between points
			LineRenderer line = GetComponent<LineRenderer>();
			List<Vector3> posArray = default;

			if (_loop)
			{
				posArray = ListV2toV3(_points.ToList());
			}
			else
			{
				HashSet<Vector2> deduplicated = new(_points);
				posArray = ListV2toV3(deduplicated.ToList());
			}

			line.positionCount = posArray.Count;
			line.SetPositions(posArray.ToArray());
		}

		private void FixedUpdate()
		{
			float timePassed = Time.time - _startTime;

			foreach (var pair in _wallPhaseMap)
			{
				float phase = pair.Value;
				float interval = (timePassed + phase) % _pathDuration / _pathDuration;

				pair.Key.position = GetWallPos(pair.Key, interval);
			}
			
		}

		private Vector2 GetWallPos(Transform wall, float interval)
		{
			// get two points sandwiching current interval [0 -> 1]
			int p1Index = default;
			for (int i = _intervals.Count - 1; i >= 0; i--)
			{
				if (interval < _intervals[i]) continue;
				p1Index = i;
				break;
			}

			// get lerped position
			Vector2 point1 = _points[p1Index];
			Vector2 point2 = _points[p1Index + 1];
			float t1 = _intervals[p1Index];
			float t2 = _intervals[p1Index + 1];
			float t = (interval - t1) / (t2 - t1);
			
			if (_useSineEasing) t = EaseIOSine(t);

			return Vector2.Lerp(point1, point2, t);
		}

		private List<Vector3> ListV2toV3(List<Vector2> original)
		{
			List<Vector3> convert = new(original.Capacity);
			foreach (Vector2 v2 in original)
			{
				convert.Add(v2);
			}

			return convert;
		}

		// https://easings.net/#easeInOutSine
		private float EaseIOSine(float t)
			=> -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
	}
}