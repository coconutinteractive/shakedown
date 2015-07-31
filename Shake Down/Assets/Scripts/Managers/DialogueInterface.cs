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
	private static int MAX_CHAR_LIMIT_PER_BODY = 120;

	#region Singleton
	public static DialogueInterface Instance 
	{
		get 
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(DialogueInterface)) as DialogueInterface;
			
			return instance;
		}
		set 
		{
			instance = value;
		}
	}
	private static DialogueInterface instance;
	#endregion

	private static bool _isActive = false;
	public static bool isActive{get{return _isActive;}}

	[System.Serializable]
	public class UIElementsGroup
	{
		public List<Image> groupImagesList = new List<Image> ();
		public List<Text> groupTextList = new List<Text> ();
	}

	private Dictionary<string, UnityEngine.Events.UnityAction> btnActionsDict = new Dictionary<string, UnityEngine.Events.UnityAction>();

	// Groups
	[SerializeField] private UIElementsGroup allBackground = null;
	[SerializeField] private UIElementsGroup leftSide = null, rightSide = null;
	[SerializeField] private UIElementsGroup oneButton = null, twoButtons = null, threeButtons = null, sixButtons = null;

	// Specific Items
	[SerializeField] private Image leftPortraitImage = null, rightPortraitImage = null;
	[SerializeField] private Image leftArrowImage = null, rightArrowImage = null;
	[SerializeField] private GameObject choicesBackground = null;
	[SerializeField] private Text dialogueText = null;
	[SerializeField] private Image ellipseGraphic = null;

	[SerializeField] private GameObject sixChoicesBox = null, threeChoicesBox = null, twoChoicesBox = null, oneChoiceBox = null;

	// Choices
	private int numberOfChoices = 0;
	private List<string> choicesText = new List<string>();
	private List<List<UnityEngine.Events.UnityAction>> currentBtnActions = new List<List<UnityEngine.Events.UnityAction>>();

	// Internal
	private bool isSomeoneTalking = false;

	private void Awake()
	{
		for (int i = 0; i < 6; ++i) 
			currentBtnActions.Add(new List<UnityEngine.Events.UnityAction>());

		PopulateButtonActionDict ();

		rightArrowImage.canvasRenderer.SetAlpha (0.0f);
		leftArrowImage.canvasRenderer.SetAlpha (0.0f);
		dialogueText.canvasRenderer.SetAlpha (0.0f);
		ellipseGraphic.canvasRenderer.SetAlpha (0.0f);

		foreach (Image curImage in allBackground.groupImagesList)
			curImage.canvasRenderer.SetAlpha(0.0f);

		foreach (Image curImage in leftSide.groupImagesList) 
			curImage.canvasRenderer.SetAlpha(0.0f);
		foreach (Image curImage in rightSide.groupImagesList) 
			curImage.canvasRenderer.SetAlpha(0.0f);	

		foreach (Text curText in leftSide.groupTextList)
			curText.canvasRenderer.SetAlpha(0.0f);	
		foreach (Text curText in rightSide.groupTextList)
			curText.canvasRenderer.SetAlpha(0.0f);	

		threeChoicesBox.SetActive (false);
		sixChoicesBox.SetActive (false);
		choicesBackground.SetActive (false);

		_isActive = false;
	}

