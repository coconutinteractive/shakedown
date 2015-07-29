using UnityEngine;
using System.Collections;

public class Dialogue_Panel_Script : MonoBehaviour
{
	static private Dialogue_Panel_Script _panelReference;
	static public Dialogue_Panel_Script panelReference { get { return _panelReference; } }

	[SerializeField] private GameObject _mainDialoguePanel;
	public GameObject mainDialoguePanel { get { return _mainDialoguePanel; } }
	private Dialogue_Script _mainDialogueScript;
	public Dialogue_Script mainDialogueScript { get { return _mainDialogueScript; } }

	[SerializeField] private GameObject _altDialoguePanel;
	public GameObject altDialoguePanel { get { return _altDialoguePanel; } }
	private Dialogue_Script _altDialogueScript;
	public Dialogue_Script altDialogueScript { get { return _altDialogueScript; } }

	void Start()
	{
		_panelReference = this;
		_mainDialogueScript = mainDialoguePanel.GetComponent<Dialogue_Script>();
		_altDialogueScript = mainDialoguePanel.GetComponent<Dialogue_Script>();
		mainDialoguePanel.SetActive(false);
		altDialoguePanel.SetActive(false);
	}

	public void StartDialogue(Resources_Master leftCharacter, Resources_Master rightCharacter, Building_Script building = null, Resources_Officer nearbyOfficer = null)
	{
		if (leftCharacter is Resources_Player || rightCharacter is Resources_Player)
		{	mainDialogueScript.GenerateDialogue(leftCharacter, rightCharacter, building, nearbyOfficer);
			mainDialoguePanel.SetActive(true); 
		}
		else
		{	altDialogueScript.GenerateDialogue(leftCharacter, rightCharacter, building, nearbyOfficer);
			altDialoguePanel.SetActive(true);
		}
	}

	public void EndDialogue(bool targetMainDialogue = true)
	{
		if(targetMainDialogue)
		{
			mainDialoguePanel.SetActive(false);
			mainDialogueScript.ClearDisplay();
		} else {
			altDialoguePanel.SetActive(false);
			altDialogueScript.ClearDisplay();
		}
	}
}