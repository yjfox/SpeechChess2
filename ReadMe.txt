Speech Application- Gomoku

1. Brief: 
* Gomoku is an abstract strategy board game. Also called Gobang or Five in a Row, it is traditionally played with Go pieces (black and white stones) on a go board with 19x19 intersections; however, because once placed, pieces are not moved or removed from the board, gomoku may also be played as a paper and pencil game. This game is known in several countries under different names. Black plays first, and players alternate in placing a stone of their color on an empty intersection. The winner is the first player to get an unbroken row of five stones horizontally, vertically, or diagonally (Reference from wiki: http://en.wikipedia.org/wiki/Gomoku).
* This is a desktop application with speech control. Users should speak two numbers as indicating X coordinate value and Y coordinate value. Two players place their chess pieces in turn. Finally, program will detect who is winner if there is an unbroken row of five stones horizontally, vertically, or diagonally.

2. Algorithm:
* Speech Lib: using Microsoft Speech Lib 5.4, this program may require to use the windows speech assistant, which shows as 

* Gomoku: Storing the chess placement into a two dimensional array (0 means no chess this position, 1 means chess existing this position). So the detection of who is winner will be easy, just to loop the array and find out if there is an unbroken row of five ��1�� horizontally, vertically, or diagonally.

3. Design:
Images as follow:







