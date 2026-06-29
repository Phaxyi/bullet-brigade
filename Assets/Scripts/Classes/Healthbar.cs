/*
	Visualises health of Entity.
*/

using UnityEngine;

public class Healthbar : MonoBehaviour
{
	public bool Show = true;

	private SpriteRenderer rd;
	private Transform parent;
	private Entity entity;

	void Awake()
	{
		rd = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
		parent = transform.parent;
		entity = parent.GetComponent<Entity>();

		Utils.SetGlobalScale(transform, Vector3.one * 1.75f);
	}

    void Update()
    {
		rd.enabled = Show;
		if (!Show) return;

		// anchor healthbar above parent
        transform.position = new Vector2(
			parent.position.x,
			parent.position.y + parent.lossyScale.y/2 + 0.4f
		);
		rd.color = new Color(1, 1, 1, entity.Health / entity.MaxHealth);
    }
}
