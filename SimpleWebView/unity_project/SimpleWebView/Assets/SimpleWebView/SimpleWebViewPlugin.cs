using UnityEngine;

using System.Collections;


namespace Ninja3.Tool.Web
{
	using Debug = UnityEngine.Debug;
	
	public class SimpleWebViewPlugin
	{
		private static SimpleWebViewPlugin sNativePlugin;

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
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
			return true;
		}

		public virtual bool openWebView(string _webviewGUID)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
			return false;
		}

		public virtual void closeWebView(string _webviewGUID)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
		}

		public virtual bool loadUrl(string _webViewGUID, string _url)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
			return false;
		}

		public virtual void enableLog(bool _enable)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
		}

		public virtual void changeWebViewSize(string _webViewGUID, Rect _paddingSize)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
		}

		public virtual void showsDialog(string _webViewGUID, bool _show)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
		}

		public virtual void showActivity(string _url)
		{
			Debug.LogWarning("SimpleWebView only support Android & iOS!");
		}
	}
}