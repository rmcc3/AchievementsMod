exec("./Profiles.cs");
exec("./AchGUI.gui");

if(!$Ach::Loaded) {
   $Ach::Gui::X = 10;
   $Ach::Gui::Y["General"] = 0;
   $Ach::Gui::Y["Deathmatch"] = 0;
   $Ach::Gui::Y["Building"] = 0;
   $Ach::Gui::Y["Special"] = 0;
   $Ach::Gui::Y["Unsorted"] = 0;
   $Ach::Loaded = true;
   
   $remapDivision[$remapCount] = "Achievements";
	$remapName[$remapCount] = "Open Achievements";
	$remapCmd[$remapCount] = "openAchGUI";
	$remapCount++;
}

AchGeneralh.visible = true;
AchDeathmatchh.visible = false;
AchBuildingh.visible = false;
AchSpecialh.visible = false;
AchUnsortedh.visible = false;
Achievements_Box_General.visible = true;
Achievements_Box_Building.visible = false;
Achievements_Box_Deathmatch.visible = false;
Achievements_Box_Special.visible = false;
Achievements_Box_Unsorted.visible = false;

$Ach::GUI::LastOpen = "General";

function openAchGUI()
{
   if(!$Ach::GUI::Open) {
      canvas.pushDialog(AchGUI);
      $Ach::GUI::Open = true;
   } else {
      canvas.popDialog(AchGUI);
      $Ach::GUI::Open = false;
   }
}

function clientCmdAddAch(%bitmapImage, %name, %text, %cat)
{
   if(%name $= "" || %text $= "" || !$Ach::Loaded)
      return;
      
   if($Ach::Gui::Y[%cat] $= "" || %cat $= "")
      %cat = "Unsorted";
      
      
   if(!isFile($Ach::Icons[%bitmapImage]))
      %bitmapImage = "Add-Ons/Script_achievements/images/unknown.png";
      
   $Ach::Bitmap::Locked[%name] = new GuiSwatchCtrl() {
                                 profile = "GuiDefaultProfile";
                                 horizSizing = "right";
                                 vertSizing = "bottom";
                                 position = $Ach::Gui::X SPC $Ach::Gui::Y[%cat];
                                 extent = "737 74";
                                 minExtent = "8 2";
                                 enabled = "1";
                                 visible = "1";
                                 clipToParent = "1";
                                 color = "255 255 255 110";

                                 new GuiBitmapCtrl() {
                                    profile = "GuiDefaultProfile";
                                    horizSizing = "right";
                                    vertSizing = "bottom";
                                    position = "5 5";
                                    extent = "64 64";
                                    minExtent = "8 2";
                                    enabled = "1";
                                    visible = "1";
                                    clipToParent = "1";
                                    bitmap = $Ach::Icons[%bitmapImage];
                                    wrap = "0";
                                    lockAspectRatio = "0";
                                    alignLeft = "0";
                                    overflowImage = "0";
                                    keepCached = "0";
                                    mColor = "255 255 255 255";
                                    mMultiply = "0";
                                 };
                                 new GuiMLTextCtrl() {
                                    profile = "GuiMLTextProfile";
                                    horizSizing = "right";
                                    vertSizing = "bottom";
                                    position = "77 3";
                                    extent = "670 20";
                                    minExtent = "8 2";
                                    enabled = "1";
                                    visible = "1";
                                    clipToParent = "1";
                                    lineSpacing = "2";
                                    allowColorChars = "0";
                                    maxChars = "-1";
                                    maxBitmapHeight = "-1";
                                    selectable = "1";
                                    text = %name;
                                 };
                                 new GuiMLTextCtrl() {
                                    profile = "GuiMLTextProfile";
                                    horizSizing = "right";
                                    vertSizing = "bottom";
                                    position = "93 27";
                                    extent = "654 39";
                                    minExtent = "8 2";
                                    enabled = "1";
                                    visible = "1";
                                    clipToParent = "1";
                                    lineSpacing = "2";
                                    allowColorChars = "0";
                                    maxChars = "-1";
                                    maxBitmapHeight = "-1";
                                    selectable = "1";
                                    text = %text;
                                 };
                              };
            
  eval("Achievements_Box_"@%cat@".add("@$Ach::Bitmap::Locked[%name]@");");
  
                       $Ach::Bitmap::Unlocked[%name] = new GuiSwatchCtrl() {
                        profile = "GuiDefaultProfile";
                        horizSizing = "right";
                        vertSizing = "bottom";
                        position = "0 0";
                        extent = "737 74";
                        minExtent = "8 2";
                        visible = "1";
                        color = "255 255 255 110";
                     };
                     
  $Ach::Bitmap::Locked[%name].add($Ach::Bitmap::Unlocked[%name]);  
  
  $Ach::Gui::Y[%cat] = $Ach::Gui::Y[%cat] + 78;
  eval("Achievements_Box_"@%cat@".resize(1, 1, 737,"@$Ach::Gui::Y[%cat]@");");
}

function registerAchievementIcon(%path, %name)
{
   if(%name $= "" || $Ach::Icons[%name] !$= "")
      return;
      
   if(!isFile(%path))
      %path = "Add-Ons/Script_achievements/images/unknown.png";
   
   $Ach::Icons[%name] = %path;
}

