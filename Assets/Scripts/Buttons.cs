using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public Button btnAll;
    public Button btnOdd;
    public Button btnEven;

    // Start is called before the first frame update
    void Start()
    {
        btnAll.GetComponent<BtnSettings>().OnClick();
		btnOdd.GetComponent<BtnSettings>().OffClick();
		btnEven.GetComponent<BtnSettings>().OffClick();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickBtnAll()
    {
		btnAll.GetComponent<BtnSettings>().OnClick();
		btnOdd.GetComponent<BtnSettings>().OffClick();
		btnEven.GetComponent<BtnSettings>().OffClick();
	}

	public void ClickBtnOdd()
	{
		btnAll.GetComponent<BtnSettings>().OffClick();
		btnOdd.GetComponent<BtnSettings>().OnClick();
		btnEven.GetComponent<BtnSettings>().OffClick();
	}

	public void ClickBtnEven()
	{
		btnAll.GetComponent<BtnSettings>().OffClick();
		btnOdd.GetComponent<BtnSettings>().OffClick();
		btnEven.GetComponent<BtnSettings>().OnClick();
	}
}
