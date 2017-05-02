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
	// �������
	String voiceResult = null;

	private Map<String, String> mapAllNameID;
	// ���е���
	private String[] strNamePro;
	// ���еĳ���
	private String[][] strNameCity;

	// ������д����
	private SpeechRecognizer mVoice;

	// �����ϳɶ���
	private SpeechSynthesizer mTts;

	// Ĭ�Ϸ�����
	private String voicer = "xiaoyan";

	// ��������
	private String mEngineType = SpeechConstant.TYPE_CLOUD;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.test);

		View playerView = mUnityPlayer.getView();
		LinearLayout ll = (LinearLayout) findViewById(R.id.unity_layout);
		ll.addView(playerView);

		SpeechUtility.createUtility(this, SpeechConstant.APPID + "=540dcea0");
		// ��ʼ��ʶ�����
		mVoice = SpeechRecognizer.createRecognizer(this, mInitListener);

		// ��ʼ���ϳɶ���
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
			// ���ò���
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
		Toast.makeText(getApplicationContext(), "�˳�", Toast.LENGTH_SHORT).show();
		System.exit(0);
	}

	private RecognizerListener voiceListener = new RecognizerListener() {

		@Override
		public void onBeginOfSpeech() {
			Toast.makeText(getApplicationContext(), "��ʼ˵��", Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onError(SpeechError error) {
			Toast.makeText(getApplicationContext(), "error", Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onEndOfSpeech() {
			Toast.makeText(getApplicationContext(), "����˵��", Toast.LENGTH_SHORT).show();
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
			// Toast.makeText(getApplicationContext(), "��ǰ����˵����������С��" + volume, Toast.LENGTH_SHORT).show();
		}

		@Override
		public void onEvent(int eventType, int arg1, int arg2, Bundle obj) {
		}
	};

	/**
	 * �ϳɻص�������
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

		// ��������
		mVoice.setParameter(SpeechConstant.LANGUAGE, "zh_cn");
		// ������������
		mVoice.setParameter(SpeechConstant.ACCENT, "mandarin");

		// ��������ǰ�˵�
		mVoice.setParameter(SpeechConstant.VAD_BOS, "4000");
		// ����������˵�
		mVoice.setParameter(SpeechConstant.VAD_EOS, "1000");
		// ���ñ�����
		mVoice.setParameter(SpeechConstant.ASR_PTT, "0");
		// ������Ƶ����·��
		mVoice.setParameter(SpeechConstant.ASR_AUDIO_PATH, "/sdcard/iflytek/wavaudio.pcm");
	}

	private void setSpeakParam() {

		// ���úϳ�
		if (mEngineType.equals(SpeechConstant.TYPE_CLOUD)) {
			mTts.setParameter(SpeechConstant.ENGINE_TYPE, SpeechConstant.TYPE_CLOUD);
			// ���÷�����
			mTts.setParameter(SpeechConstant.VOICE_NAME, voicer);
		} else {
			mTts.setParameter(SpeechConstant.ENGINE_TYPE, SpeechConstant.TYPE_LOCAL);
			// ���÷����� voicerΪ��Ĭ��ͨ������+����ָ�������ˡ�
			mTts.setParameter(SpeechConstant.VOICE_NAME, "");
		}

		// ��������
		mTts.setParameter(SpeechConstant.SPEED, "50");

		// ��������
		mTts.setParameter(SpeechConstant.PITCH, "50");

		// ��������
		mTts.setParameter(SpeechConstant.VOLUME, "50");

		// ���ò�������Ƶ������
		mTts.setParameter(SpeechConstant.STREAM_TYPE, "3");
	}

	private String checkResult(String result) {
		if (result == null) {
			isFaild = true;
			return "��ѯʧ�ܣ�������";
		} else {
			boolean isFind = false;
			for (int i = 0; i < strNamePro.length; i++) {
				if ((result).contains(strNamePro[i])) {
					isFind = true;
					isFaild = false;
					StringBuffer r = new StringBuffer();
					r.append(strNamePro[i]).append(".");
					voiceResult = r.toString();
					return "��ѯ�ɹ�" + strNamePro[i];
				}
			}

			// ���������ʡ
			if (!isFind) {

				for (int j = 0; j < strNameCity.length; j++) {
					for (int k = 0; k < strNameCity[j].length; k++) {
						if ((result).contains(strNameCity[j][k])) {
							isFaild = false;
							voiceResult = getCityIDs(j, k);
							return "��ѯ�ɹ�" + strNameCity[j][k];
						}
					}
				}
			}

		}
		isFaild = true;
		return "��ѯʧ�ܣ�������";
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
	 * ��ʼ����������
	 */
	private InitListener mInitListener = new InitListener() {

		@Override
		public void onInit(int code) {
			if (code != ErrorCode.SUCCESS) {
				Toast.makeText(getApplicationContext(), "��ʼ��ʧ��,�����룺" + code, Toast.LENGTH_SHORT).show();
			}
		}
	};

	/**
	 * ���ڻ�������
	 */
	private InitListener mTtsInitListener = new InitListener() {
		@Override
		public void onInit(int code) {

			if (code != ErrorCode.SUCCESS) {
				Toast.makeText(getApplicationContext(), "��ʼ��ʧ��,�����룺" + code, Toast.LENGTH_SHORT).show();
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
		// �õ����еļ�ֵ��
		mapAllNameID = new HashMap<String, String>();
		NameIDMap nameIDMap = new NameIDMap();
		mapAllNameID = nameIDMap.getMapAllNameID();
		// ��ʼ��ʡ�м�
		strNamePro = new String[] { "����", "�Ϻ�", "���", "����", "������", "����", "����", "���ɹ�", "�ӱ�", "ɽ��", "����", "ɽ��", "�½�", "����", "�ຣ", "����", "����", "����",
				"����", "����", "�㽭", "����", "����", "����", "����", "����", "�Ĵ�", "�㶫", "����", "����", "����", "̨��" };
		strNameCity = new String[][] {
				{ "����", "����", "����", "˳��", "����", "ͨ��", "��ƽ", "����", "��̨", "ʯ��ɽ", "����", "��ɽ", "����", "��ͷ��", "ƽ��" },
				{ "�Ϻ�", "����", "��ɽ", "��ɳ", "�ζ�", "�ϻ�", "��ɽ", "����", "�ɽ�", "����", "����", "�¼���", "��ˮ��", "��һ�", "�ֶ�" },
				{ "���", "����", "����", "����", "����", "����", "����", "����", "����", "����", "����", "���", "ƽ̨", "����" },
				{ "����", "����", "�ϴ�", "�ϴ�", "����", "��ʢ", "�山", "����", "����", "����", "ǭ��", "����", "����", "�ǿ�", "����", "��Ϫ", "���", "��ɽ", "����", "�潭", "��ƽ", "����",
						"ʯ��", "����", "�ٲ�", "ͭ��", "�ɽ", "�ᶼ", "��¡", "��ˮ" },
				{ "������", "�������", "ĵ����", "��ľ˹", "�绯", "�ں�", "���˰���", "����", "����", "����", "�׸�", "˫Ѽɽ" },
				{ "����", "����", "�Ӽ�", "��ƽ", "ͨ��", "�׳�", "��Դ", "��ԭ", "��ɽ" },
				{ "����", "����", "��ɽ", "��˳", "��Ϫ", "����", "����", "Ӫ��", "����", "����", "����", "����", "�̽�", "��«��" },
				{ "���ͺ���", "��ͷ", "�ں�", "����", "ͨ��", "���", "������˹", "�ٺ�", "���ֺ���", "������", "��������", "����������" },
				{ "ʯ��ׯ", "����", "�żҿ�", "��ɽ", "�ȷ�", "����", "��ˮ", "��̨", "����", "�ػʵ�" },
				{ "̫ԭ", "��ͬ", "��Ȫ", "����", "����", "����", "�ٷ�", "�˳�", "˷��", "����", "��ʯ" }, { "����", "��ԭ", "�ӳ�", "����", "μ��", "����", "����", "����", "����", "ͭ��" },
				{ "����", "�ൺ", "�Ͳ�", "����", "��̨", "Ϋ��", "����", "̩��", "����", "����", "����", "��Ӫ", "����", "��ׯ", "����", "����", "�ĳ�" },
				{ "��³ľ��", "��������", "ʯ����", "����", "��³��", "�����", "������", "������", "��ʲ", "����", "����", "����", "����", "����̩", "��ͼʲ", "����" },
				{ "����", "�տ���", "ɽ��", "��֥", "����", "����", "����" }, { "����", "����", "����", "����", "����", "����", "����", "����" },
				{ "����", "����", "ƽ��", "����", "����", "���", "��Ҵ", "��Ȫ", "��ˮ", "�䶼", "����", "����", "����" }, { "����", "ʯ��ɽ", "����", "��ԭ", "����" },
				{ "֣��", "����", "����", "���", "ƽ��ɽ", "����", "����", "����", "����", "����", "����", "�ױ�", "���", "�ܿ�", "���", "פ���", "����Ͽ", "��Դ" },
				{ "�Ͼ�", "����", "��", "����", "��ͨ", "����", "�γ�", "����", "����", "���Ƹ�", "����", "̩��", "��Ǩ" },
				{ "�人", "�差", "����", "Т��", "�Ƹ�", "��ʯ", "����", "����", "�˲�", "��ʩ", "ʮ��", "��ũ��", "����", "����", "����", "����", "Ǳ��" },
				{ "����", "����", "����", "����", "����", "̨��", "����", "��ˮ", "��", "����", "��ɽ" },
				{ "�Ϸ�", "����", "�ߺ�", "����", "��ɽ", "����", "����", "����", "����", "��ɽվ", "����", "����", "ͭ��", "����", "����", "����", "����" },
				{ "����", "����", "����", "����", "Ȫ��", "����", "����", "����", "��ƽ" }, { "�ϲ�", "�Ž�", "����", "����", "�˴�", "����", "����", "������", "Ƽ��", "����", "ӥ̶" },
				{ "��ɳ", "��̶", "����", "����", "����", "����", "��ɽ��", "¦��", "����", "����", "�żҽ�", "����", "ǭ��", "����", "����" },
				{ "����", "����", "��˳", "����", "����", "ͭ��", "�Ͻ�", "����ˮ", "ǭ��" },
				{ "�ɶ�", "��֦��", "�Թ�", "����", "�ϳ�", "����", "����", "�㰲", "����", "����", "�˱�", "�ڽ�", "����", "��ɽ", "üɽ", "��ɽ", "�Ű�", "����", "����", "����", "��Ԫ" },
				{ "����", "�ع�", "����", "÷��", "��ͷ", "����", "�麣", "˳��", "����", "տ��", "����", "��Դ", "��Զ", "�Ƹ�", "����", "��ݸ", "��ɽ", "����", "����", "ï��", "��β" },
				{ "����", "����", "���", "����", "��ɽ", "��ɽ", "��Ϫ", "����", "�ն�", "��ͨ", "�ٲ�", "ŭ��", "�������", "����", "�º�", "����" },
				{ "����", "����", "����", "����", "����", "����", "����", "���", "����", "��ɫ", "����", "�ӳ�", "����", "���Ǹ�" }, { "����", "����" },
				{ "̨����", "����", "̨��", "̨��", "��԰", "������", "����", "��", "����" } };

	}
}
