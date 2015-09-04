using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue_Prompt_Logic
{
	#region Prompt-to-Option Set-up
	static public void SetOptions_AidingBusiness (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_donate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_cutProtectionCost"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_AskToLowerPayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_reject"));
	}
	static public void SetOptions_AssaultInitial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));
	}
	static public void SetOptions_AssaultSecondary (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_placate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));

		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_placate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_getDetails"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_ignore"));
	}
	static public void SetOptions_GiveDetails (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_Goodbye (Dialogue_Prompt obj) {
	}
	static public void SetOptions_Greeting (Dialogue_Prompt obj) 
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_returnGreeting"));
	}
	
	static public void SetOptions_Intimidate (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_2_imply"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_2_threaten"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_2_act"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
		
	}
	static public void SetOptions_Intimidate_Action (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_3_informBoss"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_3_breakMerchandise"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_3_attackShopkeeper"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate_3_burnShopDown"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_IntimidatedCallsPolice (Dialogue_Prompt obj) {
		// TODO: follow ups for intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_IntimidatedFightsBack (Dialogue_Prompt obj) {
		// TODO: follow ups for intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_IntimidatedRattled (Dialogue_Prompt obj) {
		// TODO: follow ups for intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_IntimidatedUnaffected (Dialogue_Prompt obj) {
		// TODO: follow ups for intimidate
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
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
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
	}
	static public void SetOptions_PaymentPartial (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"));
	}
	static public void SetOptions_PaymentNone (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_checkTheRegister"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
	}
	static public void SetOptions_Placated (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	static public void SetOptions_ProblemWithPayment (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_hearProposition"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"));
	}
	static public void SetOptions_ConfirmPurchase (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_confirmPurchase"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_PurchaseFailed (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_PurchaseSuccessful (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
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
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_offerToAidBusiness"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_exitShop"));
	}
	static public void SetOptions_RegisterSomeMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_takeRegisterMoney"));
	}
	static public void SetOptions_RegisterHiddenMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_takeRegisterMoney"));
	}
	static public void SetOptions_RegisterTakeOrLeaveMoney (Dialogue_Prompt obj)
	{
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_accept"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_intimidate"));
	}
	static public void SetOptions_SchedulePayment (Dialogue_Prompt obj) {
		//obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_defaultAmount"));
		//obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_otherAmount"));
		//obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_ShopInventory (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"));
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_shopProduct"));
		
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_neverMind"));
	}
	static public void SetOptions_SmallTalk (Dialogue_Prompt obj) {
		obj.AddFollowUp(Dialogue_Option.GetOptionByName("dialogue_option_acknowledge"));
	}
	#endregion

	#region Follow-Up Option Filter Logic
	static public List<string> FilterKeys(Dialogue_Prompt prompt, Resources_Shopkeeper shopkeeper)
	{
		List<string> returnKeys = new List<string>();

		switch (prompt.promptID)
		{
			case "dialogue_prompt_root": {
				returnKeys = FilterRoot (prompt.followUpKeys, shopkeeper);
				break;
			}
			default: {
				returnKeys = prompt.followUpKeys;
				break;
			}
		}
		return returnKeys;
	}

	static public List<string> FilterRoot(List<string> keys, Resources_Shopkeeper shopkeeper)
	{
		List<string> returnKeys = new List<string>();

		if(shopkeeper.home.payment > 0)
		{
			if(shopkeeper.home.day == Manager_GameTime.Instance.currentDayOfTheWeek && !shopkeeper.home.hasPaid)
			{
				returnKeys.Add ("dialogue_option_requestPayment");
			} else {
				returnKeys.Add ("dialogue_option_earlyPayment");
			}
			returnKeys.Add ("dialogue_option_renegotiate");
		} else {
			returnKeys.Add ("dialogue_option_offerProtection");
		}
		returnKeys.Add("dialogue_option_intimidate");
		returnKeys.Add("dialogue_option_goShopping");
		returnKeys.Add("dialogue_option_chitChat");
		returnKeys.Add("dialogue_option_exitShop");
		return returnKeys;
	}
	#endregion

	#region Prompt On-Call Logic
	static public void AidingBusiness () { }
	static public void AskToLowerPayment () { }
	static public void AssaultInitial () { }
	static public void AssaultSecondary () { }
	static public void ConfirmPurchase () { }
	static public void GiveDetails () { }
	static public void Goodbye () { }
	static public void Greeting () { }
	static public void Intimidate () { }
	static public void Intimidate_Action () { }
	static public void IntimidatedCallsPolice () { }
	static public void IntimidatedFightsBack () { }
	static public void IntimidatedRattled () { }
	static public void IntimidatedUnaffected () { }
	static public void OfferAccepted () { }
	static public void OfferRefused () { }
	static public void OutsideShop () { }
	static public void PaymentFull () { }
	static public void PaymentPartial () { }
	static public void PaymentNone () { }
	static public void Placated () { }
	static public void ProblemWithPayment () { }
	static public void PurchaseFailed () { }
	static public void PurchaseSuccessful () { }
	static public void Root () { }
	static public void RegisterEmpty () { }
	static public void RegisterSomeMoney () { }
	static public void RegisterHiddenMoney () { }
	static public void RegisterTakeOrLeaveMoney () { }
	static public void SchedulePayment () { }
	static public void ShopInventory () { }
	static public void SmallTalk () { }
	#endregion
}