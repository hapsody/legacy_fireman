using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	public GameObject _characterWindow;
	public Button _characterIcon;
	public Image _coolTimeGuage;
	public float timer=0;

	// Use this for initialization
	void Start () {
		StartCoroutine ("GuageCharger");
	}

	IEnumerator GuageCharger ()
	{
		while (true) {
			if (timer < 1) {
				timer += (Time.deltaTime * 0.01f);
				_coolTimeGuage.fillAmount = Mathf.Lerp (_coolTimeGuage.fillAmount, 1f, timer);
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	// Update is called once per frame
	void Update () {
		


	}
}
