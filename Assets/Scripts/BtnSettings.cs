using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSettings : MonoBehaviour
{
    public GameObject line;
    public Text text;
    Color colorText;

	private void Awake()
	{
		colorText = text.color;
	}

    public void OnClick()
    {
        line.SetActive(true);
        text.color = line.GetComponent<Image>().color;
	}

    public void OffClick()
    {
		line.SetActive(false);
        text.color = colorText;
	}
}
