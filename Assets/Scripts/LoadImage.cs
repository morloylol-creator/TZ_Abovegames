using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{
	public GameObject galleryAll;
	public GameObject galleryOdd;
	public GameObject galleryEven;
	public string imageUrl;
	public RawImage rawImage;

	List<RawImage> imagesAll = new List<RawImage>(0);
	List<RawImage> imagesOdd = new List<RawImage>(0);
	List<RawImage> imagesEven = new List<RawImage>(0);

	List<Vector3> positionGallety = new List<Vector3>(0);
	List<RawImage> destroyList = new List<RawImage>(0);
	List<Vector3> currentPosImages = new List<Vector3>(0);
	List<Vector3> listNewPosImages = new List<Vector3>(0);

	int row;
	int column;

	public bool moveAll;
	public bool moveOdd;
	public bool moveEven;
	//bool checkMove;
	float tAll;
	float tOdd;
	float tEven;
	string direction = "";
	Vector2 posTap;
	Vector2 posTapUp;
	public GameObject upLineGallery;
	string tabBar = "";

	int numberAll;
	int numberOdd;
	int numberEven;

	bool tapImage;
	bool forwardMove;
	bool backMove;
	GameObject tapObj;
	Vector3 startPositionTapObj;
	float tObj;
	public GameObject bgForImage;
	RawImage forwardImage;

	public GameObject premiumPopup;
	float tPrem;
	bool movePrem;
	bool upPrem;
	bool downPrem;

	bool screensaver;
	public GameObject screensaverUI;
	public Animation screensaverAnim;

	public GameObject banner;

	public int loadAll;
	public int loadOdd;
	public int loadEven;

	void Start()
	{
		screensaver = true;
		screensaverAnim.Play();

		CreatePositionForGallery();

		numberAll = 1;
		numberOdd = 1;
		numberEven = 2;

		galleryAll.SetActive(true);
		galleryOdd.SetActive(false);
		galleryEven.SetActive(false);
		tabBar = "all";
		imageUrl = "https://data.ikppbb.com/test-task-unity-data/pics/";

		//checkMove = true;
		moveAll = false;
		moveOdd = false;
		moveEven = false;
		tAll = 0;
		tOdd = 0;
		tEven = 0;

		tapImage = false;
		bgForImage.SetActive(false);

		StartCoroutine(CreateGalleryAll());
		StartCoroutine(CreateGalleryOdd());
		StartCoroutine(CreateGalleryEven());

		loadAll = 0;
		loadOdd = 0;
		loadEven = 0;
	}

	IEnumerator DownloadImage(string MediaUrl, GameObject gallery, List<RawImage> images, string name)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log(request.error);
		}
		else
		{
			rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			RawImage clone = Instantiate(rawImage);
			clone.transform.SetParent(gallery.transform);
			clone.transform.localScale = Vector3.one;
			clone.name = name;
			images.Add(clone);
			clone.transform.localPosition = positionGallety[images.Count - 1 + column];
		}
	}
	IEnumerator DownloadImagePosUp(string MediaUrl, GameObject gallery, List<RawImage> images, Vector3 pos, string name)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log(request.error);
			//checkMove = false;
		}
		else
		{
			rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			RawImage clone = Instantiate(rawImage);
			clone.transform.SetParent(gallery.transform);
			clone.transform.localScale = Vector3.one;
			clone.name = name;
			images.Add(clone);
			clone.transform.localPosition = pos;
		}

		if(gallery.name == galleryAll.name)
		{
			loadAll--;
		}
		if (gallery.name == galleryOdd.name)
		{
			loadOdd--;
		}
		if (gallery.name == galleryEven.name)
		{
			loadEven--;
		}
	}
	IEnumerator DownloadImagePosDown(string MediaUrl, GameObject gallery, List<RawImage> images, Vector3 pos, string name)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log(request.error);
			//checkMove = false;
		}
		else
		{
			rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			RawImage clone = Instantiate(rawImage);
			clone.transform.SetParent(gallery.transform);
			clone.transform.localScale = Vector3.one;
			clone.name = name;
			images.Insert(int.Parse(name) - 1, clone);
			clone.transform.localPosition = pos;
		}

		if (gallery.name == galleryAll.name)
		{
			loadAll--;
		}
		if (gallery.name == galleryOdd.name)
		{
			loadOdd--;
		}
		if (gallery.name == galleryEven.name)
		{
			loadEven--;
		}
	}

	// Update is called once per frame
	void Update()
    {

		/*if (column != 0 && imagesAll.Count >= 4 * column && !screensaver)
		{
			screensaverUI.SetActive(false);
			banner.GetComponent<Banners>().bannerOn = true;
		}*/
		
		CheckPremiumAll();
		CheckPremiumOdd();
		CheckPremiumEven();
		if (!screensaverAnim.isPlaying && imagesAll.Count == row * column)
		{
			screensaver = false;
			screensaverUI.SetActive(false);
			banner.GetComponent<Banners>().bannerOn = true;
		}
		if (!screensaver)
		{
			SwapImages();

			if (moveAll)
			{
				tAll += Time.deltaTime;
				if(direction == "up")
				{
					StartMoveUpAll(tAll);
				}
				else if(direction == "down")
				{
					StartMoveDownAll(tAll);
				}
			}
			if(tAll >= 1)
			{
				moveAll = false;
				tAll = 0;

				if (direction == "up")
				{
					CorrectionGalleryImagesAllUp();
				}
				if (direction == "down")
				{
					CorrectionGalleryImagesAllDown();
				}
			}


			if (moveOdd)
			{
				tOdd += Time.deltaTime;
				if (direction == "up")
				{
					StartMoveUpOdd(tOdd);
				}
				else if (direction == "down")
				{
					StartMoveDownOdd(tOdd);
				}
			}
			if (tOdd >= 1)
			{
				moveOdd = false;
				tOdd = 0;

				if (direction == "up")
				{
					CorrectionGalleryImagesOddUp();
				}
				if (direction == "down")
				{
					CorrectionGalleryImagesOddDown();
				}
			}

			if (moveEven)
			{
				tEven += Time.deltaTime;
				if (direction == "up")
				{
					StartMoveUpEven(tEven);
				}
				else if (direction == "down")
				{
					StartMoveDownEven(tEven);
				}
			}
			if (tEven >= 1)
			{
				moveEven = false;
				tEven = 0;

				if (direction == "up")
				{
					CorrectionGalleryImagesEvenUp();
				}
				if (direction == "down")
				{
					CorrectionGalleryImagesEvenDown();
				}
			}

			/*if (move)
			{
				t += Time.deltaTime;
				if (direction == "up")
				{
					if (tabBar == "all")
					{
						StartMoveUpAll(t);
					}
					else if (tabBar == "odd")
					{
						StartMoveUpOdd(t);
					}
					else if (tabBar == "even")
					{
						StartMoveUpEven(t);
					}
				}
				else if (direction == "down")
				{
					if (tabBar == "all")
					{
						StartMoveDownAll(t);
					}
					else if (tabBar == "odd")
					{
						StartMoveDownOdd(t);
					}
					else if (tabBar == "even")
					{
						StartMoveDownEven(t);
					}
				}

				if (t >= 1)
				{
					move = false;
					t = 0;
					if (direction == "up")
					{
						if (tabBar == "all")
						{
							CorrectionGalleryImagesAllUp();
						}
						else if (tabBar == "odd")
						{
							CorrectionGalleryImagesOddUp();
						}
						else if (tabBar == "even")
						{
							CorrectionGalleryImagesEvenUp();
						}

					}
					else if (direction == "down")
					{
						if (tabBar == "all")
						{
							CorrectionGalleryImagesAllDown();
						}
						else if (tabBar == "odd")
						{
							CorrectionGalleryImagesOddDown();
						}
						else if (tabBar == "even")
						{
							CorrectionGalleryImagesEvenDown();
						}

					}
				}
			}*/

			if (tapImage)
			{
				if (forwardMove)
				{
					tObj += Time.deltaTime * 5;
					MoveTapObjectForward(tObj);
					if (tObj > 0.2f)
					{
						bgForImage.SetActive(true);
					}
					if (tObj >= 1)
					{
						tObj = 1;
						MoveTapObjectForward(tObj);
						forwardMove = false;
					}
				}
				else if (backMove)
				{
					tObj += Time.deltaTime * 5;
					MoveTapObjectBack(tObj);
					if (tObj > 0.7f)
					{
						bgForImage.SetActive(false);
						if (forwardImage != null)
						{
							Destroy(forwardImage.gameObject);
						}
					}
					if (tObj >= 1)
					{
						MoveTapObjectBack(tObj);
						backMove = false;
						tapImage = false;
					}
				}
			}

			if (movePrem)
			{
				tPrem += Time.deltaTime * 2;
				if (upPrem)
				{
					if (tPrem >= 1)
					{
						tPrem = 1;
						upPrem = false;
						movePrem = false;
					}
					MovePremUp(tPrem);
				}
				if (downPrem)
				{
					if (tPrem >= 1)
					{
						tPrem = 1;
						downPrem = false;
						premiumPopup.SetActive(false);
						movePrem = false;
					}
					MovePremDown(tPrem);

				}
			}
		}


	}

	IEnumerator CreateGalleryAll()
	{
		row = 5;
		if (Camera.main.pixelWidth > 1900 && Camera.main.pixelWidth < 2000)
		{
			column = 3;
		}
		else if (Camera.main.pixelWidth >= 2000)
		{
			column = 4;
		}
		else
		{
			column = 2;
		}
		int n = 1;

		while (n <= row * column)
		{
			yield return StartCoroutine(DownloadImage(imageUrl + n + ".jpg", galleryAll, imagesAll, n.ToString()));
			numberAll = n;
			n++;
		}

		/*float posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		float posY = 270;

		int numImage = 0;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < column; j++)
			{
				imagesAll[numImage].transform.localPosition = new Vector3(posX, posY, 0);
				numImage++;
				posX += 640 + 40;
			}
			posY -= 640 + 40;
			posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		}*/
	}

	IEnumerator CreateGalleryOdd()
	{
		row = 5;
		if (Camera.main.pixelWidth > 1900 && Camera.main.pixelWidth < 2000)
		{
			column = 3;
		}
		else if (Camera.main.pixelWidth >= 2000)
		{
			column = 4;
		}
		else
		{
			column = 2;
		}
		int n = 1;
		int number = 1;

		while (n <= row * column)
		{
			yield return StartCoroutine(DownloadImage(imageUrl + number + ".jpg", galleryOdd, imagesOdd, number.ToString()));
			numberOdd = number;
			number += 2;
			n++;
		}

		/*float posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		float posY = 270;

		int numImage = 0;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < column; j++)
			{
				imagesOdd[numImage].transform.localPosition = new Vector3(posX, posY, 0);
				numImage++;
				posX += 640 + 40;
			}
			posY -= 640 + 40;
			posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		}*/
	}

	IEnumerator CreateGalleryEven()
	{
		row = 5;
		if (Camera.main.pixelWidth > 1900 && Camera.main.pixelWidth < 2000)
		{
			column = 3;
		}
		else if (Camera.main.pixelWidth >= 2000)
		{
			column = 4;
		}
		else
		{
			column = 2;
		}
		int n = 1;
		int number = 2;

		while (n <= row * column)
		{
			yield return StartCoroutine(DownloadImage(imageUrl + number + ".jpg", galleryEven, imagesEven, number.ToString()));
			numberEven = number;
			number += 2;
			n++;
		}

		/*float posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		float posY = 270;

		int numImage = 0;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < column; j++)
			{
				imagesEven[numImage].transform.localPosition = new Vector3(posX, posY, 0);
				numImage++;
				posX += 640 + 40;
			}
			posY -= 640 + 40;
			posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		}*/
	}

	public void ButtonAll()
	{
		if (tabBar != "all")
		{
			galleryAll.SetActive(true);
			galleryOdd.SetActive(false);
			galleryEven.SetActive(false);

			/*gAll.clip = leftToNull;
			if (tabBar == "Odd")
			{
				gOdd.clip = nullToLeft;
				gOdd.Play();
			}
			if (tabBar == "Even")
			{
				gEven.clip = nullToLeft;
				gEven.Play();
			}

			gAll.Play();*/
			tabBar = "all";
		}
	}

	public void ButtonOdd()
	{
		if (tabBar != "odd")
		{
			galleryAll.SetActive(false);
			galleryOdd.SetActive(true);
			galleryEven.SetActive(false);

			/*if(tabBar == "all")
			{
				gAll.clip = nullToLeft;
				gAll.Play();
				gOdd.clip = rightToNull;
				gOdd.Play();
			}
			if(tabBar == "even")
			{
				gEven.clip = nullToRight;
				gEven.Play();
				gOdd.clip = leftToNull;
				gOdd.Play();
			}*/

			tabBar = "odd";
		}
	}

	public void ButtonEven()
	{
		if (tabBar != "even")
		{
			galleryAll.SetActive(false);
			galleryOdd.SetActive(false);
			galleryEven.SetActive(true);

			/*if(tabBar == "all")
			{
				gAll.clip = nullToLeft;
				gAll.Play();
			}
			if (tabBar == "Odd")
			{
				gOdd.clip = nullToLeft;
				gOdd.Play();
			}
			gEven.clip = rightToNull;
			gEven.Play();*/

			tabBar = "even";
		}
	}

	void CreatePositionForGallery()
	{
		positionGallety = new List<Vector3>(0);

		if (Camera.main.pixelWidth > 580 * 3 && Camera.main.pixelWidth < 640 * 3)
		{
			column = 3;
		}
		else if (Camera.main.pixelWidth >= 640 * 3)
		{
			column = 4;
		}
		else
		{
			column = 2;
		}

		float posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		float posY = 270 + 640 + 40;
		
		row = 6;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < column; j++)
			{
				positionGallety.Add(new Vector3(posX, posY, 0));
				posX += 640 + 40;
			}
			posY -= 640 + 40;
			posX = 0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2;
		}
	}

	void SwapImages()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < upLineGallery.transform.position.y && !tapImage)
		{
			posTap = Input.mousePosition;

			//Debug.Log("tap");
		}
		if (Input.GetKeyUp(KeyCode.Mouse0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < upLineGallery.transform.position.y && !tapImage)
		{
			posTapUp = Input.mousePosition;
			//Debug.Log("noTap");
			if (posTap.y + Camera.main.pixelWidth / 10 < posTapUp.y)
			{
				
				if (tabBar == "all" && !moveAll && loadAll == 0)
				{
					direction = "up";
					MoveUpAll();
					moveAll = true;
					//checkMove = true;
				}
				else if (tabBar == "odd" && !moveOdd && loadOdd == 0)
				{
					direction = "up";
					MoveUpOdd();
					moveOdd = true;
					//checkMove = true;
				}
				else if (tabBar == "even" && !moveEven && loadEven == 0)
				{
					direction = "up";
					MoveUpEven();
					moveEven = true;
					//checkMove = true;
				}
			}
			else if (posTap.y > posTapUp.y + Camera.main.pixelWidth / 10)
			{
				
				if (tabBar == "all" && imagesAll[0].transform.localPosition.y > 270 && !moveAll && loadAll == 0)
				{
					direction = "down";
					MoveDownAll();
					moveAll = true;
				}
				else if (tabBar == "odd" && imagesOdd[0].transform.localPosition.y > 270 && !moveOdd && loadOdd == 0)
				{
					direction = "down";
					MoveDownOdd();
					moveOdd = true;
				}
				else if (tabBar == "even" && imagesEven[0].transform.localPosition.y > 270 && !moveEven && loadEven == 0)
				{
					direction = "down";
					MoveDownEven();
					moveEven = true;
				}
			}
			else if (Vector3.Distance(posTap, posTapUp) < 10)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform.GetComponent<SettingImage>())
					{
						if (hit.transform.GetComponent<SettingImage>().premium)
						{
							premiumPopup.SetActive(true);
							upPrem = true;
							tPrem = 0;
							movePrem = true;
						}
						else
						{
							tObj = 0;
							forwardImage = Instantiate(hit.transform.gameObject.GetComponent<RawImage>());
							forwardImage.transform.SetParent(bgForImage.transform);
							tapImage = true;
							forwardMove = true;
							tapObj = hit.transform.gameObject;
							startPositionTapObj = hit.transform.localPosition;
						}
					}
				}
			}
			else
			{
				direction = "";
			}
		}
	}

	void MoveUpAll()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesAll.Count; i++)
		{
			currentPosImages.Add(imagesAll[i].transform.localPosition);
			listNewPosImages.Add(currentPosImages[i] + new Vector3(0, 640 + 40, 0));
			if (imagesAll[i].transform.localPosition.y > 270 + 640 + 40)
			{
				destroyList.Add(imagesAll[i]);
			}
		}
	}

	void StartMoveUpAll(float t)
	{
		for(int i = 0; i < imagesAll.Count;i++)
		{
			imagesAll[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void CorrectionGalleryImagesAllUp()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesAll.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int max = int.Parse(imagesAll[0].name);
		for (int i = 0; i < imagesAll.Count; i++)
		{
			if (int.Parse(imagesAll[i].name) > max)
			{
				max = int.Parse(imagesAll[i].name);
			}
		}

		loadAll = column;
		if (column == 2)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
		}
		if (column == 3)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
		}
		if (column == 4)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, -2450, 0), max.ToString()));
		}

	}

	void CorrectionGalleryImagesAllDown()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesAll.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int min = int.Parse(imagesAll[0].name);
		for(int i = 0; i < imagesAll.Count; i++)
		{
			if (int.Parse(imagesAll[i].name) < min)
			{
				min = int.Parse(imagesAll[i].name);
			}
		}

		if (min > 2)
		{
			loadAll = column;

			if (column == 2)
			{
				numberAll = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberAll.ToString()));
			}
			if (column == 3)
			{
				numberAll = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 + (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberAll.ToString()));
			}
			if (column == 4)
			{
				numberAll = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberAll.ToString()));
				numberAll--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberAll + ".jpg", galleryAll, imagesAll, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, 1630, 0), numberAll.ToString()));

			}
		}
	}

	void MoveDownAll()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesAll.Count; i++)
		{
			currentPosImages.Add(imagesAll[i].transform.localPosition);
			listNewPosImages.Add(imagesAll[i].transform.localPosition - new Vector3(0, 640 + 40, 0));
			if (imagesAll[i].transform.localPosition.y < -1770)
			{
				destroyList.Add(imagesAll[i]);
			}
		}
	}

	void StartMoveDownAll(float t)
	{
		for (int i = 0; i < imagesAll.Count; i++)
		{
			imagesAll[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void MoveUpOdd()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			currentPosImages.Add(imagesOdd[i].transform.localPosition);
			listNewPosImages.Add(currentPosImages[i] + new Vector3(0, 640 + 40, 0));
			if (imagesOdd[i].transform.localPosition.y > 270 + 640 + 40)
			{
				destroyList.Add(imagesOdd[i]);
			}
		}
	}

	void StartMoveUpOdd(float t)
	{
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			imagesOdd[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void CorrectionGalleryImagesOddUp()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesOdd.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int max = int.Parse(imagesOdd[0].name);
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			if (int.Parse(imagesOdd[i].name) > max)
			{
				max = int.Parse(imagesOdd[i].name);
			}
		}

		loadOdd = column;

		if(column == 2)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
		}
		if (column == 3)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
		}
		if (column == 4)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, -2450, 0), max.ToString()));
		}

	}

	void CorrectionGalleryImagesOddDown()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesOdd.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int min = int.Parse(imagesOdd[0].name);
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			if (int.Parse(imagesOdd[i].name) < min)
			{
				min = int.Parse(imagesOdd[i].name);
			}
		}

		if (min > 2)
		{
			loadOdd = column;

			if (column == 2)
			{
				numberOdd = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberOdd.ToString()));
			}
			if (column == 3)
			{
				numberOdd = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberOdd.ToString()));
			}
			if (column == 4)
			{
				numberOdd = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberOdd.ToString()));
				numberOdd--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberOdd + ".jpg", galleryOdd, imagesOdd, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, 1630, 0), numberOdd.ToString()));
			}
		}
	}

	void MoveDownOdd()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			currentPosImages.Add(imagesOdd[i].transform.localPosition);
			listNewPosImages.Add(imagesOdd[i].transform.localPosition - new Vector3(0, 640 + 40, 0));
			if (imagesOdd[i].transform.localPosition.y < -1770)
			{
				destroyList.Add(imagesOdd[i]);
			}
		}
	}

	void StartMoveDownOdd(float t)
	{
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			imagesOdd[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void MoveUpEven()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesEven.Count; i++)
		{
			currentPosImages.Add(imagesEven[i].transform.localPosition);
			listNewPosImages.Add(currentPosImages[i] + new Vector3(0, 640 + 40, 0));
			if (imagesEven[i].transform.localPosition.y > 270 + 640 + 40)
			{
					destroyList.Add(imagesEven[i]);
			}
		}
	}

	void StartMoveUpEven(float t)
	{
		for (int i = 0; i < imagesEven.Count; i++)
		{
			imagesEven[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void CorrectionGalleryImagesEvenUp()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesEven.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int max = int.Parse(imagesEven[0].name);
		for (int i = 0; i < imagesEven.Count; i++)
		{
			if (int.Parse(imagesEven[i].name) > max)
			{
				max = int.Parse(imagesEven[i].name);
			}
		}

		loadEven = column;

		if(column == 2)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
		}
		if (column == 3)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
		}
		if (column == 4)
		{
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, -2450, 0), max.ToString()));
			max++;
			StartCoroutine(DownloadImagePosUp(imageUrl + max + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, -2450, 0), max.ToString()));
		}

	}

	void CorrectionGalleryImagesEvenDown()
	{
		for (int i = 0; i < destroyList.Count; i++)
		{
			imagesEven.Remove(destroyList[i]);
			Destroy(destroyList[i].gameObject);
		}

		int min = int.Parse(imagesEven[0].name);
		for (int i = 0; i < imagesEven.Count; i++)
		{
			if (int.Parse(imagesEven[i].name) < min)
			{
				min = int.Parse(imagesEven[i].name);
			}
		}

		if (min > 2)
		{
			loadEven = column;

			if(column == 2)
			{
				numberEven = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberEven.ToString()));
			}
			if (column == 3)
			{
				numberEven = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberEven.ToString()));
			}
			if (column == 4)
			{
				numberEven = min - 1;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680, 1630, 0), numberEven.ToString()));
				numberEven--;
				StartCoroutine(DownloadImagePosDown(imageUrl + numberEven + ".jpg", galleryEven, imagesEven, new Vector3(0 - (column - 1) * 640 / 2 - (column - 1) * 40 / 2 + 680 + 680 + 680, 1630, 0), numberEven.ToString()));
			}
		}
	}

	void MoveDownEven()
	{
		destroyList = new List<RawImage>(0);
		currentPosImages = new List<Vector3>(0);
		listNewPosImages = new List<Vector3>(0);
		for (int i = 0; i < imagesEven.Count; i++)
		{
			currentPosImages.Add(imagesEven[i].transform.localPosition);
			listNewPosImages.Add(imagesEven[i].transform.localPosition - new Vector3(0, 640 + 40, 0));
			if (imagesEven[i].transform.localPosition.y < -1770)
			{
				destroyList.Add(imagesEven[i]);
			}
		}
	}

	void StartMoveDownEven(float t)
	{
		for (int i = 0; i < imagesEven.Count; i++)
		{
			imagesEven[i].transform.localPosition = Vector3.Lerp(currentPosImages[i], listNewPosImages[i], t);
		}
	}

	void MoveTapObjectForward(float t)
	{
		tapObj.transform.localPosition = Vector3.Lerp(startPositionTapObj, new Vector3(0, -300, 0), t);
		tapObj.transform.localScale = new Vector3(1, 1, 1) * (t * 2.2f);
		forwardImage.transform.localPosition = tapObj.transform.localPosition;
		forwardImage.transform.localScale = tapObj.transform.localScale;
	}
	void MoveTapObjectBack(float t)
	{
		tapObj.transform.localPosition = Vector3.Lerp(new Vector3(0, -300, 0), startPositionTapObj, t);
		tapObj.transform.localScale = new Vector3(2 - t, 2 - t, 2 - t);
		if(forwardImage != null)
		{
			forwardImage.transform.localPosition = tapObj.transform.localPosition;
			forwardImage.transform.localScale = tapObj.transform.localScale;
		}
	}

	public void BackMenu()
	{
		backMove = true;
		tObj = 0;
	}

	void CheckPremiumAll()
	{
		for(int i = 0; i < imagesAll.Count; i++)
		{
			if (int.Parse(imagesAll[i].name) % 4 == 0)
			{
				imagesAll[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(true);
				imagesAll[i].GetComponent<SettingImage>().premium = true;
			}
			else
			{
				imagesAll[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(false);
			}
		}
	}

	void CheckPremiumOdd()
	{
		for (int i = 0; i < imagesOdd.Count; i++)
		{
			if ((int.Parse(imagesOdd[i].name) + 1) % 8 == 0)
			{
				imagesOdd[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(true);
				imagesOdd[i].GetComponent<SettingImage>().premium = true;
			}
			else
			{
				imagesOdd[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(false);
			}
		}
	}

	void CheckPremiumEven()
	{
		for (int i = 0; i < imagesEven.Count; i++)
		{
			if (int.Parse(imagesEven[i].name) % 8 == 0)
			{
				imagesEven[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(true);
				imagesEven[i].GetComponent<SettingImage>().premium = true;
			}
			else
			{
				imagesEven[i].GetComponent<SettingImage>().premImage.gameObject.SetActive(false);
			}
		}
	}

	public void ButtonBackPremiumPopup()
	{
		//premiumPopup.SetActive(false);
		downPrem = true;
		tPrem = 0;
		movePrem = true;
	}

	void MovePremUp(float t)
	{
		premiumPopup.transform.localPosition = Vector3.Lerp(new Vector3(0, -3500, 0), new Vector3(0, 0, 0), t);
	}
	void MovePremDown(float t)
	{
		premiumPopup.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -3500, 0), t);
	}

}
