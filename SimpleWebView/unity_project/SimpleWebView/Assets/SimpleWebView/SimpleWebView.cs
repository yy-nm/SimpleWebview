using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;

namespace Ninja3.Tool.Web
{
	using Debug = UnityEngine.Debug;

	#region WebView Delegate
	public delegate void WebViewDelegate_LoadFinished(SimpleWebView _webView, bool _success, string _errorMessage);
	public delegate void WebViewDelegate_LoadBegin(SimpleWebView _webView, string _loadingUrl);
	public delegate void WebViewDelegate_UrlMatchedScheme(SimpleWebView _webView, string _matchedUrl);
	public delegate void WebViewDelegate_EvalJavaScriptFinished(SimpleWebView _webView, string _result);
	public delegate bool WebViewDelegate_WebViewWillClosed(SimpleWebView _webView);
	//public delegate bool WebViewDelegate_WebViewOpen(SimpleWebView _webView);
	public delegate void WebViewDelegate_KeyDown(SimpleWebView _webView, int _keyCode);
	//public delegate Rect WebViewDelegate_ScreenOreitationChanged(SimpleWebView _webView, int _orientation);
	#endregion
	
	public class SimpleWebView : MonoBehaviour
	{
		#region Events
		public event WebViewDelegate_LoadFinished onLoadFinished;
		public event WebViewDelegate_LoadBegin onLoadBegin;
		public event WebViewDelegate_UrlMatchedScheme onUrlMatchedScheme;
		public event WebViewDelegate_EvalJavaScriptFinished onEvalJavaScriptFinished;
		public event WebViewDelegate_WebViewWillClosed onWebViewWillClosed;
		//public event WebViewDelegate_WebViewOpen onWebViewCreated;
		public event WebViewDelegate_KeyDown onKeyDown;
		//public event WebViewDelegate_ScreenOreitationChanged onScreenOreitationChanged;
		#endregion

		protected class SimpleWebViewAttr
		{
			public const int cDefaultValueForbool = -1;
			public String url = string.Empty;
			public Vector4 v4Padding = Vector4.zero;
			public String htmlData = string.Empty;
			public String userAgentString = string.Empty;
			public int clearCacheOrNot = cDefaultValueForbool;
			public int enableBackButton = cDefaultValueForbool;
			public int enableOverScroll = cDefaultValueForbool;
			public int enableZoom = cDefaultValueForbool;
			public int useWideViewPort = cDefaultValueForbool;
			public HashSet<string> urlSchemes = new HashSet<string>();
		}

		protected SimpleWebViewAttr mAttr = new SimpleWebViewAttr();
		protected SimpleWebViewAttr mAttrDefault = new SimpleWebViewAttr();

		/// <summary>
		/// 在游戏恢复时延时多久显示网页, 默认值是 1.0s
		/// </summary>
		public float delayShowDialogAfterGameResume = 1.0f;

		protected bool mIsDialogCreated = false;
		private const int cResetWebViewNameCount = 3;

		private bool mIsWebViewShowing = true;

		private static SimpleWebViewPlugin sNativePlugin = null;

		private static SimpleWebViewPlugin Instance
		{
			get
			{
				if (null != sNativePlugin)
					return sNativePlugin;
#if UNITY_EDITOR
				sNativePlugin = new SimpleWebViewPlugin();
#elif UNITY_ANDROID
				sNativePlugin = new SimpleWebViewPluginAndroid();
#elif UNITY_IOS
				sNativePlugin = new SimpleWebViewPluginiOS();
#else
				sNativePlugin = new SimpleWebViewPlugin();
#endif
				return sNativePlugin;
			}
		}

		public static SimpleWebView createWebView(GameObject _obj)
		{
			int i = 0;
			string goName = Guid.NewGuid().ToString();
			while(++i > cResetWebViewNameCount)
			{
				GameObject goWithSameName = GameObject.Find(goName);
				if (null == goWithSameName)
					break;
				goName = Guid.NewGuid().ToString();
			}

			if (i > cResetWebViewNameCount)
			{
				Debug.LogError("createWebView fail");
				return null;
			}

			GameObject go = new GameObject();
			if (null != _obj)
				go.transform.SetParent(_obj.transform);
			go.name = goName;
			SimpleWebView web = go.AddComponent<SimpleWebView>();
			return web;
		}

