# ScorePercentage
Replaces the "Rank" in Map Result Screen with score percentage and adds score percentage to the "Highscore" in the Level Selection Menu.

**Warning: Percentage on menu screen will be incorrect when your highscore was set using modifiers, since Beat Saber only saves score without modifier information.**

Examples:
Map Result Screen
![Level End Screen](https://i.imgur.com/zmzj11X.jpg)


Menu
![Menu](https://i.imgur.com/7v8dYoo.jpg)


In Version 1.0.2+ you can disable parts of the by editing your Beatsaber\User Data\ScorePercentage.json

This disables replacing the "Rank" label on the result screen:

`"EnableLevelEndRank": false`

This disables adding percentage to your score in the menu:

`"EnableMenuHighscore": false`
