using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace Ninja3.Tool.Web
{
	using Logger = UnityEngine.Debug;

	public class SimpleWebViewPluginiOS : SimpleWebViewPlugin
	{
#if UNITY_IOS
		#region native func
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_EnableLog(bool enable);
		[DllImport("__Internal")]
		private static extern bool sSimpleWebViewManager_CloseWebView(string webViewName);
		[DllImport("__Internal")]
		private static extern bool sSimpleWebViewManager_OpenWebView(string webViewName);
		[DllImport("__Internal")]
		private static extern bool sSimpleWebViewManager_LoadUrl(string webViewName, string url);
		[DllImport("__Internal")]
		private static extern bool sSimpleWebViewManager_LoadHtmlData(string webViewName, string data);
		[DllImport("__Internal")]
		private static extern bool sSimpleWebViewManager_LoadDataWithBaseURL(string webViewName, string baseUrl, string data);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_Reload(string webViewName);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_StopLoading(string webViewName);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_ChangeWebViewSize(string webViewName, int paddingTop, int paddingBottom, int paddingLeft, int paddingRight);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_ShowDialog(string webViewName, bool show);
		[DllImport("__Internal")]
		private static extern string sSimpleWebViewManager_GetUserAgentString(string webViewName);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_SetUserAgentString(string webViewName, string userAgentString);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_ClearCache(string webViewName, bool clear);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_EnableOverScroll(string webViewName, bool enable);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_EnableZoom(string webViewName, bool enable);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_UseWideViewPort(string webViewName, bool use);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_AddUrlScheme(string webViewName, string scheme);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_RemoveUrlScheme(string webViewName, string scheme);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_ClearUrlScheme(string webViewName);
		[DllImport("__Internal")]
		private static extern void sSimpleWebViewManager_OpenWebActivity(string url);
		#endregion

		public SimpleWebViewPluginiOS()
		{
			mIsInstalled = false;
		}

		public override bool install ()
		{
			mIsInstalled = true;
			return mIsInstalled;
		}

		public override bool openWebView(string _webViewGUID)
		{
			return sSimpleWebViewManager_OpenWebView(_webViewGUID);
		}
		
		public override void closeWebView(string _webViewGUID)
		{
			sSimpleWebViewManager_CloseWebView(_webViewGUID);
		}
		
		public override bool loadUrl(string _webViewGUID, string _url)
		{
			return sSimpleWebViewManager_LoadUrl(_webViewGUID, _url);
		}
		
		public override bool loadDataWithBaseUrl(string _webViewGUID, string _baseUrl, string _data)
		{
			return sSimpleWebViewManager_LoadDataWithBaseURL(_webViewGUID, _baseUrl, _data);
		}
		
		public override bool loadHtmlData(string _webViewGUID, string _htmlData)
		{
			return sSimpleWebViewManager_LoadHtmlData(_webViewGUID, _htmlData);
		}
		
		public override void reload(string _webViewGUID)
		{
			sSimpleWebViewManager_Reload(_webViewGUID);
		}
		
		public override void stopLoading(string _webViewGUID)
		{
			sSimpleWebViewManager_StopLoading(_webViewGUID);
		}
		
		public override void changeWebViewSize(string _webViewGUID, Vector4 _paddingSize)
		{
			sSimpleWebViewManager_ChangeWebViewSize(_webViewGUID
			                                    , (int)_paddingSize.w, (int)_paddingSize.x
			                                    , (int)_paddingSize.y, (int)_paddingSize.z);
		}
		
		public override void showsDialog(string _webViewGUID, bool _show)
		{
			sSimpleWebViewManager_ShowDialog(_webViewGUID, _show);
		}
		
		
		public override string getUserAgentString(string _webViewGUID)
		{
			return sSimpleWebViewManager_GetUserAgentString(_webViewGUID);
		}
		
		public override void setUserAgentString(string _webViewGUID, string _userAgent)
		{
			sSimpleWebViewManager_SetUserAgentString(_webViewGUID, _userAgent);
		}
		
		public override void clearCache(string _webViewGUID, bool _clear)
		{
			sSimpleWebViewManager_ClearCache(_webViewGUID, _clear);
		}
		
		public override void enableBackButton(string _webViewGUID, bool _enable)
		{
			Logger.LogWarning("SimpleWebViewPluginiOS--enableBackButton--not support");
		}
		
		public override void enableOverScroll(string _webViewGUID, bool _enable)
		{
			sSimpleWebViewManager_EnableOverScroll(_webViewGUID, _enable);
		}
		
		public override void enableZoom(string _webViewGUID, bool _enable)
		{
			sSimpleWebViewManager_EnableZoom(_webViewGUID, _enable);
		}
		
		public override void useWideViewPort(string _webViewGUID, bool _use)
		{
			sSimpleWebViewManager_UseWideViewPort(_webViewGUID, _use);
		}
		
		
		public override void addUrlScheme(string _webViewGUID, string _urlScheme)
		{
			sSimpleWebViewManager_AddUrlScheme(_webViewGUID, _urlScheme);
		}
		
		public override void clearUrlScheme(string _webViewGUID)
		{
			sSimpleWebViewManager_ClearUrlScheme(_webViewGUID);
		}
		
		public override void removeUrlScheme(string _webViewGUID, string _urlScheme)
		{
			sSimpleWebViewManager_RemoveUrlScheme(_webViewGUID, _urlScheme);
		}
		
		
		
		public override void enableLog(bool _enable)
		{
			sSimpleWebViewManager_EnableLog(_enable);
		}
		
		
		public override void showActivity(string _url)
		{
			sSimpleWebViewManager_OpenWebActivity(_url);
		}
#endif
	}
}