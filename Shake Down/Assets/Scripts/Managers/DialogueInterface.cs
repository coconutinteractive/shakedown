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
			instance = value;
		}
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
	private Dialogue_Prompt currentDialoguePrompt = null;

	private List<Dialogue_Prompt> previousPrompts = new List<Dialogue_Prompt>();
	private List<string> previousDialogueText = new List<string>();

	public event Action
		/* OnIntimidate,
			OnIntimidateImply,
			OnIntimidateThreaten,
			OnIntimidateAct,
				OnIntimidateInformBoss,
				OnIntimidateBreakMerchandise,
				OnIntimidateAttackShopkeeper,
				OnIntimidateBurnShopDown,
		
		OnAskForPayment,
			OnAccept,
			OnCheckRegister,
				OnTakeRegisterMoney,
				OnOfferToAidBusiness,
					OnDonateConfirm,
					OnDonateCancel,
					OnCutProtectionConfirm,
					OnCutProtectionCancel,		

		OnOfferProtection,

		OnRenegotiateProtection,

		OnGoShopping,
			OnShopProduct,
				OnConfirmPurchase,

		OnChitChat,

		OnExitShop; */

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
		currentDialoguePrompt = dialoguePrompt;
		SetOptions();
		string promptTextContent = currentBuilding.GetPersonalityText(dialoguePrompt.suffix);
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
			choicesString[i] = Localization.LocalizeText(buttonTextKey);
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
		for (int i = 0; i < 6; ++i) {
			currentBtnActions[i].Clear ();
		}
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
	}

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
	public Resources_Shopkeeper shopkeeperRef { get { return _shopkeeperRef; } set { _shopkeeperRef = value; } }

	public void PopulateButtonActionDict()
	{
		/*
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate"					).id, (() => {Dialogue_Option_Logic.Intimidate (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_imply"				).id, (() => {Dialogue_Option_Logic.Intimidate2Imply (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_threaten"			).id, (() => {Dialogue_Option_Logic.Intimidate2Threaten (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_act"				).id, (() => {Dialogue_Option_Logic.Intimidate2Act (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_informBoss"		).id, (() => {Dialogue_Option_Logic.Intimidate3InformBoss (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_breakMerchandise"	).id, (() => {Dialogue_Option_Logic.Intimidate3BreakMerchandise (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_attackShopkeeper"	).id, (() => {Dialogue_Option_Logic.Intimidate3AttackShopKeeper (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_burnDownShop"		).id, (() => {Dialogue_Option_Logic.Intimidate3BurnShopDown (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_requestPayment"				).id, (() => {Dialogue_Option_Logic.RequestPayment (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_accept"						).id, (() => {Dialogue_Option_Logic.Accept (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"				).id, (() => {Dialogue_Option_Logic.CheckRegister (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_takeRegisterMoney"				).id, (() => {Dialogue_Option_Logic.TakeRegisterMoney (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"			).id, (() => {Dialogue_Option_Logic.OfferToAidBusiness (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_donate"						).id, (() => {Dialogue_Option_Logic.Donate (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_cutProtectionCost"				).id, (() => {Dialogue_Option_Logic.CutProtectionCost (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_offerProtection"				).id, (() => {Dialogue_Option_Logic.OfferProtection (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_renegotiate"					).id, (() => {Dialogue_Option_Logic.Renegotiate (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_goShopping"					).id, (() => {Dialogue_Option_Logic.GoShopping (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"					).id, (() => {Dialogue_Option_Logic.ShopProduct (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_confirmPurchase"				).id, (() => {Dialogue_Option_Logic.ConfirmPurchase (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_chitChat"						).id, (() => {Dialogue_Option_Logic.ChitChat (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_neverMind"						).id, (() => {Dialogue_Option_Logic.NeverMind (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_exitShop"						).id, (() => {Dialogue_Option_Logic.ExitShop (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"					).id, (() => {Dialogue_Option_Logic.Acknowledge (Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_cancelPurchase"				).id, (() => {Dialogue_Option_Logic.CancelPurchase(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_continueIntimidating"			).id, (() => {Dialogue_Option_Logic.ContinueIntimidating(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_defaultAmount"					).id, (() => {Dialogue_Option_Logic.DefaultAmount(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_earlyPayment"					).id, (() => {Dialogue_Option_Logic.EarlyPayment(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_enterShop"						).id, (() => {Dialogue_Option_Logic.EnterShop(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_getDetails"					).id, (() => {Dialogue_Option_Logic.GetDetails(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_hearProposition"				).id, (() => {Dialogue_Option_Logic.HearProposition(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_ignore"						).id, (() => {Dialogue_Option_Logic.Ignore(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_otherAmount"					).id, (() => {Dialogue_Option_Logic.OtherAmount(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_placate"						).id, (() => {Dialogue_Option_Logic.Placate(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_reject"						).id, (() => {Dialogue_Option_Logic.Reject(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_resumeTalking"					).id, (() => {Dialogue_Option_Logic.ResumeTalking(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_returnGreeting"				).id, (() => {Dialogue_Option_Logic.ReturnGreeting(Resources_Player.instance, shopkeeperRef);}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_tryAnotherOffer"				).id, (() => {Dialogue_Option_Logic.TryAnotherOffer(Resources_Player.instance, shopkeeperRef);}));
		*/

		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate"					).id, (() => {Intimidate ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_imply"				).id, (() => {Intimidate2Imply ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_threaten"			).id, (() => {Intimidate2Threaten ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate2_act"				).id, (() => {Intimidate2Act ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_informBoss"		).id, (() => {Intimidate3InformBoss ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_breakMerchandise"	).id, (() => {Intimidate3BreakMerchandise ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_attackShopkeeper"	).id, (() => {Intimidate3AttackShopkeeper ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_intimidate3_burnDownShop"		).id, (() => {Intimidate3BurnShopDown ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_requestPayment"				).id, (() => {RequestPayment ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_accept"						).id, (() => {Accept ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"				).id, (() => {CheckTheRegister ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_takeRegisterMoney"				).id, (() => {TakeRegisterMoney ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"			).id, (() => {OfferToAidBusiness ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_donate"						).id, (() => {Donate ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_cutProtectionCost"				).id, (() => {CutProtectionCost ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_offerProtection"				).id, (() => {OfferProtection ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_renegotiate"					).id, (() => {Renegotiate ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_goShopping"					).id, (() => {GoShopping ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"					).id, (() => {ShopProduct ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_confirmPurchase"				).id, (() => {ConfirmPurchase ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_chitChat"						).id, (() => {ChitChat ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_neverMind"						).id, (() => {NeverMind ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_exitShop"						).id, (() => {ExitShop ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"					).id, (() => {Acknowledge ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_cancelPurchase"				).id, (() => {CancelPurchase ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_continueIntimidating"			).id, (() => {ContinueIntimidating ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_defaultAmount"					).id, (() => {DefaultAmount ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_earlyPayment"					).id, (() => {EarlyPayment ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_enterShop"						).id, (() => {EnterShop ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_getDetails"					).id, (() => {GetDetails ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_hearProposition"				).id, (() => {HearProposition ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_ignore"						).id, (() => {Ignore ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_otherAmount"					).id, (() => {OtherAmount ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_placate"						).id, (() => {Placate ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_reject"						).id, (() => {Reject ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_resumeTalking"					).id, (() => {ResumeTalking ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_returnGreeting"				).id, (() => {ReturnGreeting ();}));
		btnActionsDict.Add (Dialogue_Option.GetOptionByName("dialogue_option_tryAnotherOffer"				).id, (() => {TryAnotherOffer ();}));
	}

	private void SaveCurrentDialoguePrompt()
	{
		HideChoices ();
		previousPrompts.Add (currentDialoguePrompt);
		previousDialogueText.Add (dialogueText.text);
	}
#endregion

#region Potential Actions
	private void Intimidate () 					{ SaveCurrentDialoguePrompt (); if (OnIntimidate != null) { OnIntimidate (); } }
	private void Intimidate2Imply () 			{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Imply != null) { OnIntimidate2Imply (); } }
	private void Intimidate2Threaten () 		{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Threaten != null) { OnIntimidate2Threaten (); } }
	private void Intimidate2Act () 				{ SaveCurrentDialoguePrompt (); if (OnIntimidate2Act != null) { OnIntimidate2Act (); } }
	private void Intimidate3InformBoss () 		{ SaveCurrentDialoguePrompt (); if (OnIntimidate3InformBoss != null) { OnIntimidate3InformBoss (); } }
	private void Intimidate3BreakMerchandise () { SaveCurrentDialoguePrompt (); if (OnIntimidate3BreakMerchandise != null) { OnIntimidate3BreakMerchandise (); } }
	private void Intimidate3AttackShopkeeper () { SaveCurrentDialoguePrompt (); if (OnIntimidate3AttackShopkeeper != null) { OnIntimidate3AttackShopkeeper (); } }
	private void Intimidate3BurnShopDown () 	{ SaveCurrentDialoguePrompt (); if (OnIntimidate3BurnDownShop != null) { OnIntimidate3BurnDownShop (); } }
	private void RequestPayment () 				{ SaveCurrentDialoguePrompt (); if (OnRequestPayment != null) { OnRequestPayment (); } }
	private void Accept () 						{ SaveCurrentDialoguePrompt (); if (OnAccept != null) { OnAccept (); } }
	private void CheckTheRegister () 			{ SaveCurrentDialoguePrompt (); if (OnCheckTheRegister != null) { OnCheckTheRegister (); } }
	private void TakeRegisterMoney () 			{ SaveCurrentDialoguePrompt (); if (OnTakeRegisterMoney != null) { OnTakeRegisterMoney (); } }
	private void OfferToAidBusiness () 			{ SaveCurrentDialoguePrompt (); if (OnOfferToAidBusiness != null) { OnOfferToAidBusiness (); } }
	private void Donate () 						{ SaveCurrentDialoguePrompt (); if (OnDonate != null) { OnDonate (); } }
	private void CutProtectionCost () 			{ SaveCurrentDialoguePrompt (); if (OnCutProtectionCost != null) { OnCutProtectionCost (); } }
	private void OfferProtection () 			{ SaveCurrentDialoguePrompt (); if (OnOfferProtection != null) { OnOfferProtection (); DisplayDigitSelector("00000"); } }
	private void Renegotiate () 				{ SaveCurrentDialoguePrompt (); if (OnRenegotiate != null) { OnRenegotiate (); } }
	private void GoShopping () 					{ SaveCurrentDialoguePrompt (); if (OnGoShopping != null) { OnGoShopping (); } }
	private void ShopProduct () 				{ SaveCurrentDialoguePrompt (); if (OnShopProduct != null) { OnShopProduct (); } }
	private void ConfirmPurchase () 			{ SaveCurrentDialoguePrompt (); if (OnConfirmPurchase != null) { OnConfirmPurchase (); } }
	private void ChitChat () 					{ SaveCurrentDialoguePrompt (); if (OnChitChat != null) { OnChitChat (); } }
	private void NeverMind () 					{ SaveCurrentDialoguePrompt (); if (OnNeverMind != null) { OnNeverMind (); } }
	private void ExitShop () 					{ SaveCurrentDialoguePrompt (); if (OnExitShop != null) { OnExitShop (); } }
	private void Acknowledge () 				{ SaveCurrentDialoguePrompt (); if (OnAcknowledge != null) { OnAcknowledge (); } }
	private void CancelPurchase () 				{ SaveCurrentDialoguePrompt (); if (OnCancelPurchase != null) { OnCancelPurchase (); } }
	private void ContinueIntimidating () 		{ SaveCurrentDialoguePrompt (); if (OnContinueIntimidating != null) { OnContinueIntimidating (); } }
	private void DefaultAmount () 				{ SaveCurrentDialoguePrompt (); if (OnDefaultAmount != null) { OnDefaultAmount (); } }
	private void EarlyPayment () 				{ SaveCurrentDialoguePrompt (); if (OnEarlyPayment != null) { OnEarlyPayment (); } }
	private void EnterShop () 					{ SaveCurrentDialoguePrompt (); if (OnEnterShop != null) { OnEnterShop (); } }
	private void GetDetails () 					{ SaveCurrentDialoguePrompt (); if (OnGetDetails != null) { OnGetDetails (); } }
	private void HearProposition () 			{ SaveCurrentDialoguePrompt (); if (OnHearProposition != null) { OnHearProposition (); } }
	private void Ignore () 						{ SaveCurrentDialoguePrompt (); if (OnIgnore != null) { OnIgnore (); } }
	private void OtherAmount () 				{ SaveCurrentDialoguePrompt (); if (OnOtherAmount != null) { OnOtherAmount (); } }
	private void Placate () 					{ SaveCurrentDialoguePrompt (); if (OnPlacate != null) { OnPlacate (); } }
	private void Reject () 						{ SaveCurrentDialoguePrompt (); if (OnReject != null) { OnReject (); } }
	private void ResumeTalking () 				{ SaveCurrentDialoguePrompt (); if (OnResumeTalking != null) { OnResumeTalking (); } }
	private void ReturnGreeting () 				{ SaveCurrentDialoguePrompt (); if (OnReturnGreeting != null) { OnReturnGreeting (); } }
	private void TryAnotherOffer () 			{ SaveCurrentDialoguePrompt (); if (OnTryAnotherOffer != null) { OnTryAnotherOffer (); } }


	/*
	private void Intimidate() { SaveCurrentDialoguePrompt (); if (OnIntimidate != null) { OnIntimidate (); } }

	private void IntimidateImply()
	{
		Debug.Log ("Intimidate Imply");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidate2_imply != null)
			OnIntimidate2_imply ();
		//HideChoices ();
		//SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_IMPLY)]);
		//StartDialogue (false, 0.0f, "What are you implying?", true);
	}

	private void IntimidateThreaten()
	{
		Debug.Log ("Intimidate Threaten");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidate2_threaten != null)
			OnIntimidate2_threaten ();
		//HideChoices ();
		//SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_THREATEN)]);
		//StartDialogue (false, 0.0f, "Let's hear that threat...", true);
	}
	
	private void IntimidateAct()
	{
		Debug.Log ("Intimidate Act");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidate2_act != null)
			OnIntimidate2_act ();
		//HideChoices ();
		//SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_ACT)]);
		//StartDialogue (false, 0.0f, "Wait, what are you doing!?", true);
	}

	private void IntimidateInformBoss()
	{
		Debug.Log ("Intimidate InformBoss");
		if (OnIntimidate3_informBoss != null)
			OnIntimidate3_informBoss ();
		//HideChoices ();
	}

	private void IntimidateBreakMerchandise()
	{
		Debug.Log ("Intimidate Break Merchandise");
		if (OnIntimidate3_breakMerchandise != null)
			OnIntimidate3_breakMerchandise ();
		//HideChoices ();
	}

	private void IntimidateAttackShopKeeper()
	{
		Debug.Log ("Intimidate Attack ShopKeeper");
		if (OnIntimidate3_attackShopkeeper != null)
			OnIntimidate3_attackShopkeeper ();
		//HideChoices ();
	}

	private void IntimidateBurnShopDown()
	{
		Debug.Log ("Intimidate Burn Shop Down");
		if (OnIntimidate3_burnDownShop != null)
			OnIntimidate3_burnDownShop ();
		//HideChoices ();
	}

	private void RequestPayment()
	{
		Debug.Log ("Ask for Payment");
		SaveCurrentDialoguePrompt ();
		if (OnRequestPayment != null)
			OnRequestPayment ();
		//HideChoices ();
	}
	
	private void Accept()
	{
		Debug.Log ("Accept Payment");
		if (OnAccept != null)
			OnAccept ();
		//HideChoices ();
		//GoBackToPreviousPrompt ();
	}

	private void CheckRegister()
	{
		Debug.Log ("Check Register");
		if (OnCheckTheRegister != null)
			OnCheckTheRegister ();
		//HideChoices ();
	}
	private void TakeRegisterMoney()
	{
		Debug.Log ("Take Register Money");
		if (OnTakeRegisterMoney != null)
			OnTakeRegisterMoney ();
		//HideChoices ();
	}

	private void OfferToAidBusiness()
	{
		Debug.Log ("Aid Business");
		SaveCurrentDialoguePrompt ();
		if (OnOfferToAidBusiness != null)
			OnOfferToAidBusiness ();
		//HideChoices ();
	}
	/*
	private void Donate()
	{
		Debug.Log ("Donate");
		SaveCurrentDialoguePrompt ();
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = false;

		digitConfirmBtn.onClick.RemoveAllListeners ();
		digitConfirmBtn.onClick.AddListener (() => {DonateConfirm();});
		digitCancelBtn.onClick.RemoveAllListeners ();
		digitCancelBtn.onClick.AddListener (() => {DonateCancel();});
		
		DisplayDigitSelector("00000");
	}

	private void DonateConfirm()
	{
		Debug.Log ("Donate Confirm");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnDonateConfirm != null)
			OnDonateConfirm ();

		//HideChoices ();
		HideDigitSelector ();
	}

	private void DonateCancel()
	{
		Debug.Log ("Donate Cancel");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnDonateCancel != null)
			OnDonateCancel ();

		////HideChoices ();
		HideDigitSelector ();
		PreviousPrompt ();
	}

	private void CutProtectionCost()
	{
		Debug.Log ("Cur Protection Cost");
		SaveCurrentDialoguePrompt ();
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = false;
		
		digitConfirmBtn.onClick.RemoveAllListeners ();
		digitConfirmBtn.onClick.AddListener (() => {CutProtectionCostConfirm();});
		digitCancelBtn.onClick.RemoveAllListeners ();
		digitCancelBtn.onClick.AddListener (() => {CutProtectionCostCancel();});
		
		DisplayDigitSelector("00100");
	}

	private void CutProtectionCostConfirm()
	{
		Debug.Log ("Cut Protection Cost Confirm");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnCutProtectionConfirm != null)
			OnCutProtectionConfirm ();

		//HideChoices ();
		HideDigitSelector ();
	}

	private void CutProtectionCostCancel()
	{
		Debug.Log ("Cut Protection Cost Cancel");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnCutProtectionCancel != null)
			OnCutProtectionCancel ();

		////HideChoices ();
		HideDigitSelector ();
		PreviousPrompt ();
	}

	private void OfferProtection()
	{
		Debug.Log ("Offer Protection");
		if (OnOfferProtection != null)
			OnOfferProtection ();
	}

	private void RenegotiateProtection()
	{
		Debug.Log ("Renegotiate Protection");
		if (OnRenegotiate != null)
			OnRenegotiate ();
	}

	private void GoShopping()
	{
		Debug.Log ("Go Shopping");
		SaveCurrentDialoguePrompt ();
		if (OnGoShopping != null)
			OnGoShopping ();
		//HideChoices ();

		//NewPrompt (, false, 0.0f, true);
		//Dialogue_Option
		//SetDialoguePrompt (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_GO_SHOPPING)].followUps);
		//StartDialogue (false, 0.0f, currentBuilding.GetPersonalityText(currentDialoguePrompt.suffix), true);
	}

	private void ChitChat()
	{
		Debug.Log ("Chit Chat");
		if (OnChitChat != null)
			OnChitChat ();
		//HideChoices ();
		//SetDialoguePrompt (currentDialoguePrompt.followUps [choicesTextID.IndexOf (ButtonActionsKeys.TEXT_CHIT_CHAT)].followUps);
	}


	private void ShopProduct()
	{
		Debug.Log ("Shop Product");
		SaveCurrentDialoguePrompt ();
		if (OnShopProduct != null)
			OnShopProduct ();
		//HideChoices ();
		//SetDialoguePrompt (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_SHOP_PRODUCT)].followUps);
		//StartDialogue (false, 0.0f, "An excellent choice, sir. {INSERT COST} and this {INSERT OBJECT NAME} is yours!", true);
	}

	private void ConfirmPurchase()
	{
		Debug.Log ("Confirm Purchase");
		if (OnConfirmPurchase != null)
			OnConfirmPurchase ();
		//HideChoices ();
	}

	private void NeverMind()
	{
		Debug.Log ("Never mind");
		//HideChoices ();
	}

	private void ExitShop()
	{
		Debug.Log ("Exit Shop");
		//HideChoices ();
	}
	*/
#endregion
}