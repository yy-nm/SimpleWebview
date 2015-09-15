//
//  SimplewebViewManager.m
//  SimpleWebView
//
//  Created by YH-0023 on 9/14/15.
//  Copyright (c) 2015 YH-0023. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "SimpleWebViewmanager.h"

// params: gameObjectName, functionName, paramForFunc
void UnitySendMessage(const char *, const char *, const char *);

@implementation SimpleWebViewManager

- (id) init
{
    self = [super init];
    self->mWebs = [[NSMutableDictionary alloc] init];
    self->mEnableLog = FALSE;
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(uiRotate:) name:UIDeviceOrientationDidChangeNotification object:nil];
    return self;
}

- (void) dealloc
{
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

-(void) uiRotate:(NSNotification *) notification
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--uiRotate"];
    
    UIDeviceOrientation orientation = [[UIDevice currentDevice] orientation];
    
    if (UIDeviceOrientationLandscapeLeft == orientation)
    {
        [SimpleWebViewManager Log:@"UIDeviceOrientationLandscapeLeft"];
    }
    else if(UIDeviceOrientationLandscapeRight == orientation)
    {
        [SimpleWebViewManager Log:@"UIDeviceOrientationLandscapeRight"];
    }
    else if (UIDeviceOrientationPortrait == orientation)
    {
        [SimpleWebViewManager Log:@"UIDeviceOrientationPortrait"];
    }
    else if (UIDeviceOrientationPortraitUpsideDown == orientation)
    {
        [SimpleWebViewManager Log:@"UIDeviceOrientationPortraitUpsideDown"];
    }
    else {

        [SimpleWebViewManager Log:@"%d", orientation];

    }
}

+ (SimpleWebViewManager *) getInstance
{
    static SimpleWebViewManager *s_Manager = nil;
    
    if (nil == s_Manager)
    {
        s_Manager = [[SimpleWebViewManager alloc] init];
    }
    
    return s_Manager;
}

+ (void) Log: (NSString *) _fmt withMsg:(NSObject *) _msg
{
    if ([SimpleWebViewManager getInstance]->mEnableLog)
    {
        NSLog(_fmt, _msg);
    }
}

+ (void) Log: (NSString *) fmt, ...
{
    if ([SimpleWebViewManager getInstance]->mEnableLog)
    {
        va_list params;
        va_start(params, fmt);
        NSLogv(fmt, params);
        va_end(params);
    }
}

// logic
+ (SimpleWebView *) GetDialogByName:(NSString *) webViewName
{
    return [[SimpleWebViewManager getInstance]->mWebs valueForKey:webViewName];
}


+ (void) EnableLog:(BOOL) enable
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebViewManager--EnableLog: %d", enable]];
    
    [SimpleWebViewManager getInstance]->mEnableLog = enable;
}

+ (BOOL) CloseWebView:(NSString *) webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--CloseWebView: %@" withMsg:webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        [web removeFromSuperview];
        return YES;
    }
    return NO;
}

+ (BOOL) OpenWebView:(NSString *)webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--OpenWebView: %@" withMsg:webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        web.hidden = YES;
        [[SimpleWebViewManager getInstance] webViewCreated:web];
    }
    else
    {
        UIView *view = UnityGetGLViewController().view;
        web = [[SimpleWebView alloc] initWithFrame:view.frame withGUID:webViewName addDelegate:[SimpleWebViewManager getInstance]];
        [view addSubview:web];
    }
    
    return YES;
}

+ (BOOL) LoadUrl:(NSString *)webViewName withUrl:(NSString *)url
{
    
    [SimpleWebViewManager Log:@"SimpleWebViewManager--LoadUrl: %@, url:%@", webViewName, url];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        [web loadRequest:[[NSURLRequest alloc] initWithURL:[[NSURL alloc] initWithString:url]]];
        
        return YES;
    }
    
    return NO;
}

+ (BOOL) LoadHtmlData:(NSString *)webViewName withHtmlData:(NSString *) data
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--LoadHtmlData: %@, data: %@"
     , webViewName, data];
    
    static NSString *s_Web_MIMEType = @"text/html";
    static NSString *s_Web_Encoding = @"UTF-8";
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        [web loadData:[[NSData alloc] initWithBase64Encoding:data] MIMEType:s_Web_MIMEType textEncodingName:s_Web_Encoding baseURL:nil];
        
        return YES;
    }
    
    return NO;
}

