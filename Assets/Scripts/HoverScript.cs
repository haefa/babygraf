using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : Singleton<HoverScript> {

	private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeSpriteRenderer;

    public bool IsVisible { get; private set; }

	// Use this for initialization
	void Start () {
		this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		FollowMouse();
	}
	private void FollowMouse()
	{
		if(spriteRenderer.enabled)
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		}
	}
	public void Activate(Sprite sprite)
	{
		this.spriteRenderer.sprite = sprite;
		spriteRenderer.enabled = true;
        rangeSpriteRenderer.enabled = true;
        IsVisible = true;
	}
	public void Deactive()
	{
		spriteRenderer.enabled = false;
        rangeSpriteRenderer.enabled = false;
        GameManagerScript.Instance.ClickedBtn = null;
        IsVisible = false;
	}


}
