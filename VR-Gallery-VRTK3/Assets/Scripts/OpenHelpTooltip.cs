using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OpenHelpTooltip : MonoBehaviour {

	[SerializeField] GameObject prefab;
	GameObject panel;
	void Awake () {
		panel = Instantiate(prefab, transform, false);
		panel.SetActive(false);
		transform.parent.GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += TogglePanel;
		
	}

	void TogglePanel(object o, ControllerInteractionEventArgs e){
		panel.SetActive(!panel.activeSelf);
	}
}
