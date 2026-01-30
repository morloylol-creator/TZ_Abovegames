using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banners : MonoBehaviour
{
    public GameObject banner;
    public List<GameObject> allBanners;
    public List<GameObject> banners;
	public List<Image> pagenations = new List<Image>(0);
	int currentNumberBannerInList;
	public Image pageCurrent;
    public Image pageNotCurrent;
	public GameObject downLineBanner;
	public GameObject pointImage;
    GameObject centerBanner;
    GameObject leftBanner;
    GameObject leftBannerTwo;
    GameObject rightBanner;
    GameObject rightBannerTwo;

	bool move = false;
	float t;
	string direction = "";

	Vector2 posTap;
	Vector2 posTapUp;

	float timeBanner = 5;
	public bool bannerOn;
	List<GameObject> bannersInLine = new List<GameObject>(0);

	void Start()
    {
		bannerOn = false;
		currentNumberBannerInList = 0;

		if (allBanners.Count > 2)
        {
			leftBannerTwo = allBanners[allBanners.Count - 2];
			leftBanner = allBanners[allBanners.Count - 1];
			centerBanner = allBanners[0];
			rightBanner = allBanners[1];
			rightBannerTwo = allBanners[2];
			InstallationBanners();
		}
        else if (allBanners.Count == 2)
        {
			leftBannerTwo = allBanners[allBanners.Count - 2];
			leftBanner = allBanners[allBanners.Count - 1];
			centerBanner = allBanners[0];
			rightBanner = allBanners[allBanners.Count - 1];
			rightBannerTwo = allBanners[allBanners.Count - 2];
			InstallationBanners();
		}
		banners[0].transform.localPosition = new Vector3(-2880, 1160, 0);
		banners[1].transform.localPosition = new Vector3(-1440, 1160, 0);
		banners[2].transform.localPosition = new Vector3(0, 1160, 0);
		banners[3].transform.localPosition = new Vector3(1440, 1160, 0);
		banners[4].transform.localPosition = new Vector3(2880, 1160, 0);
		t = 0;

		CreatePointPage();
		CheckPage();
	}

    // Update is called once per frame
    void Update()
    {
		CheckPosTap();
		if ((move || timeBanner <= 0) && bannerOn)
		{
			if (timeBanner <= 0)
			{
				direction = "left";
			}
			t += Time.deltaTime;

			if(direction == "left")
			{
				MoveLeft();
			}
			else if (direction == "right")
			{
				MoveRigh();
			}

			if (t > 1)
			{
				t = 1;
				move = false;
				if (direction == "left")
				{
					MoveBannerLeft();
				}
				else if (direction == "right")
				{
					MoveBannerRight();
				}
				CheckPage();
				timeBanner = 5;
				t = 0;
			}
			
		}
		if (!move)
		{
			timeBanner -= Time.deltaTime;
		}
		
	}

    void CheckPosTap()
    {
		if (allBanners.Count > 1)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) && !move && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > downLineBanner.transform.position.y)
			{
				posTap = Input.mousePosition;
			}
			if (Input.GetKeyUp(KeyCode.Mouse0) && !move && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > downLineBanner.transform.position.y)
			{
				posTapUp = Input.mousePosition;

				if (posTap.x + Camera.main.pixelWidth / 10 < posTapUp.x)
				{
					direction = "right";
				}
				else if (posTap.x > posTapUp.x + Camera.main.pixelWidth / 10)
				{
					direction = "left";
				}
				else
				{
					direction = "";
				}
				move = true;
			}
		}
	}

	void InstallationBanners()
	{
		for(int i = 0; i < banners.Count; i++)
		{
			Destroy(banners[i].gameObject);
		}
		
		banners = new List<GameObject>(0);
		banners.Add(Instantiate(leftBannerTwo));
		banners.Add(Instantiate(leftBanner));
		banners.Add(Instantiate(centerBanner));
		banners.Add(Instantiate(rightBanner));
		banners.Add(Instantiate(rightBannerTwo));

		for(int i = 0; i < banners.Count; i++)
		{
			banners[i].transform.SetParent(banner.transform);
			banners[i].transform.localScale = Vector3.one;
		}

		banners[0].transform.localPosition = new Vector3(-2880, 1160, 0);
		banners[1].transform.localPosition = new Vector3(-1440, 1160, 0);
		banners[2].transform.localPosition = new Vector3(0, 1160, 0);
		banners[3].transform.localPosition = new Vector3(1440, 1160, 0);
		banners[4].transform.localPosition = new Vector3(2880, 1160, 0);
	}

	void MoveLeft()
	{
		banners[0].transform.localPosition = new Vector3(-2880, 1160, 0);
		banners[1].transform.localPosition = Vector3.Lerp(new Vector3(-1440, 1160, 0), new Vector3(-2880, 1160, 0), t);
		banners[2].transform.localPosition = Vector3.Lerp(new Vector3(0, 1160, 0), new Vector3(-1440, 1160, 0), t);
		banners[3].transform.localPosition = Vector3.Lerp(new Vector3(1440, 1160, 0), new Vector3(0, 1160, 0), t);
		banners[4].transform.localPosition = Vector3.Lerp(new Vector3(2880, 1160, 0), new Vector3(1440, 1160, 0), t);
	}
	void MoveRigh()
	{
		banners[0].transform.localPosition = Vector3.Lerp(new Vector3(-2880, 1160, 0), new Vector3(-1440, 1160, 0), t);
		banners[1].transform.localPosition = Vector3.Lerp(new Vector3(-1440, 1160, 0), new Vector3(0, 1160, 0), t);
		banners[2].transform.localPosition = Vector3.Lerp(new Vector3(0, 1160, 0), new Vector3(1440, 1160, 0), t);
		banners[3].transform.localPosition = Vector3.Lerp(new Vector3(1440, 1160, 0), new Vector3(2880, 1160, 0), t);
		banners[4].transform.localPosition = new Vector3(2880, 1160, 0);
	}

	void MoveBannerLeft()
	{
		currentNumberBannerInList++;
		
		if(currentNumberBannerInList > allBanners.Count - 1)
		{
			currentNumberBannerInList = 0;
		}
		centerBanner = allBanners[currentNumberBannerInList];

		if(currentNumberBannerInList - 1 < 0)
		{
			leftBanner = allBanners[allBanners.Count - 1];
		}
		else
		{
			leftBanner = allBanners[currentNumberBannerInList - 1];
		}
		if (currentNumberBannerInList - 2 < 0)
		{
			leftBannerTwo = allBanners[allBanners.Count - (2 - currentNumberBannerInList)];
		}
		else
		{
			leftBannerTwo = allBanners[currentNumberBannerInList - 2];
		}
		if (currentNumberBannerInList + 1 > allBanners.Count - 1)
		{
			rightBanner = allBanners[0];
		}
		else
		{
			rightBanner = allBanners[currentNumberBannerInList + 1];
		}
		if (currentNumberBannerInList + 2 > allBanners.Count - 1)
		{
			rightBannerTwo = allBanners[System.Math.Abs(allBanners.Count - (currentNumberBannerInList + 2))];
		}
		else
		{
			rightBannerTwo = allBanners[currentNumberBannerInList + 2];
		}

		InstallationBanners();
	}
	void MoveBannerRight()
	{

		currentNumberBannerInList--;

		if (currentNumberBannerInList < 0)
		{
			currentNumberBannerInList = allBanners.Count - 1;
		}
		centerBanner = allBanners[currentNumberBannerInList];

		if (currentNumberBannerInList - 1 < 0)
		{
			leftBanner = allBanners[allBanners.Count - 1];
		}
		else
		{
			leftBanner = allBanners[currentNumberBannerInList - 1];
		}
		if (currentNumberBannerInList - 2 < 0)
		{
			leftBannerTwo = allBanners[allBanners.Count - (2 - currentNumberBannerInList)];
		}
		else
		{
			leftBannerTwo = allBanners[currentNumberBannerInList - 2];
		}
		if (currentNumberBannerInList + 1 > allBanners.Count - 1)
		{
			rightBanner = allBanners[0];
		}
		else
		{
			rightBanner = allBanners[currentNumberBannerInList + 1];
		}
		if (currentNumberBannerInList + 2 > allBanners.Count - 1)
		{
			rightBannerTwo = allBanners[System.Math.Abs(allBanners.Count - (currentNumberBannerInList + 2))];
		}
		else
		{
			rightBannerTwo = allBanners[currentNumberBannerInList + 2];
		}
		
		InstallationBanners();
	}

	void CheckPage()
	{
		for (int i = 0; i < pagenations.Count; i++)
		{
			
			if (currentNumberBannerInList == i)
			{
				pagenations[i].GetComponent<Image>().sprite = pageCurrent.GetComponent<Image>().sprite;
			}
			else
			{
				pagenations[i].GetComponent<Image>().sprite = pageNotCurrent.GetComponent<Image>().sprite;
			}
		}
	}

	void CreatePointPage()
	{
		pagenations = new List<Image>(0);
		float posX = 0 - 7 - float.Parse(allBanners.Count.ToString()) * (25 + 7) / 2 + (25 + 7) / 2;
		for (int i = 0; i < allBanners.Count; i++)
		{
			Image clone;
			if (i == currentNumberBannerInList)
			{
				clone = Instantiate(pageCurrent);
			}
			else
			{
				clone = Instantiate(pageNotCurrent);
			}
			 
			clone.transform.SetParent(pointImage.transform);
			clone.transform.localScale = Vector3.one;
			clone.transform.localPosition = new Vector3(posX, 0, 0);
			pagenations.Add(clone);
			posX += 25 + 14;
		}
	}
}
