using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class RotateSelf : MonoBehaviour {
	
	//是否被拖拽//     
	private bool onDrag = false; 
	//旋转速度//    
	private float speed = 3f;  
	//阻尼速度//     
	private float tempSpeed; 
	//鼠标沿水平方向移动的增量//  
	private float axisX;  
	//鼠标沿竖直方向移动的增量//
	private float axisY;    
	//滑动距离（鼠标）
	private float cXY;     
	//球体属性
	private float r = 10f;
	private float size = 2.2f;
	private float distance = 10f;
	
	public bool isCity = false;

	public string currentCity;
	public string currentCityID;
	public ArrayList citys;
	public GameObject textObject;
	public Transform cameraTarget;
	public Hashtable allCityID;

	public bool isFind = false;
	public string findName = "";

	void Start() {
		allCityID = Constants.initCity (allCityID);
		setNewCity (Constants.proIds, false, "北京");
	}

	// 设置城市（省）
	public void setNewCity( string[] cityIds, bool isCity, string findName) {
		this.isCity = isCity;
		citys = new ArrayList ();
		foreach (Transform child in gameObject.transform) {
			Destroy(child.gameObject);
		}

		foreach (string cityId in cityIds) {
			citys.Add(new City(cityId,cityId));
		}
		StartCoroutine(createTextObj (citys.Count, findName));
	}
	
	IEnumerator createTextObj(float N, string findName)
	{
		float inc = Mathf.PI * (3 - Mathf.Sqrt (5));
		float off = 2 / N;
		for (int k = 0; k < (N); k++) {
			float y = k * off - 1 + (off / 2);
			float r = Mathf.Sqrt (1 - y * y);
			float phi = k * inc;
			Vector3 pos = new Vector3 ((Mathf.Cos (phi) * r * size), y * size, Mathf.Sin (phi) * r * size); 

			GameObject text = (GameObject)Instantiate(textObject, pos, Quaternion.identity);
			text.transform.parent = gameObject.transform;

			City city = citys[k] as City;
			if (!isCity) {
				city.setProName(Constants.pros[k]);
			}
			getWeather (city, text);

			yield return 0;
		}

		yield return 0;
		if (findName!="") {
			//Debug.Log(findName+" 222222222222222");
			findCity(findName);
		}
	}

	public void getWeather(City city, GameObject text)
	{
		//string url = "http://m.weather.com.cn/atad/" + city.cityId +".html";
		string url = "http://www.weather.com.cn/data/cityinfo/" + city.getCityID() +".html";
		StartCoroutine(GET(url,city,text));
	}
	
	//GET请求
	IEnumerator GET(string url, City city, GameObject text)
	{
	
		
			WWW www = new WWW (url);
			yield return www;

			//GET请求失败
			if (www.error != null)
			{
				//GameObject.FindWithTag("MainCamera").GetComponent<Camera>().labelText = Constants.Error;
				Debug.Log ("error is :" + www.error);
			} 
			else 
			{
				//GET请求成功
				//Debug.Log(www.text);
				city.parseJson (www.text);
				TextMesh tm = (TextMesh)text.GetComponent<TextMesh> ();
				ChangeColor changeColor = text.GetComponent<ChangeColor>();
				if (isCity) {
					tm.text = city.getName ();
					changeColor.name = city.getName();
					changeColor.id = city.getCityID();
					changeColor.tempture = city.getTempture();
				}
				else 
				{
					tm.text = city.getProName();
					changeColor.name = city.getProName();
					changeColor.tempture = city.getTempture();
				}
				
				tm.color = city.getWeatherColor();
				
				foreach (Transform child in text.transform) {
					
					if (isCity) {
						if (child.tag == "pic1") {
							//Debug.Log(getPicPath(city.getImg1()));
							Texture2D pic = (Texture2D)Resources.Load(Util.getPicPath(city.getImg1()));
							child.renderer.material.shader = Shader.Find ("Unlit/Transparent");
							child.renderer.material.mainTexture = (Texture)pic;

						}
						else if (child.tag == "pic2") 
						{
							//Debug.Log(getPicPath(city.getImg2()));
							Texture2D pic = (Texture2D)Resources.Load(Util.getPicPath(city.getImg2()));
							child.renderer.material.shader = Shader.Find ("Unlit/Transparent");
							child.renderer.material.mainTexture = (Texture)pic;

						}
						else
						{
							TextMesh tm2 = (TextMesh)child.GetComponent<TextMesh> ();
							tm2.text = city.getDetailsName ();
							tm2.color = city.getWeatherColor();
						}
					}
					else 
					{
						child.active = false;
					}

				}
				text.transform.localScale = city.getSize();
				text.GetComponent<ChangeColor>().oldScale = text.transform.localScale;
			}
	}

	//鼠标移动的距离    
	void OnMouseDown(){   
		//接受鼠标按下的事件// 
		axisX=0f;
		axisY=0f;   
	}

	//鼠标拖拽时的操作
	void OnMouseDrag () 
	{  
		onDrag = true;
		cameraTarget.GetComponent<Camera>().labelLeftDetail = "";
		cameraTarget.GetComponent<Camera>().labelRightDetail = "";
		axisX = -Input.GetAxis ("Mouse X"); 
		axisY = Input.GetAxis ("Mouse Y");

		cXY = Mathf.Sqrt (axisX * axisX + axisY * axisY); 
		//计算鼠标移动的长度// 
		if(cXY == 0f)
		{ 
			cXY=1f; 
		}     
	}      

	//计算阻尼速度（鼠标）
	float Rigid ()          
	{  
		if (onDrag) 
		{ 
			tempSpeed = speed;
		} 
		else 
		{ 
			if (tempSpeed> 0)
			{  
				//通过除以鼠标移动长度实现拖拽越长速度减缓越慢    
				if (cXY != 0) {
					tempSpeed -= speed*2 * Time.deltaTime / cXY; 
				}
			}
			else 
			{
				tempSpeed = 0; 
			}         
		}  
		return tempSpeed; 
	} 

	// 转到当前城市
	public void findCity (string cityName) {

		foreach (Transform child in gameObject.transform)
		{
			if (child.GetComponent<TextMesh> ().text.Equals(cityName)) {
				gameObject.transform.rotation = new Quaternion(0,0,0,0);
				
				Debug.DrawLine(gameObject.transform.position, cameraTarget.position, Color.red);
				Debug.DrawLine(child.transform.position, gameObject.transform.position, Color.red);
				
				Vector3 targetDir = child.position - transform.position;
				
//				Debug.Log(child.position.x + "  " + child.position.y + "  "+child.position.z);
				Vector3 dirR = -Vector3.right;
				Vector3 dirF = -Vector3.forward;
				
				float angleY = Util.getAngle(new Vector3(targetDir.x, 0,targetDir.z), dirF);
				float angleZ = Util.getAngle(new Vector3(targetDir.x, targetDir.y,0), dirR);
				
				//Debug.Log(angleZ+"  "+angleY);
				gameObject.transform.localRotation = Quaternion.Euler (0, angleY, angleZ);
			}
		}
	}

	public void getDetail(string name) {
		StartCoroutine(GET(new Detail () ,WebUtil.getURL(name)));
	}

	//GET请求
	IEnumerator GET(Detail currentDetail,string url)
	{
		
		WWW www = new WWW (url);
		yield return www;	
		if (www.error != null)
		{
			//GET请求失败
			Debug.Log("error is :"+ www.error);	
		} else
		{
			//GET请求成功
			currentDetail.parseJson(www.text);
		}
		cameraTarget.GetComponent<Camera> ().showDetail (currentDetail);
	}

	void Update ()      
	{  
		// 搜索城市
		if(isFind){
			foreach (Transform child in gameObject.transform)
			{
				if (child.GetComponent<TextMesh> ().text.Equals(findName)) {

					Vector3 targetDir = child.position - transform.position;
					Vector3 cameraDir = -Vector3.forward;

					Quaternion rotation = Quaternion.FromToRotation(targetDir, cameraDir) * transform.rotation;
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);

					if (Vector3.Angle(targetDir, cameraDir) < 1f) {
						isFind = false;
					}
				}
			}
		}

		// 计算角度,透明度,颜色
		foreach (Transform child in gameObject.transform)
		{
			child.LookAt(new Vector3(cameraTarget.position.x, cameraTarget.position.y, -cameraTarget.position.z));
			float dis = Vector3.Distance(child.position, cameraTarget.transform.position);
			Color c = child.renderer.material.color;
			float alpha = Mathf.Abs(dis-10f)/5f + 0.1f;
			
			child.renderer.material.color = new Color(c.r,c.g,c.b,alpha);
			
			foreach (Transform cc in child.transform)
			{
				cc.renderer.material.color = new Color(c.r,c.g,c.b,alpha);
			}
			
		}
	
		// 找出选中对象
		foreach (Transform child in gameObject.transform)
		{
			ChangeColor changeColor = child.GetComponent<ChangeColor>();
			float disC_Camera = Vector3.Distance(child.position, cameraTarget.position);
			float disCamera_P = Vector3.Distance(cameraTarget.position, transform.position);
			float disC_P = Vector3.Distance(child.position, transform.position);
			float dis = disCamera_P - disC_P;
	
			if ((disC_Camera > dis-0.1f) && (disC_Camera < dis + 0.1f) && !onDrag && tempSpeed == 0f) {
				//child.renderer.material.color = Color.black;
				child.localScale = 1.5f * child.GetComponent<ChangeColor>().oldScale;

				if (isCity){
					cameraTarget.GetComponent<Camera>().labelText = changeColor.name+"市";
					cameraTarget.GetComponent<Camera>().labelText2 = "";
					currentCity = changeColor.name;
					currentCityID = changeColor.id;
				}
				else {
					if(changeColor.name.Equals("北京") || changeColor.name.Equals("上海") || changeColor.name.Equals("天津") || changeColor.name.Equals("重庆")){
						cameraTarget.GetComponent<Camera>().labelText = changeColor.name+"直辖市";
					}
					else {
						cameraTarget.GetComponent<Camera>().labelText = changeColor.name+"省";
					}
					cameraTarget.GetComponent<Camera>().labelText2 = "昼夜平均温度："+changeColor.tempture;
					currentCity = changeColor.name;
				}

			} 
			else {
 				child.localScale = child.GetComponent<ChangeColor>().oldScale;	
			}
		}

		gameObject.transform.Rotate (new Vector3 (axisY, axisX, 0) * Rigid (), Space.World); 
	
		if (!Input.GetMouseButton (0)) 
		{ 
			onDrag = false; 
		}     
	} 
	
}