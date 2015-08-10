using UnityEngine;
using System.Collections;

public class Dialogue_Prompt_Logic
{
	static public void SetOptions_AidingBusiness (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_DONATE), ButtonActionsKeys.TEXT_AID_DONATE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CUT_PROTECTION_COST), ButtonActionsKeys.TEXT_AID_CUT_PROTECTION_COST);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
	}
	static public void SetOptions_AskToLowerPayment (Dialogue_Prompt obj) {
		//obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acceptProposition"), "Accept");
		//obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_rejectProposition"), "Decline");
	}
	static public void SetOptions_AssaultInitial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"), "Ask for details");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"), "Ignore");
	}
	static public void SetOptions_AssaultSecondary (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_placate"), "Placate");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"), "Ask for details");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"), "Ignore");
	}
	static public void SetOptions_GiveDetails (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"), "Acknowledge");
	}
	static public void SetOptions_Greeting (Dialogue_Prompt obj) 
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE), ButtonActionsKeys.TEXT_INTIMIDATE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_Intimidate (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_IMPLY), ButtonActionsKeys.TEXT_INTIMIDATE_IMPLY);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_THREATEN), ButtonActionsKeys.TEXT_INTIMIDATE_THREATEN);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_ACT), ButtonActionsKeys.TEXT_INTIMIDATE_ACT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
		
	}
	static public void SetOptions_Intimidate_Action (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_INFORM_BOSS), ButtonActionsKeys.TEXT_INTIMIDATE_INFORM_BOSS);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_BREAK_MERCHANDISE), ButtonActionsKeys.TEXT_INTIMIDATE_BREAK_MERCHANDISE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_ATTACK_SHOPKEEPER), ButtonActionsKeys.TEXT_INTIMIDATE_ATTACK_SHOPKEEPER);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE_BURN_SHOP_DOWN), ButtonActionsKeys.TEXT_INTIMIDATE_BURN_SHOP_DOWN);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
	}
	static public void SetOptions_IntimidatedCallsPolice (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_IntimidatedFightsBack (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_IntimidatedRattled (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_IntimidatedUnaffected (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_OfferAccepted (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"), "Acknowledge");
	}
	static public void SetOptions_OfferRefused (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate"), ButtonActionsKeys.TEXT_INTIMIDATE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_tryAnotherOffer"), "Change offer");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
	}
	static public void SetOptions_OutsideShop (Dialogue_Prompt obj) {
		obj.AddFollowUp (Dialogue_Option.GetOptionByName("dialogue_option_enterShop"));
	}
	static public void SetOptions_PaymentFull (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT);
	}
	static public void SetOptions_PaymentPartial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHECK_REGISTER), ButtonActionsKeys.TEXT_CHECK_REGISTER);
	}
	static public void SetOptions_PaymentNone (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE), ButtonActionsKeys.TEXT_INTIMIDATE, "Give a warning");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHECK_REGISTER), ButtonActionsKeys.TEXT_CHECK_REGISTER);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT, "Leave them be");
	}
	static public void SetOptions_Placated (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_ProblemWithPayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_hearProposition"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"));
	}
	static public void SetOptions_ConfirmPurchase (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPPRODUCT_CONFIRM_PURCHASE), ButtonActionsKeys.TEXT_CONFIRM_PURCHASE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
	}
	static public void SetOptions_PurchaseFailed (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND, "Back to shop");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_PurchaseSuccessful (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND, "Yes");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_Root (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_requestPayment"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_earlyPayment"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_renegotiate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_offerProtection"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_goShopping"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_chitChat"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_RegisterEmpty (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT, "Continue");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_AID_BUSINESS), ButtonActionsKeys.TEXT_AID_BUSINESS, "Offer to donate");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
	}
	static public void SetOptions_RegisterSomeMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT, "Leave the money");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_TAKE_REGISTER_MONEY), ButtonActionsKeys.TEXT_TAKE_REGISTER_MONEY);
	}
	static public void SetOptions_RegisterHiddenMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT, "Leave the money");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_TAKE_REGISTER_MONEY), ButtonActionsKeys.TEXT_TAKE_REGISTER_MONEY);
	}
	static public void SetOptions_RegisterTakeOrLeaveMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_ACCEPT_PAYMENT), ButtonActionsKeys.TEXT_ACCEPT_PAYMENT, "Leave them be");
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE), ButtonActionsKeys.TEXT_INTIMIDATE, "Punish them");
	}
	static public void SetOptions_SchedulePayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_defaultAmount"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_otherAmount"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_ShopInventory (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_PRODUCT), ButtonActionsKeys.TEXT_SHOP_PRODUCT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_NEVERMIND), ButtonActionsKeys.TEXT_NEVERMIND);
	}
	static public void SetOptions_SmallTalk (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE), ButtonActionsKeys.TEXT_INTIMIDATE);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_REQUEST_PAYMENT), ButtonActionsKeys.TEXT_REQUEST_PAYMENT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION), ButtonActionsKeys.TEXT_OFFER_PROTECTION);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING), ButtonActionsKeys.TEXT_GO_SHOPPING);
//		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT), ButtonActionsKeys.TEXT_CHIT_CHAT);
		obj.AddFollowUp(Dialogue_Option.GetOptionByName(ButtonActionsKeys.ACTION_SHOP_EXIT), ButtonActionsKeys.TEXT_SHOP_EXIT);
//		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}


}