/*
 * create by Mardyu(yuxingde@pandadastudio.com) on 2015-09-11
 * Pandada Studio
 */

package com.pandadastudio.tool.webview;

import java.util.HashSet;

import android.app.Dialog;
import android.app.DialogFragment;
import android.content.DialogInterface;
import android.content.DialogInterface.OnKeyListener;
import android.graphics.Bitmap;
import android.graphics.Point;
import android.graphics.Rect;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.view.Display;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.FrameLayout;


public class SimpleWebViewDialog extends DialogFragment implements OnKeyListener {
	private final String cWeb_JavascriptInterfaceName = "SimpleWebView";
	private final String cWeb_MIMEType = "text/html";
	private final String cWeb_Encoding = "UTF-8";
	
	private String mWebViewGUID = null;
	private SimpleWebViewDialogListener mDialogListener = null;
	private SimpleWebView mWeb = null;

	private HashSet<String> mSchemes = null;

	private boolean mEnableBackbutton = false;
	
	private Rect mViewRect = null;
	
	private boolean mIsShowingDialog = false;

	public String getWebViewGUID() {
		return mWebViewGUID;
	}

	public SimpleWebViewDialog(String _guid,
			SimpleWebViewDialogListener _listener) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--SimpleWebViewDialog");

		this.mWebViewGUID = _guid;
		mDialogListener = _listener;
		mSchemes = new HashSet<String>();
		mViewRect = new Rect();
	}

	public void enableBackButton(boolean _enable) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--enableBackButton: " + _enable);
		mEnableBackbutton = _enable;
	}

	public void enableOverScroll(boolean _enable) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--enableOverScroll: " + _enable);
		if (_enable) {
			mWeb.setOverScrollMode(View.OVER_SCROLL_ALWAYS);
		} else {
			mWeb.setOverScrollMode(View.OVER_SCROLL_NEVER);
		}
	}

	public void enableZoom(boolean _enable) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--enableZoom: " + _enable);
		mWeb.getSettings().setBuiltInZoomControls(_enable);
	}
	
	public void addUrlScheme(String _scheme) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--addUrlScheme: " + _scheme);
		if (!mSchemes.contains(_scheme)) {
			mSchemes.add(_scheme);
		}
	}

	public void removeUrlScheme(String _scheme) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--removeUrlScheme: " + _scheme);
		if (mSchemes.contains(_scheme)) {
			mSchemes.remove(_scheme);
		}
	}

	public void clearUrlScheme() {
		SimpleWebViewManager.Log("SimpleWebViewDialog--clearUrlScheme");
		mSchemes.clear();
	}

	public void useWideViewPort(boolean _use) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--useWideViewPort: " + _use);
		mWeb.getSettings().setUseWideViewPort(_use);
	}

	public String getUserAgentString() {
		SimpleWebViewManager.Log("SimpleWebViewDialog--getUserAgentString");
		return mWeb.getSettings().getUserAgentString();
	}
	
	public void setUserAgentString(String _useragent)
	{
		SimpleWebViewManager.Log("SimpleWebViewDialog--setUserAgentString: " + _useragent);
		mWeb.setUserAgentString(_useragent);
	}
	
	public void changeViewRect(int _top, int _left, int _bottom, int _right) {
		mViewRect.bottom = _bottom;
		mViewRect.right = _right;
		mViewRect.left = _left;
		mViewRect.top = _top;
		
		updateViewRect();
    }
	
	public void stopLoading() {
		SimpleWebViewManager.Log("SimpleWebViewDialog--stopLoading");
		mWeb.stopLoading();
	}
	
	public void reload() {
		SimpleWebViewManager.Log("reload");
		mWeb.reload();
	}
	
	

    public void loadUrl(String _url) {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--loadUrl: " + _url);
    	mWeb.loadUrl(_url);
    }

    public void addJsString(String _jsString) {
    	
    	SimpleWebViewManager.Log("SimpleWebViewDialog--loadUrl: " + _jsString);
        if (_jsString == null) {
        	SimpleWebViewManager.Log("SimpleWebViewDialog--loadUrl: js is null");
            return;
        }

        String requestString = String.format("javascript:%s",_jsString);
        loadUrl(requestString);
    }

    public void loadDataWithBaseURL(String _baseUrl, String _data) {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--loadDataWithBaseURL: " + _baseUrl + ", html: " + _data);
    	mWeb.loadDataWithBaseURL(_baseUrl, _data, cWeb_MIMEType, cWeb_Encoding, null);
    }
    
    public void loadData(String _data)
    {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--loadData: " + _data);
    	mWeb.loadData(_data, cWeb_MIMEType, cWeb_Encoding);
    }
    
    public void cleanCache() 
    {
    	cleanCache(false);
    }
    
    public void cleanCache(boolean _clear) 
    {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--cleanCache");
    	mWeb.clearCache(_clear);
    }

    public void closeWeb() 
    {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--closeWeb");
    	loadUrl("about:blank");
    	this.dismiss();
    }

    /**
     * 重写父类的 setShowsDialog 使用 getDialog().show()/.hide() 函数来显示或者关闭界面
     */
    public void setShowsDialog(boolean _show) {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--setShowsDialog: " + _show);
    	if (_show)
    		this.getDialog().show();
    	else
    		this.getDialog().hide();
    }
    
    public boolean getShowsDialog()
    {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--getShowsDialog");
    	return super.getShowsDialog();
    }

    public void updateViewRect() 
    {
    	updateViewRect(this.getDialog());
    }
    
    /**
     * 使用 padding 方式设置 dialog 的大小
     * @param _dialog
     */
    public void updateViewRect(Dialog _dialog) {
    	SimpleWebViewManager.Log("SimpleWebViewDialog--setShowsDialog" 
    			+ String.format("left: %d, right: %d, top: %d, bottom: %d", mViewRect.left, mViewRect.right
    					, mViewRect.top, mViewRect.bottom));
        Window window = _dialog.getWindow();
        Display display = window.getWindowManager().getDefaultDisplay();
        int width;
        int height;

        Point size = new Point();
        display.getSize(size);
        width = size.x;
        height = size.y;

        window.setLayout(width - mViewRect.left - mViewRect.right
        		, height - mViewRect.top - mViewRect.bottom);

        WindowManager.LayoutParams layoutParam = window.getAttributes();
        layoutParam.gravity = Gravity.TOP | Gravity.LEFT;
        layoutParam.x = mViewRect.left;
        layoutParam.y = mViewRect.top;

        window.setAttributes(layoutParam);
    }

	private void createWebView(Dialog _dialog) {
		mWeb = new SimpleWebView(getActivity());

		WebViewClient webClient = new WebViewClient() {
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				SimpleWebViewManager.Log("WebViewClient--onPageStarted");
				if (null != mDialogListener)
				{
					mDialogListener.onPageStarted(SimpleWebViewDialog.this, url);
				}
				super.onPageStarted(view, url, favicon);
			}

			@Override
			public void onPageFinished(WebView view, String url) {

				SimpleWebViewManager.Log("WebViewClient--onPageFinished");
				if (null != mDialogListener)
				{
					SimpleWebViewDialog.this.mDialogListener.onPageFinished(SimpleWebViewDialog.this,
							url);
				}
				super.onPageFinished(view, url);
			}

			@Override
			public void onReceivedError(WebView view, int errorCode, String description, String failingUrl) {

				SimpleWebViewManager.Log("WebViewClient--onReceivedError");
				if (null != mDialogListener)
				{
					SimpleWebViewDialog.this.mDialogListener.onReceivedError(SimpleWebViewDialog.this,
							errorCode, description, failingUrl);
				}
				super.onReceivedError(view, errorCode, description, failingUrl);
			}

			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {

				SimpleWebViewManager.Log("WebViewClient--shouldOverrideUrlLoading");
				
				if (null == url || 0 == url.length())
					return super.shouldOverrideUrlLoading(view, url);
				int index = url.indexOf('?');
				if (-1 == index)
					return super.shouldOverrideUrlLoading(view, url);
				String urlScheme = url.substring(0, index);
				index = urlScheme.lastIndexOf('/');
				if (-1 != index)
				{
					urlScheme = urlScheme.substring(index + 1);
				}
				
				SimpleWebViewManager.Log("WebViewClient--shouldOverrideUrlLoading--scheme: " + urlScheme);
				
				for(String scheme : SimpleWebViewDialog.this.mSchemes)
				{
					if (urlScheme.startsWith(scheme))
					{
						if (null != mDialogListener)
							mDialogListener.urlMatchedScheme(SimpleWebViewDialog.this, url);
						else
						{
							SimpleWebViewManager.Log("WebViewClient--shouldOverrideUrlLoading--no callback");
						}
						return true;
					}
				}
				
				
				return super.shouldOverrideUrlLoading(view, url);
			}
		};
		mWeb.setWebViewClient(webClient);

		this.mWeb.setVisibility(View.VISIBLE);
		mWeb.addJavascriptInterface(this, cWeb_JavascriptInterfaceName);
		
		enableOverScroll(true);
	}

	
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--onCreate");
		super.onCreate(savedInstanceState);
	}

	@Override
	public Dialog onCreateDialog(Bundle savedInstanceState) {
		SimpleWebViewManager.Log("SimpleWebViewDialog--onCreateDialog");
//		Dialog dialog = super.onCreateDialog(savedInstanceState);
		// 使用 Theme_DeviceDefault_Light_Panel 可以达到无边框, 没有黑色背景
		Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_DeviceDefault_Light_Panel);
		// 先设置 WindowFeature, 再设置 ContentView, 最后设置背景以及全屏属性, 顺序错误会发生异常
		//  android.util.AndroidRuntimeException: requestFeature()
		dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		
		//去除 android 虚拟按键
		dialog.getWindow().getDecorView().setSystemUiVisibility(View.SYSTEM_UI_FLAG_HIDE_NAVIGATION);

		FrameLayout fl = new FrameLayout(dialog.getContext());
		fl.setVisibility(View.VISIBLE);

		// create webview
		createWebView(dialog);

		dialog.setContentView(mWeb);
		
		Window window = dialog.getWindow();
		window.setBackgroundDrawable(new ColorDrawable(android.graphics.Color.TRANSPARENT));
		window.addFlags(WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL);
		window.setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_ADJUST_RESIZE);
		updateViewRect(dialog);
		
		if (null != mDialogListener)
			mDialogListener.onDialogCreated(this);
		
		// 刚创建的 dialog 默认是不现实的, 不能直接调用使用 getDialog() 方法的函数, 因为当前 mDialog 变量还是 null,
		// 只有当前函数结束之后才会生成 mDialog 对象
