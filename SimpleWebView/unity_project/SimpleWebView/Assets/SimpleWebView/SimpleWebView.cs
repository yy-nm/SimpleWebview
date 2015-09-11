using UnityEngine;

using System.Collections;
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
	public delegate bool WebViewDelegate_WebViewOpen(SimpleWebView _webView);
	public delegate void WebViewDelegate_KeyDown(SimpleWebView _webView, int _keyCode);
	public delegate Rect WebViewDelegate_ScreenOreitationChanged(SimpleWebView _webView, int _orientation);
	#endregion
	
	
	public class SimpleWebView : MonoBehaviour
	{
		#region Events
		public event WebViewDelegate_LoadFinished onLoadFinished;
		public event WebViewDelegate_LoadBegin onLoadBegin;
		public event WebViewDelegate_UrlMatchedScheme onUrlMatchedScheme;
		public event WebViewDelegate_EvalJavaScriptFinished onEvalJavaScriptFinished;
		public event WebViewDelegate_WebViewWillClosed onWebViewWillClosed;
		public event WebViewDelegate_WebViewOpen onWebViewOpen;
		public event WebViewDelegate_KeyDown onKeyDown;
		public event WebViewDelegate_ScreenOreitationChanged onScreenOreitationChanged;
		#endregion

		public String curUrl
		{
			get;
			private set;
		}

		private const int cResetWebViewNameCount = 3;

		private bool mIsWebViewShowing = false;

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
					Invoke("showsDialog", 1.0f);
				}
			}
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
			curUrl = _url;

			return Instance.loadUrl(this.gameObject.name, _url);
		}

		public void enableLog(bool _enable)
		{
			Instance.enableLog(_enable);
		}

		public void changeWebViewSize(Rect _paddingSize)
		{
			Instance.changeWebViewSize(this.gameObject.name, _paddingSize);
		}

		public void showsDialog()
		{
			Debug.Log("showsDialog");
			showsDialog(true);
		}

		public void showsDialog(bool _show)
		{
			mIsWebViewShowing = _show;
			Instance.showsDialog(this.gameObject.name, _show);
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

		private void onDialogClose()
		{
			Debug.Log("SimpleWebView--onDialogClose");
			if (null != onWebViewWillClosed)
				onWebViewWillClosed(this);
		}

		private void onDialogOpen()
		{
			Debug.Log("SimpleWebView--onDialogOpen");
			if (null != onWebViewOpen)
				onWebViewOpen(this);
		}

		private void onDialogWillShow()
		{

		}

		private void onDialogWillHide()
		{

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

/*
#if UNITY_IOS || UNITY_ANDROID
//
//	UniWebView.cs
//  Created by Wang Wei(@onevcat) on 2013-10-20.
//
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The main class of UniWebView. 
/// </summary>
/// <description>
/// Each gameObject with this script represent a webview object 
/// in system. Be careful: when this script's Awake() get called, it will change the name of 
/// the gameObject to make it unique in the game. So make sure this script is appeneded to a 
/// gameObject that you don't care its name.
/// </description>
public class UniWebView : MonoBehaviour {


	[SerializeField]
	private UniWebViewEdgeInsets _insets = new UniWebViewEdgeInsets(0,0,0,0);

	/// <summary>
	/// Gets or sets the insets of a UniWebView object.
	/// </summary>
	/// <value>The insets in point from top, left, bottom and right edge from the screen.</value>
	/// <description>
	/// Default value is UniWebViewEdgeInsets(0,0,0,0), which means a full screen webpage.
	/// If you want use different insets in portrait and landscape screen, use <see cref="InsetsForScreenOreitation"/>
	/// </description>
	public UniWebViewEdgeInsets insets {
		get {
			return _insets;
		}
		set {
			if (_insets != value) {
				ForceUpdateInsetsInternal(value);
			}
		}
	}

	private void ForceUpdateInsetsInternal(UniWebViewEdgeInsets insets) {
		_insets = insets;
		UniWebViewPlugin.ChangeSize(gameObject.name,
		                            this.insets.top,
		                            this.insets.left,
		                            this.insets.bottom,
		                            this.insets.right);
#if UNITY_EDITOR

#endif
    }
    
    /// <summary>
    /// The url this UniWebView should load. You should set it before loading webpage.
	/// </summary>
	public string url;

	/// <summary>
	/// If true, load the set url when in script's Start() method. 
	/// Otherwise, you should call Load() method yourself.
	/// </summary>
	public bool loadOnStart;

	/// <summary>
	/// If true, show the webview automatically when it finished loading. 
	/// Otherwise, you should listen the OnLoadComplete event and call Show() method your self.
	/// </summary>
	public bool autoShowWhenLoadComplete;

	/// <summary>
	/// Gets the current URL of the web page.
	/// </summary>
	/// <value>The current URL of this webview.</value>
	/// <description>
	/// This value indicates the main frame url of the webpage.
	/// It will be updated only when the webpage finishs or fails loading.
	/// </description>
	public string currentUrl {
		get {
			return UniWebViewPlugin.GetCurrentUrl(gameObject.name);
		}
	}

	private bool _backButtonEnable = true;
	private bool _bouncesEnable;
	private bool _zoomEnable;
	private string _currentGUID;
	private int _lastScreenHeight;

	/// <summary>
	/// Gets or sets a value indicating whether the back button of this <see cref="UniWebView"/> is enabled.
	/// It is only for Android. If true, users can use the back button of andoird device to goBack or close the web view 
	/// if there is nothing to goBack. Otherwise, the back button will do nothing when the webview is shown.
	/// This value means nothing for iOS. There is no back button for iOS devices.
	/// </summary>
	/// <value><c>true</c> if back button enabled; otherwise, <c>false</c>. Default is true</value>.
	public bool backButtonEnable {
		get {
			return _backButtonEnable;
		}
		set {
			if (_backButtonEnable != value) {
				_backButtonEnable = value;
#if UNITY_ANDROID && !UNITY_EDITOR
				UniWebViewPlugin.SetBackButtonEnable(gameObject.name, _backButtonEnable);
#endif
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UniWebView"/> can bounces or not.
	/// The default iOS webview has a bounces effect when drag out of edge. 
	/// The default Android webview has a color indicator when drag beyond the edge.
	/// UniWebView disabled these bounces effect by default. If you want the bounces, set this property to true.
	/// This property does noting in editor.
	/// </summary>
	/// <value><c>true</c> if bounces enable; otherwise, <c>false</c>.</value>
	public bool bouncesEnable {
		get {
			return _bouncesEnable;
		}
		set {
			if (_bouncesEnable != value) {
				_bouncesEnable = value;
#if !UNITY_EDITOR
				UniWebViewPlugin.SetBounces(gameObject.name, _bouncesEnable);
#endif
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UniWebView"/> can be zoomed or not.
	/// If true, users can zoom in or zoom out the webpage by a pinch gesture.
	/// This propery will valid immediately on Android. But on iOS, it will not valid until the next page loaded.
	/// You can set this property before page loading, or use Reload() to refresh current page to make it valid.
	/// </summary>
	/// <value><c>true</c> if zoom enabled; otherwise, <c>false</c>.</value> Default is false.
	public bool zoomEnable {
		get {
			return _zoomEnable;
		}
		set {
			if (_zoomEnable != value) {
				_zoomEnable = value;
#if !UNITY_EDITOR
				UniWebViewPlugin.SetZoomEnable(gameObject.name, _zoomEnable);
#endif
			}
		}
	}

	/// <summary>
	/// Get the user agent of this webview.
	/// </summary>
	/// <value>The string of user agent using by the webview.</value>
	/// <description>
	/// It is a read-only property of webview. Once it is set when the webview gets created, the user agent can not be changed again.
	/// If you want to use a customized user agent, you should call <see cref="SetUserAgent"/> method before create a webview component.
	/// </description>
	public string userAgent {
		get {
			return UniWebViewPlugin.GetUserAgent(gameObject.name);
		}
	}

	/// <summary>
	/// Set user agent string for webview. This method has no effect on the webviews which are already initiated.
	/// The user agent of a UniWebView component can not be changed once it is created.
	/// If you want to change the user agent for the webview, you have to call it before creating the UniWebView instance.
	/// </summary>
	/// <param name="value">The value user agent should be. Null or empty string will reset the user agent to the default one.</param>
	public static void SetUserAgent(string value) {
		UniWebViewPlugin.SetUserAgent(value);
	}

	/// <summary>
	/// Reset the user agent of webview. This method has no effect on the webviews which are already initiated.
	/// The user agent of a UniWebView component can not be changed once it is created.
	/// If you want to set user agent, use the <see cref="SetUserAgent"/> method.
	/// </summary>
	public static void ResetUserAgent() {
		UniWebViewPlugin.SetUserAgent(null);
	}

	/// <summary>
	/// Load current url of this UniWebView.
	/// </summary>
	public void Load() {
		string loadUrl = String.IsNullOrEmpty(url) ? "about:blank" : url.Trim();
		UniWebViewPlugin.Load(gameObject.name, loadUrl);
	}

	/// <summary>
	/// A alias method to load a specified url. 
	/// </summary>
	/// <param name="aUrl">A url to set and load</param>
	/// <description>
	/// It will set the url of this UniWebView and then load it.
	/// </description>
	public void Load(string aUrl) {
		url = aUrl;
		Load();
	}

	/// <summary>
	/// Load a HTML string.
	/// </summary>
	/// <param name="htmlString">The content HTML string for the web page.</param>
	/// <param name="baseUrl">The base URL in which the webview should to refer other resources</param>
	public void LoadHTMLString(string htmlString, string baseUrl) {
		UniWebViewPlugin.LoadHTMLString(gameObject.name, htmlString, baseUrl);
	}

	/// <summary>
	/// Reload current page.
	/// </summary>
	public void Reload() {
		UniWebViewPlugin.Reload(gameObject.name);
	}

	/// <summary>
	/// Stop loading the current request. It will do nothing if the webpage is not loading.
	/// </summary>
	public void Stop() {
		UniWebViewPlugin.Stop(gameObject.name);
	}

	/// <summary>
	/// Show this UniWebView on screen. 
	/// </summary>
	/// <description>
	/// Usually, it should be called when you get the LoadCompleteDelegate raised with a success flag true.
	/// The webview will not be presented until you call this method on it.
	/// </description>
	public void Show() {
		_lastScreenHeight = UniWebViewHelper.screenHeight;
        ResizeInternal();

		UniWebViewPlugin.Show(gameObject.name);
#if UNITY_EDITOR
		_webViewIntPtr = UniWebViewPlugin.GetIntPtr(gameObject.name);
		_hidden = false;
#endif
	}

	/// <summary>
	/// Send a piece of javascript to the web page and evaluate (execute) it.
	/// </summary>
	/// <param name="javaScript">A single javascript method call to be sent to and executed in web page</param>
	/// <description>
	/// Call this method with a single js method caliing. The webview will evaluate (execute) the javascript.
	/// OnEvalJavaScriptFinished will be raised with the result when the js eval finished.
	/// This method can only accept a single function call. You can add your js function define
	/// in the html page directly, or add it by using AddJavaScript(string javaScript) method
	/// </description>
	public void EvaluatingJavaScript(string javaScript) {
		UniWebViewPlugin.EvaluatingJavaScript(gameObject.name, javaScript);
	}

	/// <summary>
	/// Add some javascript to the web page.
	/// </summary>
	/// <param name="javaScript">Some javascript code you want to add to the page.</param>
	/// <description>
	/// This method will execute the input javascript code without raising an 
	/// OnEvalJavaScriptFinished event. You can use this method to add some customized js
	/// function to the web page, then use EvaluatingJavaScript(string javaScript) to execute it.
	/// This method will add js in a async way in Android, so you should call it earlier than EvaluatingJavaScript
	/// </description>
	public void AddJavaScript(string javaScript) {
		UniWebViewPlugin.AddJavaScript(gameObject.name, javaScript);
	}

	/// <summary>
	/// Hide this UniWebView.
	/// </summary>
	/// <description>
	/// Calling this method on a UniWebView will hide it.
	/// </description>
	public void Hide() {
#if UNITY_EDITOR
		_hidden = true;
#endif
		UniWebViewPlugin.Dismiss(gameObject.name);
	}

	/// <summary>
	/// Clean the cache of this UniWebView.
	/// </summary>
	public void CleanCache() {
		UniWebViewPlugin.CleanCache(gameObject.name);
	}

	/// <summary>
	/// Clean the cookie using in the app.
	/// </summary>
	/// <param name="key">The key under which you want to clean the cache.</param>
	/// <description>
	/// Try to clean cookies under the specified key using in the app. 
	/// If you leave the key as null or send an empty string as key, all cache will be cleared.
	/// This method will clear the cookies in memory and try to
	/// sync the change to disk. The memory opreation will return
	/// right away, but the disk operation is async and could take some time.
	/// Caution, in Android, there is no way to remove a specified cookie.
	/// So this method will call setCookie method with the key to set
	/// it to an empty value instead. Please refer to Android 
	/// documentation on CookieManager for more information.
	/// </description>
	public void CleanCookie(string key = null) {
		UniWebViewPlugin.CleanCookie(gameObject.name, key);
	}

	/// <summary>
	/// Set the background of webview to transparent.
	/// </summary>
	/// <description>
	/// In iOS, there is a grey background in webview. If you don't want it, just call this method to set it transparent.
	/// </description>
	public void SetTransparentBackground(bool transparent = true) {
		UniWebViewPlugin.TransparentBackground(gameObject.name, transparent);
	}

	/// <summary>
	/// If the tool bar is showing or not.
	/// </summary>
	/// <description>
	/// This parameter is only available in iOS. In other platform, it will be always false.
	/// </description>
	public bool toolBarShow = false;

	/// <summary>
	/// Show the tool bar. The tool bar contains three buttons: go back, go forward and close webview.
	/// The tool bar is only available in iOS. In Android, you can use the back button of device to go back.
	/// </summary>
	/// <param name="animate">If set to <c>true</c>, show it with an animation.</param>
	public void ShowToolBar(bool animate) {
#if UNITY_IOS && !UNITY_EDITOR
		toolBarShow = true;
		UniWebViewPlugin.ShowToolBar(gameObject.name,animate);
#endif
	}

	/// <summary>
	/// Hide the tool bar. The tool bar contains three buttons: go back, go forward and close webview.
	/// The tool bar is only available in iOS. In Android, you can use the back button of device to go back.
	/// </summary>
	/// <param name="animate">If set to <c>true</c>, show it with an animation.</param>
	public void HideToolBar(bool animate) {
#if UNITY_IOS && !UNITY_EDITOR
		toolBarShow = false;
		UniWebViewPlugin.HideToolBar(gameObject.name,animate);
#endif
	}

	/// <summary>
	/// Set if a default spinner should show when loading the webpage.
	/// </summary>
	/// <description>
	/// The default value is true, which means a spinner will show when the webview is on, and it is loading some thing.
	/// The spinner contains a label and you can set a message to it. <see cref=""/>
	/// You can set it false if you do not want a spinner show when loading.
	/// </description>
	/// <param name="show">If set to <c>true</c> show.</param>
	public void SetShowSpinnerWhenLoading(bool show) {
		UniWebViewPlugin.SetSpinnerShowWhenLoading(gameObject.name, show);
	}

	/// <summary>
	/// Set the label text for the spinner showing when webview loading.
	/// The default value is "Loading..."
	/// </summary>
	/// <param name="text">Text.</param>
	public void SetSpinnerLabelText(string text) {
		UniWebViewPlugin.SetSpinnerText(gameObject.name, text);
	}

	/// <summary>
	/// Set to use wide view port support or not.
	/// </summary>
	/// <param name="use">If set to <c>true</c> use view port tag in the html to determine the layout.</param>
	/// <description>
	/// This method only works (and be necessary) for Android. If you are using viewport tag in you page, you may 
	/// want to enable it before you loading and showing your page.
	/// </description>
	public void SetUseWideViewPort(bool use) {
#if UNITY_ANDROID && !UNITY_EDITOR
		UniWebViewPlugin.SetUseWideViewPort(gameObject.name, use);
#endif
	}

	/// <summary>
	/// Go to the previous page if there is any one.
	/// </summary>
	public void GoBack() {
		UniWebViewPlugin.GoBack(gameObject.name);
	}

	/// <summary>
	/// Go to the next page if there is any one.
	/// </summary>
	public void GoForward() {
		UniWebViewPlugin.GoForward(gameObject.name);
	}

	/// <summary>
	/// Adds the URL scheme. After be added, all link of this scheme will send a message when clicked.
	/// </summary>
	/// <param name="scheme">Scheme.</param>
	public void AddUrlScheme(string scheme) {
		UniWebViewPlugin.AddUrlScheme(gameObject.name, scheme);
	}

	/// <summary>
	/// Removes the URL scheme. After be removed, this kind of url will be handled by the webview.
	/// </summary>
	/// <param name="scheme">Scheme.</param>
	public void RemoveUrlScheme(string scheme) {
		UniWebViewPlugin.RemoveUrlScheme(gameObject.name, scheme);
	}

	private bool OrientationChanged() {
		int newHeight = UniWebViewHelper.screenHeight;
		if (_lastScreenHeight != newHeight) {
			_lastScreenHeight = newHeight;
			return true;
		} else {
			return false;
		}
	}
	
	private void ResizeInternal() {
		int newHeight = UniWebViewHelper.screenHeight;
		int newWidth = UniWebViewHelper.screenWidth;
		
		UniWebViewEdgeInsets newInset = this.insets;
		if (InsetsForScreenOreitation != null) {
			UniWebViewOrientation orientation = 
				newHeight >= newWidth ? UniWebViewOrientation.Portrait : UniWebViewOrientation.LandScape;
            newInset = InsetsForScreenOreitation(this, orientation);
        }
        
        ForceUpdateInsetsInternal(newInset);
    }
    
#region Messages from native
	private void LoadComplete(string message) {
		bool loadSuc = string.Equals(message, "");
		bool hasCompleteListener = (OnLoadComplete != null);

		if (loadSuc) {
			if (hasCompleteListener) {
				OnLoadComplete(this, true, null);
			}
			if (autoShowWhenLoadComplete) {
				Show();
			}
		} else {
			Debug.LogWarning("Web page load failed: " + gameObject.name + "; url: " + url + "; error:" + message);
			if (hasCompleteListener) {
				OnLoadComplete(this, false, message);
			}
		}
	}

	private void LoadBegin(string url) {
		Debug.Log("Begin to load: " + url);
		if (OnLoadBegin != null) {
			OnLoadBegin(this, url);
		}
	}

	private void ReceivedMessage(string rawMessage) {
		UniWebViewMessage message = new UniWebViewMessage(rawMessage);
		if (OnReceivedMessage != null) {
			OnReceivedMessage(this,message);
		}
	}

	private void WebViewDone(string message) {
		bool destroy = true;
		if (OnWebViewShouldClose != null) {
			destroy = OnWebViewShouldClose(this);
		}
		if (destroy) {
			Hide();
			Destroy(this);
		}
	}

	private void WebViewKeyDown(string message) {
		int keyCode = Convert.ToInt32(message);
		if (OnReceivedKeyCode != null) {
			OnReceivedKeyCode(this, keyCode);
		}
	}

	private void EvalJavaScriptFinished(string result) {
		if (OnEvalJavaScriptFinished != null) {
			OnEvalJavaScriptFinished(this, result);
		}
	}

	private IEnumerator LoadFromJarPackage(string jarFilePath) {
		WWW stream = new WWW(jarFilePath);
		yield return stream;
		if (stream.error != null) {
			if (OnLoadComplete != null) {
				OnLoadComplete(this,false,stream.error);
			}
			yield break;
		} else {
			LoadHTMLString(stream.text, "");
		}
	}
	
	#endregion

#region Life Cycle
	void Awake() {
		_currentGUID = System.Guid.NewGuid().ToString();
		gameObject.name = gameObject.name + _currentGUID;
		UniWebViewPlugin.Init(gameObject.name,
		                      this.insets.top,
		                      this.insets.left,
		                      this.insets.bottom,
		                      this.insets.right);
		_lastScreenHeight = UniWebViewHelper.screenHeight;

#if UNITY_EDITOR
#endif
	}

	void Start() {
		if (loadOnStart) {
			Load();
		}
    }

	private void OnDestroy() {
#if UNITY_EDITOR
		Clean();
#endif
		UniWebViewPlugin.Destroy(gameObject.name);
		gameObject.name = gameObject.name.Replace(_currentGUID, "");
	}

	#endregion

	private void Update() {
#if UNITY_EDITOR

#endif
		
		//Handle screen auto orientation.
		if (OrientationChanged()) {
			//ResizeInternal();
        }
    }
    
#region UnityEditor Debug
#if UNITY_EDITOR
	private Rect _webViewRect;
	private Texture2D _texture;
	private string _inputString;
	private IntPtr _webViewIntPtr;
	private bool _hidden;

	private void CreateTexture(int x, int y, int width, int height) {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			int w = 1;
			int h = 1;
			while (w < width) { w <<= 1; }
			while (h < height) { h <<= 1; }
			_webViewRect = new Rect(x, y, width, height);
			_texture = new Texture2D(w, h, TextureFormat.ARGB32, false);
		}
	}

	private void Clean() {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			_webViewIntPtr = IntPtr.Zero;
			Destroy(_texture);
			_texture = null;
		}
	}
    
    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
			if (_webViewIntPtr != IntPtr.Zero && !_hidden) {
				Vector3 pos = Input.mousePosition;
				bool down = Input.GetMouseButton(0);
				bool press = Input.GetMouseButtonDown(0);
				bool release = Input.GetMouseButtonUp(0);
				float deltaY = Input.GetAxis("Mouse ScrollWheel");
				bool keyPress = false;
				string keyChars = "";
				short keyCode = 0;
				if (_inputString.Length > 0) {
					keyPress = true;
					keyChars = _inputString.Substring(0, 1);
					keyCode = (short)_inputString[0];
					_inputString = _inputString.Substring(1);
				}

				UniWebViewPlugin.InputEvent(gameObject.name, 
				                            (int)(pos.x - _webViewRect.x), (int)(pos.y - _webViewRect.y), deltaY,
				                            down, press, release, keyPress, keyCode, keyChars,
				                            _texture.GetNativeTextureID());
				GL.IssuePluginEvent((int)_webViewIntPtr);
				Matrix4x4 m = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS(new Vector3(0, Screen.height, 0),
				                           Quaternion.identity, new Vector3(1, -1, 1));
				GUI.DrawTexture(_webViewRect, _texture);
				GUI.matrix = m;
			}
		}
	}
#endif
	#endregion
}
#endif

*/