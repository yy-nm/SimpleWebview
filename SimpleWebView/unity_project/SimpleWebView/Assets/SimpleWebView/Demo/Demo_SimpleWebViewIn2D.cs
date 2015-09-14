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

		web.clearCache(true);
		web.openWebView();
		web.changeWebViewSize(100, 100, 100, 100);
		web.loadUrl("http://www.baidu.com");

	}

	public void clickCloseButton()
	{
		if (null != web)
		{
			web.closeWebView();
		}
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
