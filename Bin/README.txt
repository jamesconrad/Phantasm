//LAUNCHING THE GAME:

You can launch the app on the same machine twice and it will work.

There's an issue we haven't solved with the ui for part of the game, so have the resolution of the game above 800x600,
so you can properly see on the second game.

When the game starts, click on the 'options' tab on the folder. 
The page will be brought out and you can enter two numbers, one to specify the local port,
and one to specify the target port. These will of course be opposite for the two applications.

After you choose the two ports, click on the 'play' tab, and then that page will come out.
On one application, click on the create button and it will tell you tell you the ip address of the machine.

On the 'play' tab on the second app, hit enter the ip in the input field and hit join.
You should then be able to select the ability to play as either the agent or hacker.
Select the two application's respective roles and you will then enter the level.
The hacker has an issue with it's UI, as I said, but one of the views for the hacker will just about match the view of the other app.

The agent view will have some ui text that species the send rate. This can be increased or decreased with 1 or 2 on the keyboard.

The agent is the object that has dead reckoning applied to it, and this should show on the hacker's view, in the bottom right view.

//FOLDER INFO
The PhaNetworking zip folder contains the c++ dll solution that we use for the game's networking.

The assets.zip folder contains the c# script files that are relevant to the dll's use, along with the implementation of dead reckoning.
Specfically, the dead reckoning algorithm itself is contained within NetworkedMovement.cs.
