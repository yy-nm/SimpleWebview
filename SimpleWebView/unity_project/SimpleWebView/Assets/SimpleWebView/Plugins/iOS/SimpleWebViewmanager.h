//
//  SimpleWebViewmanager.h
//  Unity-iPhone
//
//  Created by YH-0023 on 9/14/15.
//
//

#ifndef Unity_iPhone_SimpleWebViewmanager_h
#define Unity_iPhone_SimpleWebViewmanager_h

#import "SimpleWebView.h"

@interface SimpleWebViewManager : NSObject<SimpleWebViewDelegate>
{
@private NSMutableDictionary *mWebs;
@private BOOL mEnableLog;
}
-(id) init;
-(void) uiRotate:(NSNotification *) notification;

+ (SimpleWebViewManager *) getInstance;

+ (void) Log: (NSString *) _fmt withMsg:(NSObject *) _msg;
+ (void) Log: (NSString *) msg, ...;

// logic
+ (SimpleWebView *) GetDialogByName:(NSString *) webViewName;
+ (void) EnableLog:(BOOL) enable;
+ (BOOL) CloseWebView:(NSString *) webViewName;
+ (BOOL) OpenWebView:(NSString *)webViewName;
+ (BOOL) LoadUrl:(NSString *)webViewName withUrl:(NSString *)url;
+ (BOOL) LoadHtmlData:(NSString *)webViewName withHtmlData:(NSString *) data;
+ (BOOL) LoadDataWithBaseURL:(NSString *)webViewName withBaseUrl:(NSString *) baseUrl withData:(NSString *)data;
+ (void) Reload:(NSString *)webViewName;
+ (void) StopLoading:(NSString *)webViewName;
+ (void) ChangeWebViewSize:(NSString* )webViewName withPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight;
+ (void ) ShowDialog:(NSString *)webViewName show:(BOOL) show;
+ (NSString *) GetUserAgentString:(NSString *)webViewName;
+ (void) SetUserAgentString:(NSString *) webViewName userAgentString:(NSString *) userAgentString;
+ (void) ClearCache:(NSString *)webViewName includeDisk:(BOOL) clear;
+ (void) EnableOverScroll:(NSString *)webViewName enable:(BOOL) enable;
+ (void) EnableZoom:(NSString *) webViewName enable:(BOOL) enable;
+ (void) UseWideViewPort:(NSString *)webViewName use:(BOOL) use;
+ (void) AddUrlScheme:(NSString *) webViewName scheme:(NSString *) scheme;
+ (void) RemoveUrlScheme:(NSString *) webViewName scheme:(NSString *) scheme;
+ (void) ClearUrlScheme:(NSString *)webViewName;

// open a activity
+ (void) OpenWebActivity:(NSString *) url;
@end

#endif
