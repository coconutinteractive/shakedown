using UnityEngine;
using System.Collections;

public static class ButtonActionsKeys
{
	// This entire script has been replaced by Dialogue_Option and Dialogue_Prompt_Logic

	// General Actions: the syntax is: ACTION_{TARGET}_{EFFECT};
	/*
	public const string ACTION_SHOPKEEP_ACCEPT							 = "dialogue_option_accept";
	public const string ACTION_SHOPKEEP_ACKNOWLEDGE						 = "dialogue_option_acknowledge";
	public const string ACTION_SHOPKEEP_CANCEL_PURCHASE					 = "dialogue_option_cancelPurchase";
	public const string ACTION_SHOPKEEP_CHECK_THE_REGISTER				 = "dialogue_option_checkTheRegister";
	public const string ACTION_SHOPKEEP_CHIT_CHAT						 = "dialogue_option_chitChat";
	public const string ACTION_SHOPKEEP_CONFIRM_PURCHASE				 = "dialogue_option_confirmPurchase";
	public const string ACTION_SHOPKEEP_CONTINUE_INTIMIDATING			 = "dialogue_option_continueIntimidating";
	public const string ACTION_SHOPKEEP_CUT_PROTECTION_COST				 = "dialogue_option_cutProtectionCost";
	public const string ACTION_SHOPKEEP_DEFAULT_AMOUNT					 = "dialogue_option_defaultAmount";
	public const string ACTION_SHOPKEEP_DONATE							 = "dialogue_option_donate";
	public const string ACTION_SHOPKEEP_EARLY_PAYMENT					 = "dialogue_option_earlyPayment";
	public const string ACTION_SHOPKEEP_ENTER_SHOP						 = "dialogue_option_enterShop";
	public const string ACTION_SHOPKEEP_EXIT_SHOP						 = "dialogue_option_exitShop";
	public const string ACTION_SHOPKEEP_GET_DETAILS						 = "dialogue_option_getDetails";
	public const string ACTION_SHOPKEEP_GO_SHOPPING						 = "dialogue_option_goShopping";
	public const string ACTION_SHOPKEEP_HEAR_PROPOSITION				 = "dialogue_option_hearProposition";
	public const string ACTION_SHOPKEEP_IGNORE							 = "dialogue_option_ignore";
	public const string ACTION_SHOPKEEP_INTIMIDATE						 = "dialogue_option_intimidate";
	public const string ACTION_SHOPKEEP_INTIMIDATE_2_ACT				 = "dialogue_option_intimidate2_act";
	public const string ACTION_SHOPKEEP_INTIMIDATE_2_IMPLY				 = "dialogue_option_intimidate2_imply";
	public const string ACTION_SHOPKEEP_INTIMIDATE_2_THREATEN			 = "dialogue_option_intimidate2_threaten";
	public const string ACTION_SHOPKEEP_INTIMIDATE_3_ATTACK_SHOPKEEPER	 = "dialogue_option_intimidate3_attackShopkeeper";
	public const string ACTION_SHOPKEEP_INTIMIDATE_3_BREAK_MERCHANDISE	 = "dialogue_option_intimidate3_breakMerchandise";
	public const string ACTION_SHOPKEEP_INTIMIDATE_3_BURN_DOWN_SHOP		 = "dialogue_option_intimidate3_burnDownShop";
	public const string ACTION_SHOPKEEP_INTIMIDATE_3_INFORM_BOSS		 = "dialogue_option_intimidate3_informBoss";
	public const string ACTION_SHOPKEEP_NEVER_MIND						 = "dialogue_option_neverMind";
	public const string ACTION_SHOPKEEP_OFFER_PROTECTION				 = "dialogue_option_offerProtection";
	public const string ACTION_SHOPKEEP_OFFER_TO_AID_BUSINESS			 = "dialogue_option_offerToAidBusiness";
	public const string ACTION_SHOPKEEP_OTHER_AMOUNT					 = "dialogue_option_otherAmount";
	public const string ACTION_SHOPKEEP_PLACATE							 = "dialogue_option_placate";
	public const string ACTION_SHOPKEEP_REJECT_PROPOSITION				 = "dialogue_option_rejectProposition";
	public const string ACTION_SHOPKEEP_RENEGOTIATE						 = "dialogue_option_renegotiate";
	public const string ACTION_SHOPKEEP_REQUEST_PAYMENT					 = "dialogue_option_requestPayment";
	public const string ACTION_SHOPKEEP_RESUME_TALKING					 = "dialogue_option_resumeTalking";
	public const string ACTION_SHOPKEEP_RETURN_GREETING					 = "dialogue_option_returnGreeting";
	public const string ACTION_SHOPKEEP_SHOP_PRODUCT					 = "dialogue_option_shopProduct";
	public const string ACTION_SHOPKEEP_TAKE_REGISTER_MONEY				 = "dialogue_option_takeRegisterMoney";
	public const string ACTION_SHOPKEEP_TRY_ANOTHER_OFFER				 = "dialogue_option_tryAnotherOffer";

	/*
	public const string ACTION_SHOPKEEP_EXIT_SHOP = "dialogue_option_exitShop";

	public const string ACTION_SHOPKEEP_INTIMIDATE = "dialogue_option_intimidate";	
	public const string ACTION_SHOPKEEP_INTIMIDATE_IMPLY = "dialogue_option_intimidate2_imply";
	public const string ACTION_SHOPKEEP_INTIMIDATE_THREATEN = "dialogue_option_intimidate2_threaten";
	public const string ACTION_SHOPKEEP_INTIMIDATE_ACT = "dialogue_option_intimidate2_act";
	public const string ACTION_SHOPKEEP_INTIMIDATE_INFORM_BOSS = "dialogue_option_intimidate3_informBoss";
	public const string ACTION_SHOPKEEP_INTIMIDATE_BREAK_MERCHANDISE = "dialogue_option_intimidate3_breakMerchandise";
	public const string ACTION_SHOPKEEP_INTIMIDATE_ATTACK_SHOPKEEPER = "dialogue_option_intimidate3_attackShopkeeper";
	public const string ACTION_SHOPKEEP_INTIMIDATE_BURN_SHOP_DOWN = "dialogue_option_intimidate3_burnDownShop";

	public const string ACTION_SHOPKEEP_REQUEST_PAYMENT = "dialogue_option_requestPayment";	
	public const string ACTION_SHOPKEEP_ACCEPT_PAYMENT = "dialogue_option_Accept";
	public const string ACTION_SHOPKEEP_CHECK_REGISTER = "dialogue_option_checkTheRegister";
	public const string ACTION_SHOPKEEP_TAKE_REGISTER_MONEY = "dialogue_option_takeRegisterMoney";
	public const string ACTION_SHOPKEEP_AID_BUSINESS = "dialogue_option_offerToAidBusiness";
	public const string ACTION_SHOPKEEP_DONATE = "dialogue_option_donate";
	public const string ACTION_SHOPKEEP_CUT_PROTECTION_COST = "dialogue_option_cutProtectionCost";
	public const string ACTION_SHOPKEEP_DONATE_CONFIRM = "dialogue_option_donateConfirm";
	public const string ACTION_SHOPKEEP_CUT_PROTECTION_COST_CONFIRM = "dialogue_option_cutProtectionCostConfirm";
	public const string ACTION_SHOPKEEP_DONATE_CANCEL = "dialogue_option_donateCancel";
	public const string ACTION_SHOPKEEP_CUT_PROTECTION_COST_CANCEL = "dialogue_option_cutProtectionCostCancel";

	public const string ACTION_SHOPKEEP_OFFER_PROTECTION = "dialogue_option_offerProtection";	
	public const string ACTION_SHOPKEEP_RENEGOTIATE_PROTECTION = "dialogue_option_renegotiate";	

	public const string ACTION_SHOPKEEP_GO_SHOPPING = "dialogue_option_goShopping";	
	public const string ACTION_SHOP_PRODUCT = "dialogue_option_shopProduct";
	public const string ACTION_SHOPPRODUCT_CONFIRM_PURCHASE = "dialogue_option_confirmPurchase";

	public const string ACTION_SHOPKEEP_CHIT_CHAT = "dialogue_option_chitChat";	

	public const string ACTION_NEVERMIND = "dialogue_option_neverMind";

	public const string ACTION_RETURN_GREETING = "dialogue_option_returnGreeting";

	// Button Text
	
	public const string TEXT_INTIMIDATE = "Intimidate";
	public const string TEXT_INTIMIDATE_IMPLY = "Imply to...";
	public const string TEXT_INTIMIDATE_THREATEN = "Threaten to...";
	public const string TEXT_INTIMIDATE_ACT = "Do...";
	public const string TEXT_INTIMIDATE_INFORM_BOSS = "Inform your boss";
	public const string TEXT_INTIMIDATE_BREAK_MERCHANDISE = "Break merchandise";
	public const string TEXT_INTIMIDATE_ATTACK_SHOPKEEPER = "Hurt the shopkeeper";
	public const string TEXT_INTIMIDATE_BURN_SHOP_DOWN = "Destroy the store";

	public const string TEXT_GO_SHOPPING = "Go shopping";
	public const string TEXT_SHOP_PRODUCT = "Purchase";
	public const string TEXT_CONFIRM_PURCHASE = "Confirm purchase";

	public const string TEXT_CHIT_CHAT = "Small talk";

	public const string TEXT_REQUEST_PAYMENT = "Request payment";
	public const string TEXT_ACCEPT_PAYMENT = "Accept payment";
	public const string TEXT_CHECK_REGISTER = "Check register";
	public const string TEXT_TAKE_REGISTER_MONEY = "Take money";
	public const string TEXT_AID_BUSINESS = "Offer to help";
	public const string TEXT_AID_DONATE = "Donate";
	public const string TEXT_AID_CUT_PROTECTION_COST = "Cut protection cost";

	public const string TEXT_OFFER_PROTECTION = "Offer protection";
	public const string TEXT_RENEGOTIATE_PROTECTION = "Protection terms";

	public const string TEXT_NEVERMIND = "Never mind";

	public const string TEXT_SHOP_EXIT = "Exit shop";
	 */
}