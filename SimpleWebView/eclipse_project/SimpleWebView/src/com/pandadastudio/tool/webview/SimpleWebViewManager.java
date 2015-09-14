/*
 * create by Mardyu(yuxingde@pandadastudio.com) on 2015-09-11
 * Pandada Studio
 */

package com.pandadastudio.tool.webview;

import java.util.HashMap;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import android.content.ComponentCallbacks;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.app.Activity;
import android.app.Application;
import android.util.Log;

/****************************************************************************/
// 需要设置 Android API Level 14 或以上
/****************************************************************************/
public class SimpleWebViewManager implements ComponentCallbacks
, Application.ActivityLifecycleCallbacks, SimpleWebViewDialog.SimpleWebViewDialogListener
{
	/****************************************************************************/
	// 实现 ComponentCallbacks 接口
	// 获得设备转向的回调
	/****************************************************************************/
	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onLowMemory() {
		// TODO Auto-generated method stub
		
	}

	/****************************************************************************/
	// 实现 Application.ActivityLifecycleCallbacks 接口
	// 从外部侦测  Application 从前台进入后台或者从后台回到前台
	/****************************************************************************/
	
	@Override
	public void onActivityCreated(Activity activity, Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivityStarted(Activity activity) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivityResumed(Activity activity) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivityPaused(Activity activity) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivityStopped(Activity activity) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivitySaveInstanceState(Activity activity, Bundle outState) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onActivityDestroyed(Activity activity) {
		Log("SimpleWebViewManager--onActivityDestroyed");
	}
	
	/****************************************************************************/
	// SimpleWebViewManager 
	/****************************************************************************/
	
	private Application mCurApplication = null;
	private UnityPlayerActivity mCurActivity = null;
	
	private static SimpleWebViewManager mManager = null;
	
	HashMap<String, SimpleWebViewDialog> mDialogs = null;
	
	private SimpleWebViewManager()
	{
		
	}
	
	private static SimpleWebViewManager getInstance()
	{
		if (null == mManager)
			mManager = new SimpleWebViewManager();
		return mManager;
	}
	
	private static boolean sEnableLog = false;
	private static final String sTAG = "SimpleWebViewManager";
	
	public static boolean install(Application _app, UnityPlayerActivity _curActivity)
	{
		Log("SimpleWebViewManager--install");
		SimpleWebViewManager swvw = getInstance();
		if (null != swvw.mCurApplication)
		{
			swvw.mCurApplication.unregisterActivityLifecycleCallbacks(swvw);
			swvw.mCurApplication.unregisterComponentCallbacks(swvw);
			
			if (null != swvw.mDialogs)
			{
				for(String webViewName : swvw.mDialogs.keySet())
				{
					sSimpleWebViewManager_CloseWebView(webViewName, true);
				}
				swvw.mDialogs.clear();
			}
			
			swvw.mCurApplication = null;
			swvw.mCurActivity = null;
		}
		
		if (null != _app && null != _curActivity)
		{
			swvw.mCurActivity = _curActivity;
			swvw.mCurApplication = _app;
			
			swvw.mCurApplication.registerActivityLifecycleCallbacks(swvw);
			swvw.mCurApplication.registerComponentCallbacks(swvw);
			
			swvw.mDialogs = new HashMap<String, SimpleWebViewDialog>();
			
			return true;
		}
		
		return false;
	}
	
	
	public static void Log(String _msg, int _msgLevel)
	{
		if (sEnableLog)
			Log.println(_msgLevel, sTAG, _msg);
	}
	public static void Log(String _msg)
	{
		if (sEnableLog)
			Log.println(Log.INFO, sTAG, _msg);
	}
	
	public static void LogError(String _msg)
	{
		if (sEnableLog)
			Log.println(Log.ERROR, sTAG, _msg);
	}
	
	public static void LogWaring(String _msg)
	{
		if (sEnableLog)
			Log.println(Log.WARN, sTAG, _msg);
	}
	
	
	/****************************************************************************/
	// function called by Unity
	/****************************************************************************/
	
	private static SimpleWebViewDialog getDialogByName(String _webViewName)
	{
		SimpleWebViewManager swvw = getInstance();
		if (null != swvw.mCurActivity)
		{
			if (null != swvw.mDialogs && swvw.mDialogs.containsKey(_webViewName) 
					&& null != swvw.mDialogs.get(_webViewName))
			{
				return swvw.mDialogs.get(_webViewName);
			}
		}
		
		return null;
	}
	
	public static void sSimpleWebViewManager_CloseWebView(String _webViewName)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_CloseWebView");
		sSimpleWebViewManager_CloseWebView(_webViewName, false);
	}
	
	public static boolean sSimpleWebViewManager_CloseWebView(String _webViewName
			, Boolean _isForceClose)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_CloseWebView: " + _webViewName);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.closeWeb();
				}
			});
			return true;
		}
		
		return false;
	}
	
	public static void sSimpleWebViewManager_EnableLog(boolean _enable)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_EnableLog");
		sEnableLog = _enable;
	}
	
	public static boolean sSimpleWebViewManager_OpenWebView(final String _webViewName)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_OpenWebView: " + _webViewName);
		
		final SimpleWebViewManager swvw = getInstance();
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			swvw.mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.setShowsDialog(false);
					swvw.onDialogCreated(dialog);
				}
			});
			
			return true;
		}
		else if (null != swvw.mCurActivity)
		{
			swvw.mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					SimpleWebViewDialog dialog = new SimpleWebViewDialog(_webViewName, swvw);
					dialog.show(swvw.mCurActivity.getFragmentManager(), _webViewName);
				}
			});
			
			return true;
		}
		
		return false;
	}
	
	public static boolean sSimpleWebViewManager_LoadUrl(final String _webViewName, final String _url)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_loadUrl: " + _webViewName + ", _url: " + _url);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.loadUrl(_url);
				}
			});
			return true;
		}
		
		return false;
	}
	
	public static boolean sSimpleWebViewManager_LoadHtmlData(final String _webViewName, final String _htmlData)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_loadUrl: " + _webViewName + ", _htmlData: " + _htmlData);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.loadData(_htmlData);
				}
			});
			return true;
		}
		
		return false;
	}
	
	//loadDataWithBaseURL
	public static boolean sSimpleWebViewManager_LoadDataWithBaseURL(final String _webViewName, final String _baseUrl
			, final String _data)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_LoadDataWithBaseURL: " + _webViewName 
				+ ", _baseUrl: " + _baseUrl + ", _data: " + _data);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.loadDataWithBaseURL(_baseUrl, _data);
				}
			});
			return true;
		}
		
		return false;
	}
	
	public static void sSimpleWebViewManager_Reload(final String _webViewName)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_Reload: " + _webViewName);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.reload();
				}
			});
		}
	}
	
	public static void sSimpleWebViewManager_StopLoading(final String _webViewName)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_StopLoading: " + _webViewName);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.stopLoading();
				}
			});
		}
	}
	
	public static void sSimpleWebViewManager_ChangeWebViewSize(final String _webViewName
			, final int _top, final int _bottom, final int _left, final int _right)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_changeWebViewSize: " + _webViewName);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.changeViewRect(_top, _left, _bottom, _right);
				}
			});
		}
	}

	public static void sSimpleWebViewManager_ShowsDialog(final String _webViewName, final boolean _show)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_ShowsDialog: " + _webViewName + ", _show: " + _show);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.setShowsDialog(_show);
				}
			});
		}
	}
	
	public String sSimpleWebViewManager_GetUserAgentString(String _webViewName) {
		SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_GetUserAgentString");
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			return dialog.getUserAgentString();
		}
		
		return null;
	}
	
	public static void sSimpleWebViewManager_SetUserAgentString(String _webViewName, String _useragent)
	{
		SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_SetUserAgentString: " + _useragent);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			dialog.setUserAgentString(_useragent);
		}
	}
	
	public static void sSimpleWebViewManager_CleanCache(String _webViewName) 
    {
		sSimpleWebViewManager_CleanCache(_webViewName, false);
    }
    
    public static void sSimpleWebViewManager_CleanCache(String _webViewName, final boolean _clear) 
    {
    	Log("SimpleWebViewManager--sSimpleWebViewManager_CleanCache: " + _webViewName + ", _clear: " + _clear);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.cleanCache(_clear);
				}
			});
		}
    }
    
    public static void sSimpleWebViewManager_EnableBackButton(String _webViewName, boolean _enable)
	{
		SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_EnableBackButton: " + _webViewName
				+ ", _enable" + _enable);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			dialog.enableBackButton(_enable);
		}
	}

    public static void sSimpleWebViewManager_EnableOverScroll(String _webViewName, final boolean _enable) 
    {
    	Log("SimpleWebViewManager--sSimpleWebViewManager_EnableOverScroll: " + _webViewName + ", _enable: " + _enable);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.enableOverScroll(_enable);
				}
			});
		}
    }
    
    public static void sSimpleWebViewManager_EnableZoom(String _webViewName, final boolean _enable) 
    {
    	Log("SimpleWebViewManager--sSimpleWebViewManager_EnableZoom: " + _webViewName + ", _enable: " + _enable);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.enableZoom(_enable);
				}
			});
		}
    }
    
    public static void sSimpleWebViewManager_UseWideViewPort(String _webViewName, final boolean _use) 
    {
    	Log("SimpleWebViewManager--sSimpleWebViewManager_UseWideViewPort: " + _webViewName + ", _use: " + _use);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			getInstance().mCurActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					dialog.useWideViewPort(_use);
				}
			});
		}
    }

    public static void sSimpleWebViewManager_AddUrlScheme(String _webViewName, String _scheme)
	{
    	SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_AddUrlScheme: " + _webViewName
				+ ", _scheme" + _scheme);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			dialog.addUrlScheme(_scheme);
		}
	}
	
    public static void sSimpleWebViewManager_RemoveUrlScheme(String _webViewName, String _scheme)
	{
    	SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_RemoveUrlScheme: " + _webViewName
				+ ", _scheme" + _scheme);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			dialog.removeUrlScheme(_scheme);
		}
	}
    
    public static void sSimpleWebViewManager_clearUrlScheme(String _webViewName)
	{
    	SimpleWebViewManager.Log("SimpleWebViewManager--sSimpleWebViewManager_RemoveUrlScheme: " + _webViewName);
		
		final SimpleWebViewDialog dialog = getDialogByName(_webViewName);
		if (null != dialog)
		{
			dialog.clearUrlScheme();
		}
	}

	
	
	/**
	 * 开启全屏网页(自带布局)
	 * @param _url
	 */
	public static void sSimpleWebViewManager_OpenWebActivity(final String _url)
	{
		Log("SimpleWebViewManager--sSimpleWebViewManager_OpenWebActivity: " + _url);
		SimpleWebViewManager swvw = getInstance();
		if (null != swvw.mCurActivity)
		{
			Bundle bundle = new Bundle();
			bundle.putString("url", _url);
			Intent intent = new Intent(swvw.mCurActivity, SimpleWebViewActivity.class);
			intent.putExtras(bundle);
			swvw.mCurActivity.startActivity(intent);
		}
	}
	
	/****************************************************************************/
	// SimpleWebViewDialog.SimpleWebViewDialogListener
	/****************************************************************************/
	@Override
	public void onDialogCloseByKey(SimpleWebViewDialog _dialog) {
		Log("SimpleWebViewManager--onDialogCloseByKey: " + _dialog.getWebViewGUID());
		
		if (null != mDialogs && mDialogs.containsKey(_dialog.getWebViewGUID()))
		{
			mDialogs.remove(_dialog.getWebViewGUID());
		}
		
		CallUnityFunc(_dialog.getWebViewGUID(), "onDialogCloseByKey");
	}

	@Override
	public void onDialogOnKey(SimpleWebViewDialog _dialog, int _keyCode) 
	{
		Log("SimpleWebViewManager--onDialogOnKey: " + _dialog.getWebViewGUID() + ", _keyCode: " + _keyCode);
		CallUnityFunc(_dialog.getWebViewGUID(), "onDialogOnKey", String.valueOf(_keyCode));
	}

	@Override
	public void onDialogClosed(SimpleWebViewDialog _dialog) {
		
		Log("SimpleWebViewManager--onDialogClose: " + _dialog.getWebViewGUID());
		
		if (null != mDialogs && mDialogs.containsKey(_dialog.getWebViewGUID()))
		{
			mDialogs.remove(_dialog.getWebViewGUID());
		}
		
		CallUnityFunc(_dialog.getWebViewGUID(), "onDialogClosed");
	}
	
	@Override
	public void onDialogCreated(SimpleWebViewDialog _dialog) {
		Log("SimpleWebViewManager--onDialogOpen: " + _dialog.getWebViewGUID());
		
		if (null != mDialogs && !mDialogs.containsKey(_dialog.getWebViewGUID()))
		{
			mDialogs.put(_dialog.getWebViewGUID(), _dialog);
		}
		
		CallUnityFunc(_dialog.getWebViewGUID(), "onDialogCreated");
		
	}

	@Override
	public void onPageStarted(SimpleWebViewDialog _dialog, String _url) 
	{
		Log("SimpleWebViewManager--onPageStarted: " + _dialog.getWebViewGUID() + ", _url: " + _url);
		CallUnityFunc(_dialog.getWebViewGUID(), "onPageStarted", _url);
	}

	@Override
	public void onPageFinished(SimpleWebViewDialog _dialog, String _url) 
	{
		Log("SimpleWebViewManager--onPageFinished: " + _dialog.getWebViewGUID() + ", _url: " + _url);
		CallUnityFunc(_dialog.getWebViewGUID(), "onPageFinished", _url);
	}

	@Override
	public void onReceivedError(SimpleWebViewDialog _dialog, int _errorCode,
			String _description, String _failingUrl) {
		Log("SimpleWebViewManager--onReceivedError: " + _dialog.getWebViewGUID() 
				+ ", _errorCode: " + _errorCode + ", _description: " + _description
				+ ", _failingUrl: " + _failingUrl);
		CallUnityFunc(_dialog.getWebViewGUID(), "onReceivedError", String.valueOf(_errorCode), _description, _failingUrl);
	}

	@Override
	public boolean urlMatchedScheme(SimpleWebViewDialog _dialog,
			String _url) {
		
		Log("SimpleWebViewManager--shouldOverrideUrlLoading: " + _dialog.getWebViewGUID() + ", _url: " + _url);
		CallUnityFunc(_dialog.getWebViewGUID(), "urlMatchedScheme", _url);
		
		return false;
	}

	@Override
	public void onJavaScriptFinished(SimpleWebViewDialog _dialog, String _result) {
		Log("SimpleWebViewManager--onJavaScriptFinished: " + _dialog.getWebViewGUID() + ", _result: " + _result);
		CallUnityFunc(_dialog.getWebViewGUID(), "onJavaScriptFinished", _result);
	}
	
	/****************************************************************************/
	// call Unity func
	/****************************************************************************/
	
	private static final String cParamSplitWord = ",param: ";
	
	public static void CallUnityFunc(String _unityObj, String _func, String... _params)
	{
		StringBuilder sb = new StringBuilder();
		sb.append(String.format("unity object: %s, func: %s", _unityObj, _func));
		for(int i = 0; null != _params && i < _params.length; i++)
		{
			sb.append(String.format("Param%d: %s", i, _params[i]));
		}
		Log(sb.toString());
		
		// 多参数合并
		sb = new StringBuilder();
		if (null != _params)
		{
			if (1 == _params.length)
				sb.append(_params[0]);
			else
			{
				for(String param : _params)
				{
					sb.append(String.format(cParamSplitWord + "%s", param));
				}
			}
		}
		
		SimpleWebViewManager swvw = getInstance();
		if (null != swvw.mCurActivity)
		{

			UnityPlayer.UnitySendMessage(_unityObj, _func, sb.toString());
		}
		else
		{
			LogError(String.format("SimpleWebViewManager--CallUnityFunc--%s--%s, activity is null", _unityObj, _func));
		}
	}


}
