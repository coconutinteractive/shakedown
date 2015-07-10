using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class UI_DisplayText : MonoBehaviour 
{
	private string textToDisplay = "bla bla";
	private List<string> linesToDisplay = new List<string>();
	[SerializeField] private string baseText = "";
	[SerializeField] private TextMesh textMeshPrefab = null; 
	[SerializeField] private Vector3 offset = Vector2.zero;
	[SerializeField] private Vector3 inLineOffset = Vector2.zero;
	private TextMesh baseTextMesh = null, currentTextMesh = null;
	private List<TextMesh> currentLinesMeshes = new List<TextMesh>();
	private bool isDisplaying = false;
	private GameObject playerObj = null;

	private void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag("Player");	
	}

	private void Update()
	{

	}

	public void SetText(string _text)
	{
		textToDisplay = _text;
	}

	public void SetText(string[] _lines)
	{
		linesToDisplay.Clear ();
		foreach (string curLine in _lines) 
			linesToDisplay.Add(curLine);
	}

	public void DisplayText()
	{
		if(!isDisplaying)
		{
			Vector3 newOffset = offset;
			if(transform.position.x < playerObj.transform.position.x)
				newOffset.x *= -5;
			if(transform.position.z > playerObj.transform.position.z)
				newOffset.z *= -3.5f;


			isDisplaying = true;
			TextMesh newBaseTextMesh = TextMesh.Instantiate(textMeshPrefab, transform.position + newOffset, transform.rotation) as TextMesh;
			newBaseTextMesh.text = baseText;
			newBaseTextMesh.transform.Rotate (90.0f, 180.0f, 0.0f);
			baseTextMesh = newBaseTextMesh;
			
			if(linesToDisplay.Count > 0)
			{
				currentLinesMeshes.Clear();
				for (int i = 1; i < linesToDisplay.Count + 1; ++i) 
				{
					TextMesh newTextMesh = TextMesh.Instantiate(textMeshPrefab, transform.position + newOffset + (inLineOffset * i), transform.rotation) as TextMesh;
					newTextMesh.text = linesToDisplay[i - 1];
					newTextMesh.transform.Rotate (90.0f, 180.0f, 0.0f);
					currentLinesMeshes.Add(newTextMesh);
				}
			}
			else
			{
				TextMesh newTextMesh = TextMesh.Instantiate(textMeshPrefab, transform.position + newOffset + inLineOffset, transform.rotation) as TextMesh;
				newTextMesh.text = textToDisplay;
				newTextMesh.transform.Rotate (90.0f, 180.0f, 0.0f);
				currentTextMesh = newTextMesh;
			}
		}
	}

	public void HideText()
	{
		if(isDisplaying)
		{
			isDisplaying = false;
			if (baseTextMesh)
				Destroy (baseTextMesh.gameObject);
			if (currentTextMesh)
				Destroy (currentTextMesh.gameObject);

			if(currentLinesMeshes.Count > 0)
			{
				foreach (TextMesh item in currentLinesMeshes)
					Destroy (item.gameObject);
			}
		}
	}
}