		// 程序退到后台时候会调用, pauseStatus = true;
		// 程序回到前台时候会调用, pauseStatus = false;
		void OnApplicationPause(bool pauseStatus)
		{
			Debug.Log("OnApplicationPause: " + pauseStatus + ", mIsWebViewShowing: " + mIsWebViewShowing);
			if (mIsWebViewShowing)
			{
				if(pauseStatus)
				{
					showsDialog(!pauseStatus);
					mIsWebViewShowing = true;
				}
				else
				{
					Debug.Log("invoke showsDialog");
					Invoke("showsDialog", delayShowDialogAfterGameResume);
				}
			}
		}


		private void applyParamsAfterWebViewCreated()
		{
			if (Vector4.zero != mAttr.v4Padding)
				changeWebViewSize(mAttr.v4Padding);

			if (!string.IsNullOrEmpty(mAttr.userAgentString))
				setUserAgentString(mAttr.userAgentString);

			if (mAttrDefault.clearCacheOrNot != mAttr.clearCacheOrNot)
				clearCache(mAttr.clearCacheOrNot == 1 ? true : false);

			#region 次要属性设置
			if (mAttrDefault.enableBackButton != mAttr.enableBackButton)
				enableBackButton(mAttr.enableBackButton == 1 ? true : false);

			if (mAttrDefault.enableOverScroll != mAttr.enableOverScroll)
				enableOverScroll(mAttr.enableOverScroll == 1 ? true : false);

			if (mAttrDefault.enableZoom != mAttr.enableZoom)
				enableZoom(mAttr.enableZoom == 1 ? true : false);

			if (mAttrDefault.useWideViewPort != mAttr.useWideViewPort)
				useWideViewPort(mAttr.useWideViewPort == 1 ? true : false);
			#endregion
			
			foreach(var urlScheme in mAttr.urlSchemes)
			{
				addUrlScheme(urlScheme);
			}

			if (! string.IsNullOrEmpty(mAttr.url) && ! string.IsNullOrEmpty(mAttr.htmlData))
			{
				loadDataWithBaseUrl(mAttr.url, mAttr.htmlData);
			}
			else if (! string.IsNullOrEmpty(mAttr.url))
			{
				loadUrl(mAttr.url);
			}
			else if (! string.IsNullOrEmpty(mAttr.htmlData))
			{
				loadHtmlData(mAttr.htmlData);
			}

			if (mIsWebViewShowing)
				showsDialog();
		}


		#region call native func
		public bool isIstalled()
		{
			return Instance.isInstalled();
		}

		/// <summary>
		/// 初始化底层环境
		/// </summary>
		public bool install()
		{
			return Instance.install();
		}

		public virtual bool openWebView()
		{
			mIsWebViewShowing = true;
			return Instance.openWebView(this.gameObject.name);
		}

		public virtual void closeWebView()
		{
			Instance.closeWebView(this.gameObject.name);
		}

		public bool loadUrl(string _url)
		{
			mAttr.url = _url;
			mAttr.htmlData = string.Empty;

			if (mIsDialogCreated)
				return Instance.loadUrl(this.gameObject.name, _url);
			else
				return true;
		}

		public bool loadHtmlData(string _htmlData)
		{
			mAttr.url = string.Empty;
			mAttr.htmlData = _htmlData;

			if (mIsDialogCreated)
				return Instance.loadHtmlData(this.gameObject.name, _htmlData);
			else
				return true;
		}

		public bool loadDataWithBaseUrl(string _baseUrl, string _data)
		{
			mAttr.url = _baseUrl;
			mAttr.htmlData = _data;

			if (mIsDialogCreated)
				return Instance.loadDataWithBaseUrl(this.gameObject.name, _baseUrl, _data);
			else
				return true;
		}

		public void reload()
		{
			Instance.reload(this.gameObject.name);
		}

		public void stopLoading()
		{
			Instance.stopLoading(this.gameObject.name);
		}

		/// <summary>
		/// 被 invoke 函数调用的函数不能有参数, 默认形参也不行
		/// </summary>
		public void showsDialog()
		{
			showsDialog(true);
		}

		public void showsDialog(bool _show)
		{
			mIsWebViewShowing = _show;
			if (mIsDialogCreated)
				Instance.showsDialog(this.gameObject.name, _show);
		}

		public void changeWebViewSize(int _paddingTop, int _paddingBottom, int _paddingLeft
			, int _paddingRight)
		{
			mAttr.v4Padding.w = _paddingTop;
			mAttr.v4Padding.x = _paddingBottom;
			mAttr.v4Padding.y = _paddingLeft;
			mAttr.v4Padding.z = _paddingRight;

			changeWebViewSize(mAttr.v4Padding);
		}

		private void changeWebViewSize(Vector4 _paddingSize)
		{
			if (mIsDialogCreated)
				Instance.changeWebViewSize(this.gameObject.name, _paddingSize);
		}