#region UI Handling
	public void Activate()
	{
		_isActive = true;
		CrossFadeGroup (allBackground, 0.92f, 0.92f, 1.0f);
	}

	public void Deactivate()
	{
		_isActive = false;
		CrossFadeGroup (allBackground, 0.0f, 0.0f, 1.0f);
	}

	public void HideAll()
	{
		HideChoices ();
		dialogueText.CrossFadeAlpha (0.0f, 1.0f, true);
		HideLeftCharacter ();
		HideRightCharacter ();
	}

	public void DisplayLeftCharacter(Sprite _portrait, ElementState _characterState, float _fadeTime, bool _isTalking)
	{
		leftPortraitImage.sprite = _portrait;
		CrossFadeGroup (leftSide, 0.92f, 1.0f, _fadeTime);

		if (_isTalking)
			leftArrowImage.CrossFadeAlpha (1.0f, _fadeTime * 0.5f, true);

	}

	public void DisplayRightCharacter(Sprite _portrait, ElementState _characterState, float _fadeTime, bool _isTalking)
	{
		rightPortraitImage.sprite = _portrait;
		CrossFadeGroup (rightSide, 0.92f, 0.92f, _fadeTime);

		if (_isTalking)
			rightArrowImage.CrossFadeAlpha (1.0f, _fadeTime * 0.5f, true);
	}

	public void HideLeftCharacter()
	{
		CrossFadeGroup (leftSide, 0.0f, 0.0f, 1.0f);
		leftArrowImage.CrossFadeAlpha (0.0f, 1.0f, true);
	}

	public void HideRightCharacter()
	{
		CrossFadeGroup (rightSide, 0.0f, 0.0f, 1.0f);
		rightArrowImage.CrossFadeAlpha (0.0f, 1.0f, true);
	}

	public void UpdateInfoValues(bool _isLeftSide, string _name, string _lastName, string _value1, string _value2, string _value3)
	{
		UIElementsGroup curElement = _isLeftSide ? leftSide : rightSide;

		curElement.groupTextList [0].text = _name;
		curElement.groupTextList [1].text = _lastName;
		curElement.groupTextList [2].text = _value1;
		curElement.groupTextList [3].text = _value2;
		curElement.groupTextList [4].text = _value3;
	}

	public void UpdateInfoValues(bool _isLeftSide, string[] _values)
	{
		UIElementsGroup curElement = _isLeftSide ? leftSide : rightSide;

		for (int i = 0; i < _values.Length; ++i) 
			curElement.groupTextList[i].text = _values[i];
	}

	public void SetChoices(string[] _choices)
	{
		choicesText.Clear ();
		numberOfChoices = _choices.Length;
		foreach (string curChoice in _choices) 
			choicesText.Add (curChoice);
	}

	private void DisplayChoices()
	{
		UpdateInfoValues(false, new string[5]{"BEEP", "BOOP", "150", "350", "None"});
		
		choicesBackground.SetActive (true);
		choicesBackground.GetComponent<Image> ().CrossFadeAlpha (0.92f, 1.0f, true);
		GameObject selectedBox = null;
		
		switch(numberOfChoices)
		{
		case 1:
		{
			oneChoiceBox.SetActive(true);
			CrossFadeGroup(oneButton, 0.92f, 1.0f, 1.0f);
			selectedBox = oneChoiceBox;
			break;
		}
		case 2:
		{
			twoChoicesBox.SetActive(true);
			CrossFadeGroup(twoButtons, 0.92f, 1.0f, 1.0f);
			selectedBox = twoChoicesBox;
			break;
		}
		case 3:
		{
			threeChoicesBox.SetActive(true);
			CrossFadeGroup(threeButtons, 0.92f, 1.0f, 1.0f);
			selectedBox = threeChoicesBox;
			break;
		}
		case 4:
		case 5:
		case 6:
		{
			sixChoicesBox.SetActive(true);
			CrossFadeGroup(sixButtons, 0.92f, 1.0f, 1.0f);
			selectedBox = sixChoicesBox;
			break;
		}
		}
		
		for (int i = 0; i < numberOfChoices; ++i)
		{
			GameObject buttonObj = selectedBox.transform.GetChild(i).gameObject;
			buttonObj.gameObject.SetActive(true);
			
			buttonObj.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
			
			for (int j = 0; j < currentBtnActions[i].Count; ++j) 
				buttonObj.gameObject.GetComponent<Button>().onClick.AddListener(currentBtnActions[i][j]);
			
			buttonObj.transform.GetChild(0).GetComponent<Text>().text = choicesText[i];
		}
	}
	
	private void HideChoices()
	{
		CrossFadeGroup (oneButton, 0.0f, 0.0f, 1.0f);
		CrossFadeGroup (twoButtons, 0.0f, 0.0f, 1.0f);
		CrossFadeGroup (threeButtons, 0.0f, 0.0f, 1.0f);
		CrossFadeGroup (sixButtons, 0.0f, 0.0f, 1.0f);
		choicesBackground.GetComponent<Image> ().CrossFadeAlpha(0.0f, 1.0f, true);

		Invoke ("DeactivateChoices", 1.0f);
	}

	private void DeactivateChoices()
	{
		List<GameObject> choicesBoxes = new List<GameObject> ();
		choicesBoxes.Add (oneChoiceBox);
		choicesBoxes.Add (twoChoicesBox);
		choicesBoxes.Add (threeChoicesBox);
		choicesBoxes.Add (sixChoicesBox);
		
		for (int i = 0; i < choicesBoxes.Count; ++i)
		{
			for (int j = 0; j < choicesBoxes[i].transform.childCount; ++j)
			{
				choicesBoxes[i].transform.GetChild(j).gameObject.SetActive(false);
			}
			choicesBoxes[i].SetActive(false);
		}
		
		choicesBackground.SetActive (false);
	}

	private IEnumerator ArrowFlicker()
	{
		Vector3 rightArrowScale = rightArrowImage.rectTransform.localScale;
		float rightMag = rightArrowScale.magnitude;
		
		bool isGrowing = true;
		
		while (isSomeoneTalking) 
		{
			if(isGrowing)
			{
				rightArrowScale *= 1.035f;
				rightArrowImage.rectTransform.localScale = rightArrowScale;
				if(rightArrowScale.magnitude >= rightMag * 1.1f)
					isGrowing = false;
			}
			else
			{
				rightArrowScale *= 0.965f;
				rightArrowImage.rectTransform.localScale = rightArrowScale;
				if(rightArrowScale.magnitude <= rightMag * 0.9f)
					isGrowing = true;
			}
			yield return null;
		}
		yield return null;
	}

	private void CrossFadeGroup(UIElementsGroup _group, float _imageAlphaValue, float _textAlphaValue, float _fadeTime)
	{
		foreach (Image curImage in _group.groupImagesList)
		{
			if(_imageAlphaValue > 0.5f)
				curImage.canvasRenderer.SetAlpha(0.0f);	
			curImage.CrossFadeAlpha(_imageAlphaValue, _fadeTime, true);
		}
		foreach (Text curText in _group.groupTextList)
		{
			if(_textAlphaValue > 0.5f)
				curText.canvasRenderer.SetAlpha(0.0f);	
			curText.CrossFadeAlpha(_textAlphaValue, _fadeTime, true);
		}
	}

