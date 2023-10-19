using Bloodstone.API;
using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Entities;

namespace AdminCommands
{
    public static class PlayerService
    {
		public static bool TryGetPlayerFromString(string input, out Player player)
        {
			var userEntities = Helper.GetEntitiesByComponentTypes<User>(true);
			foreach (var userEntity in userEntities)
			{
				var user = userEntity.Read<User>();
				if (user.CharacterName.ToString().ToLower() == input.ToLower())
				{
					player = new Player(userEntity);
					return true;
				}
				else if (ulong.TryParse(input, out ulong PlatformID))
                {
					if (user.PlatformId == PlatformID)
                    {
						player = new Player(userEntity);
						return true;
                    }						 
                }
			}
			player = default;
			return false;
		}

		public static bool TryGetCharacterFromName(string input, out Entity Character)
        {
			if (TryGetPlayerFromString(input, out Player player) )
			{
				Character = player.Character;
				return true;
            }
			Character = default;
			return false;
        }

		public static bool TryGetUserFromName(string input, out Entity User)
		{
			if (TryGetPlayerFromString(input, out Player player))
			{
				User = player.User;
				return true;
			}
			User = default;
			return false;
		}

		public struct Player
		{
			public string Name { get; set; }
			public ulong SteamID { get; set; }
			public bool IsOnline { get; set; }
			public bool IsAdmin { get; set; }
			public Entity User { get; set; }
			public Entity Character { get; set; }
			public Player(Entity userEntity = default, Entity charEntity = default)
			{
				User = userEntity;
				var user = User.Read<User>();
				Character = user.LocalCharacter._Entity;
				Name = user.CharacterName.ToString();
				IsOnline = user.IsConnected;
				IsAdmin = user.IsAdmin;
				SteamID = user.PlatformId;
			}
		}
	}
}
