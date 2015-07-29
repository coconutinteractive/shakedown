using UnityEngine;
using System.Collections;

public class Dialogue_Option_Logic
{
	//TODO: Make this and dialogue prompt work as two methods that take in option/prompt and prompt/option; :\

	static public void SetPrompts_AcceptPayment (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_AcceptProposition (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Acknowledge (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_CancelPurchase(Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_CheckTheRegister (Dialogue_Option obj) {
		// TODO: Logic for Check the Register
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_ChitChat (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_smallTalk"));
	}
	static public void SetPrompts_ConfirmPurchase(Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseSuccessful"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseFailed"));
	}
	static public void SetPrompts_ContinueIntimidating(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_DefaultAmount (Dialogue_Option obj) {
		// TODO: Logic for Accept/Refuse offer
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerAccepted"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerRefused"));
	}
	static public void SetPrompts_EarlyPayment (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone"));
	}
	static public void SetPrompts_EnterShop (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_assaultInitial"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_assaultSecondary"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_problemWithPayment"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone"));
	}
	static public void SetPrompts_ExitShop (Dialogue_Option obj) {
	}
	static public void SetPrompts_GetDetails (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_giveDetails"));
	}
	static public void SetPrompts_GoShopping (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_shopInventory"));
	}
	static public void SetPrompts_HearProposition (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_askToLowerPayment"));
	}
	static public void SetPrompts_Ignore (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate (Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate2_Act(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate2_Imply(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate2_Threaten(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate3_AttackShopkeeper(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate3_BreakMerchandise(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate3_BurnDownShop(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Intimidate3_InformBoss(Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_NeverMind (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_OfferProtection (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_schedulePayment"));
	}
	static public void SetPrompts_OfferAidToBusiness (Dialogue_Option obj) {
		// TODO: Logic for Offer To Aid Business
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_OtherAmount (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerAccepted"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerRefused"));
	}
	static public void SetPrompts_Placate (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_placated"));
	}
	static public void SetPrompts_RejectProposition (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_Renegotiate (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_schedulePayment"));
	}
	static public void SetPrompts_RequestPayment (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial"));
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone"));
	}
	static public void SetPrompts_ResumeTalking (Dialogue_Option obj) {
		// TODO: Logic for Intimidate
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_ReturnGreeting (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_root"));
	}
	static public void SetPrompts_ShopProduct (Dialogue_Option obj) {
		obj.AddFollowUp(Dialogue_Prompt.GetPromptByName("dialogue_prompt_productCost"));
	}
}