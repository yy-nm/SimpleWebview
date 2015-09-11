using UnityEngine;

using System.Collections;

using Ninja3.Tool.Web;

public class Demo_SimpleWebViewIn2D : MonoBehaviour {

	SimpleWebView web = null;

	public void clickButton()
	{
		web = SimpleWebView.createWebView(this.gameObject);
		web.enableLog(true);

		if (! web.isIstalled())
			web.install();

		web.openWebView();
	
		web.onWebViewOpen += webViewOpened;


	}

	public void clickCloseButton()
	{
		if (null != web)
		{
			web.closeWebView();
		}
	}


	public bool webViewOpened(SimpleWebView _webView)
	{
		_webView.changeWebViewSize(new Rect(100, 100, 100, 100));
		_webView.loadUrl("http://www.baidu.com");

		return true;
	}

	public void clickOpenWeb()
	{
		if (null == web)
		{
			web = SimpleWebView.createWebView(this.gameObject);
			web.enableLog(true);

			if (!web.isIstalled())
				web.install();
		}

		web.showActivity("http://www.baidu.com");
	}
}
