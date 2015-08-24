using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Dialogue_Prompt
{
	static private Dictionary<string, Dialogue_Prompt> _dialoguePrompts = new Dictionary<string, Dialogue_Prompt>();
	static public List<string> dialoguePrompts { get { return _dialoguePrompts.Keys.ToList(); } }
	static public Dialogue_Prompt GetPromptByName(string key) { return _dialoguePrompts[key]; }
	
	private List<Dialogue_Option> _followUps = new List<Dialogue_Option>();
	public List<Dialogue_Option> followUps { get { return _followUps; } }

	private List<string> _followUpKeys = new List<string> ();
	public List<string> followUpKeys{get{return _followUpKeys;}}

	private string _dialoguePromptID;
	public string promptID { get { return _dialoguePromptID; } }
	private string _suffix;
	public string suffix { get {return _suffix; } }
	
	public Dialogue_Prompt(string id, string suffix)
	{
		_dialoguePromptID = id;
		_dialoguePrompts.Add(id, this);
		_suffix = suffix;
	}
	
	static public void AddFollowUps()
	{
		for (int i = 0; i < dialoguePrompts.Count; i++)
		{
			string id = dialoguePrompts[i];
			switch(id)
			{
			case "dialogue_prompt_aidingBusiness": {
				Dialogue_Prompt_Logic.SetOptions_AidingBusiness(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_askToLowerPayment": {
				Dialogue_Prompt_Logic.SetOptions_AskToLowerPayment(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_assaultInitial": {
				Dialogue_Prompt_Logic.SetOptions_AssaultInitial(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_assaultSecondary": {
				Dialogue_Prompt_Logic.SetOptions_AssaultSecondary(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_confirmPurchase": {
				Dialogue_Prompt_Logic.SetOptions_ConfirmPurchase(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_giveDetails": {
				Dialogue_Prompt_Logic.SetOptions_GiveDetails(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_goodbye": {
				Dialogue_Prompt_Logic.SetOptions_Goodbye(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_greeting": {
				Dialogue_Prompt_Logic.SetOptions_Greeting(Dialogue_Prompt.GetPromptByName(id));
				break; }
				/*
			case "dialogue_prompt_intimidate": {
				Dialogue_Prompt_Logic.SetOptions_Intimidate(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_intimidate_action": {
				Dialogue_Prompt_Logic.SetOptions_Intimidate_Action(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_intimidatedCallsPolice": {
				Dialogue_Prompt_Logic.SetOptions_IntimidatedCallsPolice(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_intimidatedFightsBack": {
				Dialogue_Prompt_Logic.SetOptions_IntimidatedFightsBack(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_intimidatedRattled": {
				Dialogue_Prompt_Logic.SetOptions_IntimidatedRattled(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_intimidatedUnaffected": {
				Dialogue_Prompt_Logic.SetOptions_IntimidatedUnaffected(Dialogue_Prompt.GetPromptByName(id));
				break; }
				*/
			case "dialogue_prompt_offerAccepted": {
				Dialogue_Prompt_Logic.SetOptions_OfferAccepted(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_offerRefused": {
				Dialogue_Prompt_Logic.SetOptions_OfferRefused(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_outsideShop": {
				Dialogue_Prompt_Logic.SetOptions_OutsideShop(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_paymentFull": {
				Dialogue_Prompt_Logic.SetOptions_PaymentFull(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_paymentNone": {
				Dialogue_Prompt_Logic.SetOptions_PaymentNone(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_paymentPartial": {
				Dialogue_Prompt_Logic.SetOptions_PaymentPartial(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_placated": {
				Dialogue_Prompt_Logic.SetOptions_Placated(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_problemWithPayment": {
				Dialogue_Prompt_Logic.SetOptions_ProblemWithPayment(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_purchaseFailed": {
				Dialogue_Prompt_Logic.SetOptions_PurchaseFailed(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_purchaseSuccessful": {
				Dialogue_Prompt_Logic.SetOptions_PurchaseSuccessful(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_registerEmpty": {
				Dialogue_Prompt_Logic.SetOptions_RegisterEmpty(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_registerSomeMoney": {
				Dialogue_Prompt_Logic.SetOptions_RegisterSomeMoney(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_registerHiddenMoney": {
				Dialogue_Prompt_Logic.SetOptions_RegisterHiddenMoney(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_registerTakeOrLeaveMoney": {
				Dialogue_Prompt_Logic.SetOptions_RegisterTakeOrLeaveMoney(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_root": {
				Dialogue_Prompt_Logic.SetOptions_Root(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_schedulePayment": {
				Dialogue_Prompt_Logic.SetOptions_SchedulePayment(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_shopInventory": {
				Dialogue_Prompt_Logic.SetOptions_ShopInventory(Dialogue_Prompt.GetPromptByName(id));
				break; }
			case "dialogue_prompt_smallTalk": {
				Dialogue_Prompt_Logic.SetOptions_SmallTalk(Dialogue_Prompt.GetPromptByName(id));
				break; }
			default: {
				Debug.LogWarning("PROMPT NOT RECOGNIZED! " + id);
				break; }
			}
		}
	}

	public void AddFollowUp(Dialogue_Option option)
	{
		_followUps.Add (option);
		_followUpKeys.Add (option.id);
	}
}