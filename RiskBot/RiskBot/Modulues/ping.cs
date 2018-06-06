using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Threading;

namespace RiskBot.Modulues
{
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
                                defenderDice = 2;
                                //await ReplyAsync($"{attackerDice} {defenderDice}");

                                for (int i = 0; i < attackerDice; i++)
                                {
                                    //await ReplyAsync($"{attackerDice} {defenderDice}");
                                    int roll = rnd.Next(1, 7);
                                    //await ReplyAsync($"{roll}");
                                    attackRolls.Add(roll);
                                    //await ReplyAsync($"{roll}");
                                }
                                for (int i = 0; i < defenderDice; i++)
                                {
                                    int roll = rnd.Next(1, 7);
                                    defendRolls.Add(roll);
                                }
                                defendRolls.Sort();
                                attackRolls.Sort();
                                /*await ReplyAsync("Attacker rolls:");
                                Thread.Sleep(2000);
                                for (int i = 0; i < attackRolls.Count; i++)
                                {
                                    await ReplyAsync($"{attackRolls[i]}");
                                    Thread.Sleep(2000);
                                }
                                Thread.Sleep(2000);
                                await ReplyAsync("Defender rolls:");
                                Thread.Sleep(2000);
                                for (int i = 0; i < defendRolls.Count; i++)
                                {
                                    await ReplyAsync($"{defendRolls[i]}");
                                    Thread.Sleep(2000);
                                }*/
                                //await ReplyAsync($"{defendRolls.Count}");
                                if(attackRolls[attackRolls.Count - 1] > defendRolls[defendRolls.Count - 1])
                                {
                                    defenders--;
                                }
                                else if(attackRolls[attackRolls.Count - 1] < defendRolls[defendRolls.Count - 1])
                                {
                                    attackers--;
                                }
                                if(attackRolls[attackRolls.Count - 2] > defendRolls[defendRolls.Count - 2])
                                {
                                    defenders--;
                                }
                                else if(attackRolls[attackRolls.Count - 2] < defendRolls[defendRolls.Count - 2])
                                {
                                    attackers--;
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
                                    if(response.Content == "N")
                                    {
                                        loop = false;
                                    }
                                    else if(response.Content == "Y")
                                    {
                                        await ReplyAsync("How many dice do you want to roll attacker, 1-3?");
                                    }
                                    else if(response.Content == null)
                                    {
                                        await ReplyAsync("Time ran out, exiting.");
                                        loop = false;
                                    }
                                    else
                                    {
                                        await ReplyAsync("Invaid choice, exiting.");
                                        loop = false;
                                    }
                                }
                            }
                            else
                            {
                                //If attacker doesn't have enough units
                                //loop = false;
                                await ReplyAsync("You don't have enough attackers to roll that many dice, # of dice can equal at max your attackers - 1.");
                            }
                        }
                        else
                        {
                            //If attacker's input is not a number betwen 1 and 3
                            //loop = false;
                            await ReplyAsync("Please enter a number between 1 and 3.");
                        }
                    }
                    else
                    {
                        //If attacker's input is not a number
                        //loop = false;
                        await ReplyAsync("Please enter a number");
                    }
                }
            }
            else
            {
                await ReplyAsync("You didn't confirm, exiting.");
            }
        }
    }
}
