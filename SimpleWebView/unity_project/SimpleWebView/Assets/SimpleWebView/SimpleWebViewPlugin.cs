using UnityEngine;

using System.Collections;


namespace Ninja3.Tool.Web
{
	using Debug = UnityEngine.Debug;
	
	public class SimpleWebViewPlugin
	{
		private const string cLogWord = "SimpleWebView only support Android & iOS!";

		protected bool mIsInstalled = false;

		public SimpleWebViewPlugin() 
		{
			mIsInstalled = true;
		}

		public virtual bool isInstalled()
		{
			//Debug.LogWarning("SimpleWebView only support Android & iOS!");
			return mIsInstalled;
		}

		public virtual bool install()
		{
			Debug.LogWarning(cLogWord);
			return true;
		}

		public virtual bool openWebView(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
			return false;
		}

		public virtual void closeWebView(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual bool loadUrl(string _webViewGUID, string _url)
		{
			Debug.LogWarning(cLogWord);
			return false;
		}

		public virtual bool loadHtmlData(string _webViewGUID, string _htmlData)
		{
			Debug.LogWarning(cLogWord);
			return false;
		}

		public virtual bool loadDataWithBaseUrl(string _webViewGUID, string _baseUrl, string _data)
		{
			Debug.LogWarning(cLogWord);
			return false;
		}

		public virtual void reload(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void stopLoading(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void changeWebViewSize(string _webViewGUID, Vector4 _paddingSize)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void showsDialog(string _webViewGUID, bool _show)
		{
			Debug.LogWarning(cLogWord);
		}

		#region 设置网页属性
		public virtual void setUserAgentString(string _webViewGUID, string _userAgent)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual string getUserAgentString(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
			return string.Empty;
		}

		public virtual void clearCache(string _webViewGUID, bool _clear)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void enableBackButton(string _webViewGUID, bool _enable)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void enableOverScroll(string _webViewGUID, bool _enable)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void enableZoom(string _webViewGUID, bool _enable)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void useWideViewPort(string _webViewGUID, bool _use)
		{
			Debug.LogWarning(cLogWord);
		}
		#endregion

		public virtual void addUrlScheme(string _webViewGUID, string _urlScheme)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void removeUrlScheme(string _webViewGUID, string _urlScheme)
		{
			Debug.LogWarning(cLogWord);
		}

		public virtual void clearUrlScheme(string _webViewGUID)
		{
			Debug.LogWarning(cLogWord);
		}


		public virtual void enableLog(bool _enable)
		{
			Debug.LogWarning(cLogWord);
		}


		public virtual void showActivity(string _url)
		{
			Debug.LogWarning(cLogWord);
		}
	}
}