#endregion

#region Dialogue Handling

	public void StartDialogue()
	{
//		SetChoices(new string[1]{"click"});
//		SetChoices(new string[2]{"click" , "press"});
//		SetChoices(new string[3]{"click" , "press", "Exit Shop"});
//		SetChoices(new string[4]{"click" , "press", "push", "pull"});
//		SetChoices(new string[5]{"click" , "press", "push", "pull", "choose"});
		SetChoices(new string[6]{"Intimidate" , "Offer Protection", "Ask For Payment", "Go Shopping", "Chit Chat", "Exit Shop"});
		SetBtnActions(1, GetButtonAction(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE));
		SetBtnActions(2, GetButtonAction(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION));
		SetBtnActions(3, GetButtonAction(ButtonActionsKeys.ACTION_SHOPKEEP_ASK_FOR_PAYMENT));
		SetBtnActions(4, GetButtonAction(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING));
		SetBtnActions(5, GetButtonAction(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT));		
		SetBtnActions(6, GetButtonAction(ButtonActionsKeys.ACTION_SHOP_EXIT));
		StartCoroutine(AnimatedDialogue(true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed suscipit diam, eget mollis quam. Nam consectetur rhoncus metus id commodo. Cras venenatis pulvinar cursus." + "\n\n\tWhat can I do for you today?", true));
	}
	
	private IEnumerator AnimatedDialogue(bool initialDelay, string _completeString, bool _displayChoices)
	{
		if(initialDelay)
			yield return new WaitForSeconds(1.0f);

		isSomeoneTalking = true;
		StartCoroutine (ArrowFlicker ());
		int passes = Mathf.CeilToInt (_completeString.Length / MAX_CHAR_LIMIT_PER_BODY) + 1;
		int curPass = 1;
		
		List<string> passesText = new List<string> ();
		
		for (int i = 0; i < passes; ++i) 
		{
			string curPassText = "";
			for (int j = 0; j < MAX_CHAR_LIMIT_PER_BODY; ++j) 
			{
				if(MAX_CHAR_LIMIT_PER_BODY * i + j < _completeString.Length)
					curPassText += _completeString[MAX_CHAR_LIMIT_PER_BODY * i + j];
			}
			passesText.Add(curPassText);
		}
		
		while(curPass-1 < passes)
		{
			string strToDisplay = "";
			int index = 0;
			dialogueText.CrossFadeAlpha(1.0f, 0.5f, true);
			
			while(index < passesText[curPass-1].Length)
			{
				strToDisplay += passesText[curPass-1][index];
				dialogueText.text = strToDisplay;
				index += 1;
				
				if(Input.GetMouseButtonDown(0))
				{
					dialogueText.canvasRenderer.SetAlpha(0.0f);
					dialogueText.text = passesText[curPass-1];
					dialogueText.CrossFadeAlpha(1.0f, 0.5f, true);
					index = passesText[curPass-1].Length;
					break;
				}
				
				yield return new WaitForSeconds(0.015f);
			}

			isSomeoneTalking = false;
			yield return new WaitForSeconds(0.25f);
			
			if(curPass < passesText.Count)
			{
				ellipseGraphic.CrossFadeAlpha(0.85f, 0.25f, true);
				while(true)
				{
					if(Input.GetMouseButtonDown(0))
					{
						++curPass;
						isSomeoneTalking = true;
						StartCoroutine(ArrowFlicker());
						ellipseGraphic.CrossFadeAlpha(0.0f, 0.15f, true);
						dialogueText.text = "";
						yield return new WaitForSeconds(0.5f);
						break;					
					}
					yield return null;
				}
			}
			else
			{
				if(_displayChoices)
					DisplayChoices();
				break;
			}
			
			yield return null;
		}
		
		yield return null;
	}

#endregion


#region Logic

	public void SetBtnActions(UnityEngine.Events.UnityAction[][] _actions)
	{
		// idiot check: Garbage collection?
		for (int i = 0; i < 6; ++i) 
			currentBtnActions[i].Clear ();
		currentBtnActions.Clear ();


		for (int i = 0; i < _actions.Length; ++i) 
		{
			currentBtnActions.Add(new List<UnityEngine.Events.UnityAction>());
			for (int j = 0; j < _actions[i].Length; ++j) 
				currentBtnActions[i].Add(_actions[i][j]);
		}
	}

	public void SetBtnActions(int _btnIndex, UnityEngine.Events.UnityAction[] _actions)
	{
		currentBtnActions [_btnIndex-1].Clear ();

		for (int i = 0; i < _actions.Length; ++i) 
			currentBtnActions[_btnIndex-1].Add(_actions[i]);
	}

	public void SetBtnActions(int _btnIndex, UnityEngine.Events.UnityAction _action)
	{
		currentBtnActions [_btnIndex-1].Clear ();
		currentBtnActions [_btnIndex - 1].Add (_action);
	}

	public UnityEngine.Events.UnityAction GetButtonAction(string _actionKey)
	{
		if(btnActionsDict.ContainsKey(_actionKey))
		{
			return btnActionsDict[_actionKey];
		}
		else
		{
			Debug.LogError("This action does not exist!");
			return null;
		}
	}
	
	private void PopulateButtonActionDict()
	{
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOP_EXIT, (() => {ExitShop ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE, (() => {Intimidate ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_ASK_FOR_PAYMENT, (() => {AskForPayment ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION, (() => {OfferProtection ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_RENEGOTIATE_PROTECTION, (() => {RenegotiateProtection ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING, (() => {GoShopping ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT, (() => {ChitChat ();}));
	}
#endregion


#region Potential Actions
	private void ExitShop()
	{
		StartCoroutine(AnimatedDialogue(false, "Have a good day!", false));
		PlayerMovement.CanExecuteAction (true);
		HideChoices();
		Debug.Log ("Exit!");
	}

	private void Intimidate()
	{
		Debug.Log ("Intimidate");
	}

	private void AskForPayment()
	{
		Debug.Log ("Ask for Payment");
	}
	
	private void OfferProtection()
	{
		Debug.Log ("Offer Protection");
	}

	private void RenegotiateProtection()
	{
		Debug.Log ("Renegotiate Protection");
	}

	private void GoShopping()
	{
		Debug.Log ("Go Shopping");
	}

	private void ChitChat()
	{
		Debug.Log ("Chit Chat");
	}
#endregion
}
