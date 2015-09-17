//
//  SimpleWebViewActivity.m
//
//  Created by mardyu(yuxingde@pandadastudio.com) on 9/16/15.
//
//

#import "SimpleWebViewActivity.h"
#import "SimpleWebViewManager.h"


extern UIViewController* UnityGetGLViewController();

//
static NSString * const s_jsCodeForGetTitleOfWeb = @"document.title";

static NSString * const s_xibFileName = @"SimpleWebView";
static NSString * const s_bundleName = @"SimpleWebView";
static NSString * const s_bundleType = @"bundle";

static const int s_webviewPaddingTop = 50;
static const int s_webviewPaddingBottom = 50;

@implementation SimpleWebViewActivity
@synthesize webviewContainer    = _webviewContainer;
@synthesize title               = _title;
@synthesize button_return       = _button_return;
@synthesize button_goBack       = _button_goBack;
@synthesize button_goForward    = _button_goForward;
@synthesize button_reload       = _button_reload;

- (id) init
{
    UIView *view = UnityGetGLViewController().view;
    
    return [self initWithFrame:[view frame]];
}

- (id) initWithFrame:(CGRect)frame
{
//    self = [super initWithFrame:frame];
    frame = [SimpleWebView getRealRect:frame];
    self.frame = frame;
    
    [self initcomponent];
    
    return self;
}

-(id) initWithBundle
{
    NSString *path = [[NSBundle mainBundle] pathForResource:s_bundleName ofType:s_bundleType];
    NSBundle *bundle = [NSBundle bundleWithPath:path];
    if (! [bundle isLoaded])
        [bundle load];
    
//    for (NSBundle *b in [NSBundle allBundles]) {
//        [SimpleWebViewManager Log:@"id: %@, path: %@, url: %@", b.bundleIdentifier, b.bundlePath, b.bundleURL];
//    }
    
    NSArray *objs = [bundle loadNibNamed:s_xibFileName owner:self options:nil];
    if (nil == objs || 0 == objs.count)
    {
        [SimpleWebViewManager Log:@"can find specify config file"];
        return nil;
    }
    
    SimpleWebViewActivity *web = (SimpleWebViewActivity *)objs[0];
    
    web.webviewContainer    = self.webviewContainer;
    web.title               = self.title;
    web.button_goBack       = self.button_goBack;
    web.button_goForward    = self.button_goForward;
    web.button_reload       = self.button_reload;
    web.button_return       = self.button_return;

    
    return [web init];
}

-(void) initcomponent
{
    self.title.text = @"";
    [self.button_reload setEnabled:YES];
    [self.button_return setEnabled:YES];
    
    [self.button_goBack setEnabled:NO];
    [self.button_goForward setEnabled:NO];
    

    // add callback
    [self.button_reload addTarget:self action:@selector(buttonClickedCallback:) forControlEvents:UIControlEventTouchUpInside];
    [self.button_return addTarget:self action:@selector(buttonClickedCallback:) forControlEvents:UIControlEventTouchUpInside];
    [self.button_goBack addTarget:self action:@selector(buttonClickedCallback:) forControlEvents:UIControlEventTouchUpInside];
    [self.button_goForward addTarget:self action:@selector(buttonClickedCallback:) forControlEvents:UIControlEventTouchUpInside];
    
    // init webview
    CGRect frame = self.webviewContainer.frame;
//    frame = self.webviewContainer.bounds;
    frame = self.frame;
    [SimpleWebViewManager Log:@"SimpleWebViewActivity--initcomponent--websize: width: %f, height: %f, x: %f, y:%f", frame.size.width, frame.size.height, frame.origin.x, frame.origin.y];
    self.web = [[SimpleWebView alloc] initWithFrame:self.webviewContainer.frame withGUID:@"" addDelegate:self];
    self.web.hidden = false;
    
    [self.web changeSize:self.frame WithPaddingTop:s_webviewPaddingTop withPaddingBottom:s_webviewPaddingBottom withPaddingLeft:0 withPaddingRight:0];
//    [self.webviewContainer addSubview:self.web];
    [self addSubview:self.web];
}

-(void) buttonClickedCallback:(UIButton *) button
{
    [SimpleWebViewManager Log:@"button clicked: %@", button];
    
    if(button == self.button_return)
    {
        [self removeFromSuperview];
    }
    else if (button == self.button_reload)
    {
        if (nil !=self.web)
        {
            [self.web reload];
        }
    }
    else if (button == self.button_goBack)
    {
        if (nil != self.web && [self.web canGoBack])
        {
            [self.web goBack];
            [self.button_goForward setEnabled:YES];
        }
        else
        {
            [self.button_goBack setEnabled:NO];
        }
    }
    else if (button == self.button_goForward)
    {
        if (nil != self.web && [self.web canGoForward])
        {
            [self.web goForward];
            [self.button_goBack setEnabled:YES];
        }
        else
        {
            [self.button_goForward setEnabled:NO];
        }
    }
}

+(SimpleWebViewActivity *) createFromXib;
{
    SimpleWebViewActivity *activity = [[SimpleWebViewActivity alloc] initWithBundle];
    [UnityGetGLViewController().view addSubview:activity];
    
    return activity;
}

//@protocol SimpleWebViewStatusDelegate
- (void) webViewCreated:(SimpleWebView *) webView
{}

- (void) webViewClosed:(SimpleWebView *) webView
{}


//@protocol SimpleWebViewDelegate<SimpleWebViewStatusDelegate>
- (void) webView:(SimpleWebView *)webView didFailLoadWithError:(NSError *)error
{
    self.title.text = @"";
    
    
}

- (void) webViewDidFinishLoad:(SimpleWebView *)webView
{
    self.title.text = [webView stringByEvaluatingJavaScriptFromString:@"document.title"];
    //TODO
}

- (void) webViewDidStartLoad:(SimpleWebView *)webView
{
    if([webView canGoBack])
        [self.button_goBack setEnabled:YES];
    else
        [self.button_goBack setEnabled:NO];
    
    if([webView canGoForward])
        [self.button_goForward setEnabled:YES];
    else
        [self.button_goForward setEnabled:NO];
}

- (BOOL) webView:(SimpleWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    return NO;
}


@end