+ (BOOL) LoadDataWithBaseURL:(NSString *)webViewName withBaseUrl:(NSString *) baseUrl withData:(NSString *)data
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--LoadDataWithBaseURL: %@, data: %@, baseUrl: %@", webViewName, data, baseUrl];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        [web loadHTMLString:data baseURL:[[NSURL alloc] initWithString:baseUrl]];
        return YES;
    }
    
    return NO;
}

+ (void) Reload:(NSString *)webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--Reload: %@" withMsg:webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web)
    {
        [web reload];
    }
}

+ (void) StopLoading:(NSString *)webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--StopLoading: %@" withMsg:webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if (nil != web && [web isLoading])
    {
        [web stopLoading];
    }
}

+ (void) ChangeWebViewSize:(NSString* )webViewName withPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight;
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--ChangeWebViewSize: %@, top: %d, bottom: %d, left: %d, right: %d"
     , webViewName, paddingTop, paddingBottom, paddingLeft, paddingRight];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        [web changeSizeWithPaddingTop:paddingTop withPaddingBottom:paddingBottom withPaddingLeft:paddingLeft withPaddingRight:paddingRight];
    }
}

+ (void ) ShowDialog:(NSString *)webViewName show:(BOOL) show
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--ShowDialog: %@, show:%d", webViewName, show];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        web.hidden = !show;
    }
}

+ (NSString *) GetUserAgentString:(NSString *)webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--GetUserAgentString: %@", webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        NSString *userAgent = [[NSUserDefaults standardUserDefaults] objectForKey:@"UserAgent"];
        return userAgent;
    }
    
    return nil;
}

+ (void) SetUserAgentString:(NSString *) webViewName userAgentString:(NSString *) userAgentString
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--SetUserAgentString: %@, userAgentString: %@"
     , webViewName, userAgentString];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:userAgentString
                              , @"UserAgent", nil];
        [[NSUserDefaults standardUserDefaults] registerDefaults:dict];
    }
}

+ (void) ClearCache:(NSString *)webViewName includeDisk:(BOOL) clear
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--ClearCache: %@, clear:%d", webViewName, clear];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        // if clear is true, clear include cookies
        [[NSURLCache sharedURLCache] removeAllCachedResponses];
        
        if (clear)
        {
            for (NSHTTPCookie *cookie in [[NSHTTPCookieStorage sharedHTTPCookieStorage] cookies])
            {
                [[NSHTTPCookieStorage sharedHTTPCookieStorage] deleteCookie:cookie];
            }
        }
    }
}

+ (void) EnableOverScroll:(NSString *)webViewName enable:(BOOL) enable
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--EnableOverScroll: %@, enable:%d"
     , webViewName, enable];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        for (UIView *view in web.subviews) {
            if ([view isKindOfClass:[UIScrollView class]])
            {
                UIScrollView *sv = (UIScrollView *)view;
                sv.bounces = enable;
            }
        }
    }
}

+ (void) EnableZoom:(NSString *) webViewName enable:(BOOL) enable
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--EnableZoom: %@, enable:%d"
     , webViewName, enable];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        web.scalesPageToFit = enable;
    }
}

+ (void) UseWideViewPort:(NSString *)webViewName use:(BOOL) use
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--UseWideViewPort: %@, use:%d"
     , webViewName, use];
    
    [SimpleWebViewManager Log:@"ios is not/always support viewport"];
}

+ (void) AddUrlScheme:(NSString *) webViewName scheme:(NSString *) scheme
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--AddUrlScheme: %@, scheme: %@"
     , webViewName, scheme];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        [web addScheme:scheme];
    }
}

+ (void) RemoveUrlScheme:(NSString *) webViewName scheme:(NSString *) scheme
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--RemoveUrlScheme: %@, scheme:%@"
     , webViewName, scheme];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        [web removeScheme:scheme];
    }
}

+ (void) ClearUrlScheme:(NSString *)webViewName
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--ClearUrlScheme: %@", webViewName];
    
    SimpleWebView *web = [SimpleWebViewManager GetDialogByName:webViewName];
    if(nil != web)
    {
        [web clearScheme];
    }
}


// open a activity
+ (void) OpenWebActivity:(NSString *) url
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--OpenWebActivity: %@", url];
    
    [SimpleWebViewManager Log:@"not support now"];
    // TODO
}


