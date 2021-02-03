using System;
using System.Text;
using NWN.API;
using NWN.API.Constants;
using NWN.Services;
using OMG.Interface;
using OMG.Util;
using Troschuetz.Random;

namespace OMG.Service.Chat
{
    [ServiceBinding(typeof(IChatCommand))]
    public class RollCommand : IChatCommand
    {
        public string Command { get; } = "/roll";
        public bool IsDMOnly { get; } = false;
        private TRandom Random { get; } = new TRandom();

        public void ExecuteCommand(NwPlayer sender, string[] arguments)
        {
            if (arguments.Length is < 1 or > 2)
            {
                sender.SendServerMessage("Invalid argument", Colors.Red);
                Log.Logger.Warn($"Lenght error - {arguments.Length}");
                return;
            }

            var isHidden = arguments.Length == 2 && arguments[1].ToLower() == "h";
            var firstArgument = arguments[0];
            // Handle d20 dice rolls for key abilities & skills
            switch (firstArgument.ToLower())
            {
                // Abilities

                #region Abilities

                case "str":
                case "strength":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Strength, sender), isHidden);
                        return;
                    }
                case "dex":
                case "dexterity":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Dexterity, sender), isHidden);
                        return;
                    }
                case "con":
                case "constitution":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Constitution, sender), isHidden);
                        return;
                    }
                case "int":
                case "intelligence":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Intelligence, sender), isHidden);
                        return;
                    }
                case "wis":
                case "wisdom":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Wisdom, sender), isHidden);
                        return;
                    }
                case "cha":
                case "charisma":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Ability.Charisma, sender), isHidden);
                        return;
                    }

                #endregion

                // Skills

                #region Skills

                case "animal":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.AnimalEmpathy, sender), isHidden);
                        return;
                    }
                case "concentration":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Concentration, sender), isHidden);
                        return;
                    }
                case "disable_trap":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.DisableTrap, sender), isHidden);
                        return;
                    }
                case "discipline":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Discipline, sender), isHidden);
                        return;
                    }
                case "heal":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Heal, sender), isHidden);
                        return;
                    }
                case "hide":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Hide, sender), isHidden);
                        return;
                    }
                case "listen":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Listen, sender), isHidden);
                        return;
                    }
                case "lore":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Lore, sender), isHidden);
                        return;
                    }
                case "silent":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.MoveSilently, sender), isHidden);
                        return;
                    }
                case "lock":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.OpenLock, sender), isHidden);
                        return;
                    }
                case "parry":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Parry, sender), isHidden);
                        return;
                    }
                case "perform":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Perform, sender), isHidden);
                        return;
                    }
                case "persuade":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Persuade, sender), isHidden);
                        return;
                    }
                case "pick_pocket":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.PickPocket, sender), isHidden);
                        return;
                    }
                case "search":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Search, sender), isHidden);
                        return;
                    }
                case "set_trap":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.SetTrap, sender), isHidden);
                        return;
                    }
                case "spell":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Spellcraft, sender), isHidden);
                        return;
                    }
                case "spot":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Spot, sender), isHidden);
                        return;
                    }
                case "taunt":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Taunt, sender), isHidden);
                        return;
                    }
                case "magic_device":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.UseMagicDevice, sender), isHidden);
                        return;
                    }
                case "bluff":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Bluff, sender), isHidden);
                        return;
                    }
                case "intimidate":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Intimidate, sender), isHidden);
                        return;
                    }
                case "ride":
                    {
                        PrintRoll(sender, ProcessDiceRoll(Skill.Ride, sender), isHidden);
                        return;
                    }

                #endregion

                // Saving Throws

                #region SavingThrows

                case "fortitude":
                    {
                        PrintRoll(sender, ProcessDiceRoll(SavingThrow.Fortitude, sender), isHidden);
                        return;
                    }
                case "reflex":
                    {
                        PrintRoll(sender, ProcessDiceRoll(SavingThrow.Reflex, sender), isHidden);
                        return;
                    }
                case "will":
                    {
                        PrintRoll(sender, ProcessDiceRoll(SavingThrow.Will, sender), isHidden);
                        return;
                    }

                    #endregion
            }

            firstArgument = firstArgument.ToLower().Replace('d', 'k');
            if (!firstArgument.ToLower().Contains("k"))
            {
                sender.SendServerMessage("Invalid argument", Colors.Red);
                return;
            }

            var indexOfK = firstArgument.IndexOf('k');
            string result;

            // Quality of life improvement -> It is possible to call this with /roll k'number'
            if (indexOfK == 0 && int.TryParse(firstArgument[1..], out var numberOfSides))
            {
                result = ProcessDiceRoll(1, numberOfSides);
                PrintRoll(sender, result, isHidden);
                return;
            }

            // Check if dice number and side syntax is ok
            if (!int.TryParse(firstArgument[..indexOfK], out var numberOfDices) ||
                !int.TryParse(firstArgument[(indexOfK + 1)..], out numberOfSides))
            {
                sender.SendServerMessage("Invalid argument", Colors.Red);
                return;
            }

            result = ProcessDiceRoll(numberOfDices, numberOfSides);
            PrintRoll(sender, result, isHidden);
        }

        private string ProcessDiceRoll(int numberOfDices, int numberOfSides)
        {
            if (numberOfDices is < 1 or > 32 || numberOfSides < 1)
            {
                return "";
            }
            var result = new StringBuilder();
            var sum = 0;
            result.Append($"{numberOfDices}k{numberOfSides}: (");
            for (var i = 0; i < numberOfDices; ++i)
            {
                var roll = Random.Next(1, numberOfSides);
                sum += roll;
                result.Append($"{roll} + ");
            }
            result.Remove(result.Length - 3, 3);
            result.Append($") = {sum}");
            return result.ToString();
        }

        private string ProcessDiceRoll(Ability ability, NwPlayer nwPlayer)
        {
            var abilityModifier = nwPlayer.GetAbilityModifier(ability);
            var roll = Random.Next(1, 20);

            return $"{Enum.GetName(ability)}: {roll} + {abilityModifier} = {roll + abilityModifier}";
        }

        private string ProcessDiceRoll(Skill skill, NwPlayer nwPlayer)
        {
            // TODO: DECIDE WHETHER RANKS ONLY SHOULD BE FALSE
            var skillModifier = nwPlayer.GetSkillRank(skill, false);
            var roll = Random.Next(1, 20);

            return $"{Enum.GetName(skill)}: {roll} + {skillModifier} = {roll + skillModifier}";
        }

        private string ProcessDiceRoll(SavingThrow savingThrow, NwPlayer nwPlayer)
        {
            var savingThrowValue = nwPlayer.GetBaseSavingThrow(savingThrow);
            var roll = Random.Next(1, 20);

            return $"{Enum.GetName(savingThrow)}: {roll} + {savingThrowValue} = {roll + savingThrowValue}";
        }

        private void PrintRoll(NwPlayer nwPlayer, string result, bool isHidden)
        {
            var color = isHidden ? Colors.Magenta : Colors.Gold;

            if (!isHidden)
            {
                nwPlayer.SpeakString(result.ColorString(color));
            }
            else
            {
                nwPlayer.FloatingTextString($"{nwPlayer.Name} rolled - {result.ColorString(color)}", false);
                nwPlayer.SpeakString($"{"Hidden roll -".ColorString(Colors.Gray)} {result.ColorString(color)}",
                    TalkVolume.SilentShout);
            }
        }
    }
}