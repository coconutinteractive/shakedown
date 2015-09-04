using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class DialogueInterface : MonoBehaviour 
{
	#region Singleton
	public static DialogueInterface Instance 
	{
		get {
			if(instance == null)
				instance = FindObjectOfType(typeof(DialogueInterface)) as DialogueInterface;
			return instance;
		} set {
			instance = value; }
	}
	private static DialogueInterface instance;
	#endregion

	[System.Serializable]
	public class UIElementsGroup
	{
		public List<Image> groupImagesList = new List<Image> ();
		public List<Text> groupTextList = new List<Text> ();
	}

	private static bool _isActive = false;
	public static bool isActive{get{return _isActive;}}

	private static int MAX_CHAR_LIMIT_PER_BODY = 120;

	static private Building_Script _currentBuilding;
	static public Building_Script currentBuilding { get { return _currentBuilding; } set { _currentBuilding = value; } }

	private Dictionary<string, UnityEngine.Events.UnityAction> btnActionsDict = new Dictionary<string, UnityEngine.Events.UnityAction>();

	// Groups
	[SerializeField] private UIElementsGroup allBackground = null;
	[SerializeField] private UIElementsGroup leftSide = null, rightSide = null;
	[SerializeField] private UIElementsGroup oneButton = null, twoButtons = null, threeButtons = null, sixButtons = null;
	[SerializeField] private UIElementsGroup digitSelector = null, shopInventory = null;

	// Specific Items
	[SerializeField] private Image leftPortraitImage = null, rightPortraitImage = null;
	[SerializeField] private Image leftArrowImage = null, rightArrowImage = null;
	[SerializeField] private GameObject choicesBackground = null, addOnBackground = null;
	[SerializeField] private Text dialogueText = null;
	[SerializeField] private Image ellipseGraphic = null;
	[SerializeField] private Button digitConfirmBtn = null, digitCancelBtn = null;

	[SerializeField] private GameObject sixChoicesBox = null, threeChoicesBox = null, twoChoicesBox = null, oneChoiceBox = null,
										digitSelectorBox = null, shopInventoryBox = null;

	// TEMP! 
	[SerializeField] public Sprite cashRegisterPortrait = null;

	// Choices
	private List<string> choicesTextID = new List<string>(), choicesText = new List<string>();
	private List<List<UnityEngine.Events.UnityAction>> currentBtnActions = new List<List<UnityEngine.Events.UnityAction>>();

	// Internal
	private bool isSomeoneTalking = false;

	// Dialogue stuff?
	private Dialogue_Prompt _currentDialoguePrompt = null;
	public Dialogue_Prompt currentDialoguePrompt { get { return _currentDialoguePrompt; } }

	private List<Dialogue_Prompt> previousPrompts = new List<Dialogue_Prompt>();
	private List<string> previousDialogueText = new List<string>();

	public event Action
		OnIntimidate,
		OnIntimidate2Imply,
		OnIntimidate2Threaten,
		OnIntimidate2Act,
		OnIntimidate3InformBoss,
		OnIntimidate3BreakMerchandise,
		OnIntimidate3AttackShopkeeper,
		OnIntimidate3BurnDownShop,
		OnRequestPayment,
		OnAccept,
		OnCheckTheRegister,
		OnTakeRegisterMoney,
		OnOfferToAidBusiness,
		OnDonate,
		OnCutProtectionCost,
		OnOfferProtection,
		OnRenegotiate,
		OnGoShopping,
		OnShopProduct,
		OnConfirmPurchase,
		OnChitChat,
		OnNeverMind,
		OnExitShop,
		OnAcknowledge,
		OnCancelPurchase,
		OnContinueIntimidating,
		OnDefaultAmount,
		OnEarlyPayment,
		OnEnterShop,
		OnGetDetails,
		OnHearProposition,
		OnIgnore,
		OnOtherAmount,
		OnPlacate,
		OnReject,
		OnResumeTalking,
		OnReturnGreeting,
		OnTryAnotherOffer;

	private void Awake()
	{
		for (int i = 0; i < 6; ++i) 
			currentBtnActions.Add(new List<UnityEngine.Events.UnityAction>());

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

		digitConfirmBtn.onClick.RemoveAllListeners ();
		digitCancelBtn.onClick.RemoveAllListeners ();
		digitConfirmBtn.onClick.AddListener (() => {Confirm();});
		digitCancelBtn.onClick.AddListener (() => {Cancel();});

		oneChoiceBox.SetActive (false);
		twoChoicesBox.SetActive (false);
		threeChoicesBox.SetActive (false);
		sixChoicesBox.SetActive (false);
		choicesBackground.SetActive (false);

		for (int i = 0; i < 5; ++i) 
		{
			GameObject digitTextObj = digitSelectorBox.transform.GetChild(i).GetChild(0).gameObject;

			digitSelectorBox.transform.GetChild(i).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {UpdateDigitSelector(1, digitTextObj);});
			digitSelectorBox.transform.GetChild(i).transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {UpdateDigitSelector(-1, digitTextObj);});
		}

		digitSelectorBox.SetActive (false);
		//shopInventoryBox.SetActive (false);
		addOnBackground.SetActive (false);

		_isActive = false;
	}

	private void Confirm()
	{
		int value = GetDigitSelectorValue();
		DialogueInterface.Instance.NewPrompt(
			Dialogue_Option_Logic.DefaultAmount(
				Resources_Player.instance,
				shopkeeperRef,
				value
			),
			false,
			0.0f,
			true
		);
		HideDigitSelector();
	}

	private void Cancel()
	{
		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"),false,0.0f,true);
		HideDigitSelector();
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
		//HideChoices ();
		dialogueText.CrossFadeAlpha (0.0f, 1.0f, true);
		HideLeftCharacter ();
		HideRightCharacter ();
	}

	public void DisplayLeftCharacter(Sprite _portrait, float _fadeTime, bool _isTalking)
	{
		leftPortraitImage.sprite = _portrait;
		CrossFadeGroup (leftSide, 0.92f, 1.0f, _fadeTime);

		if (_isTalking)
			leftArrowImage.CrossFadeAlpha (1.0f, _fadeTime * 0.5f, true);

	}

	public void DisplayRightCharacter(Sprite _portrait, float _fadeTime, bool _isTalking)
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

	private void DisplayChoices()
	{
		choicesBackground.SetActive (true);
		choicesBackground.GetComponent<Image> ().CrossFadeAlpha (0.92f, 1.0f, true);
		GameObject selectedBox = null;

		int numberOfChoices = 0;
		for (int i = 0; i < choicesTextID.Count; i++) {
			if (choicesTextID[i] != null) { numberOfChoices++; }
		}
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
	
	public void HideChoices()
	{
		CrossFadeGroup (oneButton, 0.0f, 0.0f, 0.25f);
		CrossFadeGroup (twoButtons, 0.0f, 0.0f, 0.25f);
		CrossFadeGroup (threeButtons, 0.0f, 0.0f, 0.25f);
		CrossFadeGroup (sixButtons, 0.0f, 0.0f, 0.25f);
		choicesBackground.GetComponent<Image> ().CrossFadeAlpha(0.0f, 0.25f, true);

		Invoke ("DeactivateChoices", 0.25f);
	}

	public void DisplayDigitSelector()
	{
		digitSelectorBox.SetActive (true);
		CrossFadeGroup (digitSelector, 0.92f, 1.0f, 1.0f);
		
		for (int i = 0; i < 5; ++i) 
			digitSelectorBox.transform.GetChild (i).GetChild(0).GetComponent<Text>().text = "0";
	}

	public void DisplayDigitSelector(string _initialFiveDigitString)
	{
		addOnBackground.SetActive (true);
		digitSelectorBox.SetActive (true);
		CrossFadeGroup (digitSelector, 0.92f, 1.0f, 1.0f);

		for (int i = 0; i < 5; ++i) 
		{
			digitSelectorBox.transform.GetChild (i).GetChild(0).GetComponent<Text>().text = "";
			digitSelectorBox.transform.GetChild (i).GetChild(0).GetComponent<Text>().text += _initialFiveDigitString[i];
		}
	}

	public void HideDigitSelector()
	{
		CrossFadeGroup (digitSelector, 0.0f, 0.0f, 0.25f);
		addOnBackground.GetComponent<Image> ().CrossFadeAlpha(0.0f, 0.25f, true);

		Invoke ("DeactivateAddOn", 0.25f);
	}

	public void UpdateDigitSelector(int _val, GameObject _digit)
	{
		//Debug.Log (_val.ToString() + " " + _digit.name);
		Text digitText = _digit.GetComponent<Text> ();

		if(int.Parse(digitText.text) != 9 && int.Parse(digitText.text) != 0)
			digitText.text = (int.Parse(digitText.text) + _val).ToString();
		else if(digitText.text.Equals("9"))
			digitText.text = _val == 1 ? "0" : "8";
		else if(digitText.text.Equals("0"))
			digitText.text = _val == 1 ? "1" : "9";
	}

	public int GetDigitSelectorValue()
	{
		string value = "";

		for (int i = 0; i < 5; ++i) 
		{
			value += digitSelectorBox.transform.GetChild(i).GetChild (0).GetComponent<Text>().text;
		}

		return int.Parse (value);
	}

	private void DeactivateAddOn()
	{
		digitSelectorBox.SetActive (false);
//		shopInventoryBox.SetActive (false);
		addOnBackground.SetActive (false);
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
	public void NewPrompt(Dialogue_Prompt dialoguePrompt, bool initialDelay, float delay, bool displayChoices)
	{
		_currentDialoguePrompt = dialoguePrompt;
		SetOptions();
		List<string> parameters = new List<string>();
		string promptTextContent;

		if(currentDialoguePrompt == Dialogue_Prompt.GetPromptByName("dialogue_prompt_confirmPurchase"))
		{
			parameters.Add (Localization.LocalizeText(_item.id));
			parameters.Add (_item.price.ToString());
			promptTextContent = currentBuilding.GetPersonalityText(dialoguePrompt.suffix, parameters);
		} else {
			promptTextContent = currentBuilding.GetPersonalityText(dialoguePrompt.suffix);
		}
		StartCoroutine (AnimatedDialogue (initialDelay, delay, promptTextContent, displayChoices));
	}

	public void PreviousPrompt()
	{
		NewPrompt (previousPrompts.Last (), false, 0.0f, true);
		previousPrompts.Remove (previousPrompts.Last ());
		previousDialogueText.Remove (previousDialogueText.Last ());
	}
	
	private void SetOptions()
	{
		UnityEngine.Events.UnityAction[] actions = GetButtonActionFromDict ((Dialogue_Prompt_Logic.FilterKeys(currentDialoguePrompt, shopkeeperRef)).ToArray());

		for (int i = 0; i < actions.Length; ++i)
		{
			SetBtnActions (i+1, actions [i]);
		}

		List<string> buttonKeys = Dialogue_Prompt_Logic.FilterKeys(currentDialoguePrompt, shopkeeperRef);

		string[] choicesString = new string[6];
		for (int i = 0; i < buttonKeys.Count; ++i) {
			string buttonTextKey = Dialogue_Option.GetOptionByName(buttonKeys[i]).buttonTextKey;

			List<string> parameters = new List<string>();
			switch(buttonKeys[i])
			{
			case "dialogue_option_shopProduct": {
				if(i < shopkeeperRef.home.inventory.items.Count && shopkeeperRef.home.inventory.items[i] != null) {
					string parameter = Localization.LocalizeText(shopkeeperRef.home.inventory.items[i].id);
					parameters.Add (parameter);
					choicesString[i] = Localization.LocalizeText(buttonTextKey, parameters);
				} else {
					choicesString[i] = "DISABLE ME!"; 
				}
				break; }
			//case "dialogue_option_confirmPurchase": {
				//parameters.Add ();
				//break; }
			default: {
				choicesString[i] = Localization.LocalizeText(buttonTextKey);
				break; }
			}
		}

		choicesText.Clear ();
		choicesText = choicesString.ToList ();
		
		SetChoices (choicesString);
	}

	public void SetChoices(string[] _choices)
	{
		choicesTextID.Clear ();
		foreach (string curChoice in _choices) 
		{
			choicesTextID.Add (curChoice);
		}
	}

	private IEnumerator AnimatedDialogue(bool _initialDelay, float _delay, string _completeString, bool _displayChoices)
	{
		if(_initialDelay)
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
				//TODO: BREAK PASS WHEN ENCOUNTERING BREAK SYMBOL!!

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
				if(_displayChoices && currentBtnActions[0] != null)
					DisplayChoices();
				break;
			}
			
			yield return null;
		}
		yield return null;
	}