//		this.setShowsDialog(false);
		dialog.hide();
		
		return dialog;
	}

	@Override
	public void onStart() {
		super.onStart();
		SimpleWebViewManager.Log("SimpleWebViewDialog--onStart");
		
		// 为了方式出现网页时, 游戏从前台切入后台, 然后返回前台时游戏界面是一片黑色
		//, 这个因为 android 只渲染 active 的层, 所以这边需要把 dialog 隐藏, 然后通过 Unity 层延时调用显示
		// dialog 的机制
		if (mIsShowingDialog)
		{
			mIsShowingDialog = false;
			getDialog().hide();
		}
	}

	@Override
	public void onStop() {
		SimpleWebViewManager.Log("SimpleWebViewDialog--onStop");
		
		mIsShowingDialog = true;
		super.onStop();
	}

	@Override
	public void onDestroyView() {

		SimpleWebViewManager.Log("SimpleWebViewDialog--onDestroyView");
		if (null != this.mDialogListener) {
			mDialogListener.onDialogClosed(this);
		}
		super.onDestroyView();
	}

	// 对于 返回键特殊处理, 其他按钮事件只做通知处理
	@Override
	public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
		if (KeyEvent.KEYCODE_BACK == keyCode) {

			// 通知 Unity 网页被关闭了
			if (mEnableBackbutton && null != this.mDialogListener) {
				SimpleWebViewManager.LogWaring("SimpleWebViewDialog--onKey--close webview");
				mDialogListener.onDialogCloseByKey(this);
				return true;
			}
		}
		
		SimpleWebViewManager.LogWaring("SimpleWebViewDialog--onKey: " + keyCode);
		if (null != mDialogListener)
			mDialogListener.onDialogOnKey(this, keyCode);
		return false;
	};

	public void test() {

	}

	/****************************************************************************/
	// SimpleWebViewDialogListener
	/****************************************************************************/
	public interface SimpleWebViewDialogListener extends
			SimpleWebViewDialogStatusListener, SimpleWebViewListener {

	}

	public interface SimpleWebViewDialogStatusListener {
		void onDialogCloseByKey(SimpleWebViewDialog _dialog);

		void onDialogOnKey(SimpleWebViewDialog _dialog, int _keyCode);

		void onDialogClosed(SimpleWebViewDialog _dialog);
		
		void onDialogCreated(SimpleWebViewDialog _dialog);
	}

	public interface SimpleWebViewListener {
		void onPageStarted(SimpleWebViewDialog _dialog, String _url);

		void onPageFinished(SimpleWebViewDialog _dialog, String _url);

		void onReceivedError(SimpleWebViewDialog _dialog, int _errorCode, String _description, String _failingUrl);

		boolean urlMatchedScheme(SimpleWebViewDialog _dialog, String _url);

		void onJavaScriptFinished(SimpleWebViewDialog _dialog, String _result);
	}
}
