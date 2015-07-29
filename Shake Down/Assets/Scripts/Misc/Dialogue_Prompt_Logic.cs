using UnityEngine;
using System.Collections;

public class Dialogue_Prompt_Logic
{
	static public void SetOptions_AskToLowerPayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acceptProposition"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_rejectProposition"));
	}
	static public void SetOptions_AssaultInitial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));
	}
	static public void SetOptions_AssaultSecondary (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_placate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));
	}
	static public void SetOptions_GiveDetails (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_Greeting (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_returnGreeting"));
	}
	static public void SetOptions_IntimidatedCallsPolice (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
	}
	static public void SetOptions_IntimidatedFightsBack (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
	}
	static public void SetOptions_IntimidatedRattled (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
	}
	static public void SetOptions_IntimidatedUnaffected (Dialogue_Prompt obj) {
		// TODO: Logic for Intimidate
	}
	static public void SetOptions_OfferAccepted (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_OfferRefused (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_tryAnotherOffer"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_OutsideShop (Dialogue_Prompt obj) {
		obj.AddFollowUp (Dialogue_Option.GetOptionByName("dialogue_option_enterShop"));
	}
	static public void SetOptions_PaymentFull (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acceptPayment"));
	}
	static public void SetOptions_PaymentNone (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"));
	}
	static public void SetOptions_PaymentPartial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acceptPayment"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"));
	}
	static public void SetOptions_Placated (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_ProblemWithPayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_hearProposition"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"));
	}
	static public void SetOptions_ProductCost (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_confirmPurchase"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_cancelPurchase"));
	}
	static public void SetOptions_PurchaseFailed (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_PurchaseSuccessful (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
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
	static public void SetOptions_SchedulePayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_defaultAmount"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_otherAmount"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_ShopInventory (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_SmallTalk (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}


}