#endregion
	private Item_Root _item;
	private Item_Root item { set { _item = value;} }
	public Item_Root GetItem() { return _item; }

	private void Test1 ()
	{
		item = shopkeeperRef.home.inventory.shopItem1;
	}
	private void Test2 ()
	{
		item = shopkeeperRef.home.inventory.shopItem2;
	}
	private void Test3 ()
	{
		item = shopkeeperRef.home.inventory.shopItem3;
	}
	private void Test4 ()
	{
		item = shopkeeperRef.home.inventory.shopItem4;
	}
#region Logic

	private UnityEngine.Events.UnityAction GetTest(int testNum)
	{
		switch (testNum)
		{
		case 0: {
			return btnActionsDict["test1"]; }
		case 1: {
			return btnActionsDict["test2"]; }
		case 2: {
			return btnActionsDict["test3"]; }
		case 3: {
			return btnActionsDict["test4"]; }
		}
		return null;
	}

	public void SetBtnActions(UnityEngine.Events.UnityAction[][] _actions)
	{
		// idiot check: Garbage collection?
		for (int i = 0; i < 6; ++i) {
			currentBtnActions[i].Clear ();
		}
		currentBtnActions.Clear ();

		for (int i = 0; i < _actions.Length; ++i) 
		{
			currentBtnActions.Add(new List<UnityEngine.Events.UnityAction>());
			for (int j = 0; j < _actions[i].Length; ++j) 
			{
				if(currentDialoguePrompt == Dialogue_Prompt.GetPromptByName("dialogue_prompt_shopInventory"))
				{
					currentBtnActions[i].Add (GetTest(i));
				}
				currentBtnActions[i].Add (_actions[i][j]);
			}
		}
	}
	
	public void SetBtnActions(int _btnIndex, UnityEngine.Events.UnityAction[] _actions)
	{
		currentBtnActions [_btnIndex - 1].Clear ();

		for (int i = 0; i < _actions.Length; ++i) {
			if(currentDialoguePrompt == Dialogue_Prompt.GetPromptByName("dialogue_prompt_shopInventory"))
			{
				currentBtnActions[_btnIndex - 1].Add (GetTest(_btnIndex - 1));
			}
			currentBtnActions[_btnIndex - 1].Add (_actions[i]);
		}
	}

	public void SetBtnActions(int _btnIndex, UnityEngine.Events.UnityAction _action)
	{
		currentBtnActions [_btnIndex - 1].Clear ();

		if(currentDialoguePrompt == Dialogue_Prompt.GetPromptByName("dialogue_prompt_shopInventory"))
		{
			currentBtnActions [_btnIndex - 1].Add (GetTest(_btnIndex - 1));
		}
		currentBtnActions [_btnIndex - 1].Add (_action);
	}
	/*
	public UnityEngine.Events.UnityAction GetButtonActionFromDict(string actionKey)
	{
		if(btnActionsDict.ContainsKey(actionKey))
		{
			return btnActionsDict[actionKey];
		}
		else
		{
			Debug.LogError("This action does not exist!");
			return null;
		}
	}*/

	public UnityEngine.Events.UnityAction[] GetButtonActionFromDict(string[] actionKeys)
	{
		List<UnityEngine.Events.UnityAction> actionsList = new List<UnityEngine.Events.UnityAction> ();

		for (int i = 0; i < actionKeys.Length; ++i) 
		{
			if(btnActionsDict.ContainsKey(actionKeys[i]))
				actionsList.Add(btnActionsDict[actionKeys[i]]);
			else
				Debug.LogError("The action '" + actionKeys[i] + "' does not exist!");
		}
		return actionsList.ToArray ();
	}

	//temp to prove concept. do it better later
	private Resources_Shopkeeper _shopkeeperRef;
	public Resources_Shopkeeper shopkeeperRef { get { return _shopkeeperRef; } set { _shopkeeperRef = value; OutputShopkeeperStats();} }

	public void OutputShopkeeperStats()
	{
		Debug.Log ("Attitude: " + shopkeeperRef.attitude);
		Debug.Log ("Expenses: " + shopkeeperRef.expenses);
		Debug.Log ("Fear: " + shopkeeperRef.fear);
		Debug.Log ("Gender: " + shopkeeperRef.gender);
		Debug.Log ("Greed: " + shopkeeperRef.greed);
		Debug.Log ("Home: " + shopkeeperRef.home);
		Debug.Log ("ID: " + shopkeeperRef.id);
		Debug.Log ("Income: " + shopkeeperRef.income);
		Debug.Log ("Integrity: " + shopkeeperRef.integrity);
		Debug.Log ("Inventory: " + shopkeeperRef.inventory.id);
		for (int i = 0; i < shopkeeperRef.inventory.items.Count; i++)
		{
			Debug.Log ("--- " + shopkeeperRef.inventory.items[i].id);
		}
		Debug.Log ("Shop Inventory: " + shopkeeperRef.home.inventory.id);
		for (int i = 0; i < shopkeeperRef.home.inventory.items.Count; i++)
		{
			Debug.Log ("--- " + shopkeeperRef.home.inventory.items[i].id);
		}
		Debug.Log ("Money: " + shopkeeperRef.money);
		Debug.Log ("Name: " + shopkeeperRef.name);
		Debug.Log ("Personality: " + shopkeeperRef.personality);
		Debug.Log ("Respect: " + shopkeeperRef.respect);
		Debug.Log ("Strength: " + shopkeeperRef.strength);
		Debug.Log ("Stubbornness: " + shopkeeperRef.stubbornness);
	}

	public void PopulateButtonActionDict()
	{
		btnActionsDict.Add ("dialogue_option_accept"						, (() => {Accept ();}));
		btnActionsDict.Add ("dialogue_option_acknowledge"					, (() => {Acknowledge ();}));
		btnActionsDict.Add ("dialogue_option_cancelPurchase"				, (() => {CancelPurchase ();}));
		btnActionsDict.Add ("dialogue_option_checkTheRegister"				, (() => {CheckTheRegister ();}));
		btnActionsDict.Add ("dialogue_option_chitChat"						, (() => {ChitChat ();}));
		btnActionsDict.Add ("dialogue_option_confirmPurchase"				, (() => {ConfirmPurchase ();}));
		btnActionsDict.Add ("dialogue_option_continueIntimidating"			, (() => {ContinueIntimidating ();}));
		btnActionsDict.Add ("dialogue_option_cutProtectionCost"				, (() => {CutProtectionCost ();}));
		btnActionsDict.Add ("dialogue_option_defaultAmount"					, (() => {DefaultAmount ();}));
		btnActionsDict.Add ("dialogue_option_donate"						, (() => {Donate ();}));
		btnActionsDict.Add ("dialogue_option_earlyPayment"					, (() => {EarlyPayment ();}));
		btnActionsDict.Add ("dialogue_option_enterShop"						, (() => {EnterShop ();}));
		btnActionsDict.Add ("dialogue_option_exitShop"						, (() => {ExitShop ();}));
		btnActionsDict.Add ("dialogue_option_getDetails"					, (() => {GetDetails ();}));
		btnActionsDict.Add ("dialogue_option_goShopping"					, (() => {GoShopping ();}));
		btnActionsDict.Add ("dialogue_option_hearProposition"				, (() => {HearProposition ();}));
		btnActionsDict.Add ("dialogue_option_ignore"						, (() => {Ignore ();}));
		btnActionsDict.Add ("dialogue_option_intimidate"					, (() => {Intimidate ();}));
		btnActionsDict.Add ("dialogue_option_intimidate2_act"				, (() => {Intimidate2Act ();}));
		btnActionsDict.Add ("dialogue_option_intimidate2_imply"				, (() => {Intimidate2Imply ();}));
		btnActionsDict.Add ("dialogue_option_intimidate2_threaten"			, (() => {Intimidate2Threaten ();}));
		btnActionsDict.Add ("dialogue_option_intimidate3_attackShopkeeper"	, (() => {Intimidate3AttackShopkeeper ();}));
		btnActionsDict.Add ("dialogue_option_intimidate3_breakMerchandise"	, (() => {Intimidate3BreakMerchandise ();}));
		btnActionsDict.Add ("dialogue_option_intimidate3_burnDownShop"		, (() => {Intimidate3BurnShopDown ();}));
		btnActionsDict.Add ("dialogue_option_intimidate3_informBoss"		, (() => {Intimidate3InformBoss ();}));
		btnActionsDict.Add ("dialogue_option_neverMind"						, (() => {NeverMind ();}));
		btnActionsDict.Add ("dialogue_option_offerProtection"				, (() => {OfferProtection ();}));
		btnActionsDict.Add ("dialogue_option_offerToAidBusiness"			, (() => {OfferToAidBusiness ();}));
		btnActionsDict.Add ("dialogue_option_otherAmount"					, (() => {OtherAmount ();}));
		btnActionsDict.Add ("dialogue_option_placate"						, (() => {Placate ();}));
		btnActionsDict.Add ("dialogue_option_renegotiate"					, (() => {Renegotiate ();}));
		btnActionsDict.Add ("dialogue_option_reject"						, (() => {Reject ();}));
		btnActionsDict.Add ("dialogue_option_requestPayment"				, (() => {RequestPayment ();}));
		btnActionsDict.Add ("dialogue_option_resumeTalking"					, (() => {ResumeTalking ();}));
		btnActionsDict.Add ("dialogue_option_returnGreeting"				, (() => {ReturnGreeting ();}));
		btnActionsDict.Add ("dialogue_option_shopProduct"					, (() => {ShopProduct ();}));
		btnActionsDict.Add ("dialogue_option_takeRegisterMoney"				, (() => {TakeRegisterMoney ();}));
		btnActionsDict.Add ("dialogue_option_tryAnotherOffer"				, (() => {TryAnotherOffer ();}));
		btnActionsDict.Add ("test1"											, (() => {Test1 ();}));
		btnActionsDict.Add ("test2"											, (() => {Test2 ();}));
		btnActionsDict.Add ("test3"											, (() => {Test3 ();}));
		btnActionsDict.Add ("test4"											, (() => {Test4 ();}));
	}

	private void SaveCurrentDialoguePrompt()
	{
		HideChoices ();
		previousPrompts.Add (currentDialoguePrompt);
		previousDialogueText.Add (dialogueText.text);
	}
