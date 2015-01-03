package com.gxy.weather;

import java.util.HashMap;
import java.util.Map;

import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.iflytek.cloud.ErrorCode;
import com.iflytek.cloud.InitListener;
import com.iflytek.cloud.RecognizerListener;
import com.iflytek.cloud.RecognizerResult;
import com.iflytek.cloud.SpeechConstant;
import com.iflytek.cloud.SpeechError;
import com.iflytek.cloud.SpeechRecognizer;
import com.iflytek.cloud.SpeechSynthesizer;
import com.iflytek.cloud.SpeechUtility;
import com.iflytek.cloud.SynthesizerListener;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

/**
 * 2014.9
 * @author Xiangyu Guo
 */
public class MainActivity extends UnityPlayerActivity {
	// jar -cvf class.jar *
	private Button voiceButton;
	private Button detailButton;
	private Button returnButton;
	private Button quitButton;

	boolean isFaild = false;
	// 语音结果
	String voiceResult = null;

	private Map<String, String> mapAllNameID;
	// 所有的市
	private String[] strNamePro;
	// 所有的城市
	private String[][] strNameCity;

	// 语音听写对象
	private SpeechRecognizer mVoice;

	// 语音合成对象
	private SpeechSynthesizer mTts;

	// 默认发音人
	private String voicer = "xiaoyan";

