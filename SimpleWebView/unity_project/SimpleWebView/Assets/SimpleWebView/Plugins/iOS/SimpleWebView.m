//
//  SimpleWebViewDialog.m
//
//  Created by mardyu(yuxingde@pandadastudio.com) on 9/14/15.
//
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "UnityAppController.h"

#import "SimpleWebView.h"
#import "SimpleWebViewManager.h"

extern UIViewController* UnityGetGLViewController();
extern inline UnityAppController*	GetAppController();

@implementation SimpleWebView

- (id) initWithFrame:(CGRect)frame withGUID:(NSString *) guid addDelegate:(id<SimpleWebViewDelegate>) delegate
{
    [SimpleWebViewManager Log:@"SimpleWebView--initWithFrame, guid: %@" withMsg: guid];
    
    self = [super initWithFrame:frame];
    [self setDelegate:self];
    
    mDelegate = delegate;
    self->mGUID = guid;
    mSchemes = [[NSMutableSet alloc] init];
    
    if (nil != mDelegate)
        [mDelegate webViewCreated:self];
    
    return self;
}

- (void)dealloc
{
    [SimpleWebViewManager Log:@"SimpleWebView--dealloc"];
    if (nil != mDelegate)
        [mDelegate webViewClosed:self];
    
    mDelegate = nil;
}

- (NSString *) GUID
{
//    [SimpleWebViewManager Log:@"SimpleWebView--GUID: %@" withMsg: mGUID];
    return mGUID;
}

- (void) enableBackgroundColor:(BOOL) enable
{
    [SimpleWebViewManager Log:@"SimpleWebView--enableBackgroundColor: %@" withMsg: enable ? @"YES" : @"NO"];
    self.opaque = enable;
    self.backgroundColor = enable ? [UIColor whiteColor] : [UIColor clearColor];
}

- (void) addScheme:(NSString *) scheme
{
    [SimpleWebViewManager Log:@"SimpleWebView--addScheme: %@" withMsg:scheme];
    if (! [mSchemes containsObject:scheme])
    {
        [mSchemes addObject:scheme];
    }
}
- (void) removeScheme:(NSString *) scheme
{
    [SimpleWebViewManager Log:@"SimpleWebView--removeScheme: %@" withMsg:scheme];
    if (! [mSchemes containsObject:scheme])
        [mSchemes removeObject:scheme];
}

- (void) clearScheme
{
    [SimpleWebViewManager Log:@"SimpleWebView--clearScheme"];
    [mSchemes removeAllObjects];
}

- (void) changeSizeWithPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebView--changeSizeWithPaddingTop: top: %d, bottom: %d, left: %d, right: %d", paddingTop, paddingBottom, paddingLeft, paddingRight]];
    
    UIView *view = [UnityGetGLViewController() view];
    
    CGRect frame = [SimpleWebView getRealRect:view.frame];
    
    [self changeSize:frame WithPaddingTop:paddingTop withPaddingBottom:paddingBottom withPaddingLeft:paddingLeft withPaddingRight:paddingRight];
}

- (void) changeSize:(CGRect)frame WithPaddingTop:(int) paddingTop withPaddingBottom:(int) paddingBottom withPaddingLeft:(int) paddingLeft withPaddingRight:(int) paddingRight
{
    
    
    CGRect webViewRect = CGRectMake(paddingLeft, paddingBottom
                                    , frame.size.width - paddingLeft - paddingRight
                                    , frame.size.height - paddingBottom - paddingTop);
    self.frame = webViewRect;
    
    [SimpleWebViewManager Log:@"SimpleWebView--changeSize: width: %f, height: %f, x: %f, y: %f"
     , webViewRect.size.width, webViewRect.size.height, webViewRect.origin.x, webViewRect.origin.y];
}

+ (CGRect) getRealRect:(CGRect) frame
{
    CGFloat width = frame.size.width;
    CGFloat height = frame.size.height;
    
    //    UIDeviceOrientation orientation = [[UIDevice currentDevice] orientation];
    UIInterfaceOrientation orientation = [GetAppController() interfaceOrientation];
    
    switch (orientation) {
            
        case UIDeviceOrientationPortrait:
        case UIDeviceOrientationPortraitUpsideDown:
            break;
        case UIDeviceOrientationLandscapeLeft:
        case UIDeviceOrientationLandscapeRight:
        default:
        {
            frame.size.height = width;
            frame.size.width = height;
        }
            break;
    }
    
    return frame;
}


//--- UIWebViewDelegate
- (void) webViewDidStartLoad:(UIWebView *)webView
{
    [SimpleWebViewManager Log:@"SimpleWebView--webViewDidStartLoad: %@" withMsg:self.GUID];
    if (nil != mDelegate)
        [mDelegate webViewDidStartLoad:self];
}

- (void) webViewDidFinishLoad:(UIWebView *)webView
{
    [SimpleWebViewManager Log:@"SimpleWebView--webViewDidFinishLoad: %@" withMsg:self.GUID];
    if (nil != mDelegate)
        [mDelegate webViewDidFinishLoad:self];
}

- (void) webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebView--didFailLoadWithError--guid: %@, error: %@", self.GUID, error]];
    if (nil != mDelegate)
        [mDelegate webView:self didFailLoadWithError:error];
}

- (BOOL) webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    [SimpleWebViewManager Log:[[NSString alloc] initWithFormat:@"SimpleWebView--shouldStartLoadWithRequest--guid: %@, request: %@", self.GUID, request]];
    
    for (NSString *scheme  in mSchemes) {
        if ([request.URL.scheme hasPrefix:scheme])
        {
            if (nil != mDelegate)
                [mDelegate webView:self shouldStartLoadWithRequest:request navigationType:navigationType];
            return NO;
        }
    }
    
    return  YES;
}


@end