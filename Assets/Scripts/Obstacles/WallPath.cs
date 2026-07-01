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
		[SerializeField] private float _lineDuration; 	// time taken for total path
		[SerializeField] private bool _useSineEasing; 	// easing between each point
		[SerializeField] private bool _loop; 			// loop for >=3 point or simply reverse?

		private readonly List<Vector2> points = new();
		private readonly Dictionary<int, float> pointTimeMap = new();
		private readonly Dictionary<Transform, float> wallPhaseMap = new();

		private void Start()
		{
			// get wallPhaseMap
			foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
			{
				wallPhaseMap.Add(obstacle.transform, obstacle.phase);
			}

			// get points
			Transform posHolder = transform.Find("Positions");
			foreach (Transform posObj in posHolder)
			{
				points.Add(posObj.position);
			}
			
			points.Add(posHolder.Find("1").position);
			Destroy(posHolder.gameObject);

			// generate dict `pointTimeMap` of positions to lerp with based on given interval
			// based on distances between each other
			if (_loop && points.Count > 2)
			{
				Dictionary<int, float> intermediate = new();
				Vector2 lastPoint = points[0];
				float totalDist = 0;
				int i = 0;

				foreach (Vector2 point in points)
				{
					totalDist += (point - lastPoint).magnitude;
					lastPoint = point;
					
					intermediate.Add(i, totalDist);
					i += 1;
				}

				foreach (KeyValuePair<int, float> pair in intermediate)
				{
					Debug.Log(pair.Key);
					Debug.Log(pair.Value / totalDist);
					pointTimeMap[pair.Key] = pair.Value / totalDist;
				}
			}
			else
			{
				// TODO:
			}
		}

		private void Update()
		{
			// get two points
		}
	}
}