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

		public override bool openWebView(string _webViewGUID)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_OpenWebView", _webViewGUID);
		}

		public override void closeWebView(string _webViewGUID)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_CloseWebView", _webViewGUID);
		}

		public override bool loadUrl(string _webViewGUID, string _url)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_LoadUrl", _webViewGUID, _url);
		}

		public override bool loadDataWithBaseUrl(string _webViewGUID, string _baseUrl, string _data)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_LoadDataWithBaseURL", _webViewGUID, _baseUrl, _data);
		}

		public override bool loadHtmlData(string _webViewGUID, string _htmlData)
		{
			return this.simpleWebViewPlugin.CallStatic<bool>("sSimpleWebViewManager_LoadHtmlData", _webViewGUID, _htmlData);
		}

		public override void reload(string _webViewGUID)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_Reload", _webViewGUID);
		}

		public override void stopLoading(string _webViewGUID)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_StopLoading", _webViewGUID);
		}

		public override void changeWebViewSize(string _webViewGUID, Vector4 _paddingSize)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_ChangeWebViewSize"
				, _webViewGUID
				, (int)_paddingSize.w, (int)_paddingSize.x
				, (int)_paddingSize.y, (int)_paddingSize.z);
		}

		public override void showsDialog(string _webViewGUID, bool _show)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_ShowsDialog", _webViewGUID, _show);
		}


		public override string getUserAgentString(string _webViewGUID)
		{
			return this.simpleWebViewPlugin.CallStatic<string>("sSimpleWebViewManager_GetUserAgentString", _webViewGUID);
		}

		public override void setUserAgentString(string _webViewGUID, string _userAgent)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_SetUserAgentString", _webViewGUID, _userAgent);
		}

		public override void clearCache(string _webViewGUID, bool _clear)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_CleanCache", _webViewGUID, _clear);
		}

		public override void enableBackButton(string _webViewGUID, bool _enable)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_EnableBackButton", _webViewGUID, _enable);
		}

		public override void enableOverScroll(string _webViewGUID, bool _enable)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_EnableOverScroll", _webViewGUID, _enable);
		}

		public override void enableZoom(string _webViewGUID, bool _enable)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_EnableZoom", _webViewGUID, _enable);
		}

		public override void useWideViewPort(string _webViewGUID, bool _use)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_UseWideViewPort", _webViewGUID, _use);
		}


		public override void addUrlScheme(string _webViewGUID, string _urlScheme)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_AddUrlScheme", _webViewGUID, _urlScheme);
		}

		public override void clearUrlScheme(string _webViewGUID)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_clearUrlScheme", _webViewGUID);
		}

		public override void removeUrlScheme(string _webViewGUID, string _urlScheme)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_RemoveUrlScheme", _webViewGUID, _urlScheme);
		}



		public override void enableLog(bool _enable)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_EnableLog", _enable);
		}


		public override void showActivity(string _url)
		{
			this.simpleWebViewPlugin.CallStatic("sSimpleWebViewManager_OpenWebActivity", _url);
		}
#endif

	}
}