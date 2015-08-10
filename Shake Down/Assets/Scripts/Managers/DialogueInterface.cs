using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

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

	public event Action 
		OnIntimidate,
			OnIntimidateImply,
			OnIntimidateThreaten,
			OnIntimidateAct,
				OnIntimidateInformBoss,
				OnIntimidateBreakMerchandise,
				OnIntimidateAttackShopkeeper,
				OnIntimidateBurnShopDown,
		
		OnAskForPayment,
			OnAcceptPayment,
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

		OnExitShop;

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
	private int numberOfChoices = 0;
	private List<string> choicesTextID = new List<string>(), choicesText = new List<string>();
	private List<List<UnityEngine.Events.UnityAction>> currentBtnActions = new List<List<UnityEngine.Events.UnityAction>>();

	// Internal
	private bool isSomeoneTalking = false;

	// Dialogue stuff?
	private Dialogue_Prompt currentDialoguePrompt = null;
	private Dialogue_Option currentDialogueOption = null;

	private List<Dialogue_Prompt> previousPrompts = new List<Dialogue_Prompt>();
	private List<string> previousDialogueText = new List<string>();

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
		choicesTextID.Clear ();
		numberOfChoices = _choices.Length;
		foreach (string curChoice in _choices) 
			choicesTextID.Add (curChoice);
	}

	private void DisplayChoices()
	{
		
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

	public void StartDialogue(bool _initialDelay, float _delay, string _completeString, bool _displayChoices)
	{
		StartCoroutine (AnimatedDialogue (_initialDelay, _delay, _completeString, _displayChoices));
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

	public UnityEngine.Events.UnityAction GetButtonActionFromDict(string _actionKey)
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

	public UnityEngine.Events.UnityAction[] GetButtonActionFromDict(string[] _actionKeys)
	{
		List<UnityEngine.Events.UnityAction> actionsList = new List<UnityEngine.Events.UnityAction> ();

		for (int i = 0; i < _actionKeys.Length; ++i) 
		{
			if(btnActionsDict.ContainsKey(_actionKeys[i]))
				actionsList.Add(btnActionsDict[_actionKeys[i]]);
			else
				Debug.LogError("The action: " + _actionKeys[i] + " does not exist!");
		}
		return actionsList.ToArray ();
	}

	private void PopulateButtonActionDict()
	{
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE, (() => {Intimidate ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_IMPLY, (() => {IntimidateImply ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_THREATEN, (() => {IntimidateThreaten ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_ACT, (() => {IntimidateAct ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_INFORM_BOSS, (() => {IntimidateInformBoss ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_BREAK_MERCHANDISE, (() => {IntimidateBreakMechandise ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_ATTACK_SHOPKEEPER, (() => {IntimidateAttackShopKeeper ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_BURN_SHOP_DOWN, (() => {IntimidateBurnShopDown ();}));
		
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT, (() => {RequestPayment ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT, (() => {AcceptPayment ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_CHECK_REGISTER, (() => {CheckRegister ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_TAKE_REGISTER_MONEY, (() => {TakeRegisterMoney ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_AID_BUSINESS, (() => {OfferToAidBusiness ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_DONATE, (() => {Donate ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_CUT_PROTECTION_COST, (() => {CutProtectionCost ();}));

		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION, (() => {OfferProtection ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_RENEGOTIATE_PROTECTION, (() => {RenegotiateProtection ();}));

		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING, (() => {GoShopping ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOP_PRODUCT, (() => {ShopProduct ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPPRODUCT_CONFIRM_PURCHASE, (() => {ConfirmPurchase ();}));

		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT, (() => {ChitChat ();}));

		btnActionsDict.Add (ButtonActionsKeys.ACTION_NEVERMIND, (() => {NeverMind ();}));
		btnActionsDict.Add (ButtonActionsKeys.ACTION_SHOP_EXIT, (() => {ExitShop ();}));
	}

	private void SaveCurrentDialoguePrompt()
	{
		previousPrompts.Add (currentDialoguePrompt);
		previousDialogueText.Add (dialogueText.text);
	}

	private void GoBackToPreviousPrompt()
	{
		SetDialoguePrompt (previousPrompts.Last ());
		StartDialogue (false, 0.0f, previousDialogueText.Last (), true);
		previousPrompts.Remove (previousPrompts.Last ());
		previousDialogueText.Remove (previousDialogueText.Last ());
	}

	public void SetDialoguePrompt(Dialogue_Prompt _dialoguePrompt)
	{
		currentDialoguePrompt = _dialoguePrompt;
		UnityEngine.Events.UnityAction[] actions = GetButtonActionFromDict ((currentDialoguePrompt.followUpKeys).ToArray ());

		for (int i = 0; i < actions.Length; ++i)
			SetBtnActions (i+1, actions [i]);

		string[] choicesString = new string[6];
		for (int i = 0; i < currentDialoguePrompt.followUpBtnText.ToArray ().Length; ++i) 
		{
			if(currentDialoguePrompt.followUpAltBtnText.ToArray ()[i].Equals(""))
				choicesString[i] = currentDialoguePrompt.followUpBtnText.ToArray ()[i];
			else
				choicesString[i] = currentDialoguePrompt.followUpAltBtnText.ToArray ()[i];
		}
		choicesText.Clear ();
		choicesText = choicesString.ToList ();

		SetChoices (currentDialoguePrompt.followUpBtnText.ToArray ());
	}

	public void SetDialogueOption(Dialogue_Option _dialogueOption)
	{
		currentDialogueOption = _dialogueOption;
		UnityEngine.Events.UnityAction[] actions = GetButtonActionFromDict ((currentDialogueOption.followUps.followUpKeys).ToArray());

		for (int i = 0; i < actions.Length; ++i)
			SetBtnActions (i+1, actions [i]);

		string[] choicesString = new string[6];
		for (int i = 0; i < currentDialogueOption.followUps.followUpBtnText.ToArray ().Length; ++i) 
		{
			if(currentDialogueOption.followUps.followUpAltBtnText.ToArray ()[i].Equals(""))
				choicesString[i] = currentDialogueOption.followUps.followUpBtnText.ToArray ()[i];
			else
				choicesString[i] = currentDialogueOption.followUps.followUpAltBtnText.ToArray ()[i];
		}
		choicesText.Clear ();
		choicesText = choicesString.ToList ();

		SetChoices (currentDialogueOption.followUps.followUpBtnText.ToArray ());
	}
#endregion

#region Potential Actions
	private void ExitShop()
	{
		Debug.Log ("Exit!");
		if (OnExitShop != null)
			OnExitShop ();
	}

	private void Intimidate()
	{
		Debug.Log ("Intimidate");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidate != null)
			OnIntimidate ();
		HideChoices ();
		SetDialoguePrompt (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE)].followUps);
	}

	private void IntimidateImply()
	{
		Debug.Log ("Intimidate Imply");
		if (OnIntimidateImply != null)
			SaveCurrentDialoguePrompt ();
			OnIntimidateImply ();

		HideChoices ();
		SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_IMPLY)]);
		StartDialogue (false, 0.0f, "What are you implying?", true);
	}

	private void IntimidateThreaten()
	{
		Debug.Log ("Intimidate Threaten");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidateThreaten != null)
			OnIntimidateThreaten ();
		HideChoices ();
		SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_THREATEN)]);
		StartDialogue (false, 0.0f, "Let's hear that threat...", true);
	}
	
	private void IntimidateAct()
	{
		Debug.Log ("Intimidate Act");
		SaveCurrentDialoguePrompt ();
		if (OnIntimidateAct != null)
			OnIntimidateAct ();
		HideChoices ();
		SetDialogueOption (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_INTIMIDATE_ACT)]);
		StartDialogue (false, 0.0f, "Wait, what are you doing!?", true);
	}

	private void IntimidateInformBoss()
	{
		Debug.Log ("Intimidate InformBoss");
		if (OnIntimidateInformBoss != null)
			OnIntimidateInformBoss ();
		HideChoices ();
	}

	private void IntimidateBreakMechandise()
	{
		Debug.Log ("Intimidate Break Merchandise");
		if (OnIntimidateBreakMerchandise != null)
			OnIntimidateBreakMerchandise ();
		HideChoices ();
	}

	private void IntimidateAttackShopKeeper()
	{
		Debug.Log ("Intimidate Attack ShopKeeper");
		if (OnIntimidateAttackShopkeeper != null)
			OnIntimidateAttackShopkeeper ();
		HideChoices ();
	}

	private void IntimidateBurnShopDown()
	{
		Debug.Log ("Intimidate Burn Shop Down");
		if (OnIntimidateBurnShopDown != null)
			OnIntimidateBurnShopDown ();
		HideChoices ();
	}

	private void RequestPayment()
	{
		Debug.Log ("Ask for Payment");
		SaveCurrentDialoguePrompt ();
		if (OnAskForPayment != null)
			OnAskForPayment ();
		HideChoices ();
	}
	
	private void AcceptPayment()
	{
		Debug.Log ("Accept Payment");
		if (OnAcceptPayment != null)
			OnAcceptPayment ();
		HideChoices ();
		//GoBackToPreviousPrompt ();
	}

	private void CheckRegister()
	{
		Debug.Log ("Check Register");
		if (OnCheckRegister != null)
			OnCheckRegister ();
		HideChoices ();
	}
	private void TakeRegisterMoney()
	{
		Debug.Log ("Take Register Money");
		if (OnTakeRegisterMoney != null)
			OnTakeRegisterMoney ();
		HideChoices ();
	}

	private void OfferToAidBusiness()
	{
		Debug.Log ("Aid Business");
		SaveCurrentDialoguePrompt ();
		if (OnOfferToAidBusiness != null)
			OnOfferToAidBusiness ();
		HideChoices ();
	}

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

		HideChoices ();
		HideDigitSelector ();
	}

	private void DonateCancel()
	{
		Debug.Log ("Donate Cancel");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnDonateCancel != null)
			OnDonateCancel ();

		//HideChoices ();
		HideDigitSelector ();
		GoBackToPreviousPrompt ();
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

		HideChoices ();
		HideDigitSelector ();
	}

	private void CutProtectionCostCancel()
	{
		Debug.Log ("Cut Protection Cost Cancel");
		threeButtons.groupImagesList [2].gameObject.GetComponent<Button> ().interactable = true;
		if (OnCutProtectionCancel != null)
			OnCutProtectionCancel ();

		//HideChoices ();
		HideDigitSelector ();
		GoBackToPreviousPrompt ();
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
		if (OnRenegotiateProtection != null)
			OnRenegotiateProtection ();
	}

	private void GoShopping()
	{
		Debug.Log ("Go Shopping");
		SaveCurrentDialoguePrompt ();
		if (OnGoShopping != null)
			OnGoShopping ();
		HideChoices ();
		SetDialoguePrompt (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_GO_SHOPPING)].followUps);
		StartDialogue (false, 0.0f, "With pleasure! What would you like, sir?", true);
	}

	private void ChitChat()
	{
		Debug.Log ("Chit Chat");
		if (OnChitChat != null)
			OnChitChat ();
		HideChoices ();
		SetDialoguePrompt (currentDialoguePrompt.followUps [choicesTextID.IndexOf (ButtonActionsKeys.TEXT_CHIT_CHAT)].followUps);
	}


	private void ShopProduct()
	{
		Debug.Log ("Shop Product");
		SaveCurrentDialoguePrompt ();
		if (OnShopProduct != null)
			OnShopProduct ();
		HideChoices ();
		SetDialoguePrompt (currentDialoguePrompt.followUps[choicesTextID.IndexOf(ButtonActionsKeys.TEXT_SHOP_PRODUCT)].followUps);
		StartDialogue (false, 0.0f, "An excellent choice, sir. {INSERT COST} and this {INSERT OBJECT NAME} is yours!", true);
	}

	private void ConfirmPurchase()
	{
		Debug.Log ("Confirm Purchase");
		if (OnConfirmPurchase != null)
			OnConfirmPurchase ();
		HideChoices ();
	}

	private void NeverMind()
	{
		Debug.Log ("Never mind");
		HideChoices ();
		GoBackToPreviousPrompt ();
	}
#endregion
}