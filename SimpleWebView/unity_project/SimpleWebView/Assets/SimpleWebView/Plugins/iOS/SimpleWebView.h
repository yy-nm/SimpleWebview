//
//  SimpleWebView.h
//
//  Created by mardyu(yuxingde@pandadastudio.com) on 9/15/15.
//
//

#ifndef Unity_iPhone_SimpleWebView_h
#define Unity_iPhone_SimpleWebView_h

@class SimpleWebView;

@protocol SimpleWebViewStatusDelegate
- (void) webViewCreated:(SimpleWebView *) webView;
- (void) webViewClosed:(SimpleWebView *) webView;
@end

@protocol SimpleWebViewDelegate<SimpleWebViewStatusDelegate>
- (void) webView:(SimpleWebView *)webView didFailLoadWithError:(NSError *)error;
- (void) webViewDidFinishLoad:(SimpleWebView *)webView;
- (void) webViewDidStartLoad:(SimpleWebView *)webView;
- (BOOL) webView:(SimpleWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType;
@end

@interface SimpleWebView : UIWebView<UIWebViewDelegate>
{
@private id<SimpleWebViewDelegate> mDelegate;
@private NSString *mGUID;
@private NSMutableSet *mSchemes;
}

- (id) initWithFrame:(CGRect)frame withGUID:(NSString *) guid addDelegate:(id<SimpleWebViewDelegate>) delegate;

- (NSString *) GUID;
- (void) enableBackgroundColor:(BOOL) enable;
- (void) addScheme:(NSString *) scheme;
- (void) removeScheme:(NSString *) scheme;
- (void) clearScheme;

- (void) changeSizeWithPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight;
- (void) changeSize:(CGRect)frame WithPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight;

+ (CGRect) getRealRect:(CGRect) frame;
@end


#endif
