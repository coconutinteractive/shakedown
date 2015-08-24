using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Dialogue_Option
{
	static private Dictionary<string, Dialogue_Option> _dialogueOptions = new Dictionary<string, Dialogue_Option>();
	static private Dictionary<string, Dialogue_Option> dialogueOptions { get { return _dialogueOptions; } }
	static public Dialogue_Option GetOptionByName(string key) { return _dialogueOptions[key]; }
	
	private string _dialogueOptionID;
	public string id { get { return _dialogueOptionID; } }

	private string _buttonTextKey;
	public string buttonTextKey { get { return _buttonTextKey; } }
	
	public Dialogue_Option(string id, string textKey)
	{
		_dialogueOptionID = id;
		_dialogueOptions.Add(id, this);
		_buttonTextKey = textKey;
	}

	/*static public void AddFollowUps()
	{
		for (int i = 0; i < dialogueOptions.Count; i++)
		{
			string id = dialogueOptions[i];
			switch(id)
			{
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")ACCEPT: {
				Dialogue_Option_Logic.SetPrompts_Accept(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")ACKNOWLEDGE: {
				Dialogue_Option_Logic.SetPrompts_Acknowledge(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CANCEL_PURCHASE: {
				Dialogue_Option_Logic.SetPrompts_CancelPurchase(Dialogue_Option.GetOptionByName(id));				
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CHECK_THE_REGISTER: {
				Dialogue_Option_Logic.SetPrompts_CheckTheRegister(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CHIT_CHAT: {
				Dialogue_Option_Logic.SetPrompts_ChitChat(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CONFIRM_PURCHASE: { 
				Dialogue_Option_Logic.SetPrompts_ConfirmPurchase(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CONTINUE_INTIMIDATING: { 
				Dialogue_Option_Logic.SetPrompts_ContinueIntimidating(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")CUT_PROTECTION_COST: { 
				Dialogue_Option_Logic.SetPrompts_CutProtectionCost(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")DONATE: { 
				Dialogue_Option_Logic.SetPrompts_Donate(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_EarlyPayment(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_EnterShop(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_ExitShop(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_GetDetails(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_GoShopping(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_HearProposition(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Ignore(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate2_Imply(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate2_Threaten(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate2_Act(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate3_InformBoss(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate3_BreakMerchandise(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate3_AttackShopkeeper(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys: {
				Dialogue_Option_Logic.SetPrompts_Intimidate3_BurnDownShop(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys.ACTION_NEVERMIND: { 
				Dialogue_Option_Logic.SetPrompts_NeverMind(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")OFFER_PROTECTION: { 
				Dialogue_Option_Logic.SetPrompts_OfferProtection(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")AID_BUSINESS: { 
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
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")RENEGOTIATE_PROTECTION: { 
				Dialogue_Option_Logic.SetPrompts_Renegotiate(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")REQUEST_PAYMENT: { 
				Dialogue_Option_Logic.SetPrompts_RequestPayment(Dialogue_Option.GetOptionByName(id));
				break; }
			case "dialogue_option_resumeTalking": { 
				Dialogue_Option_Logic.SetPrompts_ResumeTalking(Dialogue_Option.GetOptionByName(id));
				break; }
			case "dialogue_option_returnGreeting": { 
				Dialogue_Option_Logic.SetPrompts_ReturnGreeting(Dialogue_Option.GetOptionByName(id));
				break; }
			case ButtonActionsKeys.ACTION_SHOP_PRODUCT: { 
				Dialogue_Option_Logic.SetPrompts_ShopProduct(Dialogue_Option.GetOptionByName(id));
				break; }
			case Dialogue_Option.GetOptionByName("dialogue_prompt_")TAKE_REGISTER_MONEY: { 
				Dialogue_Option_Logic.SetPrompts_TakeRegisterMoney(Dialogue_Option.GetOptionByName(id));
				break; }
			}
		}
	}*/
}