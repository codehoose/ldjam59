# LDJAM #59 - Signal

## 2026-04-18 11:43 Let's go!

Feeling pretty good about this one. I managed to stay up to 2am to get the theme for the jam. Signal. I'm happy with that one. I wanted to do a Chaos: Battle of the Wizards inspired game and this fits it perfectly. Substitute wizards for hackers, or why not, AI and you have a game!

Downside is that I've slept in and it's 11:43 as I type this and my main machine doesn't have half the tools I need. I'm going to split time between this and my laptop, so that I can code while watching TV. Maybe. We'll see how it goes.

## 2026-04-18 12:33 Virtual (Insanity) Re-write of Core

I started getting my framework ready last night, nothing major just some helper classes. Basically spent the last hour re-writing them. Why? Cos I'm not going for code re-use in this, I just want it to work as a game. Pretty much all the models, the state machine and base components are ready. I still have to re-work spritesheet so that it can be re-used in classes for this game, but that can come later.

## 2026-04-18 15:25 Things are appearing on screen

Had some lunch and back at it. I have the raw states done and can move the game from initialization through to displaying something on screen, and here it is!

![It's taken me all day to get here!](.images/first-shot.png "It's progress!")

Now on to the first action; summoning! I still feel better at this point in the process than any other game jam. I think my idea of coming up with a game type and trying to fit it with the jam has really worked. It gave me a week to basically come up with what I _wanted_ to do. Planning, eh?!

## 2026-04-18 17:14 Two hours on a button!?

But it's an important button. All the menu controls will be through this button, so it's best getting this right just now. On to the actual actions part of this!

## 2026-04-18 19:05 States, states and what a state

Slowly getting there. I am fixating on the smaller details and just need to hammer through. I think I can get to a point tonight where I can deploy something. Make it at least feel like a game. So far, I have a menu system working and when you select "deploy" it changes to a selection cursor; green for you can place this here, red for you can't. Progress!

## 2026-04-18 21:10 Time for a rest. Watch some TV?

I have some gameplay! The players can take turns to place items on the grid. There are currently two "AIs" that control various programs; a crawler and a drone for now. And those can be placed on the grid near the AI. There are a limited number of cycles (think: mana) so you have to watch that resource too. Next up is movement and starting processes (think: spells!) All good stuff.

![Things are starting to come together!](.images/some-gameplay.png "Finally, some gameplay!")

## 2026-04-19 10:20 Not gonna make it..?

This might be tight, I have five hours left to finish the jam. Maybe some time on Monday? Maybe even a couple of hours tonight? Jams always happen on weekends where plans materialize. It is what it is I guess. Just finished adding the ghosts and hiding them from the other player when it's their screen. The graphics need work, but they will do for now. 

## 2026-04-19 11:01 Movement is working!

Selection and movement works great! The players can now move their pieces on the board using a simple selection mechanic. Select the unit, cursor becomes a yellow square to show where you can move to. I might change that to arrows or something later. Polish stage, if I can get to it. 

## 2026-04-19 12:12 Attack! Attack! Arrrgh! Attack!

Attacking units is working. I've forgotten about running programs. D'oh! That's next, it'll use the same selector thing as before. I should probably work on the interstitial screens too. They look naff. But, for now I've fixed the bugs with movement - you could move a bit further just by flicking the mouse, so it's constrained to when you _first_ clicked. OK, onwards an upwards. Interstitial screen first. Then work on programs. Kill process and threat scan.

## 2026-04-19 12:34 Hackerman

Added a hacker image from [Pixabay](https://pixabay.com/photos/hacker-safety-computer-the-internet-8003394/) to add some spice to the visuals. Makes it a little less boring and now you can see which player is playing. Blue for the white hat hacker, slightly orange for the black hat hacker. I got a font (PixelGrunge) from [dafont.com](https://dafont.com) for the logo.

The title screen and side images are in and it's feeling like the end is in sight. I still need to do a _lot_ of polish and finish off the program run stage too.

![Title screen showing hacker man and menu options](.images/title.png "Title screen")

Here's some in-game action.

![In-game sequence showing the state after a couple of moves](.images/gameplay-action.png "Gameplay")

## 2026-04-19 14:25 The main loop is almost complete

Everything works! The only functional things not implemented are the thread scan and the game over condition. But I think that'll be straight forward enough. The game over will be next. Then after that, polish? Music? Sound effects?

I didn't mention in the last entry but I thought I'd call the game "Hack the Planet!" because it's a great movie and the game has hackers, AI, grids (Tron anyone) and I immediately thought of Crash Override and Acid Burn.

## 2026-04-19 19:13 Back from dinner and functionally it's a wrap

The game is functionally done at this point! I still have a lot of polish items to do, but functionally everything is there. There's movement! There's attacks! There are other attacks! There are game over conditions! The only thing functionally missing is a "process scan" feature. I'll implement that tonight, should be relatively easy.

What I've learned so far:

- Plan Plan Plan. You can't plan enough for the jam. It's not about getting your tech or workspace ready, it's about coming up with a couple of game style ideas and planning out how you could fit them into a theme
- Get the base functionality in first. Don't worry about looks, it's all about getting from screen to screen and the flow of the game. Concentrate on the core loop and build it piece by piece. Talking of which...
- Using a single game state to hold the current state of the game and a Finite State Machine was an amazing help. It's so easy to test just one bit of the core loop. I also created "core loop" states that always showed various bits and pieces like how many cycles were left, who the current player is etc.

## 2026-04-25 11:15 Missed the deadline, but still going

Well, I got the basic game working but ran out of time to get the polish items in. I could have handed it in. But.. Decided to spend some more time on polish. So far I've re-worked the actions into commands to make it easier to:
- Serialize the actions across a network, or via email or ...
- Replay actions for the opposition player to let them see what their opponent has been up to

At any rate, it's now going to be a mobile game. Probably :)

I've also added a [CLAUDE.md](CLAUDE.md) file for future. I didn't involve the AI at all developing the game so far and I don't think that will change. Coding is my hobby as well as my job and it's fun. And I actually enjoy this wee game and want to see it finished.

Fixed text render issues; the text was on the same layer as the front GUI so I re-ordered a couple of things. 

Reworked the game states so that there's only one place unit stage. Added toggle buttons to make it easier to deploy multiples of the same unit type.