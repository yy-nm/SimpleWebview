using UnityEngine;
using System.Collections;


namespace Ninja3.Tool.Web
{
	using Debug = UnityEngine.Debug;

	public class SimpleWebViewPluginAndroid : SimpleWebViewPlugin
	{
		private AndroidJavaClass jc = null;
		private AndroidJavaObject currentActivity = null;
		private AndroidJavaObject application = null;
		private AndroidJavaClass simpleWebViewPlugin = null;

#if UNITY_ANDROID
		public SimpleWebViewPluginAndroid()
		{
			this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			this.application = this.currentActivity.Call<AndroidJavaObject>("getApplication");
			this.simpleWebViewPlugin = new AndroidJavaClass("com.pandadastudio.tool.webview.SimpleWebViewManager");

			mIsInstalled = false;
		}

		public override bool install()
		{
			mIsInstalled = this.simpleWebViewPlugin.CallStatic<bool>("install", this.application, this.currentActivity);
			return mIsInstalled;
		}

		public override bool openWebView(string _webviewGUID)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_OpenWebView", _webviewGUID);
		}

		public override void closeWebView(string _webviewGUID)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_CloseWebView", _webviewGUID);
		}

		public override bool loadUrl(string _webViewGUID, string _url)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_LoadUrl", _webViewGUID, _url);
		}

		public override void enableLog(bool _enable)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_EnableLog", _enable);
		}

		public override void changeWebViewSize(string _webViewGUID, Rect _paddingSize)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_ChangeWebViewSize"
				, _webViewGUID
				, (int)_paddingSize.yMin, (int)_paddingSize.yMax
				, (int)_paddingSize.xMin, (int)_paddingSize.xMax);
		}

		public override void showsDialog(string _webViewGUID, bool _show)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_ShowsDialog", _webViewGUID, _show);
		}

		public override void showActivity(string _url)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_OpenWebActivity", _url);
		}
#endif

	}
}