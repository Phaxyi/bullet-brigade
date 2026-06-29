/*
	Visualises health of Entity.
*/

using UnityEngine;

public class Healthbar : MonoBehaviour
{
	public bool Show = true;

	private SpriteRenderer rd;
	private Transform holder;
	private Entity entity;

    void Start()
    {
        rd = GetComponent<SpriteRenderer>();
		holder = transform.parent;
		entity = holder.GetComponent<Entity>();
		Debug.Log(entity);
	}

    void Update()
    {
		rd.enabled = Show;
		if (!Show) return;

        transform.position = new Vector2(holder.position.x, holder.position.y + 1);
		rd.color = new Color(1, 1, 1, entity.Health / entity.MaxHealth);
    }
}
