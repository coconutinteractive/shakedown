using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum ElementState
{
	Normal,
	Angry,
	Outraged,
	Happy,
	Scared,
	Sad,
	Worried
}

public class DialogueInterface : MonoBehaviour 
{
	private static bool _isActive = false;
	public static bool isActive{get{return _isActive;}}

	[System.Serializable]
	public class UIElementsGroup
	{
		public List<Image> groupImagesList = new List<Image> ();
		public List<Text> groupTextList = new List<Text> ();
	}

	// Overral Lists
	[SerializeField] private List<Image> allImagesRefList = new List<Image>();
	[SerializeField] private List<Text> allTextsRefList = new List<Text>();
	[SerializeField] private List<Image> allButtonsRefList = new List<Image>();

	// Groups
	[SerializeField] private UIElementsGroup leftSide, rightSide;
	//[SerializeField] private UIElementsGroup 


	// Specific Items
	[SerializeField] private Image leftPortraitImage = null, rightPortraitImage = null;
	[SerializeField] private Image leftArrowImage = null, rightArrowImage = null;



	[SerializeField] private GameObject threeChoicesBox = null, sixChoicesBox = null;

	private bool isLeftSideTaken = false, isRightSideTaken = false;
	
	private void Start()
	{
		foreach (Image curImage in allImagesRefList) 
		{
			Color newColor = curImage.color;
			newColor.a = 0.0f;
			curImage.color = newColor;
		}

		foreach (Text curText in allTextsRefList) 
		{
			Color newColor = curText.color;
			newColor.a = 0.0f;
			curText.color = newColor;
		}

		foreach (Image curImage in allButtonsRefList) 
		{
			Color newColor = curImage.color;
			newColor.a = 0.0f;
			curImage.color = newColor;
		}

		threeChoicesBox.SetActive (false);
		sixChoicesBox.SetActive (false);

		_isActive = false;
		gameObject.SetActive (false);
	}

	public static void Activate()
	{
		_isActive = true;
	}

	public static void DisplayPlayer()
	{

	}

	public static void DisplayLeftCharacter(ElementState _characterState, float _fadeTime, string _name, string _lastName, bool _isTalking)
	{

	}

	public static void DisplayRightCharacter(ElementState _characterState, float _fadeTime, string _name, string _lastName, bool _isTalking)
	{

	}

}
