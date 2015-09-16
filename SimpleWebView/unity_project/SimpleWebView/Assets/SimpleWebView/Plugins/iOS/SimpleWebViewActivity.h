//
//  SimpleWebViewActivity.h
//  Unity-iPhone
//
//  Created by YH-0023 on 9/16/15.
//
//

#import <UIKit/UIKit.h>

#import "SimpleWebView.h"

@interface SimpleWebViewActivity : UIView<SimpleWebViewDelegate>

@property (nonatomic, retain) SimpleWebView *web;

@property (strong, nonatomic) IBOutlet UIView *webviewContainer;
@property (strong, nonatomic) IBOutlet UILabel *title;

@property (strong, nonatomic) IBOutlet UIButton *button_return;
@property (strong, nonatomic) IBOutlet UIButton *button_goBack;
@property (strong, nonatomic) IBOutlet UIButton *button_goForward;
@property (strong, nonatomic) IBOutlet UIButton *button_reload;

-(id) initWithBundle;

-(void) initcomponent;

//button callback
-(void) buttonClickedCallback:(UIButton *) button;

+(SimpleWebViewActivity *) createFromXib;
@end