	// 引擎类型
	private String mEngineType = SpeechConstant.TYPE_CLOUD;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.test);

		View playerView = mUnityPlayer.getView();
		LinearLayout ll = (LinearLayout) findViewById(R.id.unity_layout);
		ll.addView(playerView);

		SpeechUtility.createUtility(this, SpeechConstant.APPID + "=540dcea0");
		// 初始化识别对象
		mVoice = SpeechRecognizer.createRecognizer(this, mInitListener);

		// 初始化合成对象
		mTts = SpeechSynthesizer.createSynthesizer(this, mTtsInitListener);

		voiceButton = (Button) findViewById(R.id.voice_btn);
		voiceButton.setOnClickListener(new voiceListener());

		returnButton = (Button) findViewById(R.id.return_btn);
		returnButton.setOnClickListener(new returnListener());

		detailButton = (Button) findViewById(R.id.detail_btn);
		detailButton.setOnClickListener(new detailListener());

		quitButton = (Button) findViewById(R.id.quit_btn);
		quitButton.setOnClickListener(new quitListener());
		initVar();
	}

	public class voiceListener implements OnClickListener {

		@Override
		public void onClick(View arg0) {
			voiceResult = "";
			// 设置参数
			setParam();
			mVoice.startListening(voiceListener);
		}
	}

	public class returnListener implements OnClickListener {

		@Override
		public void onClick(View arg0) {
			UnityPlayer.UnitySendMessage("Main Camera", "back", "");
		}
	}

	public class detailListener implements OnClickListener {

		@Override
		public void onClick(View arg0) {
			UnityPlayer.UnitySendMessage("Main Camera", "detail", "");
		}
	}

	public class quitListener implements OnClickListener {

		@Override
		public void onClick(View arg0) {
			System.exit(0);
		}
	}

	public void quitApp(String str) {
		Toast.makeText(getApplicationContext(), "退出", Toast.LENGTH_SHORT).show();
		System.exit(0);
	}

	private RecognizerListener voiceListener = new RecognizerListener() {

		@Override
		public void onBeginOfSpeech() {
			Toast.makeText(getApplicationContext(), "开始说话", Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onError(SpeechError error) {
			Toast.makeText(getApplicationContext(), "error", Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onEndOfSpeech() {
			Toast.makeText(getApplicationContext(), "结束说话", Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onResult(RecognizerResult results, boolean isLast) {
			voiceResult = voiceResult + JsonParser.parseIatResult(results.getResultString());
			if (isLast) {
				setSpeakParam();
				mTts.startSpeaking(checkResult(voiceResult), mTtsListener);
				// UnityPlayer.UnitySendMessage("Main Camera","voice",getResults(voiceResult));
			}
		}

		@Override
		public void onVolumeChanged(int volume) {
			// Toast.makeText(getApplicationContext(), "当前正在说话，音量大小：" + volume, Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onEvent(int eventType, int arg1, int arg2, Bundle obj) {
		}
	};

	/**
	 * 合成回调监听。
	 */
	private SynthesizerListener mTtsListener = new SynthesizerListener() {
		@Override
		public void onSpeakBegin() {

		}

		@Override
		public void onSpeakPaused() {

		}

		@Override
		public void onSpeakResumed() {

		}

		@Override
		public void onBufferProgress(int percent, int beginPos, int endPos, String info) {
		}

		@Override
		public void onSpeakProgress(int percent, int beginPos, int endPos) {

		}

		@Override
		public void onCompleted(SpeechError error) {
			if (error == null) {
				// Toast.makeText(getApplicationContext(), voiceResult, Toast.LENGTH_LONG).show();
				if (!isFaild) {
					UnityPlayer.UnitySendMessage("Main Camera", "voice", voiceResult);
				}
			} else if (error != null) {
				Toast.makeText(getApplicationContext(), "error", Toast.LENGTH_SHORT).show();
			}
		}

		@Override
		public void onEvent(int eventType, int arg1, int arg2, Bundle obj) {

		}
	};

	public void setParam() {

		// 设置语言
		mVoice.setParameter(SpeechConstant.LANGUAGE, "zh_cn");
		// 设置语言区域
		mVoice.setParameter(SpeechConstant.ACCENT, "mandarin");

		// 设置语音前端点
		mVoice.setParameter(SpeechConstant.VAD_BOS, "4000");
		// 设置语音后端点
		mVoice.setParameter(SpeechConstant.VAD_EOS, "1000");
		// 设置标点符号
		mVoice.setParameter(SpeechConstant.ASR_PTT, "0");
		// 设置音频保存路径
		mVoice.setParameter(SpeechConstant.ASR_AUDIO_PATH, "/sdcard/iflytek/wavaudio.pcm");
	}

	private void setSpeakParam() {

		// 设置合成
		if (mEngineType.equals(SpeechConstant.TYPE_CLOUD)) {
			mTts.setParameter(SpeechConstant.ENGINE_TYPE, SpeechConstant.TYPE_CLOUD);
			// 设置发音人
			mTts.setParameter(SpeechConstant.VOICE_NAME, voicer);
		} else {
			mTts.setParameter(SpeechConstant.ENGINE_TYPE, SpeechConstant.TYPE_LOCAL);
			// 设置发音人 voicer为空默认通过语音+界面指定发音人。
			mTts.setParameter(SpeechConstant.VOICE_NAME, "");
		}

		// 设置语速
		mTts.setParameter(SpeechConstant.SPEED, "50");

		// 设置音调
		mTts.setParameter(SpeechConstant.PITCH, "50");

		// 设置音量
		mTts.setParameter(SpeechConstant.VOLUME, "50");

		// 设置播放器音频流类型
		mTts.setParameter(SpeechConstant.STREAM_TYPE, "3");
	}

	private String checkResult(String result) {
		if (result == null) {
			isFaild = true;
			return "查询失败，请重试";
		} else {
			boolean isFind = false;
			for (int i = 0; i < strNamePro.length; i++) {
				if ((result).contains(strNamePro[i])) {
					isFind = true;
					isFaild = false;
					StringBuffer r = new StringBuffer();
					r.append(strNamePro[i]).append(".");
					voiceResult = r.toString();
					return "查询成功" + strNamePro[i];
				}
			}

			// 如果不包含省
			if (!isFind) {

				for (int j = 0; j < strNameCity.length; j++) {
					for (int k = 0; k < strNameCity[j].length; k++) {
						if ((result).contains(strNameCity[j][k])) {
							isFaild = false;
							voiceResult = getCityIDs(j, k);
							return "查询成功" + strNameCity[j][k];
						}
					}
				}
			}

		}
		isFaild = true;
		return "查询失败，请重试";
	}

	private String getCityIDs(int j, int k) {

		String[] cityNames = strNameCity[j];
		StringBuffer cityNambers = new StringBuffer();
		cityNambers.append(strNameCity[j][k]).append(".");
		for (int i = 0; i < cityNames.length; i++) {
			if (i == cityNames.length - 1) {
				cityNambers.append(mapAllNameID.get(cityNames[i]));
			} else {
				cityNambers.append(mapAllNameID.get(cityNames[i])).append(",");
			}
		}
		return cityNambers.toString();
	}

	/**
	 * 初始化监听器。
	 */
	private InitListener mInitListener = new InitListener() {

		@Override
		public void onInit(int code) {
			if (code != ErrorCode.SUCCESS) {
				Toast.makeText(getApplicationContext(), "初始化失败,错误码：" + code, Toast.LENGTH_SHORT).show();
			}
		}
	};

	/**
	 * 初期化监听。
	 */
	private InitListener mTtsInitListener = new InitListener() {
		@Override
		public void onInit(int code) {

			if (code != ErrorCode.SUCCESS) {
				Toast.makeText(getApplicationContext(), "初始化失败,错误码：" + code, Toast.LENGTH_SHORT).show();
			}
		}
	};

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if (keyCode == KeyEvent.KEYCODE_BACK) {
			System.exit(0);
		}

		return false;

	}

	private void initVar() {
		// 得到所有的键值对
		mapAllNameID = new HashMap<String, String>();
		NameIDMap nameIDMap = new NameIDMap();
		mapAllNameID = nameIDMap.getMapAllNameID();
		// 初始化省市级
		strNamePro = new String[] { "北京", "上海", "天津", "重庆", "黑龙江", "吉林", "辽宁", "内蒙古", "河北", "山西", "陕西", "山东", "新疆", "西藏", "青海", "甘肃", "宁夏", "河南",
				"江苏", "湖北", "浙江", "安徽", "福建", "江西", "湖南", "贵州", "四川", "广东", "云南", "广西", "海南", "台湾" };
		strNameCity = new String[][] {
				{ "北京", "海淀", "朝阳", "顺义", "怀柔", "通州", "昌平", "延庆", "丰台", "石景山", "大兴", "房山", "密云", "门头沟", "平谷" },
				{ "上海", "闵行", "宝山", "川沙", "嘉定", "南汇", "金山", "青浦", "松江", "奉贤", "崇明", "陈家镇", "引水船", "徐家汇", "浦东" },
				{ "天津", "武清", "宝坻", "东丽", "西青", "北辰", "宁河", "汉沽", "静海", "津南", "塘沽", "大港", "平台", "蓟县" },
				{ "重庆", "永川", "合川", "南川", "江津", "万盛", "渝北", "北碚", "巴南", "长寿", "黔江", "涪陵", "开县", "城口", "云阳", "巫溪", "奉节", "巫山", "潼南", "垫江", "梁平", "忠县",
						"石柱", "大足", "荣昌", "铜梁", "璧山", "丰都", "武隆", "彭水" },
				{ "哈尔滨", "齐齐哈尔", "牡丹江", "佳木斯", "绥化", "黑河", "大兴安岭", "伊春", "大庆", "鸡西", "鹤岗", "双鸭山" },
				{ "长春", "吉林", "延吉", "四平", "通化", "白城", "辽源", "松原", "白山" },
				{ "沈阳", "大连", "鞍山", "抚顺", "本溪", "丹东", "锦州", "营口", "阜新", "辽阳", "铁岭", "朝阳", "盘锦", "葫芦岛" },
				{ "呼和浩特", "包头", "乌海", "集宁", "通辽", "赤峰", "鄂尔多斯", "临河", "锡林浩特", "海拉尔", "乌兰浩特", "阿拉善左旗" },
				{ "石家庄", "保定", "张家口", "唐山", "廊坊", "沧州", "衡水", "邢台", "邯郸", "秦皇岛" },
				{ "太原", "大同", "阳泉", "晋中", "长治", "晋城", "临汾", "运城", "朔州", "忻州", "离石" }, { "西安", "三原", "延长", "榆林", "渭南", "商洛", "安康", "汉中", "宝鸡", "铜川" },
				{ "济南", "青岛", "淄博", "德州", "烟台", "潍坊", "济宁", "泰安", "临沂", "菏泽", "滨州", "东营", "威海", "枣庄", "日照", "莱芜", "聊城" },
				{ "乌鲁木齐", "克拉玛依", "石河子", "昌吉", "吐鲁番", "库尔勒", "阿拉尔", "阿克苏", "喀什", "伊宁", "塔城", "哈密", "和田", "阿勒泰", "阿图什", "博乐" },
				{ "拉萨", "日喀则", "山南", "林芝", "昌都", "那曲", "阿里" }, { "西宁", "海东", "黄南", "海南", "果洛", "玉树", "海西", "海北" },
				{ "兰州", "定西", "平凉", "庆阳", "武威", "金昌", "张掖", "酒泉", "天水", "武都", "临夏", "合作", "白银" }, { "银川", "石嘴山", "吴忠", "固原", "中卫" },
				{ "郑州", "安阳", "新乡", "许昌", "平顶山", "信阳", "南阳", "开封", "洛阳", "商丘", "焦作", "鹤壁", "濮阳", "周口", "漯河", "驻马店", "三门峡", "济源" },
				{ "南京", "无锡", "镇江", "苏州", "南通", "扬州", "盐城", "徐州", "淮安", "连云港", "常州", "泰州", "宿迁" },
				{ "武汉", "襄樊", "鄂州", "孝感", "黄冈", "黄石", "咸宁", "荆州", "宜昌", "恩施", "十堰", "神农架", "随州", "荆门", "天门", "仙桃", "潜江" },
				{ "杭州", "湖州", "嘉兴", "宁波", "绍兴", "台州", "温州", "丽水", "金华", "衢州", "舟山" },
				{ "合肥", "蚌埠", "芜湖", "淮南", "马鞍山", "安庆", "宿州", "阜阳", "亳州", "黄山站", "滁州", "淮北", "铜陵", "宣城", "六安", "巢湖", "池州" },
				{ "福州", "厦门", "宁德", "莆田", "泉州", "漳州", "龙岩", "三明", "南平" }, { "南昌", "九江", "上饶", "抚州", "宜春", "吉安", "赣州", "景德镇", "萍乡", "新余", "鹰潭" },
				{ "长沙", "湘潭", "株洲", "衡阳", "郴州", "常德", "赫山区", "娄底", "邵阳", "岳阳", "张家界", "怀化", "黔阳", "永州", "吉首" },
				{ "贵阳", "遵义", "安顺", "都匀", "凯里", "铜仁", "毕节", "六盘水", "黔西" },
				{ "成都", "攀枝花", "自贡", "绵阳", "南充", "达州", "遂宁", "广安", "巴中", "泸州", "宜宾", "内江", "资阳", "乐山", "眉山", "凉山", "雅安", "甘孜", "阿坝", "德阳", "广元" },
				{ "广州", "韶关", "惠州", "梅州", "汕头", "深圳", "珠海", "顺德", "肇庆", "湛江", "江门", "河源", "清远", "云浮", "潮州", "东莞", "中山", "阳江", "揭阳", "茂名", "汕尾" },
				{ "昆明", "大理", "红河", "曲靖", "保山", "文山", "玉溪", "楚雄", "普洱", "昭通", "临沧", "怒江", "香格里拉", "丽江", "德宏", "景洪" },
				{ "南宁", "崇左", "柳州", "来宾", "桂林", "梧州", "贺州", "贵港", "玉林", "百色", "钦州", "河池", "北海", "防城港" }, { "海口", "三亚" },
				{ "台北县", "高雄", "台南", "台中", "桃园", "新竹县", "宜兰", "马公", "嘉义" } };

	}
}