function clientCmdRegisterAchievementIcon(%path, %name)
{
   if(%name $= "" || $Ach::Icons[%name] !$= "")
      return;
      
   if(!isFile(%path))
      %path = "Add-Ons/Script_achievements/images/unknown.png";
   
   $Ach::Icons[%name] = %path;
}

function clientCmdUnlockAch(%name)
{
   if(%name $= "")
      return;   
   
   // Now check it, because it was returning weird errors before.
   if(isObject($Ach::Bitmap::Unlocked[%name]))
      $Ach::Bitmap::Unlocked[%name].delete();
}

function AchGUI::CatClick(%this, %Cat) 
{
   eval("Ach"@$Ach::GUI::LastOpen@"h.visible=false;Achievements_Box_"@$Ach::GUI::LastOpen@".visible=false;");
   eval("Ach"@%Cat@"h.visible=true;Achievements_Box_"@%Cat@".visible=true;");
   $Ach::GUI::LastOpen = %Cat;
}

function clientCmdclearAch()
{
   Achievements_Box_General.clear();
   Achievements_Box_Building.clear();
   Achievements_Box_Deathmatch.clear();
   Achievements_Box_Special.clear();
   Achievements_Box_Unsorted.clear();
   $Ach::Gui::X = 10;
   $Ach::Gui::Y["General"] = 0;
   $Ach::Gui::Y["Deathmatch"] = 0;
   $Ach::Gui::Y["Building"] = 0;
   $Ach::Gui::Y["Special"] = 0;
   $Ach::Gui::Y["Unsorted"] = 0;
}

function clientCmdIHaveAchievementsMod()
{
   commandtoserver('IHaveAchievementsMod');
}

// Now register the icons.
registerAchievementIcon("Add-Ons/Script_achievements/images/....png", "...");
registerAchievementIcon("Add-Ons/Script_achievements/images/crap.png", "crap");
registerAchievementIcon("Add-Ons/Script_achievements/images/eye.png", "eye");
registerAchievementIcon("Add-Ons/Script_achievements/images/money_sign.png", "moneysign");
registerAchievementIcon("Add-Ons/Script_achievements/images/pedo.png", "pedo");
registerAchievementIcon("Add-Ons/Script_achievements/images/banana.png", "banana");
registerAchievementIcon("Add-Ons/Script_achievements/images/blablabla.png", "blablabla");
registerAchievementIcon("Add-Ons/Script_achievements/images/bsod.png", "bsod");
registerAchievementIcon("Add-Ons/Script_achievements/images/censored.png", "censored");
registerAchievementIcon("Add-Ons/Script_achievements/images/dice.png", "dice");
registerAchievementIcon("Add-Ons/Script_achievements/images/fail.png", "fail");
registerAchievementIcon("Add-Ons/Script_achievements/images/heart.png", "heart");
registerAchievementIcon("Add-Ons/Script_achievements/images/lol.png", "lol");
registerAchievementIcon("Add-Ons/Script_achievements/images/mac.png", "mac");
registerAchievementIcon("Add-Ons/Script_achievements/images/middlefinger.png", "middlefinger");
registerAchievementIcon("Add-Ons/Script_achievements/images/mouse.png", "mouse");
registerAchievementIcon("Add-Ons/Script_achievements/images/mw.png", "mw");
registerAchievementIcon("Add-Ons/Script_achievements/images/number1.png", "number1");
registerAchievementIcon("Add-Ons/Script_achievements/images/pa.png", "pa");
registerAchievementIcon("Add-Ons/Script_achievements/images/pacman.png", "pacman");
registerAchievementIcon("Add-Ons/Script_achievements/images/pwned.png", "pwned");
registerAchievementIcon("Add-Ons/Script_achievements/images/stop.png", "stop");
registerAchievementIcon("Add-Ons/Script_achievements/images/target.png", "target");
registerAchievementIcon("Add-Ons/Script_achievements/images/trans.png", "trans");
registerAchievementIcon("Add-Ons/Script_achievements/images/trash.png", "trash");
registerAchievementIcon("Add-Ons/Script_achievements/images/unknown.png", "unknown");
registerAchievementIcon("Add-Ons/Script_achievements/images/void.png", "void");
registerAchievementIcon("Add-Ons/Script_achievements/images/weirdface.png", "weirdface");
registerAchievementIcon("Add-Ons/Script_achievements/images/hugger.png", "hugger");
registerAchievementIcon("Add-Ons/Script_achievements/images/GunSpam.png", "gunspam");
registerAchievementIcon("Add-Ons/Script_achievements/images/windows.png", "windows");
registerAchievementIcon("Add-Ons/Script_achievements/images/nuke.png", "nuke");
registerAchievementIcon("Add-Ons/Script_achievements/images/health.png", "health");
registerAchievementIcon("Add-Ons/Script_achievements/images/blockland.png", "blockland");
registerAchievementIcon("Add-Ons/Script_achievements/images/lego.png", "lego");
registerAchievementIcon("Add-Ons/Script_achievements/images/arrows.png", "arrows");

// User based packs.
registerAchievementIcon("Add-Ons/Script_achievements/images/Regulith/eventer.png", "eventer");
registerAchievementIcon("Add-Ons/Script_achievements/images/Regulith/hammer.png", "hammer");