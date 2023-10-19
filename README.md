![](logo.png)
# AdminCommands
AdminCommands is an extension of the [CommunityCommands](https://github.com/decaprime/CommunityCommands) mod and can be used in conjunction with it. There are some duplicate commands, in which case this mod's commands will be prioritized. It also provides a wide variety of new commands for admins.

This is a work in progress with more stuff on the way. At some point I plan to peel out some of these commands into a separate mod as this one is all over the place.

## Reworked Commands 
- `.bloodpotion` -- Reworked to be able to specify a quantity / Gives it in a blood merlot instead of the old bottle.
- `.buff` -- Reworked to to be able to specify a duration / whether the buff should persist through death. Set duration to make it last forever.
- `.unbuff` -- Works the same, just a different name.
- `.give` -- You don't need to look up the PrefabGUID to use this anymore. Just use part of the name, i.e. something like: `.give brick 1000` or `.give strawhat 1`.
- `.revive` -- Will do the same as before, but will also bring up a downed player.
## New Commands 
### Misc Commands
- `.clearbuffs {Player Name}` -- Removes atypical buffs. Use it if you need to make a character normal again.
- `.listbuffs {Player Name}` -- Lists all of the buffs a player has
- `.cast {Prefab Name or GUID}` -- Lets you cast fun boss abilities
- `.control` -- Lets you play as the NPC you are hovering with your mouse. WIP
- `.unlock {Player Name}` -- Unlocks all VBloods/Research/DLC
- `.toggleadmin {Player Name}` -- A way to permanently make someone an admin without restarting the server
- `.togglefreebuild` -- Makes building free for everyone and removes building placement restrictions. Don't place any hearts while enabled or it will crash you server.
- `.reset {Player Name}` -- Basically .r from Arena servers, but also restores spell charges.
- `.repair {Player Name}` -- Repairs all of a player's gear
- `.breakgear {Player Name}` -- Breaks all of a player's gear
- `.down {Player Name}` -- Similar to .kill, but just downs them
- `.ping` -- Tells you your ping
- `.lightning {Player Name} {Intensity}` -- Lets you control how much lightning a player attracts in gloomrot. 25 is default intensity.
### Waypoint Commands
[BloodyPoint](https://github.com/oscarpedrero/BloodyPoints) beat me to this. Check it out for a great alternative.
- `.teleport {Waypoint ID or Name}` -- Teleports you to the specified waypoint if you have permission and aren't in combat.
- `.waypoint create {Waypoint Name} {Admin Only}` -- Creates a waypoint. The waypoint can be public or admin-only.
- `.waypoint remove {Waypoint ID or Name}` -- Removes a waypoint.
- `.waypoint list` -- Lists all waypoints you have access to.
### Servant Commands
- `.setservants {Servant Types} {Quality} {Include Gear} {Target Player}` -- Fills all your servant coffins with living servants of your choice. Do `.setservants ?` to see the servant options. The options go from 0 - F. {Servant Types} should look something like `156AB` (this selects servant 1, servant 5, servant 6, servant A, etc...). So `.setservants 15` will fill your servant coffins equally with servants of type 1 and type 5.
- `.reviveservants {Player Name}` -- Revives all of the servants of the specified player.
### God Mode Commands
- `.god {Player Name}` -- Similar to the normal admin modes but you can still interact with things / fight. Gives you a lot of fun buffs (huge damage, no cooldown, very fast attack speed, long range, etc)
- `.troll {Player Name}` -- The same as .god but you do no damage to anything.
- `.normal {Player Name}` -- Tries to make your character completely normal again regardless of its previous state.
- `.invisible {Player Name}` -- Makes you immortal/invisible.
- `.hp {Amount} {Player Name}` -- Sets HP of player to specified amount.
- `.speed {Amount} {Player Name}` -- Sets speed of player to specified amount.
- `.nocd {Player Name}` -- Toggles no cooldown mode.
- `.immortal {Player Name}` -- Toggles immortal immortal mode (they can be hit, but it does no damage).
- `.immaterial {Player Name}` -- Toggles immaterial mode (they can't be hit at all by anything).
- `.attackspeed {Player Name}` -- Toggles fast attack speed / cast speed mode.
- `.damage {Player Name}` -- Toggles god-mode damage on and off.
- `.trolldamage {Player Name}` -- Toggles troll-mode damage on and off.
- `.projectilespeed {Speed} {Player Name}` -- Changes the speed of all projectiles of player.
- `.projectilerange {Speed} {Player Name}` -- Changes the range of all projectiles of player.
- `.projectilebounces {Number of Bounces} {Player Name}` -- Controls the bounce amount for projectiles of player that bounce.
- `.autorevive {Player Name}` -- Toggles autorevive mode.
- `.spectate {Player Name}` -- Similar to .invisible but player can't interact with anything. Good for letting people spectate a raid without interfering. When you toggle it off it teleports the player back to you.
- `.golem {Player Name}` -- Makes player a golem.
- `.banish` -- Temporarily makes the player unable to play or chat.
### Gated Progression Commands
- `.banregion {Region Name}` -- Makes it so that any non-admins who enter the specified zone will immediately die.
- `.unbanregion {Region Name}` -- Undoes `.banregion`
- `.lockboss {Boss Name}` -- Removes a boss from the world when it is damaged so that it can't be killed. Attempting to track the boss will inform the player that the boss is locked.
- `.unlockboss` -- Undoes `.lockboss`
- `.listlockedbosses` -- Lists all currently locked bosses.

### Support
Reach out to `willis#0575` on Discord for support.