#endregion

#region Potential Actions
	private void Accept () 						{ SaveCurrentDialoguePrompt (); if (OnAccept != null) { OnAccept (); } }
	private void Acknowledge () 				{ SaveCurrentDialoguePrompt (); if (OnAcknowledge != null) { OnAcknowledge (); } }
	private void CancelPurchase () 				{ SaveCurrentDialoguePrompt (); if (OnCancelPurchase != null) { OnCancelPurchase (); } }
	private void CheckTheRegister () 			{ SaveCurrentDialoguePrompt (); if (OnCheckTheRegister != null) { OnCheckTheRegister (); } }
	private void ChitChat () 					{ SaveCurrentDialoguePrompt (); if (OnChitChat != null) { OnChitChat (); } }
	private void ConfirmPurchase () 			{ SaveCurrentDialoguePrompt (); if (OnConfirmPurchase != null) { OnConfirmPurchase (); } }
	private void ContinueIntimidating () 		{ SaveCurrentDialoguePrompt (); if (OnContinueIntimidating != null) { OnContinueIntimidating (); } }
	private void CutProtectionCost () 			{ SaveCurrentDialoguePrompt (); if (OnCutProtectionCost != null) { OnCutProtectionCost (); } }
	private void DefaultAmount () 				{ SaveCurrentDialoguePrompt (); if (OnDefaultAmount != null) { OnDefaultAmount (); } }
	private void Donate () 						{ SaveCurrentDialoguePrompt (); if (OnDonate != null) { OnDonate (); } }
	private void EarlyPayment () 				{ SaveCurrentDialoguePrompt (); if (OnEarlyPayment != null) { OnEarlyPayment (); } }
	private void EnterShop () 					{ SaveCurrentDialoguePrompt (); if (OnEnterShop != null) { OnEnterShop (); } }
	private void ExitShop () 					{ SaveCurrentDialoguePrompt (); if (OnExitShop != null) { OnExitShop (); } }
	private void GetDetails () 					{ SaveCurrentDialoguePrompt (); if (OnGetDetails != null) { OnGetDetails (); } }
	private void GoShopping () 					{ SaveCurrentDialoguePrompt (); if (OnGoShopping != null) { OnGoShopping (); } }
	private void HearProposition () 			{ SaveCurrentDialoguePrompt (); if (OnHearProposition != null) { OnHearProposition (); } }
	private void Ignore () 						{ SaveCurrentDialoguePrompt (); if (OnIgnore != null) { OnIgnore (); } }
	private void Intimidate () 					{ SaveCurrentDialoguePrompt (); if (OnIntimidate != null) { OnIntimidate (); } }
	private void Intimidate2Imply () 			{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Imply != null) { OnIntimidate2Imply (); } }
	private void Intimidate2Threaten () 		{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Threaten != null) { OnIntimidate2Threaten (); } }
	private void Intimidate2Act () 				{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Act != null) { OnIntimidate2Act (); } }
	private void Intimidate3InformBoss () 		{ SaveCurrentDialoguePrompt (); if (OnIntimidate3InformBoss != null) { OnIntimidate3InformBoss (); } }
	private void Intimidate3BreakMerchandise () { SaveCurrentDialoguePrompt (); if (OnIntimidate3BreakMerchandise != null) { OnIntimidate3BreakMerchandise (); } }
	private void Intimidate3AttackShopkeeper () { SaveCurrentDialoguePrompt (); if (OnIntimidate3AttackShopkeeper != null) { OnIntimidate3AttackShopkeeper (); } }
	private void Intimidate3BurnShopDown () 	{ SaveCurrentDialoguePrompt (); if (OnIntimidate3BurnDownShop != null) { OnIntimidate3BurnDownShop (); } }
	private void NeverMind () 					{ SaveCurrentDialoguePrompt (); if (OnNeverMind != null) { OnNeverMind (); } }
	private void OfferToAidBusiness () 			{ SaveCurrentDialoguePrompt (); if (OnOfferToAidBusiness != null) { OnOfferToAidBusiness (); } }
	private void OfferProtection () 			{ SaveCurrentDialoguePrompt (); if (OnOfferProtection != null) { OnOfferProtection (); DisplayDigitSelector(shopkeeperRef.home.DefaultProtectionOffer()); } }
	private void OtherAmount () 				{ SaveCurrentDialoguePrompt (); if (OnOtherAmount != null) { OnOtherAmount (); } }
	private void Placate () 					{ SaveCurrentDialoguePrompt (); if (OnPlacate != null) { OnPlacate (); } }
	private void Reject () 						{ SaveCurrentDialoguePrompt (); if (OnReject != null) { OnReject (); } }
	private void Renegotiate () 				{ SaveCurrentDialoguePrompt (); if (OnRenegotiate != null) { OnRenegotiate (); } }
	private void RequestPayment () 				{ SaveCurrentDialoguePrompt (); if (OnRequestPayment != null) { OnRequestPayment (); } }
	private void ResumeTalking () 				{ SaveCurrentDialoguePrompt (); if (OnResumeTalking != null) { OnResumeTalking (); } }
	private void ReturnGreeting () 				{ SaveCurrentDialoguePrompt (); if (OnReturnGreeting != null) { OnReturnGreeting (); } }
	private void ShopProduct () 				{ SaveCurrentDialoguePrompt (); if (OnShopProduct != null) { OnShopProduct (); } }
	private void TakeRegisterMoney () 			{ SaveCurrentDialoguePrompt (); if (OnTakeRegisterMoney != null) { OnTakeRegisterMoney (); } }
	private void TryAnotherOffer () 			{ SaveCurrentDialoguePrompt (); if (OnTryAnotherOffer != null) { OnTryAnotherOffer (); } }
#endregion
}