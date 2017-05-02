using UnityEngine;
using System.Collections;
using LitJson;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class City : MonoBehaviour {

	private string name;	//大连

	private string proName;

	private string result;

	private double tempture;

	private string cityId;


	private string temperature;	//温度：17℃~30℃

	private string temperature2;	//温度：17℃~30℃

	private string weather;	//天气：晴

	private string img1;	//0

	private string img2;	//0
	
	public City(string name, string cityId) {
		this.name = name;
		this.cityId = cityId;
	}

	public Color getWeatherColor () {
		
		Color color  = Constants.c_0;
		
		if (tempture > 0) {
			
			if (tempture > 0 && tempture < 4)
				color = Constants.c_4;
			else if (tempture >= 4 && tempture < 8) 
				color = Constants.c_8;
			else if (tempture >= 8 && tempture < 12) 
				color = Constants.c_12;
			else if (tempture >= 12 && tempture < 16) 
				color = Constants.c_16;
			else if (tempture >= 16 && tempture < 20) 
				color = Constants.c_20;
			else if (tempture >= 20 && tempture < 24) 
				color = Constants.c_24;
			else if (tempture >= 24 && tempture < 28) 
				color = Constants.c_28;
			else if (tempture >= 28 && tempture < 32) 
				color = Constants.c_32;
			else if (tempture >= 32 && tempture < 36) 
				color = Constants.c_36;
			else if (tempture >= 36 && tempture < 40) 
				color = Constants.c_40;
			else 
				color = Constants.c_44;
		}
		if(tempture < 0) {
			
			if (tempture >= -4 && tempture < 0)
				color = Constants.c_0_4;
			else if (tempture >= -8 && tempture < -4) 
				color = Constants.c_0_8;
			else if (tempture >= -12 && tempture < -8) 
				color = Constants.c_0_12;
			else if (tempture >= -16 && tempture < -12) 
				color = Constants.c_0_16;
			else if (tempture >= -20 && tempture < -16) 
				color = Constants.c_0_20;
			else if (tempture >= -24 && tempture < -20) 
				color = Constants.c_0_24;
			else 
				color = Constants.c_0_28;
		}
		
		return color;
	}

	public string getName() {

		return name;
	}

	public string getCityID() {
		
		return cityId;
	}

	public Vector3 getSize() {		
		return new Vector3(0.1f, 0.1f, 0.1f);
	}

	public float getDetailsSize() {		
		return 0.5f;
	}

	public Vector3 getScaleSize(Transform transform) {		
		return transform.localScale*0.5f;
	}

	public string getDetailsName() {
		return temperature+"~"+temperature2+"  "+weather;
	}

	public string getImg1() {
		return img1.Split('.')[0];
	}

	public string getImg2() {
		return img2.Split('.')[0];
	}

	public void setTempture(string str1, string str2){
		string[] temp1 = str1.Split ('℃');
		string[] temp2 = str2.Split ('℃');
		this.tempture = (double.Parse(temp1[0])+double.Parse(temp2[0]))/2;
	}

	public string getTempture() {
		return this.tempture + "℃";
	}

	public void setProName(string name){
		this.proName = name;
	}
	
	public string getProName() {
		return this.proName;
	}

	public void parseJson(string result)
	{
		JsonData jd = JsonMapper.ToObject(result);

		JsonData jdResult = jd["weatherinfo"]; 

		this.name = (string)jdResult["city"];

		this.temperature = (string)jdResult["temp1"];
		this.temperature2 = (string)jdResult["temp2"];
		setTempture (temperature, temperature2);
		this.weather = (string)jdResult ["weather"];
		this.img1 = (string)jdResult ["img1"];
		this.img2 = (string)jdResult ["img2"];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