// SimpleWebViewDelegate
- (void) webViewCreated:(SimpleWebView *) webView
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--webViewCreated: %@" withMsg:webView.GUID];
    
    if (![mWebs.allKeys containsObject:webView.GUID])
        [mWebs setObject:webView forKey:webView.GUID];
    
    UnitySendMessage([webView.GUID UTF8String], "onDialogCreated", "");
}

- (void) webViewClosed:(SimpleWebView *) webView
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--webViewClosed: %@" withMsg:webView.GUID];
    [mWebs removeObjectForKey:webView.GUID];
    
    UnitySendMessage([webView.GUID UTF8String], "onDialogClosed", NULL);
}

- (void) webView:(SimpleWebView *)webView didFailLoadWithError:(NSError *)error
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebViewManager--didFailLoadWithError--guid: %@, error: %@", webView.GUID, error]];
    
    NSString *params = [[NSString alloc] initWithFormat:@",param: %d,param: %@, param: %@"
                       , error.code, error.description, webView.request.URL.absoluteString];
    UnitySendMessage([webView.GUID UTF8String], "onReceivedError", [params UTF8String]);
}

- (void) webViewDidFinishLoad:(SimpleWebView *)webView
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--webViewDidFinishLoad: %@" withMsg:webView.GUID];
    
    UnitySendMessage([webView.GUID UTF8String], "onPageFinished", [webView.request.URL.absoluteString UTF8String]);
}

- (void) webViewDidStartLoad:(SimpleWebView *)webView
{
    [SimpleWebViewManager Log:@"SimpleWebViewManager--webViewDidStartLoad: %@" withMsg:webView.GUID];
    
    UnitySendMessage([webView.GUID UTF8String], "onPageStarted", [webView.request.URL.absoluteString  UTF8String]);
}

- (BOOL) webView:(SimpleWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebViewManager--shouldStartLoadWithRequest--guid: %@, request: %@", webView.GUID, request]];
    
    UnitySendMessage([webView.GUID UTF8String], "urlMatchedScheme", [request.URL.absoluteString UTF8String]);
    return YES;
}

@end


/****************************************
 declare extern c func
*****************************************/

#ifdef __cplusplus
#define EXTERN extern "C"
#else
#define EXTERN extern
#endif

EXTERN void sSimpleWebViewManager_EnableLog(bool enable);
EXTERN bool sSimpleWebViewManager_CloseWebView(const char *webViewName);
EXTERN bool sSimpleWebViewManager_OpenWebView(const char *webViewName);
EXTERN bool sSimpleWebViewManager_LoadUrl(const char *webViewName, const char *url);
EXTERN bool sSimpleWebViewManager_LoadHtmlData(const char *webViewName, const char *data);
EXTERN bool sSimpleWebViewManager_LoadDataWithBaseURL(const char *webViewName, const char *baseUrl, const char *data);
EXTERN void sSimpleWebViewManager_Reload(const char *webViewName);
EXTERN void sSimpleWebViewManager_StopLoading(const char *webViewName);
EXTERN void sSimpleWebViewManager_ChangeWebViewSize(const char *webViewName, int paddingTop, int paddingBottom, int paddingLeft, int paddingRight);
EXTERN void sSimpleWebViewManager_ShowDialog(const char *webViewName, bool show);
EXTERN const char* sSimpleWebViewManager_GetUserAgentString(const char *webViewName);
EXTERN void sSimpleWebViewManager_SetUserAgentString(const char *webViewName, const char *userAgentString);
EXTERN void sSimpleWebViewManager_ClearCache(const char *webViewName, bool clear);
EXTERN void sSimpleWebViewManager_EnableOverScroll(const char *webViewName, bool enable);
EXTERN void sSimpleWebViewManager_EnableZoom(const char *webViewName, bool enable);
EXTERN void sSimpleWebViewManager_UseWideViewPort(const char *webViewName, bool use);
EXTERN void sSimpleWebViewManager_AddUrlScheme(const char *webViewName, const char *scheme);
EXTERN void sSimpleWebViewManager_RemoveUrlScheme(const char *webViewName, const char *scheme);
EXTERN void sSimpleWebViewManager_ClearUrlScheme(const char *webViewName);
// open a activity
EXTERN void sSimpleWebViewManager_OpenWebActivity(const char *url);


/****************************************
 c func called by Unity
 *****************************************/

NSString * getNSStringFromCString(const char *str)
{
    return [[NSString alloc] initWithUTF8String:str];
}

