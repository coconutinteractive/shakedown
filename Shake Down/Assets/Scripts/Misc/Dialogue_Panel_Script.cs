using UnityEngine;
using System.Collections;

public class Dialogue_Panel_Script : MonoBehaviour
{
	[SerializeField] private GameObject _mainDialogue;
	private Dialogue_Script mainDialogueScript;
	public GameObject mainDialogue { get { return _mainDialogue; } }

	[SerializeField] private GameObject _altDialogue;
	private Dialogue_Script altDialogueScript;
	public GameObject altDialogue { get { return _altDialogue; } }

	void Start()
	{
		mainDialogueScript = mainDialogue.GetComponent<Dialogue_Script>();
		altDialogueScript = mainDialogue.GetComponent<Dialogue_Script>();
		mainDialogue.SetActive(false);
		altDialogue.SetActive(false);
	}

<<<<<<< HEAD
	public void StartDialogue(Resources_Root leftCharacter, Resources_Root rightCharacter, Building_Script building = null, Resources_Officer nearbyOfficer = null)
=======
	public void StartDialogue(Resources_Master leftCharacter, Resources_Master rightCharacter, Building_Script building, Resources_Officer nearbyOfficer = null)
>>>>>>> origin/master
	{
		if (leftCharacter is Resources_Player || rightCharacter is Resources_Player)
			mainDialogueScript.GenerateDialogue(leftCharacter, rightCharacter, building, nearbyOfficer);
		else
			altDialogueScript.GenerateDialogue(leftCharacter, rightCharacter, building, nearbyOfficer);
	}

	public void EndDialogue(bool targetMainDialogue = true)
	{
		if(targetMainDialogue)
		{
			mainDialogue.SetActive(false);
			mainDialogueScript.ClearDisplay();
		} else {
			altDialogue.SetActive(false);
			altDialogueScript.ClearDisplay();
		}
	}
}