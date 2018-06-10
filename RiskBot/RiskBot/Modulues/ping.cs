using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Threading;

namespace RiskBot.Modulues
{
    public class variables
    {
        public static bool debugMode = false;
    }
    public class mainRisk : InteractiveBase
    {
        Random rnd = new Random();
        int attackerDice;
        int defenderDice;
        List<int> attackRolls = new List<int>();
        List<int> defendRolls = new List<int>();

        [Command("begin", RunMode = RunMode.Async)]
        public async Task riskTime(int attackers, int defenders)
        {
            if (attackers <= 1)
            {
                await ReplyAsync("Not enough attackers, try the command again but with more than 1 attackers.");

            }
            else
            {
                await ReplyAsync($"You wish to battle with {attackers} attackers and {defenders} defenders?  If so say confirm.");
                //Waits for user to say confirm or till a certain time
                var response = await NextMessageAsync();
                if (response.Content == "confirm")
                {
                    await ReplyAsync("You confirmed.  Attacker, how many troops do you want to attack with from 1-3?");
                    //Waits for number of attacker dice
                    bool loop = true;
                    while (loop)
                    {
                        response = await NextMessageAsync();
                        //Checks if what they said is a number
                        if (Int32.TryParse(response.Content, out attackerDice))
                        {
                            //Checks if it is less or equal to 3 and greater than or equal to 1
                            if (1 <= attackerDice && attackerDice <= 3)
                            {
                                //Checks to make sure they have enough untis to do this
                                if (attackers - 1 >= attackerDice)
                                {
                                    if (defenders >= 2)
                                    {
                                        defenderDice = 2;
                                    }
                                    else if (defenders == 1)
                                    {
                                        defenderDice = 1;
                                    }
                                    else
                                    {
                                        await ReplyAsync("Defenders less than 0 error.  @Warlord_Sdocy#8097 ");
                                        loop = false;
                                        continue;
                                    }
                                    for (int i = 0; i < attackerDice; i++)
                                    {
                                        int roll = rnd.Next(1, 7);
                                        attackRolls.Add(roll);
                                    }
                                    for (int i = 0; i < defenderDice; i++)
                                    {
                                        int roll = rnd.Next(1, 7);
                                        defendRolls.Add(roll);
                                    }
                                    defendRolls.Sort();
                                    attackRolls.Sort();
                                    if (variables.debugMode)
                                    {
                                        await ReplyAsync("Attacker rolls:");
                                        Thread.Sleep(2000);
                                        for (int i = 0; i < attackRolls.Count; i++)
                                        {
                                            await ReplyAsync($"{attackRolls[i]}");
                                            Thread.Sleep(2000);
                                        }
                                        await ReplyAsync("Defender rolls:");
                                        Thread.Sleep(2000);
                                        for (int i = 0; i < defendRolls.Count; i++)
                                        {
                                            await ReplyAsync($"{defendRolls[i]}");
                                            Thread.Sleep(2000);
                                        }
                                    }
                                    if (attackRolls[attackRolls.Count - 1] > defendRolls[defendRolls.Count - 1])
                                    {
                                        defenders--;
                                    }
                                    else if (attackRolls[attackRolls.Count - 1] <= defendRolls[defendRolls.Count - 1])
                                    {
                                        attackers--;
                                    }
                                    if (attackerDice > 2 && defenderDice > 1)
                                    {
                                        if (attackRolls[attackRolls.Count - 2] > defendRolls[defendRolls.Count - 2])
                                        {
                                            defenders--;
                                        }
                                        else if (attackRolls[attackRolls.Count - 2] <= defendRolls[defendRolls.Count - 2])
                                        {
                                            attackers--;
                                        }
                                    }

                                    attackRolls.Clear();
                                    defendRolls.Clear();
                                    await ReplyAsync($"Now there are {attackers} attackers left and {defenders} defenders left.");
                                    Thread.Sleep(1000);
                                    if (defenders <= 0)
                                    {
                                        await ReplyAsync("The attacker wins and takes the territory!");
                                        loop = false;
                                    }
                                    else if (attackers <= 1)
                                    {
                                        await ReplyAsync("The attacker has no more troops left to attack.");
                                        loop = false;
                                    }
                                    else
                                    {
                                        await ReplyAsync($"Do you want to battle again, Y or N?");
                                        response = await NextMessageAsync();
                                        if (response.Content == "N" || response.Content == "n")
                                        {
                                            loop = false;
                                        }
                                        else if (response.Content == "Y" || response.Content == "y")
                                        {
                                            await ReplyAsync("How many dice do you want to roll attacker, 1-3?");
                                        }
                                        else if (response.Content == null)
                                        {
                                            await ReplyAsync("Time ran out, exiting.");
                                            loop = false;
                                        }
                                        else
                                        {
                                            await ReplyAsync("Invalid choice, exiting.");
                                            loop = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //If attacker doesn't have enough units
                                    //loop = false;
                                    await ReplyAsync("You don't have enough attackers to roll that many dice, # of dice can equal at max your attackers - 1.");
                                    continue;
                                }
                            }
                            else
                            {
                                //If attacker's input is not a number betwen 1 and 3
                                //loop = false;
                                await ReplyAsync("Please enter a number between 1 and 3.");
                                continue;
                            }
                        }
                        else
                        {
                            //If attacker's input is not a number
                            //loop = false;
                            await ReplyAsync("Please enter a number");
                            continue;
                        }
                    }
                }
                else
                {
                    await ReplyAsync("You didn't confirm, exiting.");
                }

            }
        }
        [Command("quickbegin", RunMode = RunMode.Async)]
        public async Task quickRiskTime(int attackers, int defenders)
        {
            if (attackers <= 1)
            {
                await ReplyAsync("Not enough attackers, try the command again but with more than 1 attackers.");
            }
            else
            {
                await ReplyAsync($"You wish to quick battle with {attackers} attackers and {defenders} defenders?  If so say confirm.");
                //Waits for user to say confirm or till a certain time
                var response = await NextMessageAsync();
                if (response.Content == "confirm")
                {
                    bool loop = true;
                    while (loop)
                    {
                        if (defenders >= 2)
                        {
                            defenderDice = 2;
                        }
                        else if (defenders == 1)
                        {
                            defenderDice = 1;
                        }
                        else
                        {
                            await ReplyAsync("Defenders less than 0 error.  @Warlord_Sdocy#8097 ");
                            loop = false;
                            continue;
                        }


                        if (attackers >= 4)
                        {
                            attackerDice = 3;
                        }
                        else if (attackers == 3)
                        {
                            attackerDice = 2;
                        }
                        else if (attackers == 2)
                        {
                            attackerDice = 1;
                        }
                        else
                        {
                            await ReplyAsync("Attackers less than 0 error.  @Warlord_Sdocy#8097 ");
                            loop = false;
                            continue;
                        }


                        for (int i = 0; i < attackerDice; i++)
                        {
                            int roll = rnd.Next(1, 7);
                            attackRolls.Add(roll);
                        }


                        for (int i = 0; i < defenderDice; i++)
                        {
                            int roll = rnd.Next(1, 7);
                            defendRolls.Add(roll);
                        }


                        defendRolls.Sort();
                        attackRolls.Sort();
                        if (variables.debugMode)
                        {
                            await ReplyAsync("Attacker rolls:");
                            Thread.Sleep(2000);
                            for (int i = 0; i < attackRolls.Count; i++)
                            {
                                await ReplyAsync($"{attackRolls[i]}");
                                Thread.Sleep(2000);
                            }
                            await ReplyAsync("Defender rolls:");
                            Thread.Sleep(2000);
                            for (int i = 0; i < defendRolls.Count; i++)
                            {
                                await ReplyAsync($"{defendRolls[i]}");
                                Thread.Sleep(2000);
                            }
                        }

                        if (attackRolls[attackRolls.Count - 1] > defendRolls[defendRolls.Count - 1])
                        {
                            defenders--;
                        }
                        else if (attackRolls[attackRolls.Count - 1] <= defendRolls[defendRolls.Count - 1])
                        {
                            attackers--;
                        }

                        if (defenderDice > 1 && attackerDice > 1)
                        {
                            if (attackRolls[attackRolls.Count - 2] > defendRolls[defendRolls.Count - 2])
                            {
                                defenders--;
                            }
                            else if (attackRolls[attackRolls.Count - 2] <= defendRolls[defendRolls.Count - 2])
                            {
                                attackers--;
                            }
                        }

                        attackRolls.Clear();
                        defendRolls.Clear();
                        //await ReplyAsync($"Round complete, {attackers} attackers left and {defenders} defenders left.");
                        //Thread.Sleep(1000);
                        if (defenders <= 0)
                        {
                            await ReplyAsync($"The attacker wins and takes the territory with {attackers} attackers left!");
                            loop = false;
                        }
                        else if (attackers <= 1)
                        {
                            await ReplyAsync($"The attacker has no more troops left to attack and there are {defenders} defenders left.");
                            loop = false;
                        }

                    }
                }
                else
                {
                    await ReplyAsync("You didn't confirm, exiting.");
                }
            }
        }
        [Command("debug", RunMode = RunMode.Async)]
        public async Task debug()
        {
            if (!variables.debugMode)
            {
                variables.debugMode = true;
                await ReplyAsync("Debug mode is on");
            }
            else if (variables.debugMode)
            {
                variables.debugMode = false;
                await ReplyAsync("Debug mode is off");
            }
            else
            {
                await ReplyAsync("How can a bool not be true or false :thinking: ");
            }
            
        }
    }
}
