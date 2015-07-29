using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Dialogue_Option
{
	static private Dictionary<string, Dialogue_Option> _dialogueOptions = new Dictionary<string, Dialogue_Option>();
	static public List<string> dialogueOptions { get { return _dialogueOptions.Keys.ToList(); } }
	static public Dialogue_Option GetOptionByName(string key) { return _dialogueOptions[key]; }
	
	private List<Dialogue_Prompt> _followUps = new List<Dialogue_Prompt>();
	public List<Dialogue_Prompt> followUps { get { return SelectFollowUp(_followUps); } }
	
	private string _dialogueOptionID;
	public string optionID { get { return _dialogueOptionID; } }
	
	public Dialogue_Option(string id)
	{
		_dialogueOptionID = id;
		_dialogueOptions.Add(id, this);
	}

	static public void AddFollowUps()
	{
		for (int i = 0; i < dialogueOptions.Count; i++)
		{
			string id = dialogueOptions[i];
			switch(id)
			{
				case "dialogue_option_acceptPayment": {
					Dialogue_Option_Logic.SetPrompts_AcceptPayment(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_acceptProposition": {
					Dialogue_Option_Logic.SetPrompts_AcceptProposition(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_acknowledge": { 
					Dialogue_Option_Logic.SetPrompts_Acknowledge(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_cancelPurchase": { 
					Dialogue_Option_Logic.SetPrompts_CancelPurchase(Dialogue_Option.GetOptionByName(id));				
					break; }
				case "dialogue_option_checkTheRegister": { 
					Dialogue_Option_Logic.SetPrompts_CheckTheRegister(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_chitChat": {
					Dialogue_Option_Logic.SetPrompts_ChitChat(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_confirmPurchase": { 
					Dialogue_Option_Logic.SetPrompts_ConfirmPurchase(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_continueIntimidating": { 
					Dialogue_Option_Logic.SetPrompts_ContinueIntimidating(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_earlyPayment": { 
					Dialogue_Option_Logic.SetPrompts_EarlyPayment(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_enterShop": { 
					Dialogue_Option_Logic.SetPrompts_EnterShop(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_exitShop": { 
					Dialogue_Option_Logic.SetPrompts_ExitShop(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_getDetails": { 
					Dialogue_Option_Logic.SetPrompts_GetDetails(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_goShopping": { 
					Dialogue_Option_Logic.SetPrompts_GoShopping(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_hearProposition": { 
					Dialogue_Option_Logic.SetPrompts_HearProposition(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_ignore": { 
					Dialogue_Option_Logic.SetPrompts_Ignore(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate2_Act": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate2_Act(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate2_Imply": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate2_Imply(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate2_Threaten": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate2_Threaten(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate3_AttackShopkeeper": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate3_AttackShopkeeper(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate3_BreakMerchandise": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate3_BreakMerchandise(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate3_BurnDownShop": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate3_BurnDownShop(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_intimidate3_InformBoss": { 
					Dialogue_Option_Logic.SetPrompts_Intimidate3_InformBoss(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_neverMind": { 
					Dialogue_Option_Logic.SetPrompts_NeverMind(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_offerProtection": { 
					Dialogue_Option_Logic.SetPrompts_OfferProtection(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_offerToAidBusiness": { 
					Dialogue_Option_Logic.SetPrompts_OfferAidToBusiness(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_otherAmount": { 
					Dialogue_Option_Logic.SetPrompts_OtherAmount(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_placate": { 
					Dialogue_Option_Logic.SetPrompts_Placate(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_defaultAmount": { 
					Dialogue_Option_Logic.SetPrompts_DefaultAmount(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_rejectProposition": { 
					Dialogue_Option_Logic.SetPrompts_RejectProposition(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_renegotiate": { 
					Dialogue_Option_Logic.SetPrompts_Renegotiate(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_requestPayment": { 
					Dialogue_Option_Logic.SetPrompts_RequestPayment(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_resumeTalking": { 
					Dialogue_Option_Logic.SetPrompts_ResumeTalking(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_returnGreeting": { 
					Dialogue_Option_Logic.SetPrompts_ReturnGreeting(Dialogue_Option.GetOptionByName(id));
					break; }
				case "dialogue_option_shopProduct": { 
					Dialogue_Option_Logic.SetPrompts_ShopProduct(Dialogue_Option.GetOptionByName(id));
					break; }
			}
		}
	}

	
	public void AddFollowUp(Dialogue_Prompt prompts)
	{
		_followUps.Add (prompts);
	}

	private Dialogue_Prompt SelectFollowUp (List<Dialogue_Prompt> baseList)
	{
		Dialogue_Prompt followUp;
		string promptID;
		switch (optionID) {
			case "dialogue_option_enterShop": {
				promptID = DeterminePrompt_FirstEntry();
				break; }
			case "dialogue_option_requestPayment":
			case "dialogue_option_earlyPayment": {
				promptID = DeterminePrompt_GetPayment();
				break; }
			case "dialogue_option_defaultAmount":
			case "dialogue_option_otherAmount": {
				promptID = DeterminePrompt_Offer();
				break; }
			case "dialogue_option_intimidate": {
				promptID = DeterminePrompt_Intimidate();
				break; }
			case "dialogue_option_confirmPurchase": {
				promptID = DeterminePrompt_Purchase();
				break; }
			}
		return promptID;
	}

	private string DeterminePrompt_FirstEntry()
	{
		// TODO: Logic for Shopkeeper Assault State vs Shopkeeper Money/Profit vs Shopkeeper Respect
		return "dialogue_prompt_greeting";
		return "dialogue_prompt_assaultInitial";
		return "dialogue_prompt_assaultSecondary";
		return "dialogue_prompt_problemWithPayment";
		return "dialogue_prompt_paymentFull";
		return "dialogue_prompt_paymentPartial";
		return "dialogue_prompt_paymentNone";
	}

	private string DeterminePrompt_GetPayment()
	{
		// TODO: Logic for Shopkeeper Money/Profit vs Shopkeeper Fear/Respect
		return "dialogue_prompt_paymentFull";
		return "dialogue_prompt_paymentPartial";
		return "dialogue_prompt_paymentNone";
	}

	private string DeterminePrompt_Offer()
	{
		// TODO: Logic for Player Strength/Presence vs Shopkeeper Money/Profit/Respect
		return "dialogue_prompt_offerAccepted";
		return "dialogue_prompt_offerRefused";
	}

	private string DeterminePrompt_Intimidate()
	{
		// TODO: Logic for Player Strength/Presence vs Shopkeeper Strength/Fear/Respect
		return "dialogue_prompt_intimidatedCallsPolice";
		return "dialogue_prompt_intimidatedFightsBack";
		return "dialogue_prompt_intimidatedNoEffect";
		return "dialogue_prompt_intimidatedRattled";
	}

	private string DeterminePrompt_Purchase()
	{
		// TODO: Logic for Player Money vs Shop Inventory Price
		if(Manager_Resources.player.money > 50)
		{
			return "dialogue_prompt_purchaseSuccessful";
		}
		return "dialogue_prompt_purchaseFailed";
	}
}