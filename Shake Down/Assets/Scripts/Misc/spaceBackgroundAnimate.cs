using UnityEngine;
using System.Collections;

public class spaceBackgroundAnimate : MonoBehaviour 
{
	private Material myMaterial = null;
	private Vector2 currentAnimationSpeed = Vector3.zero;
	[SerializeField] private Vector2 targetAnimationSpeed = Vector3.zero;
	public Vector2 _targetAnimationSpeed {get{return targetAnimationSpeed;} set{targetAnimationSpeed = value;}}

	private void Start()
	{
		myMaterial = GetComponent<Renderer> ().material;
	}

	private void Update()
	{
		currentAnimationSpeed = Vector2.Lerp(currentAnimationSpeed, targetAnimationSpeed, Time.deltaTime);
		myMaterial.mainTextureOffset -= targetAnimationSpeed;
	}
}
