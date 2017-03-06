package AchAdminR {
  
  function sendAchievements(%client)
  {
     parent::sendAchievements(%client);
     
     sendLockedAchievementToClient(%client, "teleport", "You teleport me right round, baby right round.", "Teleport 100 times without leaving the game.", "Special");
  }
  
  function servercmddropplayeratcamera(%client)
  {
     if(%client.isAdmin) {
        %client.adminCamTeleCount++;
        if(%client.adminCamTeleCount >= 100) {
           unlockClientAchievement(%client, "You teleport me right round, baby right round.");
        }
     }
     parent::servercmddropplayeratcamera(%client);
  }
   
};

activatePackage(AchAdminR);

registerAchievementIcon("Add-Ons/Script_achievements/images/DarkLight/teleport.png", "teleport");