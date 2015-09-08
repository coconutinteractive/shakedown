using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Script : MonoBehaviour
{
	#region Static Data / Design World Comparisson Stuff
	[SerializeField] private string _buildingID;
	static private Dictionary<string, Building_Script> _buildings = new Dictionary<string, Building_Script>();
	static private List<string> _buildingIDs = new List<string>();
	static public List<string> buildingIDs { get { return _buildingIDs; } }
	private bool _firstVisitToday = true;
	public bool firstVisitToday { get { return _firstVisitToday; } set { _firstVisitToday = value; } } 

	private void Start()
	{
		if(!registerBuilding())
			Debug.LogError ("Hey. Yo, buddy. There's a duplicate in the Building IDs. You're gonna want to change one of the " + _buildingID + "'s to something else. -Scott");
	}

	private bool registerBuilding()
	{
		if(buildingIDs.Contains (_buildingID)) {
			return false;
		} else { 
			buildingIDs.Add (_buildingID);
			_buildings.Add (_buildingID, this.GetComponent<Building_Script>());
			return true;
		}
	}

	static public Building_Script GetBuilding(string buildingID) 
	{
		if(_buildings.ContainsKey(buildingID))
		{
			return _buildings[buildingID]; 
		} else {
			Debug.LogError ("Hey. Yo, Shmuck. There's no building with the id '" + buildingID + "' anywhere in the design world! You ought to do something about that. -Scott");
			return null;
		}
	}
	#endregion

	private Resources_Shopkeeper _shopkeeper;
	public Resources_Shopkeeper shopkeeper { 	get { return _shopkeeper; }	set { _shopkeeper = value; } }
	private Resources_Building _resources;
	public Resources_Building resources { 		get { return _resources; } 	set { _resources = value; } }

	private bool isGuilty = false;
	private bool isUnderProtection = false;

	#region Personality Text Methods
	public string GetPersonalityText(string key)
	{
		List<Resources_Character> list = new List<Resources_Character>();
		list.Add(Resources_Player.instance);
		list.Add(shopkeeper);
		
		string newKey = "loc_prompt_";
		newKey += shopkeeper.personality.ToString().ToLower() + "_";
		newKey += shopkeeper.attitude.ToString().ToLower() + "_";
		newKey += key;

		return Localization.LocalizeText(newKey, list);
	}
	public string GetPersonalityText(string key, List<string> parameters)
	{
		List<Resources_Character> list = new List<Resources_Character>();
		list.Add(Resources_Player.instance);
		list.Add(shopkeeper);

		string newKey = "loc_prompt_";
		newKey += shopkeeper.personality.ToString().ToLower() + "_";
		newKey += shopkeeper.attitude.ToString().ToLower() + "_";
		newKey += key;
		
		return Localization.LocalizeText(newKey, list, parameters);
	}
	#endregion

	#region Utils
	public void StartDialogue(Sprite _playerPortrait, string _playerName, string _playerLastName)
	{
		DialogueInterface.Instance.shopkeeperRef = shopkeeper;
		if(firstVisitToday)
		{
			shopkeeper.respect++;
			firstVisitToday = false;
		}

		DialogueInterface.Instance.Activate ();
		DialogueInterface.Instance.DisplayLeftCharacter (_playerPortrait, 1.0f, false);
		DialogueInterface.Instance.DisplayRightCharacter (Utilities.GetSpriteFromID(shopkeeper.image), 2.5f, true);

		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString (), "", ""});
		DialogueInterface.currentBuilding = this;
		shopkeeper.UpdateAttitude ();

		//TODO: Check conditions! If all of them are false, then go to regular greeting. Or not.
		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting"), true, 1.25f, true);

	}

	private void UpdateValues(int _fear, int _respect)
	{
		shopkeeper.fear += _fear;
		Mathf.Clamp (shopkeeper.fear, 0, GameChart_Shopkeeper.Instance.maxFearValue);
		shopkeeper.respect += _respect;
		Mathf.Clamp (shopkeeper.respect, 0, GameChart_Shopkeeper.Instance.maxRespectValue);
		shopkeeper.UpdateAttitude ();
	}
	
	/// <summary>
	/// <<TEMP>> Calculates the threat level.
	/// </summary>
	/// <returns>The threat level.</returns>
	/// <param name="_fearMultiplier">_fear multiplier.</param>
	private int CalculateThreatLevel(float _fearMultiplier)
	{
		int value = 0;
		
		value = Mathf.RoundToInt((shopkeeper.fear * _fearMultiplier) - (shopkeeper.respect / _fearMultiplier));
		value = Mathf.Clamp (value, 0, 1000);
		
		return value;
	}
	
	/// <summary>
	/// <<TEMP>> Calculates the tolerance level.
	/// </summary>
	/// <returns>The tolerance level.</returns>
	/// <param name="_respectMultiplier">_respect multiplier.</param>
	private int CalculateToleranceLevel(float _respectMultiplier)
	{
		int value = 0;
		
		value = Mathf.RoundToInt((shopkeeper.respect * _respectMultiplier) - (shopkeeper.fear / (_respectMultiplier * 2.0f)));
		value = Mathf.Clamp (value, 0, 1000);
		
		return value;
	}
	
	/// <summary>
	/// <<TEMP>> Determines the intimidate reaction based on the threat and tolerance level, and on the type of intimidation (imply/threaten/act)
	/// </summary>
	/// <returns>The intimidate reaction.</returns>
	/// <param name="_threatLevel">_threat level.</param>
	/// <param name="_toleranceLevel">_tolerance level.</param>
	private string DetermineIntimidateReaction(int _threatLevel, int _toleranceLevel)
	{
		string prompt = "";
		
		int result = Mathf.RoundToInt(_threatLevel - _toleranceLevel);
		Debug.Log (result);
		int rando = UnityEngine.Random.Range (0, 100);
		if(rando > result)
		{
			prompt = "dialogue_prompt_intimidatedCallsPolice";
		}
		else if(rando < 0)
			prompt = "dialogue_prompt_intimidatedNoEffect";
		else
			prompt = "dialogue_prompt_intimidatedRattled";
		
		return prompt;
	}
	#endregion

	#region Event Handlers
	public void ApplyEventMethods()
	{
		DialogueInterface.Instance.OnAccept += HandleOnAccept;
		DialogueInterface.Instance.OnAcknowledge += HandleOnAcknowledge;
		DialogueInterface.Instance.OnCancelPurchase += HandleOnCancelPurchase;
		DialogueInterface.Instance.OnCheckTheRegister += HandleOnCheckTheRegister;
		DialogueInterface.Instance.OnChitChat += HandleOnChitChat;
		DialogueInterface.Instance.OnContinueIntimidating += HandleOnContinueIntimidating;
		DialogueInterface.Instance.OnConfirmPurchase += HandleOnConfirmPurchase;
		DialogueInterface.Instance.OnCutProtectionCost += HandleOnCutProtectionCost;
		DialogueInterface.Instance.OnDefaultAmount += HandleOnDefaultAmount;
		DialogueInterface.Instance.OnDonate += HandleOnDonate;
		DialogueInterface.Instance.OnEarlyPayment += HandleOnEarlyPayment;
		DialogueInterface.Instance.OnEnterShop += HandleOnEnterShop;
		DialogueInterface.Instance.OnExitShop += HandleOnExitShop;
		DialogueInterface.Instance.OnGoShopping += HandleOnGoShopping;
		DialogueInterface.Instance.OnGetDetails += HandleOnGetDetails;
		DialogueInterface.Instance.OnHearProposition += HandleOnHearProposition;
		DialogueInterface.Instance.OnIgnore += HandleOnIgnore;
		DialogueInterface.Instance.OnIntimidate += HandleOnIntimidate;
		DialogueInterface.Instance.OnIntimidate2Imply += HandleOnIntimidate2Imply;
		DialogueInterface.Instance.OnIntimidate2Threaten += HandleOnIntimidate2Threaten;
		DialogueInterface.Instance.OnIntimidate2Act += HandleOnIntimidate2Act;
		DialogueInterface.Instance.OnIntimidate3InformBoss += HandleOnIntimidate3InformBoss;
		DialogueInterface.Instance.OnIntimidate3BreakMerchandise += HandleOnIntimidate3BreakMerchandise;
		DialogueInterface.Instance.OnIntimidate3AttackShopkeeper += HandleOnIntimidate3AttackShopkeeper;
		DialogueInterface.Instance.OnIntimidate3BurnDownShop += HandleOnIntimidate3BurnDownShop;
		DialogueInterface.Instance.OnNeverMind += HandleOnNeverMind;
		DialogueInterface.Instance.OnOfferProtection += HandleOnOfferProtection;
		DialogueInterface.Instance.OnOfferToAidBusiness += HandleOnOfferToAidBusiness;
		DialogueInterface.Instance.OnOtherAmount += HandleOnOtherAmount;
		DialogueInterface.Instance.OnPlacate += HandleOnPlacate;
		DialogueInterface.Instance.OnReject += HandleOnReject;
		DialogueInterface.Instance.OnRenegotiate += HandleOnRenegotiate;
		DialogueInterface.Instance.OnRequestPayment += HandleOnRequestPayment;
		DialogueInterface.Instance.OnResumeTalking += HandleOnResumeTalking;
		DialogueInterface.Instance.OnReturnGreeting += HandleOnReturnGreeting;
		DialogueInterface.Instance.OnShopProduct += HandleOnShopProduct;
		DialogueInterface.Instance.OnTakeRegisterMoney += HandleOnTakeRegisterMoney;
		DialogueInterface.Instance.OnTryAnotherOffer += HandleOnTryAnotherOffer;
	}
	
	public void RemoveEventMethods()
	{
		DialogueInterface.Instance.HideAll ();
		DialogueInterface.Instance.Deactivate ();

		DialogueInterface.Instance.OnAccept -= HandleOnAccept;
		DialogueInterface.Instance.OnAcknowledge -= HandleOnAcknowledge;
		DialogueInterface.Instance.OnCancelPurchase -= HandleOnCancelPurchase;
		DialogueInterface.Instance.OnCheckTheRegister -= HandleOnCheckTheRegister;
		DialogueInterface.Instance.OnChitChat -= HandleOnChitChat;
		DialogueInterface.Instance.OnContinueIntimidating -= HandleOnContinueIntimidating;
		DialogueInterface.Instance.OnConfirmPurchase -= HandleOnConfirmPurchase;
		DialogueInterface.Instance.OnCutProtectionCost -= HandleOnCutProtectionCost;
		DialogueInterface.Instance.OnDefaultAmount -= HandleOnDefaultAmount;
		DialogueInterface.Instance.OnDonate -= HandleOnDonate;
		DialogueInterface.Instance.OnEarlyPayment -= HandleOnEarlyPayment;
		DialogueInterface.Instance.OnEnterShop -= HandleOnEnterShop;
		DialogueInterface.Instance.OnExitShop -= HandleOnExitShop;
		DialogueInterface.Instance.OnGoShopping -= HandleOnGoShopping;
		DialogueInterface.Instance.OnGetDetails -= HandleOnGetDetails;
		DialogueInterface.Instance.OnHearProposition -= HandleOnHearProposition;
		DialogueInterface.Instance.OnIgnore -= HandleOnIgnore;
		DialogueInterface.Instance.OnIntimidate -= HandleOnIntimidate;
		DialogueInterface.Instance.OnIntimidate2Imply -= HandleOnIntimidate2Imply;
		DialogueInterface.Instance.OnIntimidate2Threaten -= HandleOnIntimidate2Threaten;
		DialogueInterface.Instance.OnIntimidate2Act -= HandleOnIntimidate2Act;
		DialogueInterface.Instance.OnIntimidate3InformBoss -= HandleOnIntimidate3InformBoss;
		DialogueInterface.Instance.OnIntimidate3BreakMerchandise -= HandleOnIntimidate3BreakMerchandise;
		DialogueInterface.Instance.OnIntimidate3AttackShopkeeper -= HandleOnIntimidate3AttackShopkeeper;
		DialogueInterface.Instance.OnIntimidate3BurnDownShop -= HandleOnIntimidate3BurnDownShop;
		DialogueInterface.Instance.OnNeverMind -= HandleOnNeverMind;
		DialogueInterface.Instance.OnOfferProtection -= HandleOnOfferProtection;
		DialogueInterface.Instance.OnOfferToAidBusiness -= HandleOnOfferToAidBusiness;
		DialogueInterface.Instance.OnOtherAmount -= HandleOnOtherAmount;
		DialogueInterface.Instance.OnPlacate -= HandleOnPlacate;
		DialogueInterface.Instance.OnReject -= HandleOnReject;
		DialogueInterface.Instance.OnRenegotiate -= HandleOnRenegotiate;
		DialogueInterface.Instance.OnRequestPayment -= HandleOnRequestPayment;
		DialogueInterface.Instance.OnResumeTalking -= HandleOnResumeTalking;
		DialogueInterface.Instance.OnReturnGreeting -= HandleOnReturnGreeting;
		DialogueInterface.Instance.OnShopProduct -= HandleOnShopProduct;
		DialogueInterface.Instance.OnTakeRegisterMoney -= HandleOnTakeRegisterMoney;
		DialogueInterface.Instance.OnTryAnotherOffer -= HandleOnTryAnotherOffer;
	}

	#endregion

	private void HandleOnAccept () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Accept(), false, 0.0f, true);}
	private void HandleOnAcknowledge () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Acknowledge(), false, 0.0f, true);}
	private void HandleOnCancelPurchase () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CancelPurchase(), false, 0.0f, true);}
	private void HandleOnCheckTheRegister () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CheckTheRegister(), false, 0.0f, true);}
	private void HandleOnChitChat () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ChitChat(), false, 0.0f, true);}
	private void HandleOnContinueIntimidating () {			DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ContinueIntimidating(), false, 0.0f, true);}
	private void HandleOnConfirmPurchase () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ConfirmPurchase(Resources_Player.instance.money, DialogueInterface.Instance.GetItem ()), false, 0.0f, true);}
	private void HandleOnCutProtectionCost () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CutProtectionCost(), false, 0.0f, true);}
	private void HandleOnDefaultAmount () {					/*DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.DefaultAmount(Resources_Player.instance, shopkeeper, 200), false, 0.0f, true);*/} // TODO: add asking price
	private void HandleOnDonate () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Donate(), false, 0.0f, true);}
	private void HandleOnEarlyPayment () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.EarlyPayment(Resources_Player.instance, shopkeeper), false, 0.0f, true);}
	private void HandleOnEnterShop () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.EnterShop(), false, 0.0f, true);}
	private void HandleOnGoShopping () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.GoShopping(), false, 0.0f, true);}
	private void HandleOnGetDetails () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.GetDetails(), false, 0.0f, true);}
	private void HandleOnHearProposition () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.HearProposition(), false, 0.0f, true);}
	private void HandleOnIgnore () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Ignore(), false, 0.0f, true);}
	private void HandleOnIntimidate () { 					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate(), false, 0.0f, true);}
	private void HandleOnIntimidate2Imply () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate2Imply(), false, 0.0f, true);}
	private void HandleOnIntimidate2Threaten () {			DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate2Threaten(), false, 0.0f, true);}
	private void HandleOnIntimidate2Act () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate2Act(), false, 0.0f, true);}
	private void HandleOnIntimidate3InformBoss () {			DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate3InformBoss(), false, 0.0f, true);}
	private void HandleOnIntimidate3BreakMerchandise () {	DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate3BreakMerchandise(), false, 0.0f, true);}
	private void HandleOnIntimidate3AttackShopkeeper () {	DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate3AttackShopkeeper(), false, 0.0f, true);}
	private void HandleOnIntimidate3BurnDownShop () {		DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Intimidate3BurnDownShop(), false, 0.0f, true);}
	private void HandleOnNeverMind () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.NeverMind(), false, 0.0f, true);}
	private void HandleOnOfferProtection () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.OfferProtection(), false, 0.0f, false);}
	private void HandleOnOfferToAidBusiness () {			DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.OfferToAidBusiness(), false, 0.0f, true);}
	private void HandleOnOtherAmount () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.OtherAmount(Resources_Player.instance, shopkeeper, 0), false, 0.0f, true);} // TODO: add asking price
	private void HandleOnPlacate () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Placate(), false, 0.0f, true);}
	private void HandleOnReject () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Reject(), false, 0.0f, true);}
	private void HandleOnRenegotiate () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Renegotiate(), false, 0.0f, true);}
	private void HandleOnRequestPayment () { 				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.RequestPayment(Resources_Player.instance, shopkeeper), false, 0.0f, true);}
	private void HandleOnResumeTalking () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ResumeTalking(), false, 0.0f, true);}
	private void HandleOnReturnGreeting () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ReturnGreeting(), false, 0.0f, true);}
	private void HandleOnShopProduct () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ShopProduct(), false, 0.0f, true);}
	private void HandleOnTakeRegisterMoney () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.TakeRegisterMoney(Resources_Player.instance, shopkeeper, shopkeeper.home.money), false, 0.0f, true);}
	private void HandleOnTryAnotherOffer () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.TryAnotherOffer(), false, 0.0f, true);}

	private void HandleOnExitShop () {
		DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ExitShop(), false, 0.0f, false);
		PlayerMovement.CanExecuteAction (true);
		DialogueInterface.Instance.HideChoices();
	}
}