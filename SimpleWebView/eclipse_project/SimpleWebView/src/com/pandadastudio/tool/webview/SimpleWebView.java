/*
 * create by Mardyu(yuxingde@pandadastudio.com) on 2015-09-11
 * Pandada Studio
 */

package com.pandadastudio.tool.webview;

import android.annotation.SuppressLint;
import android.content.Context;
import android.util.AttributeSet;
import android.view.ViewGroup;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.FrameLayout;

@SuppressLint({ "SetJavaScriptEnabled", "NewApi" })
public class SimpleWebView extends WebView {

	private String mUserAgentString = "";

	public String getUserAgentString() {
		return mUserAgentString;
	}
	
	public WebView getWebView()
	{
		return this;
	}
	
	public SimpleWebView(Context context) {
		super(context);
		
		initWebView();
	}

	private void initWebView() {
		SimpleWebViewManager.Log("SimpleWebView--SimpleWebView");
		WebView web = getWebView();
		WebSettings webSettings = web.getSettings();
		webSettings.setJavaScriptEnabled(true);
		webSettings.setDatabaseEnabled(true);
		webSettings.setDomStorageEnabled(true);
		webSettings.setAllowFileAccess(true);
		webSettings.setGeolocationEnabled(true);

		webSettings.setPluginState(WebSettings.PluginState.ON);
		webSettings.setDisplayZoomControls(false);

		web.setScrollBarStyle(WebView.SCROLLBARS_INSIDE_OVERLAY);
		web.setVerticalScrollbarOverlay(true);
		web.setLayoutParams(new FrameLayout.LayoutParams(
				ViewGroup.LayoutParams.MATCH_PARENT,
				ViewGroup.LayoutParams.MATCH_PARENT));
	}
	
	

	public SimpleWebView(Context context, AttributeSet attrs, int defStyle,
			boolean privateBrowsing) {
		super(context, attrs, defStyle, privateBrowsing);
		initWebView();
	}

	public SimpleWebView(Context context, AttributeSet attrs, int defStyleAttr) {
		super(context, attrs, defStyleAttr);
		initWebView();
	}

	public SimpleWebView(Context context, AttributeSet attrs) {
		super(context, attrs);
		initWebView();
	}

	public void setUserAgentString(String _userAgentString) {
		SimpleWebViewManager.Log("SimpleWebView--setUserAgentString:" + _userAgentString);
		if (null != _userAgentString 
				&& !_userAgentString.equals("")
				&& !this.mUserAgentString.equals(_userAgentString))
		{
			this.mUserAgentString = _userAgentString;
			
			WebSettings webSettings = this.getWebView().getSettings();
			webSettings.setUserAgentString(this.mUserAgentString);
		}
	}
	
	public void enableBackgroundColor(Boolean _enable)
	{
		SimpleWebViewManager.Log("SimpleWebView--enableBackgroundColor:" + _enable);
		if (_enable)
		{
			getWebView().setBackgroundColor(0xffffffff);
			getWebView().setLayerType(WebView.LAYER_TYPE_NONE, null);
		}
		else
		{
			getWebView().setBackgroundColor(0x00000000);
			getWebView().setLayerType(WebView.LAYER_TYPE_SOFTWARE, null);
		}
	}
}
