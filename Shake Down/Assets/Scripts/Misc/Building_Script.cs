using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building_Script : MonoBehaviour
{
	#region Static Data / Design World Comparisson Stuff
	[SerializeField] private string _buildingID;
	static private Dictionary<string, Building_Script> _buildings = new Dictionary<string, Building_Script>();
	static public Building_Script GetBuilding(string buildingID) { return _buildings[buildingID]; }
	static private List<string> _buildingIDs = new List<string>();
	static public List<string> buildingIDs { get { return _buildingIDs; } }
	
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
	#endregion

	private Resources_Shopkeeper _shopkeeper;
	public Resources_Shopkeeper shopkeeper { 	get { return _shopkeeper; }	set { _shopkeeper = value; } }
	private Resources_Building _building;
	public Resources_Building building { 		get { return _building; } 	set { _building = value; } }

	private bool isGuilty = false;
	private bool isUnderProtection = false;

	//private Enums.IntimidateAction currentIntimidateAction = Enums.IntimidateAction.None;

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

		Debug.Log (shopkeeper.personality);

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

	/*
	private void HandleOnAskForPayment ()
	{
		// The register contains enough money to pay the protection, even without taking money from his pocket
		if(building.money >= building.payment)
		{
			//TODO: Does the shopkeep actually WANT to give you their money?
			building.money -= building.payment;
			Resources_Player.instance.money += building.payment;

			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"), false, 0.0f, true);
		}
		else
		{
			//TODO:
			//There isn't enough money in the register, so the shopkeeper has to reach in his own pocket
			if(building.money + shopkeeper.money >= building.payment)
			{
				shopkeeper.money -= (building.payment - building.money);
				building.money = 0;
				Resources_Player.instance.money += building.payment;

				DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"), false, 0.0f, true);
				return;
			}
			//the shopkeep cannot match the protection cost => partial payment
			if(building.money + shopkeeper.money > building.payment * 0.25f)
			{
				shopkeeper.money -= shopkeeper.money;
				building.money -= building.money;

				DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial"), false, 0.0f, true);
				
				return;
			}
			//Even by combining his money with the register's, the shopkeep can't even cover a fraction of the protection cost. He doesn't pay at all.
			else if(shopkeeper.money + building.money <= building.payment * 0.25f)
			{
				DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone"), false, 0.0f, true);
				
				return;
			}
		}
	}
	
	private void HandleOnAccept ()
	{
		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"), false, 0.0f, true);

		DialogueInterface.Instance.DisplayRightCharacter (Utilities.GetSpriteFromID(shopkeeper.image), 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString(), "", ""});
	}
	
	private void HandleOnCheckRegister ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (DialogueInterface.Instance.cashRegisterPortrait, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Cash", "Register", building.money.ToString(), "", ""});
		
		//There is enough in the register alone to pay for the protection and more. Clearly the shopkeep was lying about not having enough money.
		if(building.money >= building.payment)
		{
			isGuilty = true;
			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerHiddenMoney"), false, 0.0f, true);

			return;
			//TODO: Does the shopkeeper have a reason to keep that money? (Investment, family member in the hospital...)
		}
		
		//There is money in the register, but not enough to pay for protection.
		//Leaving it there may help the store grow, but clearly the shopkeep was lying about not having anything left.
		if(building.money < building.payment && building.money >= building.payment * 0.25f)
		{
			isGuilty = true;
			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerSomeMoney"), true, 1.25f, true);

			return;
		}
		
		//There is close to nothing in the register. The shopkeep wasn't lying.
		if(building.money < building.payment * 0.25f)
		{
			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerEmpty"), false, 0.0f, true);

			return;
		}
	}
	private void HandleOnTakeRegisterMoney()
	{
		DialogueInterface.Instance.DisplayRightCharacter (Utilities.GetSpriteFromID(shopkeeper.image), 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString(), "", ""});

		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerTakeOrLeaveMoney"), false, 0.0f, true);
	}
	
	private void HandleOnOfferToAidBusiness ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (Utilities.GetSpriteFromID(shopkeeper.image), 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString(), "", ""});

		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_aidingBusiness"), false, 0.0f, true);
	}
	
	private void HandleOnDonateConfirm ()
	{
		int donatedAmount = DialogueInterface.Instance.GetDigitSelectorValue ();
		//TODO:CHECK if the player can afford it, and if yes deduce he amount from it!
		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting"), false, 0.0f, true);

		if (donatedAmount <= 5)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Are you trying to mess with me, sir?", true);
			return;
		}
		if (donatedAmount < building.payment * 0.25f)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Erm... thanks, I guess. Sir.", true);
			return;
		}
		else if (donatedAmount < building.payment * 0.5f)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Thank you, sir. It's better than nothing.", true);
			return;
		}
		else if (donatedAmount <= building.payment)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Thank you so much sir. This money will definitely help us stay afloat.", true);
			return;
		}
		else if (donatedAmount < 1000)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "With this money, we'll get back into business again! Thank you so much, sir!", true);
			return;
		}
		else if (donatedAmount >= 1000)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Are... are you sure, sir? That is... that is a lot of money! Thank you so very much!", true);
			return;

		shopkeeper.money += donatedAmount;
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString(), "", ""});
	}
	private void HandleOnDonateCancel ()
	{
		
	}
	private void HandleOnCutProtectionConfirm ()
	{
		
	}
	private void HandleOnCutProtectionCancel ()
	{
		
	}
	
	private void HandleOnIntimidate ()
	{
		if(isGuilty)
		{
			//TODO: different answers depending on shopkeep's mood & reason to lie to the player
			//DialogueInterface.Instance.StartDialogue (false, 0.0f, "No, please! I won't do it anymore!", true);
			isGuilty = false;
		}
		else
			//DialogueInterface.Instance.StartDialogue (false, 0.0f, "...", true);
			;
	}

	private void HandleOnIntimidateInformBoss()
	{
		//TODO: Calculations to determine the shopkeeper's reaction
		shopkeeper.UpdateAttitude ();
		
		int threatLevel = CalculateThreatLevel(1), toleranceLevel = CalculateToleranceLevel(4);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";

		switch (currentAttitude)
		{
		case Enums.ShopkeeperAttitude.Neutral:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case Enums.ShopkeeperAttitude.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case Enums.ShopkeeperAttitude.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case Enums.ShopkeeperAttitude.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}


		//DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		//DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLineVariation, true);
	}
	
	private void HandleOnIntimidateBreakMerchandise()
	{
		shopkeeper.UpdateAttitude ();
		
		int threatLevel = CalculateThreatLevel(2), toleranceLevel = CalculateToleranceLevel(3);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";


		switch (currentAttitude)
		{
		case Enums.ShopkeeperAttitude.Neutral:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case Enums.ShopkeeperAttitude.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case Enums.ShopkeeperAttitude.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case Enums.ShopkeeperAttitude.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}
		
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false,0.0f, dialogueLineVariation, true);

	}
	
	private void HandleOnIntimidateAttackShopkeeper()
	{
		shopkeeper.UpdateAttitude ();
		
		int threatLevel = CalculateThreatLevel(3), toleranceLevel = CalculateToleranceLevel(2);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";

		switch (currentAttitude)
		{
		case Enums.ShopkeeperAttitude.Neutral:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case Enums.ShopkeeperAttitude.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case Enums.ShopkeeperAttitude.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case Enums.ShopkeeperAttitude.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}
		
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLineVariation, true);
	}
	
	private void HandleOnIntimidateBurnDownShop()
	{
		shopkeeper.UpdateAttitude ();
		
		int threatLevel = CalculateThreatLevel(4), toleranceLevel = CalculateToleranceLevel(1);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";
		
		switch (currentAttitude)
		{
		case Enums.ShopkeeperAttitude.Neutral:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case Enums.ShopkeeperAttitude.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case Enums.ShopkeeperAttitude.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case Enums.ShopkeeperAttitude.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case Enums.ShopkeeperAttitude.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}
		
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLineVariation, true);
	}
	
	private void HandleOnOfferProtection ()
	{
		
	}
	
	private void HandleOnRenegotiateProtection ()
	{
		
	}
	
	private void HandleOnGoShopping ()
	{
		
	}
	private void HandleOnConfirmPurchase ()
	{
		string prompt = "", dialogueLine = "";
		
		//TODO: Get the actual cost of the object
		if(Manager_Resources.player.money > 50)
		{
			prompt = "dialogue_prompt_purchaseSuccessful";
			dialogueLine = "Thank you for your patronnage! Do you need anything else?";
			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseSuccessful"), false, 0.0f, true);
		}
		else
		{
			prompt = "dialogue_prompt_purchaseFailed";
			dialogueLine = "A little short, sir? No problems, pick something else!";
			DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseFailed"), false, 0.0f, true);
		}

		//DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		//DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLine, true);
	}
	
	private void HandleOnChitChat ()
	{
		Debug.Log ("HANDLE ON CHIT CHAT CALLED!");
		//TODO: Different chit chat depending on how the store is doing. We want this to subtly tell the player if the store is facing difficulties
		UpdateValues (0, 5);
		DialogueInterface.Instance.NewPrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_smallTalk"), false, 0.0f, true);
	}
	
	private void HandleOnExitShop ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (Utilities.GetSpriteFromID(shopkeeper.image), 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopkeeper.money.ToString(), "", ""});
		PlayerMovement.CanExecuteAction (true);
		DialogueInterface.Instance.HideChoices();
	}*/
	#endregion

	private void HandleOnAccept () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Accept(), false, 0.0f, true);}
	private void HandleOnAcknowledge () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Acknowledge(), false, 0.0f, true);}
	private void HandleOnCancelPurchase () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CancelPurchase(), false, 0.0f, true);}
	private void HandleOnCheckTheRegister () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CheckTheRegister(), false, 0.0f, true);}
	private void HandleOnChitChat () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ChitChat(), false, 0.0f, true);}
	private void HandleOnContinueIntimidating () {			DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ContinueIntimidating(), false, 0.0f, true);}
	private void HandleOnConfirmPurchase () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.ConfirmPurchase(Resources_Player.instance.money, 0), false, 0.0f, true);} // TODO: add item price
	private void HandleOnCutProtectionCost () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.CutProtectionCost(), false, 0.0f, true);}
	private void HandleOnDefaultAmount () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.DefaultAmount(Resources_Player.instance, shopkeeper, 200), false, 0.0f, true);} // TODO: add asking price
	private void HandleOnDonate () {						DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.Donate(), false, 0.0f, true);}
	private void HandleOnEarlyPayment () {					DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.EarlyPayment(Resources_Player.instance, shopkeeper), false, 0.0f, true);} // TODO: add asking price
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
	private void HandleOnOfferProtection () {				DialogueInterface.Instance.NewPrompt(Dialogue_Option_Logic.OfferProtection(), false, 0.0f, true);}
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