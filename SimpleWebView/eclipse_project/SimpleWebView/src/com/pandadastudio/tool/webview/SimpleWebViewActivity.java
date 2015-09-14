/*
 * create by Mardyu(yuxingde@pandadastudio.com) on 2015-09-11
 * Pandada Studio
 */


package com.pandadastudio.tool.webview;

import android.app.Activity;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.webkit.WebChromeClient;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.TextView;

// 必须主动 import R 文件, 这是因为这个工程中的  package 与真实打包的 unity 中的 bundle id 是不一样
// 这个就会导致 R 中的 id/attr/... 找不到的问题
import com.pandadastudio.tool.webview.R;

public class SimpleWebViewActivity extends Activity 
{
	private SimpleWebView mWeb = null;
	private ImageButton mButton_goBack = null;
	private ImageButton mButton_goForward = null;
	
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		// 去除最上面的 title 栏
		requestWindowFeature(Window.FEATURE_NO_TITLE);

		setContentView(R.layout.simplewebview_activity);

		initComponent();
		
		loadWebUrl();
	}

	protected void initComponent() {
		final TextView web_title = (TextView) this.findViewById(R.id.SimpleWebView_Title);
		Button web_button_return = (Button) this.findViewById(R.id.SimpleWebView_ReturnButton);
		mWeb = (SimpleWebView) this.findViewById(R.id.SimpleWebView);
		mButton_goBack = (ImageButton) this.findViewById(R.id.SimpleWebView_GoBack);
		mButton_goForward = (ImageButton) this.findViewById(R.id.SimpleWebView_GoForward);
		ImageButton web_button_reload = (ImageButton) this.findViewById(R.id.SimpleWebView_Reload);
		
		// 返回游戏场景
		if (null != web_button_return)
			web_button_return.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					SimpleWebViewActivity.this.finish();
				}
			});

		// 返回到上一个页面
		if (null != mButton_goBack) {
			mButton_goBack.setEnabled(false);
			mButton_goBack.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					SimpleWebViewManager.Log("SimpleWebViewActivity--onClick--web_button_goBack");
					if (null != mWeb) {
						mWeb.goBack();
						if (!mWeb.canGoBack())
							mButton_goBack.setEnabled(false);
						if (null != mButton_goForward && !mButton_goForward.isEnabled())
							mButton_goForward.setEnabled(true);
					}
				}
			});
		}
		
		// 游览器中的前进
		if (null != mButton_goForward) {
			mButton_goForward.setEnabled(false);
			mButton_goForward.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					SimpleWebViewManager.Log("SimpleWebViewActivity--onClick--web_button_goForward");
					if (null != mWeb) {
						
						mWeb.goForward();
						if (!mWeb.canGoForward())
							mButton_goForward.setEnabled(false);
						if (null != mButton_goBack && !mButton_goBack.isEnabled())
							mButton_goBack.setEnabled(true);
					}
				}
			});
		}
		
		// 重载当前网页
		if (null != web_button_reload)
		{
			web_button_reload.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					SimpleWebViewManager.Log("SimpleWebViewActivity--onClick--web_button_reload");
					if (null != mWeb)
						mWeb.reload();
				}
			});
		}
		
		// 更新网页的 title
		WebChromeClient wcc = new WebChromeClient() {
			@Override
			public void onReceivedTitle(WebView view, String title) {
				super.onReceivedTitle(view, title);
				SimpleWebViewManager.Log("SimpleWebViewActivity--onReceivedTitle--WebChromeClient: " + title);
				if (null != web_title)
					web_title.setText(title);
			}
		};

		mWeb.setWebChromeClient(wcc);

		WebViewClient wvc = new WebViewClient() {
			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				SimpleWebViewManager.Log("SimpleWebViewActivity--shouldOverrideUrlLoading--WebViewClient: " + url);
				// 使用自己的WebView组件来响应Url加载事件，而不是使用默认浏览器器加载页面
				mWeb.loadUrl(url);
				return true;
			}
			
			//页面加载完成之后才激活后退/前进操作
			@Override
			public void onPageFinished(WebView view, String url) {
				super.onPageFinished(view, url);
				
				// 只有网页载入完成之后才会激活 goBack/goForward
				if (null != mButton_goBack && mWeb.canGoBack() && !mButton_goBack.isEnabled())
					mButton_goBack.setEnabled(true);
				
				if (null != mButton_goForward && !mWeb.canGoForward() && mButton_goForward.isEnabled())
					mButton_goForward.setEnabled(false);
			}
		};
		mWeb.setWebViewClient(wvc);
	}

	
	protected void loadWebUrl()
	{
		SimpleWebViewManager.Log("SimpleWebViewActivity--loadWebUrl");
		if (null != mWeb)
		{
			Bundle bundle = getIntent().getExtras();
			String url = bundle.getString("url");
			mWeb.loadUrl(url);
			SimpleWebViewManager.Log("SimpleWebViewActivity--loadWebUrl, url: " + url);
		}
	}

	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		SimpleWebViewManager.Log("SimpleWebViewActivity--onConfigurationChanged");
		super.onConfigurationChanged(newConfig);
	}
	
	
}
