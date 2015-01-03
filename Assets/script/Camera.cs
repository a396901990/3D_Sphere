using UnityEngine;
using System.Collections;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class Camera : MonoBehaviour {

	private string cityIds;

	private int currentID = 0;

	private Hashtable proLabel;

	private Hashtable cityLabel;

	private string proName;

	private string cityName;

	public string labelText;

	public string labelText2;

	public string labelText3;

	public string labelLeftDetail;

	public string labelRightDetail;

	public Hashtable Labels;

	private RotateSelf mySphere;

	void Start () {
		cityLabel = Constants.initLabel (cityLabel);
		mySphere = GameObject.FindWithTag ("Sphere").GetComponent<RotateSelf> ();
		Screen.orientation = ScreenOrientation.Portrait;
		labelText3 = Constants.welcome;
		InvokeRepeating("setHelpLabel", 1,5);
		// for test
		//cityIds = "101050501,101050502,101050503,101050504,101050505,101050506,101050507,101050508,101050509,101050510,101050601,101050602,101050603,101050604,101050605,101050606,101050701,101050702,101050703";
	}

	// 语音
	void voice(string cityName)
	{
		string[] results = cityName.Split ('.');
		cityName = results [0];
		string[] cityIDs = results [1].Split (',');
		if (isPro(cityName) && mySphere.isCity) {  // 查询的是省份，当前是城市
			Debug.Log(cityName);
			mySphere.setNewCity(Constants.proIds, false, cityName);
		}
		else if (!isPro(cityName) && !mySphere.isCity) {	 // 查询的是城市，当前是省份
			mySphere.setNewCity(cityIDs, true, cityName);
		}
		else {

			if (mySphere.isCity) {
				bool isTrue = true;
				ArrayList citys = mySphere.citys;
				foreach (City city in citys) {
					if (cityName.Equals(city.getName())) {
						isTrue = false;
						mySphere.findCity(cityName);
					}
				}
				if (isTrue) {
					labelText3 = "该省不存在此市,或识别错误.请重试！";
					StartCoroutine(wait(3,""));
				}  
			}
			else {
				bool isTrue = true;
				for (int i=0; i<Constants.pros.Length; i++) {
					if (Constants.pros[i].Equals(cityName)) {
						mySphere.findCity(cityName);
						isTrue = false;
					}
				}
				if (isTrue) {
					labelText3 = "该省不存在,或语音识别错误.请重试！";
					StartCoroutine(wait(3,""));
				}
				
			}
		}
		
	}
	
	// 回退
	void back(string str) {

		if(mySphere.isCity){
			mySphere.setNewCity(Constants.proIds, false, "北京");
			labelText3 = Constants.welcome;
		}

	}

	// 详细
	void detail(string str) {

		string currentCity = mySphere.currentCity;
		string currentCityID = mySphere.currentCityID;
		bool isCity = mySphere.isCity;
		Hashtable allCityID = mySphere.allCityID;

		if (isCity)
		{
			mySphere.getDetail(currentCityID);
		}
		else
		{
			mySphere.setNewCity((string[])allCityID[currentCity],true,"");
		}

	}

	// 显示详细城市天气
	public void showDetail (Detail detail) {
		labelLeftDetail = detail.getWeatherDetail();
		labelRightDetail = detail.getIndexDetail();
	}
	
	void OnGUI() {
		GUIStyle aa=new GUIStyle();
		aa.normal.background = null;		    
		aa.alignment = TextAnchor.MiddleCenter;
		aa.normal.textColor=new Color(1,1,1); 
		aa.fontSize = 60;       
		GUI.Label(new Rect(0, 0, Screen.width, 200), labelText, aa);

		GUIStyle bb=new GUIStyle();
		bb.normal.background = null;		    
		bb.alignment = TextAnchor.MiddleCenter;
		bb.normal.textColor=new Color(1,1,1);   
		bb.fontSize = 30;       
		GUI.Label(new Rect(0, 0, Screen.width, 350), labelText2, bb);

		GUIStyle cc=new GUIStyle();
		cc.normal.background = null;		    
		cc.alignment = TextAnchor.MiddleCenter;
		cc.normal.textColor=new Color(1,1,1);   
		cc.fontSize = 20;       
		GUI.Label(new Rect(0, 0, Screen.width, 50), labelText3, cc);

		
		GUIStyle detailL=new GUIStyle();
		detailL.normal.background = null;		    
		detailL.alignment = TextAnchor.MiddleCenter;
		detailL.normal.textColor=new Color(1,1,1);   
		detailL.fontSize = 25;       
		GUI.Label(new Rect(0, 0, Screen.width/2, 400), labelLeftDetail, detailL);

		GUIStyle detailR=new GUIStyle();
		detailR.normal.background = null;		    
		detailR.alignment = TextAnchor.MiddleCenter;
		detailR.normal.textColor=new Color(1,1,1);   
		detailR.fontSize = 25;       
		GUI.Label(new Rect(Screen.width/2, 0, Screen.width/2, 400), labelRightDetail, detailR);
		// for test
//		if(GUILayout.Button("callVoice",GUILayout.Height(50)))
//		{
//			detail("");
//		}
//
//		if(GUILayout.Button("callVoice2",GUILayout.Height(50)))
//		{
//			voice("黑河");
//		}
//
//		if(GUILayout.Button("33333",GUILayout.Height(50)))
//		{
//			mySphere.getDetail("本溪");
//		}
	}

	// Update is called once per frame
	void Update () {

		// 返回键
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			back("");
		}

		// Home键
		if (Input.GetKeyDown(KeyCode.Home) )
		{
			quitApp();
		}
	}

	// 退出
	void quitApp(){
		//注解1
		using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				//调用Android插件中UnityTestActivity中StartActivity0方法，stringToEdit表示它的参数
				jo.Call("quitApp","");
			}
			
		}

		Application.Quit();
	}

	// 当前城市是否是省
	public bool isPro (string cityName) {
		
		foreach (string pro in Constants.pros) {
			if (cityName.Equals (pro)) {
				return true;
			}
		}
		
		return false;
	}

	IEnumerator wait(int seconds, string labelWord) {
		yield return new WaitForSeconds (seconds);
		labelText3 = labelWord;
	}

	public void setHelpLabel() {
		currentID = currentID == 3 ? 0 : ++currentID;

		labelText3 = (string)cityLabel[currentID];
	}

}
