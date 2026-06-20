using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private int speedMult;

	private Rigidbody2D rb;
	private Vector2 direction;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {		
       rb.linearVelocity = direction * speedMult;
    }

	public void OnMove(InputValue inputVal)
	{
		Debug.Log("Sup bro");
		direction = inputVal.Get<Vector2>();
	}
}