		#region 设置网页属性
		public void setUserAgentString(string _userAgent)
		{
			mAttr.userAgentString = _userAgent;
			if (mIsDialogCreated)
				Instance.setUserAgentString(this.gameObject.name, _userAgent);
		}

		public string getUserAgentString()
		{
			if (mIsDialogCreated)
				return Instance.getUserAgentString(this.gameObject.name);
			return mAttr.userAgentString;
		}

		public void clearCache(bool _clearFileInDisk = false)
		{
			mAttr.clearCacheOrNot = _clearFileInDisk ? 1 : 0;
			if (mIsDialogCreated)
				Instance.clearCache(this.gameObject.name, _clearFileInDisk);
		}

		public void enableBackButton(bool _enable)
		{
			mAttr.enableBackButton = _enable ? 1 : 0;
			if (mIsDialogCreated)
				Instance.enableBackButton(this.gameObject.name, _enable);
		}

		/// <summary>
		/// 显示 网页上的 scrollbar
		/// </summary>
		/// <param name="_enable"></param>
		public void enableOverScroll(bool _enable)
		{
			mAttr.enableOverScroll = _enable ? 1 : 0;
			if (mIsDialogCreated)
				Instance.enableOverScroll(this.gameObject.name, _enable);
		}

		public void enableZoom(bool _enable)
		{
			mAttr.enableZoom = _enable ? 1 : 0;
			if (mIsDialogCreated)
				Instance.enableZoom(this.gameObject.name, _enable);
		}

		public void useWideViewPort(bool _use)
		{
			mAttr.useWideViewPort = _use ? 1 : 0;
			if (mIsDialogCreated)
				Instance.useWideViewPort(this.gameObject.name, _use);
		}
		#endregion

		public void addUrlScheme(string _urlScheme)
		{
			mAttr.urlSchemes.Add(_urlScheme);

			if (mIsDialogCreated)
				Instance.addUrlScheme(this.gameObject.name, _urlScheme);
		}

		public void removeUrlScheme(string _urlScheme)
		{
			if (mAttr.urlSchemes.Contains(_urlScheme))
				mAttr.urlSchemes.Remove(_urlScheme);

			if (mIsDialogCreated)
				Instance.removeUrlScheme(this.gameObject.name, _urlScheme);
		}

		public void clearUrlScheme()
		{
			mAttr.urlSchemes.Clear();
			if (mIsDialogCreated)
				Instance.clearUrlScheme(this.gameObject.name);
		}


		public void enableLog(bool _enable)
		{
			Instance.enableLog(_enable);
		}


		public void showActivity(string _url)
		{
			Instance.showActivity(_url);
		}
		#endregion


		#region call by Native
		private void onDialogCloseByKey()
		{
			Debug.Log("SimpleWebView--onDialogCloseByKey");
			if (null != onWebViewWillClosed)
				onWebViewWillClosed(this);
		}

		private void onDialogOnKey(int _keyCode)
		{
			Debug.Log("SimpleWebView--onDialogOnKey: " + _keyCode);
			if (null != onKeyDown)
				onKeyDown(this, _keyCode);
		}

		private void onDialogClosed()
		{
			Debug.Log("SimpleWebView--onDialogClosed");
			if (null != onWebViewWillClosed)
				onWebViewWillClosed(this);
		}

		private void onDialogCreated()
		{
			Debug.Log("SimpleWebView--onDialogCreated");
			mIsDialogCreated = true;

			applyParamsAfterWebViewCreated();
		}

		private void onPageStarted(string _url)
		{
			Debug.Log("SimpleWebView--onPageStarted: " + _url);
			if (null != onLoadBegin)
				onLoadBegin(this, _url);
		}

		private void onPageFinished(string _url)
		{
			Debug.Log("SimpleWebView--onPageFinished: " + _url);
			if (null != onLoadFinished)
				onLoadFinished(this, true, string.Empty);
		}

		private void onReceivedError(string _errorMsg)
		{
			Debug.Log("SimpleWebView--onReceivedError: " + _errorMsg);
			if (null != onLoadFinished)
				onLoadFinished(this, false, _errorMsg);
		}

		private void urlMatchedScheme(string _url)
		{
			Debug.Log("SimpleWebView--urlMatchedScheme: " + _url);
			if (null != onUrlMatchedScheme)
				onUrlMatchedScheme(this, _url);
		}

		private void onJavaScriptFinished(string _result)
		{
			Debug.Log("SimpleWebView--onJavaScriptFinished: " + _result);
			if (null != onEvalJavaScriptFinished)
				onEvalJavaScriptFinished(this, _result);
		}
		#endregion
	}

}