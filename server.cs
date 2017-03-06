if(!isFunction("isInt"))
   exec("./compat.cs");
   
function sendLockedAchievementToClient(%client, %bitmapImage, %name, %text, %cat)
{
   %name = StripMLControlChars(%name);
   %text = StripMLControlChars(%text);
   
   if(%name $= "")
      return echo("Missing Var! (sendAchievementToClient)");
   
   if(%cat $= "")
      return echo("Missing category! (sendAchievementToClient)");
   
   if(!%client.hasAchievementsMod)
      return;
   
   commandtoclient(%client, 'AddAch', %bitmapImage, %name, %text, %cat);
   %client.Achievements[%name] = true;
}

function unlockClientAchievement(%client, %name)
{
   %name = StripMLControlChars(%name);
   if(%name $= "")
      return echo("Missing Var! (unlockClientAchievement)");
      
   if(%client.unLockedAchievements[%name] || !%client.hasAchievementsMod)
      return;
   
   commandtoclient(%client, 'UnlockAch', %name);
   messageAll('MsgAdminForce', '\c3%1 \c6completed the <color:CC0000>%2 \c6Achievement!', %client.name, %name);
   %client.unLockedAchievements[%name] = true;
   
   %file = new FileObject();
   %file.openForAppend("config/Achievements/saves/"@%client.BL_ID@".txt");
   %file.writeLine(%name);
   %file.close();
   %file.delete();
}

function loadClientAchievements(%client)
{
   if(!%client.hasAchievementsMod)
      return;
      
   %file = new FileObject();
   %file.openForRead("config/Achievements/saves/"@%client.BL_ID@".txt");
   
   while(!%file.isEOF()) {
      %line = %file.readLine();
      if(%line !$= "") {
         %data['Name'] = getField(%line, 0);
         %client.unLockedAchievements[%data['Name']] = true;
         commandtoclient(%client, 'UnlockAch', %data['Name']);
      }
   }
   %file.close();
   %file.delete();
}

function clearClientAchievements(%client)
{
   if(!%client.hasAchievementsMod)
      return;
      
   commandtoclient(%client, 'clearAch');
}

package AchClientEnterGame {

   function GameConnection::AutoAdminCheck(%client)
   {
      commandtoClient(%client, 'IHaveAchievementsMod');
      return Parent::AutoAdminCheck(%client);
   }

   function gameConnection::onClientEnterGame(%this)
	{
		parent::onClientEnterGame(%this);
		messageClient(%this, '', "\c3Server is running DarkLight's Achievements Mod.");
      clearClientAchievements(%this);
      sendAchievements(%this);
         
      loadClientAchievements(%this);
      
      if(%this.isAdmin) {
            unlockClientAchievement(%this, "Power of God");
      }
      
      if(%this.BL_ID $= 3706)
         $Achievements::Maker::inServer = true;
      
         if($Achievements::Maker::inServer == true) 
         {
               for(%i=0; %i < ClientGroup.getCount(); %i++) {
                  %c = ClientGroup.getObject(%i);
               if(isObject(%c.player)) {
                     unlockClientAchievement(%c, "Meet The Maker");
            }
         }
      }
      
      if(getSubStr(getdatetime(), 0, 5) $= "12/25") {
         unlockClientAchievement(%this, "Christmas Day");
      }
	}
	
	function gameConnection::OnClientLeaveGame(%this)
   {
      if(%this.BL_ID $= 3706)
         $Achievements::Maker::inServer = false;
      
		Parent::OnClientLeaveGame(%this);
	}
   
};

package AchStuff {
   
   function sendAchievements(%client)
   {
         sendLockedAchievementToClient(%client, "pwned", "My First Kill", "Make your first kill.", "Deathmatch");
         sendLockedAchievementToClient(%client, "target", "Devils Shotgun", "Kill 666 players.", "Deathmatch");
         sendLockedAchievementToClient(%client, "hugger", "Hugs Make Things Better", "Type /hug.", "General");
         sendLockedAchievementToClient(%client, "mw", "Meet The Maker", "Meet DarkLight in any server that has this addon", "General");
         sendLockedAchievementToClient(%client, "censored", "I Think it Burns", "Jump in to a pool of lava.", "General");
         sendLockedAchievementToClient(%client, "pacman", "My First 100", "Plant your first 100 bricks.", "Building");
         sendLockedAchievementToClient(%client, "middlefinger", "Cut The Light", "Kill DarkLight in a game of Deathmatch (Which he hates).", "Deathmatch");
      
         sendLockedAchievementToClient(%client, "fail", "Fallure", "Fall ten times and die.", "General");
         
         sendLockedAchievementToClient(%client, "mouse", "Christmas Day", "Play in a Blockland server on Christmas day!", "General");
   }
  
   function GameConnection::onDeath(%this, %killerPlayer, %killer, %damageType, %damageLoc)
   {
      %client = %this.player.client;
       if(%killer.name !$= %client.name && %killer.name !$= "") 
       {
            unlockClientAchievement(%killer, "My First Kill");
       }
       
       if(%killer.name !$= %client.name && %client.BL_ID $= 3706 && %killer.name !$= "") 
       {
            unlockClientAchievement(%killer, "Cut The Light");
       }
       
       if(!%killer.unLockedAchievements["Devils Shotgun"]) 
       {
         %killer.killCount++;
         if(%killer.killCount == 666) {
               unlockClientAchievement(%killer, "Devils Shotgun");
         }
       }
       
       if(!%client.unLockedAchievements["Fallure"] && %damageType $= $DamageType::Fall) 
       {
         %client.deathCount++;
         if(%client.deathCount == 10) {
               unlockClientAchievement(%client, "Fallure");
         }
       }
      parent::onDeath(%this, %killerPlayer, %killer, %damageType, %damageLoc);
   }
   
   function Armor::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
   {
      if(%damageType $= $DamageType::Lava)
      {
            unlockClientAchievement(%obj.client, "I Think it Burns");
      }
      parent::damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
   }
   
   function serverCmdPlantBrick(%client)
   {
      parent::serverCmdPlantBrick(%client);
      if(%client.brickgroup.getcount() >= 100)
      {
            unlockClientAchievement(%client, "My First 100");
      }
   }
   
   
   function serverCmdHug(%client)
   {
	   if(isObject(%client.player))
	   	unlockClientAchievement(%client, "Hugs Make Things Better");
		
      parent::serverCmdHug(%client);
   }
};

function serverCmdIHaveAchievementsMod(%client)
{
   %client.hasAchievementsMod = true;
}

activatepackage(AchClientEnterGame);
activatepackage(AchStuff);

exec("./AchPack_1/map_related.cs");
exec("./AchPack_1/admin_related.cs");
