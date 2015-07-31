using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue_Script : MonoBehaviour
{
	[SerializeField] static private GameObject _dialoguePanel;
	static public GameObject dialoguePanel { get { return _dialoguePanel; } }

	[SerializeField] private GameObject leftProfileImage;
	[SerializeField] private GameObject rightProfileImage;
	private Material leftProfileMaterial;
	private Material rightProfileMaterial;

	private Resources_Root characterLeft;
	private Resources_Root characterRight;
	private Resources_Player player;
	private Resources_Root npc;
	private Building_Script building;
	private Resources_Officer presentOfficer;
<<<<<<< HEAD

	private List<ShopItem> shopItems = new List<ShopItem>();
	private Enums.ShopkeeperStates shopkeeperState;
	private Enums.OfficerStates officerState;
=======
	
	private Utilities.ShopkeeperStates shopkeeperState;
	private Utilities.OfficerStates officerState;
>>>>>>> origin/master

	public void GenerateDialogue (Resources_Root leftCharacter, Resources_Root rightCharacter, Building_Script building, Resources_Officer nearbyOfficer = null)
	{
		SetMaterials();
		characterLeft = leftCharacter;
		characterRight = rightCharacter;
		presentOfficer = nearbyOfficer;
		this.building = building;
		UpdateProfileImages();
		if (characterRight is Resources_Player)
		{ if(characterLeft is Resources_Shopkeeper)
			{ DetermineShopOptions((Resources_Player)characterRight, (Resources_Shopkeeper)characterLeft); }
		} else if (characterLeft is Resources_Player) {
			if(characterRight is Resources_Shopkeeper)
			{ DetermineShopOptions((Resources_Player)characterLeft, (Resources_Shopkeeper)characterRight); }
		}
		else
		{
			//TODO: non-player dialogue
		}
	}

	private void UpdateProfileImages()
	{
<<<<<<< HEAD
		leftProfileMaterial	= Utilities.GetMaterialFromID(characterLeft.image);
		rightProfileMaterial = Utilities.GetMaterialFromID(characterRight.image);
=======
		leftProfileMaterial	= characterLeft.profileImage;
		rightProfileMaterial = characterRight.profileImage;
>>>>>>> origin/master
	}

	public void ClearDisplay()
	{
		SetMaterials();
		leftProfileMaterial = Utilities.GetMaterialFromID("BlankFace");
		rightProfileMaterial = Utilities.GetMaterialFromID("BlankFace");
	}

	private void DetermineShopOptions(Resources_Player player, Resources_Shopkeeper shopkeeper)
	{
		List<string> Options = new List<string>();

		if(building.buildingRobbed)
			shopkeeperState = Enums.ShopkeeperStates.Robbed;
		else if(building.buildingVandalized)
			shopkeeperState = Enums.ShopkeeperStates.Vandalized;
		//else if(shopkeeper.IsShopkeeperAggrivated(player.presence))
		//	shopkeeperState = Enums.ShopkeeperStates.Aggrivated;
		else
			shopkeeperState = Enums.ShopkeeperStates.Passive;

		if(shopkeeperState != Enums.ShopkeeperStates.Robbed)
		{
			Options.Add ("Purchase Wares");
			if(shopkeeperState == Enums.ShopkeeperStates.Vandalized)
			{ Options.Add("Inquire"); }
		}else{
			/*if(player.getProof(shopkeeper.referenceID))
			{ Options.Add ("Placate"); }*/
			Options.Add ("Inquire");
		}

		if(shopkeeper.home.payment > 0)
		{
			Options.Add ("Request Payment");
			Options.Add ("Renegotiate Payment");
		} else {
			Options.Add ("Suggest Protection");
		}
	}

	private void SetMaterials()
	{
		if(leftProfileMaterial)
		{
			leftProfileMaterial = leftProfileImage.GetComponent<MeshRenderer>().material;
			rightProfileMaterial = rightProfileImage.GetComponent<MeshRenderer>().material;
		}
	}
}
