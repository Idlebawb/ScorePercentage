# ScorePercentage
Replaces the "Rank" in Map Result Screen with score percentage and adds score percentage to the "Highscore" in the Level Selection Menu below the Leaderboard.

Since 1.2:
Adds Score- and Percentage Difference to Result Screen, if you already had a score on the map / difficulty.

**Warning: Percentage on menu screen will be incorrect when your highscore was set using modifiers, since Beat Saber only saves score without modifier information.**

Examples:
Map Result Screen

![Level End Screen](https://i.imgur.com/71K3EGN.png)


Below Leaderboard

![Menu](https://i.imgur.com/2lxclRO.png)


Every Change this mod makes can be disabled / enabled one by one to fit your preferences.
These can be set in Beat Saber\UserData\ScorePercentage.json.

Currently available Options (as of 1.2.3):

This enables replacing the "Rank" label on the result screen (default: true):

`"EnableLevelEndRank": true`

This enables adding percentage to your score in the menu (default:true):

`"EnableMenuHighscore": true`

This enables score difference display on results (default:true)

`"EnableScoreDifference": true`

This enables score percentage difference display on results (default: true)

`"EnableScorePercentageDifference: true`


~~This enables average cut score display on results (default:false)~~

~~`"EnableAvarageCutScore: true`~~

~~AverageCutScore and ScorePercentageDifference are exclusive of each other. So if you enable both, percentage will take priority.~~


AverageCutScore Option has been removed as of Version 1.4.11