void sSimpleWebViewManager_EnableLog(bool enable)
{
    [SimpleWebViewManager EnableLog:enable];
}

bool sSimpleWebViewManager_CloseWebView(const char *webViewName)
{
    return [SimpleWebViewManager CloseWebView:getNSStringFromCString(webViewName)];
}

bool sSimpleWebViewManager_OpenWebView(const char *webViewName)
{
    return [SimpleWebViewManager OpenWebView:getNSStringFromCString(webViewName)];
}

bool sSimpleWebViewManager_LoadUrl(const char *webViewName, const char *url)
{
    return [SimpleWebViewManager LoadUrl:getNSStringFromCString(webViewName)  withUrl:getNSStringFromCString(url)];
}

bool sSimpleWebViewManager_LoadHtmlData(const char *webViewName, const char *data)
{
    return [SimpleWebViewManager LoadHtmlData:getNSStringFromCString(webViewName) withHtmlData:getNSStringFromCString(data)];
}

bool sSimpleWebViewManager_LoadDataWithBaseURL(const char *webViewName, const char *baseUrl, const char *data)
{
    return [SimpleWebViewManager LoadDataWithBaseURL:getNSStringFromCString(webViewName) withBaseUrl:getNSStringFromCString(baseUrl) withData:getNSStringFromCString(data)];
}

void sSimpleWebViewManager_Reload(const char *webViewName)
{
    [SimpleWebViewManager Reload:getNSStringFromCString(webViewName)];
}

void sSimpleWebViewManager_StopLoading(const char *webViewName)
{
    [SimpleWebViewManager StopLoading:getNSStringFromCString(webViewName)];
}

void sSimpleWebViewManager_ChangeWebViewSize(const char *webViewName, int paddingTop, int paddingBottom, int paddingLeft, int paddingRight)
{
    [SimpleWebViewManager ChangeWebViewSize:getNSStringFromCString(webViewName) withPaddingTop:paddingTop withPaddingBottom:paddingBottom withPaddingLeft:paddingLeft withPaddingRight:paddingRight];
}

void sSimpleWebViewManager_ShowDialog(const char *webViewName, bool show)
{
    [SimpleWebViewManager ShowDialog:getNSStringFromCString(webViewName) show:show];
}

const char* sSimpleWebViewManager_GetUserAgentString(const char *webViewName)
{
    NSString *result = [SimpleWebViewManager GetUserAgentString:getNSStringFromCString(webViewName)];
    if (nil == result)
        return NULL;
    else
        return [result UTF8String];
}

void sSimpleWebViewManager_SetUserAgentString(const char *webViewName, const char *userAgentString)
{
    [SimpleWebViewManager SetUserAgentString:getNSStringFromCString(webViewName) userAgentString:getNSStringFromCString(userAgentString)];
}

void sSimpleWebViewManager_ClearCache(const char *webViewName, bool clear)
{
    [SimpleWebViewManager ClearCache:getNSStringFromCString(webViewName) includeDisk:clear];
}

void sSimpleWebViewManager_EnableOverScroll(const char *webViewName, bool enable)
{
    [SimpleWebViewManager EnableOverScroll:getNSStringFromCString(webViewName) enable:enable];
}

void sSimpleWebViewManager_EnableZoom(const char *webViewName, bool enable)
{
    [SimpleWebViewManager EnableZoom:getNSStringFromCString(webViewName) enable:enable];
}

void sSimpleWebViewManager_UseWideViewPort(const char *webViewName, bool use)
{
    [SimpleWebViewManager UseWideViewPort:getNSStringFromCString(webViewName) use:use];
}

void sSimpleWebViewManager_AddUrlScheme(const char *webViewName, const char *scheme)
{
    [SimpleWebViewManager AddUrlScheme:getNSStringFromCString(webViewName) scheme:getNSStringFromCString(scheme)];
}

void sSimpleWebViewManager_RemoveUrlScheme(const char *webViewName, const char *scheme)
{
    [SimpleWebViewManager RemoveUrlScheme:getNSStringFromCString(webViewName)scheme:getNSStringFromCString(scheme)];
}

void sSimpleWebViewManager_ClearUrlScheme(const char *webViewName)
{
    [SimpleWebViewManager ClearUrlScheme:getNSStringFromCString(webViewName)];
}

void sSimpleWebViewManager_OpenWebActivity(const char *url)
{
    [SimpleWebViewManager OpenWebActivity:getNSStringFromCString(url)];
}
