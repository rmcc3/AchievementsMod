package AchMapR {
  
  function Armor::onLeaveMissionArea(%this, %obj)
  {
     %client = %obj.client;
     unlockClientAchievement(%client, "Where the F**K am I?");
     parent::onLeaveMissionArea(%this, %obj);
  }
  
  function sendAchievements(%client)
  {
     parent::sendAchievements(%client);
     
     sendLockedAchievementToClient(%client, "unknown", "Where the F**K am I?", "Fly far away from the spawn.", "Special");
  }
   
};

activatePackage(AchMapR);