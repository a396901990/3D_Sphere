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
		return "http://weather.51wnl.com/weatherinfo/GetMoreWeather?cityCode="+name+"&weatherType=0";
	}

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
}