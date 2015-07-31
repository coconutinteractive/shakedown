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

	[SerializeField] private int initialFearValue = 0, initialRespectValue = 0;

	private int currentFearValue = 0;
	private int currentRespectValue = 0;

	[SerializeField] private Sprite shopKeepPortrait = null;

	private void Start()
	{
		int[] values = GameChart_Shopkeeper.Instance.GetPosInArea (GameChart_Shopkeeper.Areas.Neutral);
		currentFearValue = values [0] + initialFearValue;
		currentRespectValue = values [1] + initialRespectValue;
	}

	public void Initialize()
	{
		DialogueInterface.Instance.OnIntimidate += HandleOnIntimidate;
		DialogueInterface.Instance.OnAskForPayment += HandleOnAskForPayment;
		DialogueInterface.Instance.OnOfferProtection += HandleOnOfferProtection;
		DialogueInterface.Instance.OnRenegotiateProtection += HandleOnRenegotiateProtection;
		DialogueInterface.Instance.OnGoShopping += HandleOnGoShopping;
		DialogueInterface.Instance.OnChitChat += HandleOnChitChat;
		DialogueInterface.Instance.OnExitShop += HandleOnExitShop;
	}
	
	public void StartDialogue(Sprite _playerPortrait, string _playerName, string _playerLastName)
	{
		DialogueInterface.Instance.Activate ();
		DialogueInterface.Instance.DisplayLeftCharacter (_playerPortrait, ElementState.Normal, 1.0f, false);
		DialogueInterface.Instance.DisplayRightCharacter (shopKeepPortrait, ElementState.Normal, 2.5f, true);

		UnityEngine.Events.UnityAction[] actions = DialogueInterface.Instance.GetButtonActionFromDict (new string[6]{ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE,
		ButtonActionsKeys.ACTION_SHOPKEEP_OFFER_PROTECTION, ButtonActionsKeys.ACTION_SHOPKEEP_ASK_FOR_PAYMENT, ButtonActionsKeys.ACTION_SHOPKEEP_GO_SHOPPING,
		ButtonActionsKeys.ACTION_SHOPKEEP_CHITCHAT, ButtonActionsKeys.ACTION_SHOP_EXIT});

		DialogueInterface.Instance.SetChoices (new string[6]
        {
			"Intimidate",
			"Offer Protection",
			"Ask For Payment",
			"Go Shopping",
			"Chit Chat",
			"Exit Store"
		});

		for (int i = 0; i < actions.Length; ++i)
			DialogueInterface.Instance.SetBtnActions(i+1, actions[i]);

		UpdateState ();
		string dialogueLineVariation = "";

		switch (currentState)
		{
		case ShopkeeperState.Neutral:
		case ShopkeeperState.Normal:
			dialogueLineVariation = Dialogues_Lines.SHOPKEEP_GREETINGS_NORMAL;
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

		DialogueInterface.Instance.StartDialogue (true, dialogueLineVariation, true);
		
	}

	public void EndDialogue()
	{
		DialogueInterface.Instance.HideAll ();
		DialogueInterface.Instance.Deactivate ();
		DialogueInterface.Instance.OnIntimidate -= HandleOnIntimidate;
		DialogueInterface.Instance.OnAskForPayment -= HandleOnAskForPayment;
		DialogueInterface.Instance.OnOfferProtection -= HandleOnOfferProtection;
		DialogueInterface.Instance.OnRenegotiateProtection -= HandleOnRenegotiateProtection;
		DialogueInterface.Instance.OnGoShopping -= HandleOnGoShopping;
		DialogueInterface.Instance.OnChitChat -= HandleOnChitChat;
		DialogueInterface.Instance.OnExitShop -= HandleOnExitShop;
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

	private void HandleOnAskForPayment ()
	{
		DialogueInterface.Instance.HideChoices ();
		UnityEngine.Events.UnityAction[] actions = DialogueInterface.Instance.GetButtonActionFromDict (new string[2]{ButtonActionsKeys.ACTION_SHOPKEEP_INTIMIDATE,
		ButtonActionsKeys.ACTION_SHOP_EXIT});
		DialogueInterface.Instance.SetChoices (new string[2]
        {
			"Intimidate",
			"Exit Store"
		});

		for (int i = 0; i < actions.Length; ++i)
			DialogueInterface.Instance.SetBtnActions(i+1, actions[i]);

		DialogueInterface.Instance.StartDialogue (false, Dialogues_Lines.SHOPKEEP_ASKFORPAYMENT_NONE_BROKE, true);
	}

	private void HandleOnIntimidate ()
	{
		DialogueInterface.Instance.HideChoices ();
		UnityEngine.Events.UnityAction action = DialogueInterface.Instance.GetButtonActionFromDict (ButtonActionsKeys.ACTION_SHOP_EXIT);
		DialogueInterface.Instance.SetChoices (new string[1]
        {
			"Exit Store"
		});
		DialogueInterface.Instance.SetBtnActions (1, action);
	
		UpdateValues (0, 25);
		UpdateState ();

		DialogueInterface.Instance.StartDialogue (false, Dialogues_Lines.SHOPKEEP_INTIMIDATE_TERRIFIED, true);
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
	
	private void HandleOnChitChat ()
	{
		
	}
	
	private void HandleOnExitShop ()
	{
		
	}
}
