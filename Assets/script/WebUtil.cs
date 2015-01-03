using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class WebUtil : MonoBehaviour {

	private string result;

	private City city;



	public void setCity(City city)
	{
		this.city = city;
	}

	public static string getURL(string name) {
		//http://weather.123.duba.net/static/weather_info/101121301.html
		//http://weather.51wnl.com/weatherinfo/GetMoreWeather?cityCode=101040100&weatherType=0
		//http://v.juhe.cn/weather/index?key=18d2129bc32a54f6995cd24e6d3f2422&dtype=json&cityname="+name+"&format=2
		return "http://weather.51wnl.com/weatherinfo/GetMoreWeather?cityCode="+name+"&weatherType=0";
	}

	public void getWeather(string name)
	{
		string url = "http://v.juhe.cn/weather/index?key=18d2129bc32a54f6995cd24e6d3f2422&dtype=json&cityname="+name+"&format=2";
		StartCoroutine(GET(url));
	}
	
	public void getAir()
	{
		//登录请求 POST 把参数写在字典用 通过www类来请求
		Dictionary<string,string> dic = new Dictionary<string, string> ();
		//参数
		dic.Add("city","北京");
		dic.Add("key","83ecdccc7b8511220a8d1e7e7a0ca5c5");
		
		StartCoroutine(POST("http://web.juhe.cn:8080/environment/air/cityair",dic));
		
	}
	
	//POST请求
	IEnumerator POST(string url, Dictionary<string,string> post)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string,string> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}

		WWW www = new WWW(url, form);
		yield return www;
		
		if (www.error != null)
		{
			//POST请求失败
			Debug.Log("error is :"+ www.error);
			
		} else
		{
			//POST请求成功
			Debug.Log("request ok : " + www.text);
		}
	}
	
	//GET请求
	IEnumerator GET(string url)
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

		
			//parseJson(www.text);
			Debug.Log("request ok : " + result);
		}
	}

	public void parseJson(string result)
	{
		JsonData jd = JsonMapper.ToObject(result);
		
		
		JsonData jdResult = jd["result"]; 
		JsonData jdToday = jdResult["today"]; 
//
//		private string city ;
//		private string date_y ;
//		private string week ;
//		private string temperature ;
//		private string weather ;
//		private string wind ;
//		private string dressing_index ;
//		private string uv_index ;
//		private string comfort_index ;
//		private string wash_index ;
//		private string travel_index ;
//		private string exercise_index ;
//		private string drying_index ;

//		city = (string)jdToday["city"];
//		date_y =(string)jdToday["date_y"];
//		week = (string)jdToday["week"];
//		temperature = (string)jdToday["temperature"];
//		weather = (string)jdToday["weather"];
//		wind = (string)jdToday["wind"];
//		dressing_index = (string)jdToday["dressing_index"];
//		uv_index = (string)jdToday["uv_index"];
//		comfort_index = (string)jdToday["comfort_index"];
//		wash_index = (string)jdToday["wash_index"];
//		travel_index = (string)jdToday["travel_index"];
//		exercise_index = (string)jdToday["exercise_index"];
//		drying_index = (string)jdToday["drying_index"];
		
		//		Debug.Log("resultcode = " + (string)jd["resultcode"]);
		//		Debug.Log("reason = " + (string)jd["reason"]);
		//		Debug.Log("city = " + (string)jdToday["city"]);
		//		Debug.Log("date_y = " + (string)jdToday["date_y"]);
		//		Debug.Log("week = " + (string)jdToday["week"]);
		//		Debug.Log("temperature = " + (string)jdToday["temperature"]);
		//		Debug.Log("weather = " + (string)jdToday["weather"]);
		//		Debug.Log("wind = " + (string)jdToday["wind"]);
		//		Debug.Log("dressing_index = " + (string)jdToday["dressing_index"]);
		//		Debug.Log("uv_index = " + (string)jdToday["uv_index"]);
		//		Debug.Log("comfort_index = " + (string)jdToday["comfort_index"]);
		//		Debug.Log("wash_index = " + (string)jdToday["wash_index"]);
		//		Debug.Log("travel_index = " + (string)jdToday["travel_index"]);
		//		Debug.Log("exercise_index = " + (string)jdToday["exercise_index"]);
		//		Debug.Log("drying_index = " + (string)jdToday["drying_index"]);
	}


	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
}