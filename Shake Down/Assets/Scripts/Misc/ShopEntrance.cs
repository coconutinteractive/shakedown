using UnityEngine;
using System.Collections;

public class ShopEntrance : MonoBehaviour 
{
	public enum ShopkeeperState
	{
		Normal,
		Disdain,
		Neutral,
		Terror,
		Admiration, 
		Awe,
		High_Respect,
		High_Fear
	}

	private ShopkeeperState currentState = ShopkeeperState.Normal;

	public enum IntimidateAction
	{
		None, 
		Imply = 2,
		Threaten = 3,
		Act = 4
	}

	private IntimidateAction currentIntimidateAction = IntimidateAction.None;

	[SerializeField] private int initialFearValue = 0, initialRespectValue = 0;

	private int currentFearValue = 0;
	private int currentRespectValue = 0;

	[SerializeField] private Sprite shopKeepPortrait = null, cashRegisterPortrait = null;

	private int shopKeepMoney = 0;
	private int registerMoney = 0;
	private int protectionCost = 100;

	// Flags
	private bool hasMetPlayer = false, hasSeenPlayerToday = false;
	private bool isGuilty = false;
	private bool isUnderProtection = false;

	private void Start()
	{
		shopKeepMoney = UnityEngine.Random.Range (0, 200);
		registerMoney = UnityEngine.Random.Range (0, 200);
		int[] values = GameChart_Shopkeeper.Instance.GetPosInArea (GameChart_Shopkeeper.Areas.Neutral);
		currentFearValue = values [0] + initialFearValue;
		currentRespectValue = values [1] + initialRespectValue;
	}

