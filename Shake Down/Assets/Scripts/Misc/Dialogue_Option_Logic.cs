using UnityEngine;
using System.Collections;

public class Dialogue_Option_Logic
{
	static public Dialogue_Prompt Accept () {
		Dialogue_Prompt_Logic.Root ();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt Acknowledge () {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt CancelPurchase() {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt CheckTheRegister () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt ChitChat () {
		Dialogue_Prompt_Logic.SmallTalk();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_smallTalk");
	}
	static public Dialogue_Prompt ConfirmPurchase (int playerCash, Item_Root item) {
		if (playerCash >= item.price) {
			Resources_Player.instance.inventory.GetItem(item);
			Resources_Player.instance.money -= item.price;
			DialogueInterface.Instance.shopkeeperRef.home.money += item.price;
			Dialogue_Prompt_Logic.PurchaseSuccessful();
			DialogueInterface.Instance.shopkeeperRef.respect += 5;
			return Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseSuccessful");
		} else {
			Dialogue_Prompt_Logic.PurchaseFailed();
			return Dialogue_Prompt.GetPromptByName("dialogue_prompt_purchaseFailed"); }
	}
	static public Dialogue_Prompt ContinueIntimidating() {
		Dialogue_Prompt_Logic.Intimidate();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_intimidate");
	}
	static public Dialogue_Prompt CutProtectionCost () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt DefaultAmount(Resources_Player player, Resources_Shopkeeper shopkeeper, int askingPrice) {
		Resources_Building shop = shopkeeper.home;
		float profit = shop.income - shop.expenses;
		if(profit == 0) { profit = 0.0001f; }
		
		float affordValue = 2 * ((1 - askingPrice / profit * shopkeeper.greed) * (shopkeeper.fear / shopkeeper.greed) - 50);
		float strengthValue = (Utilities.difficultyHandicap/5 + player.strength) * player.presence - (shopkeeper.strength * 100/shopkeeper.fear);
		float moralValue = Utilities.difficultyHandicap * shopkeeper.fear/10 - shopkeeper.integrity / shopkeeper.respect;
		Debug.Log ("Afford Value: " + affordValue + ". Strength Value: " + strengthValue + ". Moral Value: " + moralValue + ". Total: " + (affordValue+strengthValue+moralValue) + ".");

		if((affordValue + strengthValue + moralValue) >= 0)
		{
			Dialogue_Prompt_Logic.OfferAccepted();
			return Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerAccepted");
		} else {
			Dialogue_Prompt_Logic.OfferRefused();
			return Dialogue_Prompt.GetPromptByName("dialogue_prompt_offerRefused");
		}
	}
	static public Dialogue_Prompt Donate () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt EarlyPayment(Resources_Player player, Resources_Shopkeeper shopkeeper) {
		shopkeeper.respect -= 10;
		return RequestPayment (player, shopkeeper);
	}
	static public Dialogue_Prompt EnterShop() {
		Dialogue_Prompt_Logic.Greeting();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_greeting");
		/*
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_assaultInitial");
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_assaultSecondary");
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_problemWithPayment");
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentFull");
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial");
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone");
		*/
	}
	static public Dialogue_Prompt ExitShop () {
		Dialogue_Prompt_Logic.Goodbye();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_goodbye");
	}
	static public Dialogue_Prompt GetDetails() {
		Dialogue_Prompt_Logic.GiveDetails();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_giveDetails");
	}
	static public Dialogue_Prompt GoShopping () {
		Dialogue_Prompt_Logic.ShopInventory();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_shopInventory");
	}
	static public Dialogue_Prompt HearProposition() {
		Dialogue_Prompt_Logic.AskToLowerPayment();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_askToLowerPayment");
	}
	static public Dialogue_Prompt Ignore() {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt Intimidate () {
		Debug.Log ("INTIMIDATE CLICKED"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate2Imply () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate2Act () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate2Threaten () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate3InformBoss () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate3BreakMerchandise () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate3AttackShopkeeper () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt Intimidate3BurnDownShop () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
	static public Dialogue_Prompt NeverMind () {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt OtherAmount(Resources_Player player, Resources_Shopkeeper shopkeeper, int askingPrice) {
		return DefaultAmount (player, shopkeeper, askingPrice);
	}
	static public Dialogue_Prompt OfferToAidBusiness () {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt OfferProtection () {
		Dialogue_Prompt_Logic.SchedulePayment();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_schedulePayment");
	}
	static public Dialogue_Prompt Placate() {
		Dialogue_Prompt_Logic.Placated();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_placated");
	}
	static public Dialogue_Prompt Reject() {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt Renegotiate () {
		Dialogue_Prompt_Logic.SchedulePayment();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_schedulePayment");
	}
	static public Dialogue_Prompt RequestPayment(Resources_Player player, Resources_Shopkeeper shopkeeper) {
		int randNum  = Random.Range(0, 50);
		int paymentAmount = shopkeeper.home.payment;
		
		// Determine if the shopkeeper is feeling cocky enough to not pay you.
		if(randNum > shopkeeper.respect && shopkeeper.strength > player.strength)
		{
			// The shopkeeper doesn't respect you, and isn't afraid of you, so you are not getting your payment
			Dialogue_Prompt_Logic.PaymentNone();
			return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentNone");
		} else {
			// Check to see if the store can afford to make the payment
			if (shopkeeper.home.money < paymentAmount)
			{
				// If the shop can't afford the payment
				// Check to see if the shopkeeper is fearful enough to try to pay out of pocket
				if (shopkeeper.fear > 50) 
				{
					// The shopkeeper is fearful enought to pay out of pocket
					// Check to see if the shopkeeper can afford it
					if (shopkeeper.home.money + shopkeeper.money < paymentAmount)
					{
						// The shopkeeper can't make up the difference and must pay only partially
						player.money += shopkeeper.home.money;
						shopkeeper.home.money = 0;
						Dialogue_Prompt_Logic.PaymentPartial();
						return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial");
					}
					else
					{
						// The shopkeeper will make up the difference out of pocket
						player.money += paymentAmount;
						int difAmount = paymentAmount;
						difAmount -= shopkeeper.home.money;
						shopkeeper.home.money = 0;
						shopkeeper.money -= difAmount;
						Dialogue_Prompt_Logic.PaymentFull();
						return Dialogue_Prompt.GetPromptByName ("dialogue_prompt_paymentFull");
					}
				} else {
					// The shopkeeper is unwilling to pay out of pocket and will pay only partially
					player.money += shopkeeper.home.money;
					shopkeeper.home.money = 0;
					Dialogue_Prompt_Logic.PaymentPartial();
					return Dialogue_Prompt.GetPromptByName("dialogue_prompt_paymentPartial");
				}
			} else {
				// The shopkeeper can afford the full payment
				player.money += paymentAmount;
				shopkeeper.home.money -= paymentAmount;
				Dialogue_Prompt_Logic.PaymentFull();
				return Dialogue_Prompt.GetPromptByName ("dialogue_prompt_paymentFull");
			}
		}
	}
	static public Dialogue_Prompt ResumeTalking() {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt ReturnGreeting() {
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt ShopProduct () {
		Dialogue_Prompt_Logic.ConfirmPurchase();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_confirmPurchase");
	}
	static public Dialogue_Prompt TakeRegisterMoney (Resources_Player player, Resources_Shopkeeper shopkeeper, int amount) {
		shopkeeper.home.money -= amount;
		player.money += amount;
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");
	}
	static public Dialogue_Prompt TryAnotherOffer() {
		Debug.Log ("THIS LOGIC IS MISSING"); // TODO:
		Dialogue_Prompt_Logic.Root();
		return Dialogue_Prompt.GetPromptByName("dialogue_prompt_root");	
	}
}