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
	
	private List<ShopItem> shopItems = new List<ShopItem>();
	private Enums.ShopkeeperState shopkeeperState;
	private Enums.OfficerState officerState;
	
	public void GenerateDialogue (Resources_Root leftCharacter, Resources_Root rightCharacter, Building_Script building, Resources_Officer nearbyOfficer = null)
	{
		// TODO: Set Up Shop Items
		shopItems.Add (new ShopItem("item1", 50));
		shopItems.Add (new ShopItem("item2", 150));
		shopItems.Add (new ShopItem("item3", 250));
		
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
		leftProfileMaterial	= Utilities.GetMaterialFromID(characterLeft.image);
		rightProfileMaterial = Utilities.GetMaterialFromID(characterRight.image);
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
		
		shopkeeperState = Enums.ShopkeeperState.Passive;
		
		if(shopkeeperState != Enums.ShopkeeperState.Robbed)
		{
			Options.Add ("Purchase Wares");
			if(shopkeeperState == Enums.ShopkeeperState.Vandalized)
			{ Options.Add("Inquire"); }
		} else {
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
	
	static public void SetupDialogueOptionsFromJSON(JSONObject json)
	{
		int i;
		for(i = 0; i < json.Count; i++)
		{
			int j;
			JSONObject foo;
			JSONObject bar;
			if(json.keys[i] == "Prompts")
			{
				foo = json.list[i];
				for (j = 0; j < foo.Count; j++)
				{
					bar = foo.list[j];
					new Dialogue_Prompt(bar.GetField("id").str, bar.GetField("locsuffix").str);
				}
			}
			else if (json.keys[i] == "Options")
			{
				foo = json.list[i];
				for (j = 0; j < foo.Count; j++)
				{
					bar = foo.list[j];
					Dialogue_Option newOption = new Dialogue_Option(bar.GetField("id").str, bar.GetField ("buttontext").ToString());
				}
			}
		}
		Dialogue_Prompt.AddFollowUps();
	}
}
