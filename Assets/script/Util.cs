using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;


/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class Util : MonoBehaviour {

	// 得到SmartWeather密钥
	public static string GetKey(string appid, string privateKey, string areaId, string date, string type)
	{
		//使用SHA1的HMAC
		HMAC hmac = HMACSHA1.Create();
		var publicKey = "http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}";
		var data = System.Text.Encoding.UTF8.GetBytes(string.Format(publicKey, areaId, type, date, appid));
		//密钥
		var key = System.Text.Encoding.UTF8.GetBytes(privateKey);
		hmac.Key = key;
		
		//对数据进行签名
		var signedData = hmac.ComputeHash(data);
		return Convert.ToBase64String(signedData);
		
	}

	// test not a number
	public static void testNAN(float X) {
		bool b_NaN = ( X == X );
		
		if(!b_NaN)
			Debug.Log("X is not a number !");
	}
	
	// 获取图片路径
	public static string getPicPath(string picName){
		if (picName.Contains("d"))
		{
			picName = picName.Split('d')[1];
			return "day/"+picName;
		}
		else if (picName.Contains("n")) 
		{
			picName = picName.Split('n')[1];
			return "night/"+picName;
		}
		return "";
	}

	// 计算角度
	public static float getAngle(Vector3 v3_from,Vector3 v3_to)
	{
		Vector3 v3=Vector3.Cross(v3_from,v3_to);
		if(v3.z>0)
		{
			return Vector3.Angle(v3_from,v3_to); 
		}
		else
		{
			return -Vector3.Angle(v3_from,v3_to);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
