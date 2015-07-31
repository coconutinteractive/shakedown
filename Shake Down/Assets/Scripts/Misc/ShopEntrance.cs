using UnityEngine;
using System.Collections;

public class ShopEntrance : MonoBehaviour 
{
	[SerializeField] private Sprite placeholderShopKeepPortrait = null;
	
	public void StartDialogue(Sprite _playerPortrait, string _playerName, string _playerLastName)
	{
		DialogueInterface.Instance.Activate ();
		DialogueInterface.Instance.DisplayLeftCharacter (_playerPortrait, ElementState.Normal, 1.0f, false);
		DialogueInterface.Instance.DisplayRightCharacter (placeholderShopKeepPortrait, ElementState.Normal, 2.5f, true);

		DialogueInterface.Instance.StartDialogue ();
	}

	public void EndDialogue()
	{
		DialogueInterface.Instance.HideAll ();
		DialogueInterface.Instance.Deactivate ();
	}
}