	public void Initialize()
	{
		DialogueInterface.Instance.OnIntimidate += HandleOnIntimidate;
		DialogueInterface.Instance.OnIntimidateImply += HandleOnIntimidateImply;
		DialogueInterface.Instance.OnIntimidateThreaten += HandleOnIntimidateThreaten;
		DialogueInterface.Instance.OnIntimidateAct += HandleOnIntimidateAct;
		DialogueInterface.Instance.OnIntimidateInformBoss += HandleOnIntimidateInformBoss;
		DialogueInterface.Instance.OnIntimidateBreakMerchandise += HandleOnIntimidateBreakMerchandise;
		DialogueInterface.Instance.OnIntimidateAttackShopkeeper += HandleOnIntimidateAttackShopkeeper;
		DialogueInterface.Instance.OnIntimidateBurnShopDown += HandleOnIntimidateBurnShopDown;
		
		DialogueInterface.Instance.OnAskForPayment += HandleOnAskForPayment;
		DialogueInterface.Instance.OnAcceptPayment += HandleOnAcceptPayment;
		DialogueInterface.Instance.OnCheckRegister += HandleOnCheckRegister;
		DialogueInterface.Instance.OnTakeRegisterMoney += HandleOnTakeRegisterMoney;
		DialogueInterface.Instance.OnOfferToAidBusiness += HandleOnOfferToAidBusiness;
		DialogueInterface.Instance.OnDonateConfirm += HandleOnDonateConfirm;
		DialogueInterface.Instance.OnDonateCancel += HandleOnDonateCancel;
		DialogueInterface.Instance.OnCutProtectionConfirm += HandleOnCutProtectionConfirm;
		DialogueInterface.Instance.OnCutProtectionCancel += HandleOnCutProtectionCancel;
		

		DialogueInterface.Instance.OnOfferProtection += HandleOnOfferProtection;
		DialogueInterface.Instance.OnRenegotiateProtection += HandleOnRenegotiateProtection;

		DialogueInterface.Instance.OnGoShopping += HandleOnGoShopping;
		DialogueInterface.Instance.OnConfirmPurchase += HandleOnConfirmPurchase;;
		
		DialogueInterface.Instance.OnChitChat += HandleOnChitChat;

		DialogueInterface.Instance.OnExitShop += HandleOnExitShop;

		currentIntimidateAction = IntimidateAction.None;
	}


	
	public void StartDialogue(Sprite _playerPortrait, string _playerName, string _playerLastName)
	{
		DialogueInterface.Instance.Activate ();
		DialogueInterface.Instance.DisplayLeftCharacter (_playerPortrait, ElementState.Normal, 1.0f, false);
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 2.5f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString (), "", ""});

		//TODO: Check conditions! If all of them are false, then go to regular greeting. Or not.
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting"));

		UpdateState ();
		string dialogueLineVariation = "";

		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = hasSeenPlayerToday ? "Hello again sir!\n" : "Hello sir, \n" + Dialogues_Lines.SHOPKEEP_GREETINGS_NORMAL;
			break;
		case ShopkeeperState.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_DISDAIN;
			break;
		case ShopkeeperState.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_TERROR;
			break;
		case ShopkeeperState.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_ADMIRATION;
			break;
		case ShopkeeperState.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_AWE;
			break;
		case ShopkeeperState.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_HIGHFEAR;
			break;
		case ShopkeeperState.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_HIGHRESPECT;
			break;
		default:
			break;
		}

		DialogueInterface.Instance.StartDialogue (true, 1.25f, dialogueLineVariation, true);
		hasMetPlayer = true;
		hasSeenPlayerToday = true;
	}

	public void EndDialogue()
	{
		DialogueInterface.Instance.HideAll ();
		DialogueInterface.Instance.Deactivate ();
		DialogueInterface.Instance.OnIntimidate -= HandleOnIntimidate;
		DialogueInterface.Instance.OnIntimidateImply -= HandleOnIntimidateImply;
		DialogueInterface.Instance.OnIntimidateThreaten -= HandleOnIntimidateThreaten;
		DialogueInterface.Instance.OnIntimidateAct -= HandleOnIntimidateAct;
		DialogueInterface.Instance.OnIntimidateInformBoss -= HandleOnIntimidateInformBoss;
		DialogueInterface.Instance.OnIntimidateBreakMerchandise -= HandleOnIntimidateBreakMerchandise;
		DialogueInterface.Instance.OnIntimidateAttackShopkeeper -= HandleOnIntimidateAttackShopkeeper;
		DialogueInterface.Instance.OnIntimidateBurnShopDown -= HandleOnIntimidateBurnShopDown;

		DialogueInterface.Instance.OnAskForPayment -= HandleOnAskForPayment;
		DialogueInterface.Instance.OnAcceptPayment -= HandleOnAcceptPayment;
		DialogueInterface.Instance.OnCheckRegister -= HandleOnCheckRegister;
		DialogueInterface.Instance.OnTakeRegisterMoney -= HandleOnTakeRegisterMoney;
		DialogueInterface.Instance.OnOfferToAidBusiness -= HandleOnOfferToAidBusiness;
		DialogueInterface.Instance.OnDonateConfirm -= HandleOnDonateConfirm;
		DialogueInterface.Instance.OnDonateCancel -= HandleOnDonateCancel;
		DialogueInterface.Instance.OnCutProtectionConfirm -= HandleOnCutProtectionConfirm;
		DialogueInterface.Instance.OnCutProtectionCancel -= HandleOnCutProtectionCancel;

		DialogueInterface.Instance.OnOfferProtection -= HandleOnOfferProtection;
		DialogueInterface.Instance.OnRenegotiateProtection -= HandleOnRenegotiateProtection;

		DialogueInterface.Instance.OnGoShopping -= HandleOnGoShopping;
		DialogueInterface.Instance.OnConfirmPurchase -= HandleOnConfirmPurchase;

		DialogueInterface.Instance.OnChitChat -= HandleOnChitChat;

		DialogueInterface.Instance.OnExitShop -= HandleOnExitShop;

		currentIntimidateAction = IntimidateAction.None;
	}

	private void UpdateState()
	{
		GameChart_Shopkeeper.Areas area = GameChart_Shopkeeper.Instance.CheckForAreas (currentFearValue, currentRespectValue); 

		if(area.Equals(GameChart_Shopkeeper.Areas.Normal))
		{
			if(currentFearValue > GameChart_Shopkeeper.Instance.maxFearValue * 0.5f)
			{
				if(currentFearValue > currentRespectValue)
				{
					currentState = ShopkeeperState.High_Fear;
					return;
				}
			}
			else if(currentRespectValue > GameChart_Shopkeeper.Instance.maxRespectValue * 0.5f)
			{
				if(currentRespectValue > currentFearValue)
				{
					currentState = ShopkeeperState.High_Respect;
					return;
				}
			}
		}
		else
		{
			int state = (int)area;
			currentState = (ShopkeeperState)state;
		}
	}

	private void UpdateValues(int _fear, int _respect)
	{
		currentFearValue += _fear;
		Mathf.Clamp (currentFearValue, 0, GameChart_Shopkeeper.Instance.maxFearValue);
		currentRespectValue += _respect;
		Mathf.Clamp (currentRespectValue, 0, GameChart_Shopkeeper.Instance.maxRespectValue);

		UpdateState ();
	}

	/// <summary>
	/// <<TEMP>> Calculates the threat level.
	/// </summary>
	/// <returns>The threat level.</returns>
	/// <param name="_fearMultiplier">_fear multiplier.</param>
	private int CalculateThreatLevel(float _fearMultiplier)
	{
		int value = 0;

		value = Mathf.RoundToInt((currentFearValue * _fearMultiplier) - (currentRespectValue / _fearMultiplier));
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

		value = Mathf.RoundToInt((currentRespectValue * _respectMultiplier) - (currentFearValue / (_respectMultiplier * 2.0f)));
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

		int result = Mathf.RoundToInt(_threatLevel * (int)currentIntimidateAction - _toleranceLevel);
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

	#region Handlers!~

	private void HandleOnAskForPayment ()
	{
		// The shopkeep has enough on him to pay the protection, even without taking money from the register
		if(shopKeepMoney >= protectionCost)
		{
			//TODO: Does the shopkeep actually WANT to give you their money?
			shopKeepMoney -= protectionCost;
			registerMoney += shopKeepMoney;
			shopKeepMoney = 0;
			DialogueInterface.Instance.SetDialoguePrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"));
			DialogueInterface.Instance.StartDialogue (false, 0.0f, Dialogues_Lines.SHOPKEEP_ASKFORPAYMENT_FULL, true);
		}
		else
		{
			//The shopkeep doesn't have enough on him, and goes to the register to match the protection cost
			if(shopKeepMoney + registerMoney >= protectionCost)
			{
				registerMoney -= (protectionCost - shopKeepMoney);
				shopKeepMoney = 0;
				DialogueInterface.Instance.SetDialoguePrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"));
				DialogueInterface.Instance.StartDialogue (false, 0.0f, "I barely made it this week, but here it is.\n" + Dialogues_Lines.SHOPKEEP_ASKFORPAYMENT_FULL, true);
				return;
			}
			//The shopkeep doesn't have enough on him, and can't match the protection cost eve5n after taking the register money. => partial payment
			if(registerMoney + shopKeepMoney > protectionCost * 0.25f)
			{
				shopKeepMoney -= shopKeepMoney;
				registerMoney -= registerMoney;
				DialogueInterface.Instance.SetDialoguePrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial"));
				DialogueInterface.Instance.StartDialogue (false, 0.0f, Dialogues_Lines.SHOPKEEP_ASKFORPAYMENT_PARTIAL, true);
				return;
			}
			//Even by combining his money with the register's, the shopkeep can't even cover a fraction of the protection cost. He doesn't pay at all.
			else if(shopKeepMoney + registerMoney <= protectionCost * 0.25f)
			{
				DialogueInterface.Instance.SetDialoguePrompt(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone"));
				DialogueInterface.Instance.StartDialogue (false, 0.0f, Dialogues_Lines.SHOPKEEP_ASKFORPAYMENT_NONE_BROKE, true);
				return;
			}
		}
	}

	private void HandleOnAcceptPayment ()
	{
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting"));
		DialogueInterface.Instance.StartDialogue(false, 0.0f, "Will that be everything, sir?", true);
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString(), "", ""});
	}

	private void HandleOnCheckRegister ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (cashRegisterPortrait, ElementState.Normal, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Cash", "Register", registerMoney.ToString(), "", ""});

		//There is enough in the register alone to pay for the protection and more. Clearly the shopkeep was lying about not having enough money.
		if(registerMoney >= protectionCost)
		{
			isGuilty = true;
			DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerHiddenMoney"));
			//TODO: Random chance to find the hidden stash?
			DialogueInterface.Instance.StartDialogue (true, 1.25f, "< The register seems empty at first. But upon closer inspection, you find a hidden stash in a secret compartment. There is quite a bit in there. >", true);

			return;
			//TODO: Does the shopkeeper have a reason to keep that money? (Investment, family member in the hospital...)
		}

		//There is money in the register, but not enough to pay for protection.
		//Leaving it there may help the store grow, but clearly the shopkeep was lying about not having anything left.
		if(registerMoney < protectionCost && registerMoney >= protectionCost * 0.25f)
		{
			isGuilty = true;
			DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerSomeMoney"));
			DialogueInterface.Instance.StartDialogue (true, 1.25f, "< There is some money in there, but not much. Business must not have been great lately. >" , true);
			return;
		}

		//There is close to nothing in the register. The shopkeep wasn't lying.
		if(registerMoney < protectionCost * 0.25f)
		{
			DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName("dialogue_prompt_registerEmpty"));
			DialogueInterface.Instance.StartDialogue (true, 1.25f, "< There is close to nothing in the register. Clearly the store is facing difficulties. >" , true);
			return;
		}
	}
	private void HandleOnTakeRegisterMoney()
	{
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString(), "", ""});
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName ("dialogue_prompt_registerTakeOrLeaveMoney"));

		//TODO: Have the shopkeep explain why they were keeping money from the player
		DialogueInterface.Instance.StartDialogue (true, 0.75f, "Uhh... I'm so sorry sir! I was just trying to save a little money on the side! Please forgive me!", true);
	}

	private void HandleOnOfferToAidBusiness ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString(), "", ""});
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName ("dialogue_prompt_aidingBusiness"));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, "...", false);
		DialogueInterface.Instance.StartDialogue (true, 1.5f, "You would? That sure would be a great help!", true);
	}
	private void HandleOnDonateConfirm ()
	{
		int donatedAmount = DialogueInterface.Instance.GetDigitSelectorValue ();
		//TODO:CHECK if the player can afford it, and if yes deduce he amount from it!
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName ("dialogue_prompt_greeting"));

		if (donatedAmount <= 5)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Are you trying to mess with me, sir?", true);
			return;
		}
		if (donatedAmount < protectionCost * 0.25f)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Erm... thanks, I guess. Sir.", true);
			return;
		}
		else if (donatedAmount < protectionCost * 0.5f)
		{
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "Thank you, sir. It's better than nothing.", true);
			return;
		}
		else if (donatedAmount <= protectionCost)
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
		}
			
		shopKeepMoney += donatedAmount;
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString(), "", ""});
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
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "No, please! I won't do it anymore!", true);
			isGuilty = false;
		}
		else
			DialogueInterface.Instance.StartDialogue (false, 0.0f, "...", true);
	}

	private void HandleOnIntimidateImply ()
	{
		currentIntimidateAction = IntimidateAction.Imply;
	}

	private void HandleOnIntimidateThreaten ()
	{
		currentIntimidateAction = IntimidateAction.Threaten;
	}

	private void HandleOnIntimidateAct ()
	{
		currentIntimidateAction = IntimidateAction.Act;
	}

	private void HandleOnIntimidateInformBoss()
	{
		//TODO: Calculations to determine the shopkeeper's reaction
		UpdateState ();

		int threatLevel = CalculateThreatLevel(1), toleranceLevel = CalculateToleranceLevel(4);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";

		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case ShopkeeperState.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case ShopkeeperState.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case ShopkeeperState.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLineVariation, true);
	}
	
	private void HandleOnIntimidateBreakMerchandise()
	{
		UpdateState ();
		
		int threatLevel = CalculateThreatLevel(2), toleranceLevel = CalculateToleranceLevel(3);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";
		
		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case ShopkeeperState.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case ShopkeeperState.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case ShopkeeperState.High_Respect:
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
		UpdateState ();
		
		int threatLevel = CalculateThreatLevel(3), toleranceLevel = CalculateToleranceLevel(2);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";
		
		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case ShopkeeperState.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case ShopkeeperState.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case ShopkeeperState.High_Respect:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		default:
			break;
		}
		
		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLineVariation, true);
	}

	private void HandleOnIntimidateBurnShopDown()
	{
		UpdateState ();
		
		int threatLevel = CalculateThreatLevel(4), toleranceLevel = CalculateToleranceLevel(1);
		string prompt = DetermineIntimidateReaction(threatLevel, toleranceLevel);
		string dialogueLineVariation = "";
		
		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Disdain:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_FIGHTSBACK;
			break;
		case ShopkeeperState.Terror:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.Admiration:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_RATTLED;
			break;
		case ShopkeeperState.Awe:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED;
			break;
		case ShopkeeperState.High_Fear:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_INTIMIDATE_CALLSPOLICE;
			break;
		case ShopkeeperState.High_Respect:
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
		}
		else
		{
			prompt = "dialogue_prompt_purchaseFailed";
			dialogueLine = "A little short, sir? No problems, pick something else!";
		}

		DialogueInterface.Instance.SetDialoguePrompt (Dialogue_Prompt.GetPromptByName (prompt));
		DialogueInterface.Instance.StartDialogue (false, 0.0f, dialogueLine, true);
	}

	private void HandleOnChitChat ()
	{
		//TODO: Different chit chat depending on how the store is doing. We want this to subtly tell the player if the store is facing difficulties
		UpdateValues (0, 5);
		DialogueInterface.Instance.StartDialogue(false, 0.0f, "Oh my good sir!\nThings are going all right! The daughter just started high school " +
			"and is already the first of her class! Also, my cousin is visiting us, so I'm having her watch over the little boy while I am running the store. \n" +
            "Damn that is some fine chit chat.", true);
	}
	
	private void HandleOnExitShop ()
	{
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 1.0f, true);
		DialogueInterface.Instance.UpdateInfoValues (false, new string[5]{"Shop", "Keeper", shopKeepMoney.ToString(), "", ""});
		DialogueInterface.Instance.StartDialogue (false, 0.0f, Dialogues_Lines.SHOPKEEP_GOODBYE_TERROR, false);
		PlayerMovement.CanExecuteAction (true);
		DialogueInterface.Instance.HideChoices();
	}
	#endregion
}
