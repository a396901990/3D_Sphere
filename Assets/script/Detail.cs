using UnityEngine;
using System.Collections;
using LitJson;


/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class Detail : MonoBehaviour {

	private string name;	//大连

	private string weather;	//天气：晴

	private string temperature;	//温度：17℃~30℃

	private string date_y;	//2014年3月4日
	
	private string date;	//八月十四
	
	private string week;	//星期日

	private string wind;	 //风力：东南风3-4级转小于3级
	
	private string fl; //风力描述，东南风
	
	private string index;  //穿衣指数：热
	
	private string index_uv;	//紫外线强度：强
	
	private string index_xc; 	//洗车指数：不宜
	
	private string index_tr;  	//旅游指数：较适宜
	
	private string index_co;	//舒适指数:适宜
	
	private string index_cl;	//锻炼指数：适宜
	
	private string index_ls;	//晾晒指数：非常干燥
	
	private string index_ag;	//过敏指数：非常干燥


	public void parseJson(string result)
	{
		JsonData jd = JsonMapper.ToObject(result);
		
		JsonData jdResult = jd["weatherinfo"]; 
		
		this.name = (string)jdResult["city"];
		this.date_y = (string)jdResult["date_y"];
		this.date = (string)jdResult["date"];
		this.week = (string)jdResult["week"];
		this.temperature = (string)jdResult["temp1"];
		this.weather = (string)jdResult ["weather1"];
		this.wind = (string)jdResult ["wind1"];
		this.fl = (string)jdResult ["fl1"];
		this.index = (string)jdResult ["index"];
		this.index_uv = (string)jdResult ["index_uv"];
		this.index_xc = (string)jdResult ["index_xc"];
		this.index_tr = (string)jdResult ["index_tr"];
		this.index_co = (string)jdResult ["index_co"];
		this.index_cl = (string)jdResult ["index_cl"];
		this.index_ls = (string)jdResult ["index_ls"];
		this.index_ag = (string)jdResult ["index_ag"];
	}

	public string getCity() {
		return name;
	}
	
	public string getDate() {
		return "农历: "+date+" "+week;
	}
	
	public string getDate_y() {
		return "日期:"+date_y;
	}
	public string getWeek() {
		return week;
	}
	public string getTemperature() {
		return "温度: "+temperature;
	}
	public string getWeather() {
		return "天气: "+weather;
	}
	public string getWind() {
		return "风力: "+wind+" "+fl;
	}
	public string getDressing_index() {
		return "穿衣指数: "+ index;
	}
	public string getUv_index() {
		return "紫外线指数: "+index_uv;
	}
	public string getComfort_index() {
		return "舒适指数: "+index_co;
	}
	public string getTravel_index() {
		return "旅行指数: "+index_tr;
	}
	public string getExercise_index() {
		return "晨练指数: "+index_cl;
	}
	public string getDrying_index() {
		return "晾晒指数: "+index_ls;
	}

	public string getWeatherDetail() {
		return getDate_y()+"\n"+getTemperature()+"\n"+getWeather()+"\n"+getWind();
	}

	public string getIndexDetail() {
		return getUv_index()+"\n"+getComfort_index()+"\n"+getTravel_index()+"\n"+getExercise_index();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
